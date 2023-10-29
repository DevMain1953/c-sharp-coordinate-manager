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

namespace CoordinateManager.Forms.AmnesiaTDD
{
    public partial class AmnesiaTDD : Form
    {
        private readonly MemoryManagerFor32BitProcesses memoryManager;
        private readonly int[] offsetsInBytes = new int[1];
        private int baseAddressOfPlayer = 0;
        private int baseAddressOfPlayerBody = 0;
        private float multiplierOfPlayerSpeed = 0.5f;

        private readonly int codeOfWKey = 87;
        private readonly int codeOfTildeKey = 192;
        private readonly int codeOfCapsLockKey = 20;
        private readonly int codeOfNumpad0Key = 96;

        private bool isNoclipEnabled = false;

        private void FreezeXYZVelocityOfPlayer()
        {
            memoryManager.ConvertFloatValueToBytes(0.0f).WriteBytesToAddress(baseAddressOfPlayerBody + 0x88, 4);
            memoryManager.ConvertFloatValueToBytes(0.0f).WriteBytesToAddress(baseAddressOfPlayerBody + 0x8C, 4);
            memoryManager.ConvertFloatValueToBytes(0.0f).WriteBytesToAddress(baseAddressOfPlayerBody + 0xF4, 4);
        }

        private void ManageZCoordinateOfPlayer()
        {
            float zCoordinate = memoryManager.ReadBytesFromAddress(baseAddressOfPlayerBody + 0x4C, 4).ConvertBytesToFloatValue();
            if (KeyboardManager.IsKeyPushedDown(codeOfTildeKey))
            {
                zCoordinate = zCoordinate + 0.5f;
                memoryManager.ConvertFloatValueToBytes(zCoordinate).WriteBytesToAddress(baseAddressOfPlayerBody + 0x4C, 4);
            }
            if (KeyboardManager.IsKeyPushedDown(codeOfCapsLockKey))
            {
                zCoordinate = zCoordinate - 0.5f;
                memoryManager.ConvertFloatValueToBytes(zCoordinate).WriteBytesToAddress(baseAddressOfPlayerBody + 0x4C, 4);
            }
        }

        private void MovePlayer()
        {
            float sineOfYawAngle = memoryManager.ReadBytesFromAddress(baseAddressOfPlayerBody + 0x160, 4).ConvertBytesToFloatValue();
            float cosineOfYawAngle = memoryManager.ReadBytesFromAddress(baseAddressOfPlayerBody + 0x168, 4).ConvertBytesToFloatValue();
            float xCoordinate = memoryManager.ReadBytesFromAddress(baseAddressOfPlayerBody + 0x50, 4).ConvertBytesToFloatValue();
            float yCoordinate = memoryManager.ReadBytesFromAddress(baseAddressOfPlayerBody + 0x48, 4).ConvertBytesToFloatValue();

            if (KeyboardManager.IsKeyPushedDown(codeOfWKey))
            {
                float newXCoordinate = xCoordinate - (sineOfYawAngle * multiplierOfPlayerSpeed);
                float newYCoordinate = yCoordinate + (cosineOfYawAngle * multiplierOfPlayerSpeed);

                memoryManager.ConvertFloatValueToBytes(newXCoordinate).WriteBytesToAddress(baseAddressOfPlayerBody + 0x50, 4);
                memoryManager.ConvertFloatValueToBytes(newYCoordinate).WriteBytesToAddress(baseAddressOfPlayerBody + 0x48, 4);
            }
        }

        public AmnesiaTDD()
        {
            InitializeComponent();
            memoryManager = new MemoryManagerFor32BitProcesses();
            offsetsInBytes[0] = 0x84;
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
                memoryManager.FindProcessByNameThenAttachToIt("Amnesia");
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
            int pointerToBaseAddressOfPlayer = memoryManager.GetTargetAddressByPointersUsingOffsets(offsetsInBytes, 0x0073845C);
            baseAddressOfPlayer = memoryManager.ReadBytesFromAddress(pointerToBaseAddressOfPlayer, 4).ConvertBytesToIntegerValue();
            baseAddressOfPlayerBody = memoryManager.ReadBytesFromAddress(baseAddressOfPlayer + 0x54, 4).ConvertBytesToIntegerValue();
            if (isNoclipEnabled)
            {
                FreezeXYZVelocityOfPlayer();
                ManageZCoordinateOfPlayer();
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
