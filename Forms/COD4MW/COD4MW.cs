using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MemoryManagement;
using KeyboardManagement;

namespace CoordinateManager.Forms.COD4MW
{
    public partial class COD4MW : Form
    {
        private readonly MemoryManagerFor32BitProcesses memoryManager;
        private int baseAddressOfPlayer = 0;
        private float multiplierOfPlayerSpeed = 30.0f;
        private float addendumForZCoordinate = 20.0f;

        private readonly int codeOfWKey = 87;
        private readonly int codeOfLeftShiftKey = 160;
        private readonly int codeOfTildeKey = 192;
        private readonly int codeOfCapsLockKey = 20;
        private readonly int codeOfNumpad0Key = 96;

        private bool isNoclipEnabled = false;

        private void FreezeXYZVelocityOfPlayer()
        {
            memoryManager.ConvertFloatValueToBytes(0.0f).WriteBytesToAddress(baseAddressOfPlayer + 0x28, 4);
            memoryManager.ConvertFloatValueToBytes(0.0f).WriteBytesToAddress(baseAddressOfPlayer + 0x2C, 4);
            memoryManager.ConvertFloatValueToBytes(0.0f).WriteBytesToAddress(baseAddressOfPlayer + 0x30, 4);
        }

        private void ManageZCoordinateOfPlayer()
        {
            float zCoordinate = memoryManager.ReadBytesFromAddress(baseAddressOfPlayer + 0x24, 4).ConvertBytesToFloatValue();
            if (KeyboardManager.IsKeyPushedDown(codeOfTildeKey))
            {
                zCoordinate = zCoordinate + addendumForZCoordinate;
                memoryManager.ConvertFloatValueToBytes(zCoordinate).WriteBytesToAddress(baseAddressOfPlayer + 0x24, 4);
            }
            if (KeyboardManager.IsKeyPushedDown(codeOfCapsLockKey))
            {
                zCoordinate = zCoordinate - addendumForZCoordinate;
                memoryManager.ConvertFloatValueToBytes(zCoordinate).WriteBytesToAddress(baseAddressOfPlayer + 0x24, 4);
            }
        }

        private float GetAngleInRadiansFromAngleInDegrees(float angleInDegrees)
        {
            return Convert.ToSingle((Math.PI / 180) * angleInDegrees);
        }

        private void IncreaseMovementVelocityOfPlayer()
        {
            if (KeyboardManager.IsKeyPushedDown(codeOfLeftShiftKey))
            {
                multiplierOfPlayerSpeed = 250.0f;
                addendumForZCoordinate = 120.0f;
            }
            else
            {
                multiplierOfPlayerSpeed = 30.0f;
                addendumForZCoordinate = 20.0f;
            }
        }

        private void MovePlayer()
        {
            float yawAngleInDegrees = memoryManager.ReadBytesFromAddress(baseAddressOfPlayer + 0xE8, 4).ConvertBytesToFloatValue();
            float xCoordinate = memoryManager.ReadBytesFromAddress(baseAddressOfPlayer + 0x20, 4).ConvertBytesToFloatValue();
            float yCoordinate = memoryManager.ReadBytesFromAddress(baseAddressOfPlayer + 0x1C, 4).ConvertBytesToFloatValue();

            if (KeyboardManager.IsKeyPushedDown(codeOfWKey))
            {
                float sineOfYawAngle = Convert.ToSingle(Math.Sin(GetAngleInRadiansFromAngleInDegrees(yawAngleInDegrees)));
                float cosineOfYawAngle = Convert.ToSingle(Math.Cos(GetAngleInRadiansFromAngleInDegrees(yawAngleInDegrees)));

                float newXCoordinate = xCoordinate - ((sineOfYawAngle * -1) * multiplierOfPlayerSpeed);
                float newYCoordinate = yCoordinate + (cosineOfYawAngle * multiplierOfPlayerSpeed);

                memoryManager.ConvertFloatValueToBytes(newXCoordinate).WriteBytesToAddress(baseAddressOfPlayer + 0x20, 4);
                memoryManager.ConvertFloatValueToBytes(newYCoordinate).WriteBytesToAddress(baseAddressOfPlayer + 0x1C, 4);
            }
        }

        public COD4MW()
        {
            InitializeComponent();
            memoryManager = new MemoryManagerFor32BitProcesses();
        }

        private void timer_loopExecutor_Tick(object sender, EventArgs e)
        {
            baseAddressOfPlayer = memoryManager.ReadBytesFromAddress(0x00E18E18, 4).ConvertBytesToIntegerValue();
            if (isNoclipEnabled)
            {
                FreezeXYZVelocityOfPlayer();
                ManageZCoordinateOfPlayer();
                IncreaseMovementVelocityOfPlayer();
                MovePlayer();
            }
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
                memoryManager.FindProcessByNameThenAttachToIt("iw3sp");
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

        private void timer_hotkeyHandler_Tick(object sender, EventArgs e)
        {
            if (KeyboardManager.IsKeyPushedDown(codeOfNumpad0Key))
            {
                button_Noclip.PerformClick();
                Thread.Sleep(500);
            }
        }
    }
}
