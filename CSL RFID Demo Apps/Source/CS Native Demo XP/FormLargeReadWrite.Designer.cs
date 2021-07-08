namespace CS203_CALLBACK_API_DEMO
{
    partial class FormLargeReadWrite
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
            this.labelEPC = new System.Windows.Forms.Label();
            this.textBox_EPC = new System.Windows.Forms.TextBox();
            this.label_DataPattern = new System.Windows.Forms.Label();
            this.comboBox_DataPattern = new System.Windows.Forms.ComboBox();
            this.label_Offset = new System.Windows.Forms.Label();
            this.label_Length = new System.Windows.Forms.Label();
            this.textBox_Offset = new System.Windows.Forms.TextBox();
            this.textBox_Length = new System.Windows.Forms.TextBox();
            this.button_Read = new System.Windows.Forms.Button();
            this.button_GeneralWrite = new System.Windows.Forms.Button();
            this.textBox_Data = new System.Windows.Forms.TextBox();
            this.button_BlockWrite = new System.Windows.Forms.Button();
            this.button_SelectTag = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelEPC
            // 
            this.labelEPC.AutoSize = true;
            this.labelEPC.Location = new System.Drawing.Point(149, 31);
            this.labelEPC.Name = "labelEPC";
            this.labelEPC.Size = new System.Drawing.Size(28, 13);
            this.labelEPC.TabIndex = 1;
            this.labelEPC.Text = "EPC";
            // 
            // textBox_EPC
            // 
            this.textBox_EPC.Location = new System.Drawing.Point(199, 28);
            this.textBox_EPC.Name = "textBox_EPC";
            this.textBox_EPC.Size = new System.Drawing.Size(301, 20);
            this.textBox_EPC.TabIndex = 2;
            // 
            // label_DataPattern
            // 
            this.label_DataPattern.AutoSize = true;
            this.label_DataPattern.Location = new System.Drawing.Point(149, 64);
            this.label_DataPattern.Name = "label_DataPattern";
            this.label_DataPattern.Size = new System.Drawing.Size(67, 13);
            this.label_DataPattern.TabIndex = 3;
            this.label_DataPattern.Text = "Data Pattern";
            // 
            // comboBox_DataPattern
            // 
            this.comboBox_DataPattern.FormattingEnabled = true;
            this.comboBox_DataPattern.Items.AddRange(new object[] {
            "55AA",
            "AA55",
            "0000",
            "FFFF",
            "0001",
            "0002",
            "0004",
            "0008",
            "0010",
            "0020",
            "0040",
            "0080",
            "0100",
            "0200",
            "0400",
            "0800",
            "1000",
            "2000",
            "4000",
            "8000",
            "FFFE",
            "FFFD",
            "FFFB",
            "FFF7",
            "FFEF",
            "FFDF",
            "FFBF",
            "FF7F",
            "FEFF",
            "FDFF",
            "FBFF",
            "F7FF",
            "EFFF",
            "DFFF",
            "BFFF",
            "7FFF"});
            this.comboBox_DataPattern.Location = new System.Drawing.Point(270, 61);
            this.comboBox_DataPattern.Name = "comboBox_DataPattern";
            this.comboBox_DataPattern.Size = new System.Drawing.Size(230, 21);
            this.comboBox_DataPattern.TabIndex = 4;
            this.comboBox_DataPattern.Text = "55AA";
            // 
            // label_Offset
            // 
            this.label_Offset.AutoSize = true;
            this.label_Offset.Location = new System.Drawing.Point(149, 97);
            this.label_Offset.Name = "label_Offset";
            this.label_Offset.Size = new System.Drawing.Size(35, 13);
            this.label_Offset.TabIndex = 5;
            this.label_Offset.Text = "Offset";
            // 
            // label_Length
            // 
            this.label_Length.AutoSize = true;
            this.label_Length.Location = new System.Drawing.Point(149, 134);
            this.label_Length.Name = "label_Length";
            this.label_Length.Size = new System.Drawing.Size(75, 13);
            this.label_Length.TabIndex = 6;
            this.label_Length.Text = "Length (Word)";
            // 
            // textBox_Offset
            // 
            this.textBox_Offset.Location = new System.Drawing.Point(270, 97);
            this.textBox_Offset.Name = "textBox_Offset";
            this.textBox_Offset.Size = new System.Drawing.Size(230, 20);
            this.textBox_Offset.TabIndex = 7;
            this.textBox_Offset.Text = "0";
            // 
            // textBox_Length
            // 
            this.textBox_Length.Location = new System.Drawing.Point(270, 131);
            this.textBox_Length.Name = "textBox_Length";
            this.textBox_Length.Size = new System.Drawing.Size(230, 20);
            this.textBox_Length.TabIndex = 8;
            this.textBox_Length.Text = "192";
            // 
            // button_Read
            // 
            this.button_Read.Location = new System.Drawing.Point(477, 295);
            this.button_Read.Name = "button_Read";
            this.button_Read.Size = new System.Drawing.Size(149, 54);
            this.button_Read.TabIndex = 9;
            this.button_Read.Text = "Read Verify";
            this.button_Read.UseVisualStyleBackColor = true;
            this.button_Read.Click += new System.EventHandler(this.button_Read_Click);
            // 
            // button_GeneralWrite
            // 
            this.button_GeneralWrite.Location = new System.Drawing.Point(167, 295);
            this.button_GeneralWrite.Name = "button_GeneralWrite";
            this.button_GeneralWrite.Size = new System.Drawing.Size(149, 54);
            this.button_GeneralWrite.TabIndex = 10;
            this.button_GeneralWrite.Text = "General Write";
            this.button_GeneralWrite.UseVisualStyleBackColor = true;
            this.button_GeneralWrite.Click += new System.EventHandler(this.button_GeneralWrite_Click);
            // 
            // textBox_Data
            // 
            this.textBox_Data.Location = new System.Drawing.Point(12, 189);
            this.textBox_Data.Multiline = true;
            this.textBox_Data.Name = "textBox_Data";
            this.textBox_Data.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Data.Size = new System.Drawing.Size(614, 100);
            this.textBox_Data.TabIndex = 12;
            // 
            // button_BlockWrite
            // 
            this.button_BlockWrite.Location = new System.Drawing.Point(322, 295);
            this.button_BlockWrite.Name = "button_BlockWrite";
            this.button_BlockWrite.Size = new System.Drawing.Size(149, 54);
            this.button_BlockWrite.TabIndex = 13;
            this.button_BlockWrite.Text = "Block Write";
            this.button_BlockWrite.UseVisualStyleBackColor = true;
            this.button_BlockWrite.Click += new System.EventHandler(this.button_BlockWrite_Click);
            // 
            // button_SelectTag
            // 
            this.button_SelectTag.Location = new System.Drawing.Point(12, 295);
            this.button_SelectTag.Name = "button_SelectTag";
            this.button_SelectTag.Size = new System.Drawing.Size(149, 54);
            this.button_SelectTag.TabIndex = 14;
            this.button_SelectTag.Text = "Select Tag";
            this.button_SelectTag.UseVisualStyleBackColor = true;
            this.button_SelectTag.Click += new System.EventHandler(this.button_SelectTag_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 173);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Read Data";
            // 
            // FormLargeReadWrite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 365);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button_SelectTag);
            this.Controls.Add(this.button_BlockWrite);
            this.Controls.Add(this.textBox_Data);
            this.Controls.Add(this.button_GeneralWrite);
            this.Controls.Add(this.button_Read);
            this.Controls.Add(this.textBox_Length);
            this.Controls.Add(this.textBox_Offset);
            this.Controls.Add(this.label_Length);
            this.Controls.Add(this.label_Offset);
            this.Controls.Add(this.comboBox_DataPattern);
            this.Controls.Add(this.label_DataPattern);
            this.Controls.Add(this.textBox_EPC);
            this.Controls.Add(this.labelEPC);
            this.Name = "FormLargeReadWrite";
            this.Text = "Read/Write User Bank with Large Data";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelEPC;
        private System.Windows.Forms.TextBox textBox_EPC;
        private System.Windows.Forms.Label label_DataPattern;
        private System.Windows.Forms.ComboBox comboBox_DataPattern;
        private System.Windows.Forms.Label label_Offset;
        private System.Windows.Forms.Label label_Length;
        private System.Windows.Forms.TextBox textBox_Offset;
        private System.Windows.Forms.TextBox textBox_Length;
        private System.Windows.Forms.Button button_Read;
        private System.Windows.Forms.Button button_GeneralWrite;
        private System.Windows.Forms.TextBox textBox_Data;
        private System.Windows.Forms.Button button_BlockWrite;
        private System.Windows.Forms.Button button_SelectTag;
        private System.Windows.Forms.Label label5;
    }
}