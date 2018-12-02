namespace LD43.LevelEditor
{
    partial class Form1
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.paintComboBox = new System.Windows.Forms.ComboBox();
            this.tileLabel = new System.Windows.Forms.Label();
            this.altPaintComboBox = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.levelWindow1 = new LD43.LevelEditor.LevelWindow();
            this.modeComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.modeComboBox);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.paintComboBox);
            this.splitContainer1.Panel1.Controls.Add(this.tileLabel);
            this.splitContainer1.Panel1.Controls.Add(this.altPaintComboBox);
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.levelWindow1);
            this.splitContainer1.Size = new System.Drawing.Size(1904, 1041);
            this.splitContainer1.SplitterDistance = 287;
            this.splitContainer1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 638);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Paint:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // paintComboBox
            // 
            this.paintComboBox.FormattingEnabled = true;
            this.paintComboBox.Location = new System.Drawing.Point(12, 654);
            this.paintComboBox.Name = "paintComboBox";
            this.paintComboBox.Size = new System.Drawing.Size(260, 21);
            this.paintComboBox.TabIndex = 5;
            this.paintComboBox.SelectedIndexChanged += new System.EventHandler(this.paintComboBox_SelectedIndexChanged);
            // 
            // tileLabel
            // 
            this.tileLabel.AutoSize = true;
            this.tileLabel.Location = new System.Drawing.Point(9, 961);
            this.tileLabel.Name = "tileLabel";
            this.tileLabel.Size = new System.Drawing.Size(27, 13);
            this.tileLabel.TabIndex = 4;
            this.tileLabel.Text = "Tile:";
            // 
            // altPaintComboBox
            // 
            this.altPaintComboBox.FormattingEnabled = true;
            this.altPaintComboBox.Location = new System.Drawing.Point(12, 697);
            this.altPaintComboBox.Name = "altPaintComboBox";
            this.altPaintComboBox.Size = new System.Drawing.Size(259, 21);
            this.altPaintComboBox.TabIndex = 3;
            this.altPaintComboBox.SelectedIndexChanged += new System.EventHandler(this.tileComboBox_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 977);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(272, 52);
            this.button1.TabIndex = 2;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "json";
            this.saveFileDialog1.FileName = "room.json";
            this.saveFileDialog1.Filter = "JSON Files|*.json";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.SelectedPath = "C:\\Users\\srako\\Desktop\\LD43\\LD43.DesktopGL\\Content";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 678);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Alt Paint";
            // 
            // levelWindow1
            // 
            this.levelWindow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.levelWindow1.Location = new System.Drawing.Point(0, 0);
            this.levelWindow1.Name = "levelWindow1";
            this.levelWindow1.Size = new System.Drawing.Size(1613, 1041);
            this.levelWindow1.TabIndex = 0;
            this.levelWindow1.Text = "levelWindow1";
            this.levelWindow1.TexturesToLoad = null;
            this.levelWindow1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.levelWindow1_MouseClick);
            // 
            // modeComboBox
            // 
            this.modeComboBox.FormattingEnabled = true;
            this.modeComboBox.Location = new System.Drawing.Point(16, 25);
            this.modeComboBox.Name = "modeComboBox";
            this.modeComboBox.Size = new System.Drawing.Size(256, 21);
            this.modeComboBox.TabIndex = 8;
            this.modeComboBox.SelectedIndexChanged += new System.EventHandler(this.modeComboBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Mode";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private LevelWindow levelWindow1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label tileLabel;
        private System.Windows.Forms.ComboBox altPaintComboBox;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox paintComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox modeComboBox;
    }
}

