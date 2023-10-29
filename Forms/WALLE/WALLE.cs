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

namespace CoordinateManager.Forms.WALLE
{
    public partial class WALLE : Form
    {
        private readonly MemoryManagerFor32BitProcesses memoryManager;
        private readonly int[] offsetsInBytes = new int[4];
        private int baseAddressOfPlayer = 0;
        private int baseAddressOfPlayerBody = 0;
        private float multiplierOfPlayerSpeed = 0.5f;
        private float yawAngleInDegrees = 0.0f;

        private readonly int codeOfWKey = 87;
        private readonly int codeOfAKey = 65;
        private readonly int codeOfDKey = 68;
        private readonly int codeOfTildeKey = 192;
        private readonly int codeOfCapsLockKey = 20;
        private readonly int codeOfNumpad7Key = 103;
        private readonly int codeOfNumpad0Key = 96;

        private bool isNoclipEnabled = false;

        private void FreezeXYZVelocityOfPlayer()
        {
            memoryManager.ConvertFloatValueToBytes(0.0f).WriteBytesToAddress(baseAddressOfPlayerBody + 0x30, 4);
            memoryManager.ConvertFloatValueToBytes(0.0f).WriteBytesToAddress(baseAddressOfPlayerBody + 0x34, 4);
            memoryManager.ConvertFloatValueToBytes(0.0f).WriteBytesToAddress(baseAddressOfPlayerBody + 0x38, 4);
        }

        private void ManageZCoordinateOfPlayer()
        {
            float zCoordinate = memoryManager.ReadBytesFromAddress(baseAddressOfPlayerBody + 0x14, 4).ConvertBytesToFloatValue();
            if (KeyboardManager.IsKeyPushedDown(codeOfTildeKey))
            {
                zCoordinate = zCoordinate + 0.5f;
                memoryManager.ConvertFloatValueToBytes(zCoordinate).WriteBytesToAddress(baseAddressOfPlayerBody + 0x14, 4);
            }
            if (KeyboardManager.IsKeyPushedDown(codeOfCapsLockKey))
            {
                zCoordinate = zCoordinate - 0.5f;
                memoryManager.ConvertFloatValueToBytes(zCoordinate).WriteBytesToAddress(baseAddressOfPlayerBody + 0x14, 4);
            }
        }

        private void ManageYawAngleOfPlayerToFlipItOver()
        {
            if (KeyboardManager.IsKeyPushedDown(codeOfNumpad7Key))
            {
                if (yawAngleInDegrees != 90)
                {
                    yawAngleInDegrees = 90;
                }
                else
                {
                    yawAngleInDegrees = -90;
                }
            }
        }

        private void RotatePlayerAroundZAxis()
        {
            if (KeyboardManager.IsKeyPushedDown(codeOfDKey))
            {
                if (yawAngleInDegrees > 180)
                {
                    yawAngleInDegrees = yawAngleInDegrees * -1;
                }
                yawAngleInDegrees = yawAngleInDegrees + (1 * Convert.ToSingle(1.0));
            }

            if (KeyboardManager.IsKeyPushedDown(codeOfAKey))
            {
                if (yawAngleInDegrees < -180)
                {
                    yawAngleInDegrees = yawAngleInDegrees * -1;
                }
                yawAngleInDegrees = yawAngleInDegrees - (1 * Convert.ToSingle(1.0));
            }
            ManageYawAngleOfPlayerToFlipItOver();

            float newSinusOfYawAngle = Convert.ToSingle(Math.Sin(GetAngleInRadiansFromAngleInDegrees(yawAngleInDegrees)));
            float newCosineOfYawAngle = Convert.ToSingle(Math.Cos(GetAngleInRadiansFromAngleInDegrees(yawAngleInDegrees)));

            memoryManager.ConvertFloatValueToBytes(newSinusOfYawAngle).WriteBytesToAddress(baseAddressOfPlayerBody + 0xC, 4);
            memoryManager.ConvertFloatValueToBytes(newCosineOfYawAngle).WriteBytesToAddress(baseAddressOfPlayerBody + 0x4, 4);
        }

        private float GetAngleInRadiansFromAngleInDegrees(float angleInDegrees)
        {
            return Convert.ToSingle((Math.PI / 180) * angleInDegrees);
        }

        private void MovePlayer()
        {
            float sineOfYawAngle = memoryManager.ReadBytesFromAddress(baseAddressOfPlayerBody + 0x4, 4).ConvertBytesToFloatValue();
            float cosineOfYawAngle = memoryManager.ReadBytesFromAddress(baseAddressOfPlayerBody + 0xC, 4).ConvertBytesToFloatValue();
            float xCoordinate = memoryManager.ReadBytesFromAddress(baseAddressOfPlayerBody + 0x18, 4).ConvertBytesToFloatValue();
            float yCoordinate = memoryManager.ReadBytesFromAddress(baseAddressOfPlayerBody + 0x10, 4).ConvertBytesToFloatValue();

            if (KeyboardManager.IsKeyPushedDown(codeOfWKey))
            {
                float newXCoordinate = xCoordinate - (sineOfYawAngle * multiplierOfPlayerSpeed);
                float newYCoordinate = yCoordinate + (cosineOfYawAngle * multiplierOfPlayerSpeed);

                memoryManager.ConvertFloatValueToBytes(newXCoordinate).WriteBytesToAddress(baseAddressOfPlayerBody + 0x18, 4);
                memoryManager.ConvertFloatValueToBytes(newYCoordinate).WriteBytesToAddress(baseAddressOfPlayerBody + 0x10, 4);
            }
        }

        public WALLE()
        {
            InitializeComponent();
            memoryManager = new MemoryManagerFor32BitProcesses();
            offsetsInBytes[0] = 0x54;
            offsetsInBytes[1] = 0xDC;
            offsetsInBytes[2] = 0xC;
            offsetsInBytes[3] = 0xCC;
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
                memoryManager.FindProcessByNameThenAttachToIt("wall-e");
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
            int pointerToBaseAddressOfPlayer = memoryManager.GetTargetAddressByPointersUsingOffsets(offsetsInBytes, 0x0092E760);
            baseAddressOfPlayer = memoryManager.ReadBytesFromAddress(pointerToBaseAddressOfPlayer, 4).ConvertBytesToIntegerValue();
            baseAddressOfPlayerBody = baseAddressOfPlayer + 0x948;
            if (isNoclipEnabled)
            {
                FreezeXYZVelocityOfPlayer();
                ManageZCoordinateOfPlayer();
                RotatePlayerAroundZAxis();
                MovePlayer();
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
