using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MemoryManagement;
using KeyboardManagement;

namespace CoordinateManager.Forms.COD1
{
    public partial class COD1 : Form
    {
        private readonly MemoryManagerFor32BitProcesses memoryManager;
        private int baseAddressOfPlayer = 0;
        private float multiplierOfPlayerSpeed = 5.0f;

        private readonly int codeOfWKey = 87;
        private readonly int codeOfLeftMouseButton = 1;
        private readonly int codeOfRightMouseButton = 2;

        private bool isNoclipEnabled = false;

        private void FreezeXYZVelocityOfPlayer()
        {
            memoryManager.ConvertFloatValueToBytes(0.0f).WriteBytesToAddress(baseAddressOfPlayer + 0x20, 4);
            memoryManager.ConvertFloatValueToBytes(0.0f).WriteBytesToAddress(baseAddressOfPlayer + 0x24, 4);
            memoryManager.ConvertFloatValueToBytes(0.0f).WriteBytesToAddress(baseAddressOfPlayer + 0x28, 4);
        }

        private void ManageZCoordinateOfPlayer()
        {
            float zCoordinate = memoryManager.ReadBytesFromAddress(baseAddressOfPlayer + 0x1C, 4).ConvertBytesToFloatValue();
            if (KeyboardManager.IsKeyPushedDown(codeOfLeftMouseButton))
            {
                zCoordinate = zCoordinate + 5.0f;
                memoryManager.ConvertFloatValueToBytes(zCoordinate).WriteBytesToAddress(baseAddressOfPlayer + 0x1C, 4);
            }
            if (KeyboardManager.IsKeyPushedDown(codeOfRightMouseButton))
            {
                zCoordinate = zCoordinate - 5.0f;
                memoryManager.ConvertFloatValueToBytes(zCoordinate).WriteBytesToAddress(baseAddressOfPlayer + 0x1C, 4);
            }
        }

        private void MovePlayer()
        {
            float yawAngleInDegrees = memoryManager.ReadBytesFromAddress(baseAddressOfPlayer + (0x2 * 0x4) + 0xAC, 4).ConvertBytesToFloatValue();
            float xCoordinate = memoryManager.ReadBytesFromAddress(baseAddressOfPlayer + 0x14, 4).ConvertBytesToFloatValue();
            float yCoordinate = memoryManager.ReadBytesFromAddress(baseAddressOfPlayer + 0x18, 4).ConvertBytesToFloatValue();

            if (KeyboardManager.IsKeyPushedDown(codeOfWKey))
            {
                float sineOfYawAngle = Convert.ToSingle(Math.Sin(yawAngleInDegrees));
                float cosineOfYawAngle = Convert.ToSingle(Math.Cos(yawAngleInDegrees));

                float newXCoordinate = xCoordinate - ((sineOfYawAngle * -1) * multiplierOfPlayerSpeed);
                float newYCoordinate = yCoordinate + (cosineOfYawAngle * multiplierOfPlayerSpeed);

                memoryManager.ConvertFloatValueToBytes(newXCoordinate).WriteBytesToAddress(baseAddressOfPlayer + 0x14, 4);
                memoryManager.ConvertFloatValueToBytes(newYCoordinate).WriteBytesToAddress(baseAddressOfPlayer + 0x18, 4);
            }
        }

        public COD1()
        {
            InitializeComponent();
            memoryManager = new MemoryManagerFor32BitProcesses();
        }

        private void button_OpenCloseProcess_Click(object sender, EventArgs e)
        {
            if (timer_loopExecutor.Enabled)
            {
                timer_loopExecutor.Enabled = false;
                memoryManager.DetachFromProcess();

                button_OpenCloseProcess.ForeColor = Color.Black;
                button_OpenCloseProcess.Text = "Open process";
                label_ProcessHandle.Text = "Process handle: " + memoryManager.GetProcessHandle();
            }
            else
            {
                memoryManager.FindProcessByNameThenAttachToIt("CoDSP");
                timer_loopExecutor.Enabled = true;

                button_OpenCloseProcess.ForeColor = Color.Red;
                button_OpenCloseProcess.Text = "Close process handle";
                label_ProcessHandle.Text = "Process handle: " + memoryManager.GetProcessHandle();
            }
        }

        private void button_Noclip_Click(object sender, EventArgs e)
        {
            if (isNoclipEnabled)
            {
                isNoclipEnabled = false;
                button_Noclip.ForeColor = Color.Black;
                button_Noclip.Text = "ON";
            }
            else
            {
                isNoclipEnabled = true;
                button_Noclip.ForeColor = Color.Red;
                button_Noclip.Text = "OFF";
            }
        }

        private void timer_loopExecutor_Tick(object sender, EventArgs e)
        {
            baseAddressOfPlayer = memoryManager.ReadBytesFromAddress(0x0152BC64, 4).ConvertBytesToIntegerValue();
            if (isNoclipEnabled)
            {
                FreezeXYZVelocityOfPlayer();
                ManageZCoordinateOfPlayer();
                MovePlayer();
            }
        }
    }
}
