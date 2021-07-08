namespace CS203_CALLBACK_API_DEMO
{
    partial class FormOnOffTest
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.labelTime = new System.Windows.Forms.Label();
            this.timerReaderOn = new System.Windows.Forms.Timer(this.components);
            this.timerReaderOff = new System.Windows.Forms.Timer(this.components);
            this.labelCycle = new System.Windows.Forms.Label();
            this.labelTagsperSecond = new System.Windows.Forms.Label();
            this.timerTime = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.labelReaderStatus = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBoxSavetoLog = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxLogFile = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.labelStartDate = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.labelNonZeroCount = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.labelZeroCount = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.labelDisconnectCount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(275, 40);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(80, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(88, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Reader On Period (minutes):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(88, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Reader On Time (seconds):";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(275, 69);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(80, 20);
            this.textBox2.TabIndex = 2;
            this.textBox2.Text = "40";
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(172, 132);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(126, 25);
            this.buttonStart.TabIndex = 4;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new System.Drawing.Point(380, 195);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(13, 13);
            this.labelTime.TabIndex = 5;
            this.labelTime.Text = "0";
            this.labelTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // timerReaderOn
            // 
            this.timerReaderOn.Tick += new System.EventHandler(this.timerReaderOn_Tick);
            // 
            // timerReaderOff
            // 
            this.timerReaderOff.Tick += new System.EventHandler(this.timerReaderOff_Tick);
            // 
            // labelCycle
            // 
            this.labelCycle.AutoSize = true;
            this.labelCycle.Location = new System.Drawing.Point(380, 283);
            this.labelCycle.Name = "labelCycle";
            this.labelCycle.Size = new System.Drawing.Size(13, 13);
            this.labelCycle.TabIndex = 6;
            this.labelCycle.Text = "0";
            this.labelCycle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelTagsperSecond
            // 
            this.labelTagsperSecond.AutoSize = true;
            this.labelTagsperSecond.Location = new System.Drawing.Point(380, 261);
            this.labelTagsperSecond.Name = "labelTagsperSecond";
            this.labelTagsperSecond.Size = new System.Drawing.Size(13, 13);
            this.labelTagsperSecond.TabIndex = 7;
            this.labelTagsperSecond.Text = "0";
            this.labelTagsperSecond.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // timerTime
            // 
            this.timerTime.Interval = 1000;
            this.timerTime.Tick += new System.EventHandler(this.timerTime_Tick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(59, 173);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Reader Status";
            // 
            // labelReaderStatus
            // 
            this.labelReaderStatus.AutoSize = true;
            this.labelReaderStatus.Location = new System.Drawing.Point(140, 173);
            this.labelReaderStatus.Name = "labelReaderStatus";
            this.labelReaderStatus.Size = new System.Drawing.Size(0, 13);
            this.labelReaderStatus.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(300, 195);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Elapsed Time:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(289, 261);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Tag per second:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(264, 283);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Reader On/Off Cycle:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // checkBoxSavetoLog
            // 
            this.checkBoxSavetoLog.AutoSize = true;
            this.checkBoxSavetoLog.Checked = true;
            this.checkBoxSavetoLog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSavetoLog.Location = new System.Drawing.Point(245, 98);
            this.checkBoxSavetoLog.Name = "checkBoxSavetoLog";
            this.checkBoxSavetoLog.Size = new System.Drawing.Size(15, 14);
            this.checkBoxSavetoLog.TabIndex = 13;
            this.checkBoxSavetoLog.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(88, 98);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Save to Tag Log File";
            // 
            // textBoxLogFile
            // 
            this.textBoxLogFile.Location = new System.Drawing.Point(275, 95);
            this.textBoxLogFile.Name = "textBoxLogFile";
            this.textBoxLogFile.Size = new System.Drawing.Size(172, 20);
            this.textBoxLogFile.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(242, 173);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(132, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Start Running Date/Time: ";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelStartDate
            // 
            this.labelStartDate.AutoSize = true;
            this.labelStartDate.Location = new System.Drawing.Point(380, 173);
            this.labelStartDate.Name = "labelStartDate";
            this.labelStartDate.Size = new System.Drawing.Size(13, 13);
            this.labelStartDate.TabIndex = 16;
            this.labelStartDate.Text = "0";
            this.labelStartDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(266, 217);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(108, 13);
            this.label10.TabIndex = 21;
            this.label10.Text = "Non-Zero Tag Count:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelNonZeroCount
            // 
            this.labelNonZeroCount.AutoSize = true;
            this.labelNonZeroCount.Location = new System.Drawing.Point(380, 217);
            this.labelNonZeroCount.Name = "labelNonZeroCount";
            this.labelNonZeroCount.Size = new System.Drawing.Size(13, 13);
            this.labelNonZeroCount.TabIndex = 20;
            this.labelNonZeroCount.Text = "0";
            this.labelNonZeroCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(289, 239);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(85, 13);
            this.label12.TabIndex = 19;
            this.label12.Text = "Zero Tag Count:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelZeroCount
            // 
            this.labelZeroCount.AutoSize = true;
            this.labelZeroCount.Location = new System.Drawing.Point(380, 239);
            this.labelZeroCount.Name = "labelZeroCount";
            this.labelZeroCount.Size = new System.Drawing.Size(13, 13);
            this.labelZeroCount.TabIndex = 18;
            this.labelZeroCount.Text = "0";
            this.labelZeroCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(279, 305);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 13);
            this.label9.TabIndex = 23;
            this.label9.Text = "Disconnect Count:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelDisconnectCount
            // 
            this.labelDisconnectCount.AutoSize = true;
            this.labelDisconnectCount.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelDisconnectCount.Location = new System.Drawing.Point(380, 305);
            this.labelDisconnectCount.Name = "labelDisconnectCount";
            this.labelDisconnectCount.Size = new System.Drawing.Size(13, 13);
            this.labelDisconnectCount.TabIndex = 22;
            this.labelDisconnectCount.Text = "0";
            this.labelDisconnectCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // FormOnOffTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 335);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.labelDisconnectCount);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.labelNonZeroCount);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.labelZeroCount);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.labelStartDate);
            this.Controls.Add(this.textBoxLogFile);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.checkBoxSavetoLog);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelReaderStatus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelTagsperSecond);
            this.Controls.Add(this.labelCycle);
            this.Controls.Add(this.labelTime);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Name = "FormOnOffTest";
            this.Text = "On\\Off Test";
            this.Load += new System.EventHandler(this.FormOnOffTest_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormOnOffTest_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Timer timerReaderOn;
        private System.Windows.Forms.Timer timerReaderOff;
        private System.Windows.Forms.Label labelCycle;
        private System.Windows.Forms.Label labelTagsperSecond;
        private System.Windows.Forms.Timer timerTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelReaderStatus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBoxSavetoLog;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxLogFile;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelStartDate;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label labelNonZeroCount;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelZeroCount;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelDisconnectCount;
    }
}