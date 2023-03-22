using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using MemoryManagement;
using KeyboardManagement;

namespace CoordinateManager.Forms.GTA3
{
    public partial class GTA3 : Form
    {
        private readonly MemoryManagerFor32BitProcesses memoryManager;
        private readonly int[] offsetsInBytes = new int[5];
        private int baseAddressOfPlayer = 0;
        
        private int baseAddressOfCar = 0;
        private float multiplierOfCarSpeed = 0.01f;
        private float multiplierOfPlayerSpeed = 1.0f;
        private float yawAngleInDegrees = 0.0f;

        private readonly int codeOfWKey = 87;
        private readonly int codeOfAKey = 65;
        private readonly int codeOfDKey = 68;
        private readonly int codeOfTildeKey = 192;
        private readonly int codeOfCapsLockKey = 20;
        private readonly int codeOfNumpad7Key = 103;
        private readonly int codeOfNumpad0Key = 96;

        private bool isFlyingCarEnabled = false;
        private bool isNoclipEnabled = false;

        private void ManageZVelocityOfCar()
        {
            memoryManager.ConvertFloatValueToBytes(0.0f).WriteBytesToAddress(baseAddressOfCar + 0x80, 4);
            if (KeyboardManager.IsKeyPushedDown(codeOfTildeKey))
            {
                memoryManager.ConvertFloatValueToBytes(1.0f).WriteBytesToAddress(baseAddressOfCar + 0x80, 4);
            }
            if (KeyboardManager.IsKeyPushedDown(codeOfCapsLockKey))
            {
                memoryManager.ConvertFloatValueToBytes(-1.0f).WriteBytesToAddress(baseAddressOfCar + 0x80, 4);
            }
        }

        private void FreezeRollAndPitchAngles()
        {
            memoryManager.ConvertFloatValueToBytes(0.0f).WriteBytesToAddress(baseAddressOfCar + 0xC, 4);
            memoryManager.ConvertFloatValueToBytes(0.0f).WriteBytesToAddress(baseAddressOfCar + 0x1C, 4);
        }

        private void ManageYawAngleOfCarToFlipItOver()
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

        private void RotateCarAroundZAxis()
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
            ManageYawAngleOfCarToFlipItOver();

            float newSinusOfYawAngle = Convert.ToSingle(Math.Sin(GetAngleInRadiansFromAngleInDegrees(yawAngleInDegrees)));
            float newCosineOfYawAngle = Convert.ToSingle(Math.Cos(GetAngleInRadiansFromAngleInDegrees(yawAngleInDegrees)));

            memoryManager.ConvertFloatValueToBytes(newSinusOfYawAngle).WriteBytesToAddress(baseAddressOfCar + 0x14, 4);
            memoryManager.ConvertFloatValueToBytes(newCosineOfYawAngle).WriteBytesToAddress(baseAddressOfCar + 0x18, 4);
        }

        private float GetAngleInRadiansFromAngleInDegrees(float angleInDegrees)
        {
            return Convert.ToSingle((Math.PI / 180) * angleInDegrees);
        }

        private void MoveCar()
        {
            float sineOfYawAngle = memoryManager.ReadBytesFromAddress(baseAddressOfCar + 0x14, 4).ConvertBytesToFloatValue();
            float cosineOfYawAngle = memoryManager.ReadBytesFromAddress(baseAddressOfCar + 0x18, 4).ConvertBytesToFloatValue();
            float xVelocityOfCar = memoryManager.ReadBytesFromAddress(baseAddressOfCar + 0x78, 4).ConvertBytesToFloatValue();
            float yVelocityOfCar = memoryManager.ReadBytesFromAddress(baseAddressOfCar + 0x7C, 4).ConvertBytesToFloatValue();

            float speedOfCar = GetSpeedOfObject(xVelocityOfCar, yVelocityOfCar);
            if (KeyboardManager.IsKeyPushedDown(codeOfWKey))
            {
                if (speedOfCar < 1.8f)
                {
                    float newXVelocityOfCar = xVelocityOfCar - ((sineOfYawAngle * -1) * multiplierOfCarSpeed);
                    float newYVelocityOfCar = yVelocityOfCar + (cosineOfYawAngle * multiplierOfCarSpeed);

                    memoryManager.ConvertFloatValueToBytes(newXVelocityOfCar).WriteBytesToAddress(baseAddressOfCar + 0x78, 4);
                    memoryManager.ConvertFloatValueToBytes(newYVelocityOfCar).WriteBytesToAddress(baseAddressOfCar + 0x7C, 4);
                }
            }
            else
            {
                memoryManager.ConvertFloatValueToBytes(0.0f).WriteBytesToAddress(baseAddressOfCar + 0x78, 4);
                memoryManager.ConvertFloatValueToBytes(0.0f).WriteBytesToAddress(baseAddressOfCar + 0x7C, 4);
            }
        }

        private void FreezeXYZVelocityOfPlayer()
        {
            memoryManager.ConvertFloatValueToBytes(0.0f).WriteBytesToAddress(baseAddressOfPlayer + 0x78, 4);
            memoryManager.ConvertFloatValueToBytes(0.0f).WriteBytesToAddress(baseAddressOfPlayer + 0x7C, 4);
            memoryManager.ConvertFloatValueToBytes(0.0f).WriteBytesToAddress(baseAddressOfPlayer + 0x80, 4);
        }

        private void ManageZCoordinateOfPlayer()
        {
            float zCoordinate = memoryManager.ReadBytesFromAddress(baseAddressOfPlayer + 0x3C, 4).ConvertBytesToFloatValue();
            if (KeyboardManager.IsKeyPushedDown(codeOfTildeKey))
            {
                zCoordinate = zCoordinate + 1.0f;
                memoryManager.ConvertFloatValueToBytes(zCoordinate).WriteBytesToAddress(baseAddressOfPlayer + 0x3C, 4);
            }
            if (KeyboardManager.IsKeyPushedDown(codeOfCapsLockKey))
            {
                zCoordinate = zCoordinate - 1.0f;
                memoryManager.ConvertFloatValueToBytes(zCoordinate).WriteBytesToAddress(baseAddressOfPlayer + 0x3C, 4);
            }
        }

        private void MovePlayer()
        {
            float sineOfYawAngle = memoryManager.ReadBytesFromAddress(baseAddressOfPlayer + 0x14, 4).ConvertBytesToFloatValue();
            float cosineOfYawAngle = memoryManager.ReadBytesFromAddress(baseAddressOfPlayer + 0x18, 4).ConvertBytesToFloatValue();
            float xCoordinate = memoryManager.ReadBytesFromAddress(baseAddressOfPlayer + 0x34, 4).ConvertBytesToFloatValue();
            float yCoordinate = memoryManager.ReadBytesFromAddress(baseAddressOfPlayer + 0x38, 4).ConvertBytesToFloatValue();

            if (KeyboardManager.IsKeyPushedDown(codeOfWKey))
            {
                float newXCoordinate = xCoordinate - ((sineOfYawAngle * -1) * multiplierOfPlayerSpeed);
                float newYCoordinate = yCoordinate + (cosineOfYawAngle * multiplierOfPlayerSpeed);

                memoryManager.ConvertFloatValueToBytes(newXCoordinate).WriteBytesToAddress(baseAddressOfPlayer + 0x34, 4);
                memoryManager.ConvertFloatValueToBytes(newYCoordinate).WriteBytesToAddress(baseAddressOfPlayer + 0x38, 4);
            }
        }

        private float GetSpeedOfObject(float xVelocity, float yVelocity)
        {
            double speedOfObject = Math.Sqrt((xVelocity * xVelocity) + (yVelocity * yVelocity));
            return Convert.ToSingle(speedOfObject);
        }

        public GTA3()
        {
            InitializeComponent();
            memoryManager = new MemoryManagerFor32BitProcesses();
        }

        private void timer_loopExecutor_Tick(object sender, EventArgs e)
        {
            baseAddressOfPlayer = memoryManager.ReadBytesFromAddress(0x006FB1C8, 4).ConvertBytesToIntegerValue();
            baseAddressOfCar = memoryManager.ReadBytesFromAddress(baseAddressOfPlayer + 0x310, 4).ConvertBytesToIntegerValue();
            if (isFlyingCarEnabled)
            {
                ManageZVelocityOfCar();
                FreezeRollAndPitchAngles();
                RotateCarAroundZAxis();
                MoveCar();
            }
            if (isNoclipEnabled)
            {
                FreezeXYZVelocityOfPlayer();
                ManageZCoordinateOfPlayer();
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
                memoryManager.FindProcessByNameThenAttachToIt("gta3");
                timer_loopExecutor.Enabled = true;

                button_OpenCloseProcess.ForeColor = Color.Red;
                button_OpenCloseProcess.Text = "Close process handle";
                label_ProcessHandle.Text = "Process handle: " + memoryManager.GetProcessHandle();
            }
        }

        private void button_FlyingCar_Click(object sender, EventArgs e)
        {
            if (isFlyingCarEnabled)
            {
                isFlyingCarEnabled = false;
                button_FlyingCar.ForeColor = Color.Black;
                button_FlyingCar.Text = "ON";
            }
            else
            {
                isFlyingCarEnabled = true;
                button_FlyingCar.ForeColor = Color.Red;
                button_FlyingCar.Text = "OFF";
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

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            memoryManager.DetachFromProcess();
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
