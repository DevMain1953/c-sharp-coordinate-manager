using System;
using System.Drawing;
using System.Windows.Forms;
using MemoryManagement;
using KeyboardManagement;

namespace CoordinateManager.Forms.GTAVC
{
    public partial class GTAVC : Form
    {
        private readonly MemoryManagerFor32BitProcesses memoryManager;
        private int baseAddressOfPlayer = 0;
        
        private int baseAddressOfCar = 0;
        private float multiplierOfCarSpeed = 0.01f;
        private float yawAngleInDegrees = 0.0f;

        private readonly int codeOfWKey = 87;
        private readonly int codeOfAKey = 65;
        private readonly int codeOfDKey = 68;
        private readonly int codeOfTildeKey = 192;
        private readonly int codeOfCapsLockKey = 20;
        private readonly int codeOfNumpad7Key = 103;

        private bool isFlyingCarEnabled = false;

        private void ManageZVelocityOfCar()
        {
            memoryManager.ConvertFloatValueToBytes(0.0f).WriteBytesToAddress(baseAddressOfCar + 0x78, 4);
            if (KeyboardManager.IsKeyPushedDown(codeOfTildeKey))
            {
                memoryManager.ConvertFloatValueToBytes(1.0f).WriteBytesToAddress(baseAddressOfCar + 0x78, 4);
            }
            if (KeyboardManager.IsKeyPushedDown(codeOfCapsLockKey))
            {
                memoryManager.ConvertFloatValueToBytes(-1.0f).WriteBytesToAddress(baseAddressOfCar + 0x78, 4);
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
            float xVelocityOfCar = memoryManager.ReadBytesFromAddress(baseAddressOfCar + 0x70, 4).ConvertBytesToFloatValue();
            float yVelocityOfCar = memoryManager.ReadBytesFromAddress(baseAddressOfCar + 0x74, 4).ConvertBytesToFloatValue();

            float speedOfCar = GetSpeedOfObject(xVelocityOfCar, yVelocityOfCar);
            if (KeyboardManager.IsKeyPushedDown(codeOfWKey))
            {
                if (speedOfCar < 1.8f)
                {
                    float newXVelocityOfCar = xVelocityOfCar - ((sineOfYawAngle * -1) * multiplierOfCarSpeed);
                    float newYVelocityOfCar = yVelocityOfCar + (cosineOfYawAngle * multiplierOfCarSpeed);

                    memoryManager.ConvertFloatValueToBytes(newXVelocityOfCar).WriteBytesToAddress(baseAddressOfCar + 0x70, 4);
                    memoryManager.ConvertFloatValueToBytes(newYVelocityOfCar).WriteBytesToAddress(baseAddressOfCar + 0x74, 4);
                }
            }
            else
            {
                memoryManager.ConvertFloatValueToBytes(0.0f).WriteBytesToAddress(baseAddressOfCar + 0x70, 4);
                memoryManager.ConvertFloatValueToBytes(0.0f).WriteBytesToAddress(baseAddressOfCar + 0x74, 4);
            }
        }

        private float GetSpeedOfObject(float xVelocity, float yVelocity)
        {
            double speedOfObject = Math.Sqrt((xVelocity * xVelocity) + (yVelocity * yVelocity));
            return Convert.ToSingle(speedOfObject);
        }

        public GTAVC()
        {
            InitializeComponent();
            memoryManager = new MemoryManagerFor32BitProcesses();
        }

        private void timer_loopExecutor_Tick(object sender, EventArgs e)
        {
            baseAddressOfPlayer = memoryManager.ReadBytesFromAddress(0x007E4B94, 4).ConvertBytesToIntegerValue();
            baseAddressOfCar = memoryManager.ReadBytesFromAddress(baseAddressOfPlayer + 0x3A8, 4).ConvertBytesToIntegerValue();
            if (isFlyingCarEnabled)
            {
                ManageZVelocityOfCar();
                FreezeRollAndPitchAngles();
                RotateCarAroundZAxis();
                MoveCar();
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
                memoryManager.FindProcessByNameThenAttachToIt("gta-vc");
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

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            memoryManager.DetachFromProcess();
        }
    }
}
