namespace CS203_CALLBACK_API_DEMO
{
    partial class TagRFMicroForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        protected new bool IsDisposed = false;
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
            base.Dispose(IsDisposed = disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.btn_readallbank = new System.Windows.Forms.Button();
            this.lb_ReadInfo = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox_tagID = new System.Windows.Forms.CheckBox();
            this.checkBox_calData = new System.Windows.Forms.CheckBox();
            this.checkBox_OCRSSI = new System.Windows.Forms.CheckBox();
            this.checkBox_temp = new System.Windows.Forms.CheckBox();
            this.textBox_tagID = new System.Windows.Forms.TextBox();
            this.textBox_calData = new System.Windows.Forms.TextBox();
            this.textBox_OCRSSI = new System.Windows.Forms.TextBox();
            this.textBox_temp = new System.Windows.Forms.TextBox();
            this.checkBox_sensorCode = new System.Windows.Forms.CheckBox();
            this.textBox_sensorCode = new System.Windows.Forms.TextBox();
            this.label_EPC = new System.Windows.Forms.Label();
            this.textBox_selectedEPC = new System.Windows.Forms.TextBox();
            this.label_ocrssi = new System.Windows.Forms.Label();
            this.label_Temp = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tb_entertag = new Custom.Control.HexOnlyTextbox();
            this.lb_clear = new System.Windows.Forms.LinkLabel();
            this.btn_search = new System.Windows.Forms.Button();
            this.nTable1 = new CSLibrary.Windows.UI.NTable();
            this.tc_readAndWrite = new System.Windows.Forms.TabControl();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tc_readAndWrite.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(4, 153);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(60, 24);
            this.button1.TabIndex = 0;
            this.button1.Text = "USER";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button2.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button2.Location = new System.Drawing.Point(4, 123);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(60, 24);
            this.button2.TabIndex = 0;
            this.button2.Text = "TID";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button3.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button3.Location = new System.Drawing.Point(4, 93);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(60, 24);
            this.button3.TabIndex = 0;
            this.button3.Text = "EPC";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button4.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button4.Location = new System.Drawing.Point(4, 63);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(60, 24);
            this.button4.TabIndex = 0;
            this.button4.Text = "PC";
            this.button4.UseVisualStyleBackColor = false;
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button5.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button5.Location = new System.Drawing.Point(4, 33);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(60, 24);
            this.button5.TabIndex = 0;
            this.button5.Text = "ACC";
            this.button5.UseVisualStyleBackColor = false;
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button6.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button6.Location = new System.Drawing.Point(4, 3);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(60, 24);
            this.button6.TabIndex = 0;
            this.button6.Text = "KILL";
            this.button6.UseVisualStyleBackColor = false;
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.tabPage4.Controls.Add(this.label1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(465, 330);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Exit";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial Unicode MS", 48F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.label1.Location = new System.Drawing.Point(3, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(306, 146);
            this.label1.TabIndex = 0;
            this.label1.Text = "TODO";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tabPage5.Controls.Add(this.groupBox1);
            this.tabPage5.Controls.Add(this.lb_ReadInfo);
            this.tabPage5.Controls.Add(this.btn_readallbank);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(465, 330);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "2.Read Tag";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // btn_readallbank
            // 
            this.btn_readallbank.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btn_readallbank.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btn_readallbank.Location = new System.Drawing.Point(3, 186);
            this.btn_readallbank.Name = "btn_readallbank";
            this.btn_readallbank.Size = new System.Drawing.Size(72, 20);
            this.btn_readallbank.TabIndex = 1;
            this.btn_readallbank.Text = "Read";
            this.btn_readallbank.UseVisualStyleBackColor = false;
            this.btn_readallbank.Click += new System.EventHandler(this.btn_readallbank_Click);
            // 
            // lb_ReadInfo
            // 
            this.lb_ReadInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lb_ReadInfo.Location = new System.Drawing.Point(81, 186);
            this.lb_ReadInfo.Name = "lb_ReadInfo";
            this.lb_ReadInfo.Size = new System.Drawing.Size(228, 20);
            this.lb_ReadInfo.TabIndex = 0;
            this.lb_ReadInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lb_ReadInfo.Click += new System.EventHandler(this.lb_ReadInfo_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_Temp);
            this.groupBox1.Controls.Add(this.label_ocrssi);
            this.groupBox1.Controls.Add(this.textBox_selectedEPC);
            this.groupBox1.Controls.Add(this.label_EPC);
            this.groupBox1.Controls.Add(this.textBox_sensorCode);
            this.groupBox1.Controls.Add(this.checkBox_sensorCode);
            this.groupBox1.Controls.Add(this.textBox_temp);
            this.groupBox1.Controls.Add(this.textBox_OCRSSI);
            this.groupBox1.Controls.Add(this.textBox_calData);
            this.groupBox1.Controls.Add(this.textBox_tagID);
            this.groupBox1.Controls.Add(this.checkBox_temp);
            this.groupBox1.Controls.Add(this.checkBox_OCRSSI);
            this.groupBox1.Controls.Add(this.checkBox_calData);
            this.groupBox1.Controls.Add(this.checkBox_tagID);
            this.groupBox1.Location = new System.Drawing.Point(3, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(305, 186);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // checkBox_tagID
            // 
            this.checkBox_tagID.AutoSize = true;
            this.checkBox_tagID.Checked = true;
            this.checkBox_tagID.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_tagID.Location = new System.Drawing.Point(17, 38);
            this.checkBox_tagID.Name = "checkBox_tagID";
            this.checkBox_tagID.Size = new System.Drawing.Size(56, 17);
            this.checkBox_tagID.TabIndex = 0;
            this.checkBox_tagID.Text = "TagID";
            this.checkBox_tagID.UseVisualStyleBackColor = true;
            // 
            // checkBox_calData
            // 
            this.checkBox_calData.AutoSize = true;
            this.checkBox_calData.Checked = true;
            this.checkBox_calData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_calData.Location = new System.Drawing.Point(17, 64);
            this.checkBox_calData.Name = "checkBox_calData";
            this.checkBox_calData.Size = new System.Drawing.Size(67, 17);
            this.checkBox_calData.TabIndex = 1;
            this.checkBox_calData.Text = "Cal Data";
            this.checkBox_calData.UseVisualStyleBackColor = true;
            // 
            // checkBox_OCRSSI
            // 
            this.checkBox_OCRSSI.AutoSize = true;
            this.checkBox_OCRSSI.Checked = true;
            this.checkBox_OCRSSI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_OCRSSI.Location = new System.Drawing.Point(17, 116);
            this.checkBox_OCRSSI.Name = "checkBox_OCRSSI";
            this.checkBox_OCRSSI.Size = new System.Drawing.Size(69, 17);
            this.checkBox_OCRSSI.TabIndex = 2;
            this.checkBox_OCRSSI.Text = "OC RSSI";
            this.checkBox_OCRSSI.UseVisualStyleBackColor = true;
            // 
            // checkBox_temp
            // 
            this.checkBox_temp.AutoSize = true;
            this.checkBox_temp.Checked = true;
            this.checkBox_temp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_temp.Location = new System.Drawing.Point(17, 142);
            this.checkBox_temp.Name = "checkBox_temp";
            this.checkBox_temp.Size = new System.Drawing.Size(53, 17);
            this.checkBox_temp.TabIndex = 3;
            this.checkBox_temp.Text = "Temp";
            this.checkBox_temp.UseVisualStyleBackColor = true;
            // 
            // textBox_tagID
            // 
            this.textBox_tagID.Location = new System.Drawing.Point(98, 36);
            this.textBox_tagID.Name = "textBox_tagID";
            this.textBox_tagID.Size = new System.Drawing.Size(176, 20);
            this.textBox_tagID.TabIndex = 4;
            // 
            // textBox_calData
            // 
            this.textBox_calData.Location = new System.Drawing.Point(98, 62);
            this.textBox_calData.Name = "textBox_calData";
            this.textBox_calData.Size = new System.Drawing.Size(176, 20);
            this.textBox_calData.TabIndex = 5;
            // 
            // textBox_OCRSSI
            // 
            this.textBox_OCRSSI.Location = new System.Drawing.Point(98, 116);
            this.textBox_OCRSSI.Name = "textBox_OCRSSI";
            this.textBox_OCRSSI.Size = new System.Drawing.Size(176, 20);
            this.textBox_OCRSSI.TabIndex = 6;
            // 
            // textBox_temp
            // 
            this.textBox_temp.Location = new System.Drawing.Point(98, 140);
            this.textBox_temp.Name = "textBox_temp";
            this.textBox_temp.Size = new System.Drawing.Size(176, 20);
            this.textBox_temp.TabIndex = 7;
            // 
            // checkBox_sensorCode
            // 
            this.checkBox_sensorCode.AutoSize = true;
            this.checkBox_sensorCode.Checked = true;
            this.checkBox_sensorCode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_sensorCode.Location = new System.Drawing.Point(17, 90);
            this.checkBox_sensorCode.Name = "checkBox_sensorCode";
            this.checkBox_sensorCode.Size = new System.Drawing.Size(87, 17);
            this.checkBox_sensorCode.TabIndex = 8;
            this.checkBox_sensorCode.Text = "Sensor Code";
            this.checkBox_sensorCode.UseVisualStyleBackColor = true;
            // 
            // textBox_sensorCode
            // 
            this.textBox_sensorCode.Location = new System.Drawing.Point(98, 88);
            this.textBox_sensorCode.Name = "textBox_sensorCode";
            this.textBox_sensorCode.Size = new System.Drawing.Size(176, 20);
            this.textBox_sensorCode.TabIndex = 9;
            // 
            // label_EPC
            // 
            this.label_EPC.AutoSize = true;
            this.label_EPC.Location = new System.Drawing.Point(6, 12);
            this.label_EPC.Name = "label_EPC";
            this.label_EPC.Size = new System.Drawing.Size(67, 13);
            this.label_EPC.TabIndex = 10;
            this.label_EPC.Text = "Slected EPC";
            // 
            // textBox_selectedEPC
            // 
            this.textBox_selectedEPC.Location = new System.Drawing.Point(79, 9);
            this.textBox_selectedEPC.Name = "textBox_selectedEPC";
            this.textBox_selectedEPC.Size = new System.Drawing.Size(220, 20);
            this.textBox_selectedEPC.TabIndex = 11;
            // 
            // label_ocrssi
            // 
            this.label_ocrssi.AutoSize = true;
            this.label_ocrssi.Location = new System.Drawing.Point(52, 162);
            this.label_ocrssi.Name = "label_ocrssi";
            this.label_ocrssi.Size = new System.Drawing.Size(50, 13);
            this.label_ocrssi.TabIndex = 12;
            this.label_ocrssi.Text = "OC RSSI";
            // 
            // label_Temp
            // 
            this.label_Temp.AutoSize = true;
            this.label_Temp.Location = new System.Drawing.Point(145, 162);
            this.label_Temp.Name = "label_Temp";
            this.label_Temp.Size = new System.Drawing.Size(34, 13);
            this.label_Temp.TabIndex = 13;
            this.label_Temp.Text = "Temp";
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.tabPage1.Controls.Add(this.nTable1);
            this.tabPage1.Controls.Add(this.btn_search);
            this.tabPage1.Controls.Add(this.lb_clear);
            this.tabPage1.Controls.Add(this.tb_entertag);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(465, 330);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "1.Select Tag";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tb_entertag
            // 
            this.tb_entertag.BackColor = System.Drawing.Color.LightGreen;
            this.tb_entertag.BackgroundText = "Please type your EPC here!";
            this.tb_entertag.FontColor = System.Drawing.Color.Black;
            this.tb_entertag.ForeColor = System.Drawing.Color.Gray;
            this.tb_entertag.Location = new System.Drawing.Point(81, 287);
            this.tb_entertag.MaxLength = 24;
            this.tb_entertag.Name = "tb_entertag";
            this.tb_entertag.PaddingZero = true;
            this.tb_entertag.Size = new System.Drawing.Size(189, 20);
            this.tb_entertag.TabIndex = 0;
            this.tb_entertag.TextChanged += new System.EventHandler(this.tb_entertag_TextChanged);
            // 
            // lb_clear
            // 
            this.lb_clear.Location = new System.Drawing.Point(276, 290);
            this.lb_clear.Name = "lb_clear";
            this.lb_clear.Size = new System.Drawing.Size(33, 20);
            this.lb_clear.TabIndex = 2;
            this.lb_clear.TabStop = true;
            this.lb_clear.Text = "CLR";
            this.lb_clear.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lb_clear_LinkClicked);
            this.lb_clear.Click += new System.EventHandler(this.lb_clear_Click);
            // 
            // btn_search
            // 
            this.btn_search.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btn_search.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btn_search.Location = new System.Drawing.Point(3, 279);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(72, 43);
            this.btn_search.TabIndex = 3;
            this.btn_search.Text = "Search";
            this.btn_search.UseVisualStyleBackColor = false;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // nTable1
            // 
            this.nTable1.AllowColumnResize = false;
            this.nTable1.AltBackColor = System.Drawing.Color.Khaki;
            this.nTable1.AltForeColor = System.Drawing.Color.Black;
            this.nTable1.AutoColumnSize = true;
            this.nTable1.AutoMoveRow = true;
            this.nTable1.BackColor = System.Drawing.Color.White;
            this.nTable1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.nTable1.ColumnBackColor = System.Drawing.Color.Chocolate;
            this.nTable1.ColumnFont = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.nTable1.ColumnForeColor = System.Drawing.Color.White;
            this.nTable1.DefaultLineAligment = System.Drawing.StringAlignment.Center;
            this.nTable1.DefaultRowHeight = 20;
            this.nTable1.DefaultTextAligment = System.Drawing.StringAlignment.Center;
            this.nTable1.DrawGridBorder = true;
            this.nTable1.FocusCellBackColor = System.Drawing.Color.Black;
            this.nTable1.FocusCellForeColor = System.Drawing.Color.White;
            this.nTable1.LeftHeader = false;
            this.nTable1.Location = new System.Drawing.Point(3, 3);
            this.nTable1.MultipleSelection = false;
            this.nTable1.Name = "nTable1";
            this.nTable1.SelectionBackColor = System.Drawing.Color.DarkOrange;
            this.nTable1.SelectionForeColor = System.Drawing.Color.Black;
            this.nTable1.ShowSplitterValue = true;
            this.nTable1.ShowStartSplitter = true;
            this.nTable1.Size = new System.Drawing.Size(454, 237);
            this.nTable1.SplitterColor = System.Drawing.Color.Red;
            this.nTable1.SplitterMode = CSLibrary.Windows.UI.NTableSplitterMode.Default;
            this.nTable1.SplitterStartColor = System.Drawing.Color.Brown;
            this.nTable1.SplitterWidth = 1;
            this.nTable1.TabIndex = 4;
            this.nTable1.Text = "nTable1";
            this.nTable1.RowChanged += new CSLibrary.Windows.UI.NTableRowHandler(this.nTable1_RowChanged);
            // 
            // tc_readAndWrite
            // 
            this.tc_readAndWrite.Controls.Add(this.tabPage1);
            this.tc_readAndWrite.Controls.Add(this.tabPage5);
            this.tc_readAndWrite.Controls.Add(this.tabPage4);
            this.tc_readAndWrite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tc_readAndWrite.Location = new System.Drawing.Point(0, 0);
            this.tc_readAndWrite.Name = "tc_readAndWrite";
            this.tc_readAndWrite.SelectedIndex = 0;
            this.tc_readAndWrite.Size = new System.Drawing.Size(473, 356);
            this.tc_readAndWrite.TabIndex = 0;
            this.tc_readAndWrite.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // TagRFMicroForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.ClientSize = new System.Drawing.Size(473, 356);
            this.ControlBox = false;
            this.Controls.Add(this.tc_readAndWrite);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TagRFMicroForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NewReadWriteForm";
            this.Load += new System.EventHandler(this.NewReadWriteForm_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.NewReadWriteForm_Closing);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tc_readAndWrite.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label_Temp;
        private System.Windows.Forms.Label label_ocrssi;
        private System.Windows.Forms.TextBox textBox_selectedEPC;
        private System.Windows.Forms.Label label_EPC;
        private System.Windows.Forms.TextBox textBox_sensorCode;
        private System.Windows.Forms.CheckBox checkBox_sensorCode;
        private System.Windows.Forms.TextBox textBox_temp;
        private System.Windows.Forms.TextBox textBox_OCRSSI;
        private System.Windows.Forms.TextBox textBox_calData;
        private System.Windows.Forms.TextBox textBox_tagID;
        private System.Windows.Forms.CheckBox checkBox_temp;
        private System.Windows.Forms.CheckBox checkBox_OCRSSI;
        private System.Windows.Forms.CheckBox checkBox_calData;
        private System.Windows.Forms.CheckBox checkBox_tagID;
        private System.Windows.Forms.Label lb_ReadInfo;
        private System.Windows.Forms.Button btn_readallbank;
        private System.Windows.Forms.TabPage tabPage1;
        private CSLibrary.Windows.UI.NTable nTable1;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.LinkLabel lb_clear;
        private Custom.Control.HexOnlyTextbox tb_entertag;
        private System.Windows.Forms.TabControl tc_readAndWrite;
    }
}