namespace CS203_CALLBACK_API_DEMO
{
    partial class FormPerformanceTest
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
            this.timerMonitor = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.buttonStart = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxRunningTime = new System.Windows.Forms.TextBox();
            this.textBoxTotalTags = new System.Windows.Forms.TextBox();
            this.textBoxTagsPerSecond = new System.Windows.Forms.TextBox();
            this.textBoxNewTags = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBoxCountwithTID = new System.Windows.Forms.CheckBox();
            this.textBoxminRSSIEPC = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxminRSSIdBm = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxminRSSIdBmv = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerMonitor
            // 
            this.timerMonitor.Interval = 1000;
            this.timerMonitor.Tick += new System.EventHandler(this.timerMonitor_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(46, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Running Time";
            // 
            // buttonStart
            // 
            this.buttonStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStart.Location = new System.Drawing.Point(246, 403);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(155, 47);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(46, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 29);
            this.label2.TabIndex = 2;
            this.label2.Text = "Total Tags";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(46, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 29);
            this.label3.TabIndex = 3;
            this.label3.Text = "Tags /s";
            // 
            // textBoxRunningTime
            // 
            this.textBoxRunningTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRunningTime.Location = new System.Drawing.Point(281, 21);
            this.textBoxRunningTime.Name = "textBoxRunningTime";
            this.textBoxRunningTime.Size = new System.Drawing.Size(219, 35);
            this.textBoxRunningTime.TabIndex = 4;
            // 
            // textBoxTotalTags
            // 
            this.textBoxTotalTags.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTotalTags.Location = new System.Drawing.Point(281, 57);
            this.textBoxTotalTags.Name = "textBoxTotalTags";
            this.textBoxTotalTags.Size = new System.Drawing.Size(219, 35);
            this.textBoxTotalTags.TabIndex = 5;
            // 
            // textBoxTagsPerSecond
            // 
            this.textBoxTagsPerSecond.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTagsPerSecond.Location = new System.Drawing.Point(281, 95);
            this.textBoxTagsPerSecond.Name = "textBoxTagsPerSecond";
            this.textBoxTagsPerSecond.Size = new System.Drawing.Size(219, 35);
            this.textBoxTagsPerSecond.TabIndex = 6;
            // 
            // textBoxNewTags
            // 
            this.textBoxNewTags.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxNewTags.Location = new System.Drawing.Point(281, 133);
            this.textBoxNewTags.Name = "textBoxNewTags";
            this.textBoxNewTags.Size = new System.Drawing.Size(219, 35);
            this.textBoxNewTags.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(46, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 29);
            this.label4.TabIndex = 7;
            this.label4.Text = "New Tags";
            // 
            // checkBoxCountwithTID
            // 
            this.checkBoxCountwithTID.AutoSize = true;
            this.checkBoxCountwithTID.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCountwithTID.Location = new System.Drawing.Point(28, 411);
            this.checkBoxCountwithTID.Name = "checkBoxCountwithTID";
            this.checkBoxCountwithTID.Size = new System.Drawing.Size(189, 33);
            this.checkBoxCountwithTID.TabIndex = 9;
            this.checkBoxCountwithTID.Text = "Count with TID";
            this.checkBoxCountwithTID.UseVisualStyleBackColor = true;
            // 
            // textBoxminRSSIEPC
            // 
            this.textBoxminRSSIEPC.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxminRSSIEPC.Location = new System.Drawing.Point(107, 21);
            this.textBoxminRSSIEPC.Name = "textBoxminRSSIEPC";
            this.textBoxminRSSIEPC.Size = new System.Drawing.Size(342, 35);
            this.textBoxminRSSIEPC.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(17, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 29);
            this.label5.TabIndex = 11;
            this.label5.Text = "EPC";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.textBoxminRSSIdBm);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textBoxminRSSIdBmv);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textBoxminRSSIEPC);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(51, 192);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(486, 127);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Minimum RSSI information";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(402, 71);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 29);
            this.label8.TabIndex = 16;
            this.label8.Text = "dBm";
            // 
            // textBoxminRSSIdBm
            // 
            this.textBoxminRSSIdBm.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxminRSSIdBm.Location = new System.Drawing.Point(299, 68);
            this.textBoxminRSSIdBm.Name = "textBoxminRSSIdBm";
            this.textBoxminRSSIdBm.Size = new System.Drawing.Size(103, 35);
            this.textBoxminRSSIdBm.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(211, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 29);
            this.label7.TabIndex = 14;
            this.label7.Text = "dBμV";
            // 
            // textBoxminRSSIdBmv
            // 
            this.textBoxminRSSIdBmv.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxminRSSIdBmv.Location = new System.Drawing.Point(107, 68);
            this.textBoxminRSSIdBmv.Name = "textBoxminRSSIdBmv";
            this.textBoxminRSSIdBmv.Size = new System.Drawing.Size(103, 35);
            this.textBoxminRSSIdBmv.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(17, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 29);
            this.label6.TabIndex = 12;
            this.label6.Text = "RSSI";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Controls.Add(this.textBoxNewTags);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBoxTagsPerSecond);
            this.groupBox2.Controls.Add(this.textBoxTotalTags);
            this.groupBox2.Controls.Add(this.textBoxRunningTime);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(28, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(597, 368);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Measurement Data";
            // 
            // FormPerformanceTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 489);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.checkBoxCountwithTID);
            this.Controls.Add(this.buttonStart);
            this.Name = "FormPerformanceTest";
            this.Text = "FormPerformanceTest";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerMonitor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxRunningTime;
        private System.Windows.Forms.TextBox textBoxTotalTags;
        private System.Windows.Forms.TextBox textBoxTagsPerSecond;
        private System.Windows.Forms.TextBox textBoxNewTags;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBoxCountwithTID;
        private System.Windows.Forms.TextBox textBoxminRSSIEPC;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxminRSSIdBm;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxminRSSIdBmv;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}