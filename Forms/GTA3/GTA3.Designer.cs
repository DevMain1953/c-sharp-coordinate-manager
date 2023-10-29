
namespace CoordinateManager.Forms.GTA3
{
    partial class GTA3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button_OpenCloseProcess = new System.Windows.Forms.Button();
            this.label_ProcessHandle = new System.Windows.Forms.Label();
            this.timer_loopExecutor = new System.Windows.Forms.Timer(this.components);
            this.button_FlyingCar = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.button_Noclip = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.timer_hotkeyHandler = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // button_OpenCloseProcess
            // 
            this.button_OpenCloseProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_OpenCloseProcess.Location = new System.Drawing.Point(12, 31);
            this.button_OpenCloseProcess.Name = "button_OpenCloseProcess";
            this.button_OpenCloseProcess.Size = new System.Drawing.Size(171, 33);
            this.button_OpenCloseProcess.TabIndex = 0;
            this.button_OpenCloseProcess.Text = "Open process";
            this.button_OpenCloseProcess.UseVisualStyleBackColor = true;
            this.button_OpenCloseProcess.Click += new System.EventHandler(this.button_OpenCloseProcess_Click);
            // 
            // label_ProcessHandle
            // 
            this.label_ProcessHandle.AutoSize = true;
            this.label_ProcessHandle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_ProcessHandle.Location = new System.Drawing.Point(189, 39);
            this.label_ProcessHandle.Name = "label_ProcessHandle";
            this.label_ProcessHandle.Size = new System.Drawing.Size(110, 17);
            this.label_ProcessHandle.TabIndex = 1;
            this.label_ProcessHandle.Text = "Process handle:";
            // 
            // timer_loopExecutor
            // 
            this.timer_loopExecutor.Interval = 1;
            this.timer_loopExecutor.Tick += new System.EventHandler(this.timer_loopExecutor_Tick);
            // 
            // button_FlyingCar
            // 
            this.button_FlyingCar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_FlyingCar.Location = new System.Drawing.Point(12, 70);
            this.button_FlyingCar.Name = "button_FlyingCar";
            this.button_FlyingCar.Size = new System.Drawing.Size(61, 33);
            this.button_FlyingCar.TabIndex = 9;
            this.button_FlyingCar.Text = "ON";
            this.button_FlyingCar.UseVisualStyleBackColor = true;
            this.button_FlyingCar.Click += new System.EventHandler(this.button_FlyingCar_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(79, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "Flying car";
            // 
            // button_Noclip
            // 
            this.button_Noclip.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_Noclip.Location = new System.Drawing.Point(12, 109);
            this.button_Noclip.Name = "button_Noclip";
            this.button_Noclip.Size = new System.Drawing.Size(61, 33);
            this.button_Noclip.TabIndex = 11;
            this.button_Noclip.Text = "ON";
            this.button_Noclip.UseVisualStyleBackColor = true;
            this.button_Noclip.Click += new System.EventHandler(this.button_Noclip_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(79, 117);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 17);
            this.label1.TabIndex = 10;
            this.label1.Text = "Noclip";
            // 
            // timer_hotkeyHandler
            // 
            this.timer_hotkeyHandler.Enabled = true;
            this.timer_hotkeyHandler.Interval = 1;
            this.timer_hotkeyHandler.Tick += new System.EventHandler(this.timer_hotkeyHandler_Tick);
            // 
            // GTA3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 450);
            this.Controls.Add(this.button_Noclip);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_FlyingCar);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label_ProcessHandle);
            this.Controls.Add(this.button_OpenCloseProcess);
            this.MaximumSize = new System.Drawing.Size(356, 489);
            this.MinimumSize = new System.Drawing.Size(356, 489);
            this.Name = "GTA3";
            this.Text = "GTA3";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_OpenCloseProcess;
        private System.Windows.Forms.Label label_ProcessHandle;
        private System.Windows.Forms.Timer timer_loopExecutor;
        private System.Windows.Forms.Button button_FlyingCar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_Noclip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer_hotkeyHandler;
    }
}

