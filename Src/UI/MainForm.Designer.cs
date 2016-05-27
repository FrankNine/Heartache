namespace Heartache.UI
{
    partial class Heartache
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Heartache));
            this.buttonAssemble = new System.Windows.Forms.Button();
            this.buttonDisassemble = new System.Windows.Forms.Button();
            this.buttonBuildFontChunk = new System.Windows.Forms.Button();
            this.labelDataWinPath = new System.Windows.Forms.Label();
            this.labelDisassembledDataPath = new System.Windows.Forms.Label();
            this.labelTranslatedDataWinPath = new System.Windows.Forms.Label();
            this.buttonOpenDataWin = new System.Windows.Forms.Button();
            this.buttonOpenDisassembledDataPath = new System.Windows.Forms.Button();
            this.buttonOpenTranslatedDataWinPath = new System.Windows.Forms.Button();
            this.labelTranslationCSV = new System.Windows.Forms.Label();
            this.buttonOpenTranslationCSV = new System.Windows.Forms.Button();
            this.labelReplaceFontChunksPath = new System.Windows.Forms.Label();
            this.buttonOpenReplaceFontChunksPath = new System.Windows.Forms.Button();
            this.textBoxReplaceFontChunksPath = new System.Windows.Forms.TextBox();
            this.textBoxTranslationCSVPath = new System.Windows.Forms.TextBox();
            this.textBoxTranslatedDataWinPath = new System.Windows.Forms.TextBox();
            this.textBoxDisassembledDataPath = new System.Windows.Forms.TextBox();
            this.textBoxDataWinPath = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonAssemble
            // 
            this.buttonAssemble.Location = new System.Drawing.Point(12, 347);
            this.buttonAssemble.Name = "buttonAssemble";
            this.buttonAssemble.Size = new System.Drawing.Size(394, 23);
            this.buttonAssemble.TabIndex = 0;
            this.buttonAssemble.Text = "Assemble";
            this.buttonAssemble.UseVisualStyleBackColor = true;
            this.buttonAssemble.Click += new System.EventHandler(this.buttonAssemble_Click);
            // 
            // buttonDisassemble
            // 
            this.buttonDisassemble.Location = new System.Drawing.Point(12, 52);
            this.buttonDisassemble.Name = "buttonDisassemble";
            this.buttonDisassemble.Size = new System.Drawing.Size(394, 23);
            this.buttonDisassemble.TabIndex = 1;
            this.buttonDisassemble.Text = "Disassemble";
            this.buttonDisassemble.UseVisualStyleBackColor = true;
            this.buttonDisassemble.Click += new System.EventHandler(this.buttonDisassemble_Click);
            // 
            // buttonBuildFontChunk
            // 
            this.buttonBuildFontChunk.Location = new System.Drawing.Point(11, 194);
            this.buttonBuildFontChunk.Name = "buttonBuildFontChunk";
            this.buttonBuildFontChunk.Size = new System.Drawing.Size(395, 23);
            this.buttonBuildFontChunk.TabIndex = 2;
            this.buttonBuildFontChunk.Text = "Build Font Chunk";
            this.buttonBuildFontChunk.UseVisualStyleBackColor = true;
            // 
            // labelDataWinPath
            // 
            this.labelDataWinPath.AutoSize = true;
            this.labelDataWinPath.Location = new System.Drawing.Point(10, 9);
            this.labelDataWinPath.Name = "labelDataWinPath";
            this.labelDataWinPath.Size = new System.Drawing.Size(69, 12);
            this.labelDataWinPath.TabIndex = 3;
            this.labelDataWinPath.Text = "Data.win Path";
            // 
            // labelDisassembledDataPath
            // 
            this.labelDisassembledDataPath.AutoSize = true;
            this.labelDisassembledDataPath.Location = new System.Drawing.Point(10, 90);
            this.labelDisassembledDataPath.Name = "labelDisassembledDataPath";
            this.labelDisassembledDataPath.Size = new System.Drawing.Size(114, 12);
            this.labelDisassembledDataPath.TabIndex = 6;
            this.labelDisassembledDataPath.Text = "Disassembled Data Path";
            // 
            // labelTranslatedDataWinPath
            // 
            this.labelTranslatedDataWinPath.AutoSize = true;
            this.labelTranslatedDataWinPath.Location = new System.Drawing.Point(10, 373);
            this.labelTranslatedDataWinPath.Name = "labelTranslatedDataWinPath";
            this.labelTranslatedDataWinPath.Size = new System.Drawing.Size(120, 12);
            this.labelTranslatedDataWinPath.TabIndex = 7;
            this.labelTranslatedDataWinPath.Text = "Translated Data.win Path";
            // 
            // buttonOpenDataWin
            // 
            this.buttonOpenDataWin.Location = new System.Drawing.Point(412, 24);
            this.buttonOpenDataWin.Name = "buttonOpenDataWin";
            this.buttonOpenDataWin.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenDataWin.TabIndex = 9;
            this.buttonOpenDataWin.Text = "Open";
            this.buttonOpenDataWin.UseVisualStyleBackColor = true;
            this.buttonOpenDataWin.Click += new System.EventHandler(this.buttonOpenDataWin_Click);
            // 
            // buttonOpenDisassembledDataPath
            // 
            this.buttonOpenDisassembledDataPath.Location = new System.Drawing.Point(412, 109);
            this.buttonOpenDisassembledDataPath.Name = "buttonOpenDisassembledDataPath";
            this.buttonOpenDisassembledDataPath.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenDisassembledDataPath.TabIndex = 10;
            this.buttonOpenDisassembledDataPath.Text = "Open";
            this.buttonOpenDisassembledDataPath.UseVisualStyleBackColor = true;
            this.buttonOpenDisassembledDataPath.Click += new System.EventHandler(this.buttonOpenDisassembledDataPath_Click);
            // 
            // buttonOpenTranslatedDataWinPath
            // 
            this.buttonOpenTranslatedDataWinPath.Location = new System.Drawing.Point(412, 388);
            this.buttonOpenTranslatedDataWinPath.Name = "buttonOpenTranslatedDataWinPath";
            this.buttonOpenTranslatedDataWinPath.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenTranslatedDataWinPath.TabIndex = 11;
            this.buttonOpenTranslatedDataWinPath.Text = "Open";
            this.buttonOpenTranslatedDataWinPath.UseVisualStyleBackColor = true;
            this.buttonOpenTranslatedDataWinPath.Click += new System.EventHandler(this.buttonOpenTranslatedDataWinPath_Click);
            // 
            // labelTranslationCSV
            // 
            this.labelTranslationCSV.AutoSize = true;
            this.labelTranslationCSV.Location = new System.Drawing.Point(10, 134);
            this.labelTranslationCSV.Name = "labelTranslationCSV";
            this.labelTranslationCSV.Size = new System.Drawing.Size(105, 12);
            this.labelTranslationCSV.TabIndex = 12;
            this.labelTranslationCSV.Text = "Translation CSV Path";
            // 
            // buttonOpenTranslationCSV
            // 
            this.buttonOpenTranslationCSV.Location = new System.Drawing.Point(412, 149);
            this.buttonOpenTranslationCSV.Name = "buttonOpenTranslationCSV";
            this.buttonOpenTranslationCSV.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenTranslationCSV.TabIndex = 14;
            this.buttonOpenTranslationCSV.Text = "Open";
            this.buttonOpenTranslationCSV.UseVisualStyleBackColor = true;
            this.buttonOpenTranslationCSV.Click += new System.EventHandler(this.buttonOpenTranslationCSV_Click);
            // 
            // labelReplaceFontChunksPath
            // 
            this.labelReplaceFontChunksPath.AutoSize = true;
            this.labelReplaceFontChunksPath.Location = new System.Drawing.Point(12, 235);
            this.labelReplaceFontChunksPath.Name = "labelReplaceFontChunksPath";
            this.labelReplaceFontChunksPath.Size = new System.Drawing.Size(128, 12);
            this.labelReplaceFontChunksPath.TabIndex = 15;
            this.labelReplaceFontChunksPath.Text = "Replace Font Chunks Path";
            // 
            // buttonOpenReplaceFontChunksPath
            // 
            this.buttonOpenReplaceFontChunksPath.Location = new System.Drawing.Point(412, 248);
            this.buttonOpenReplaceFontChunksPath.Name = "buttonOpenReplaceFontChunksPath";
            this.buttonOpenReplaceFontChunksPath.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenReplaceFontChunksPath.TabIndex = 17;
            this.buttonOpenReplaceFontChunksPath.Text = "Open";
            this.buttonOpenReplaceFontChunksPath.UseVisualStyleBackColor = true;
            this.buttonOpenReplaceFontChunksPath.Click += new System.EventHandler(this.buttonOpenReplaceFontChunksPath_Click);
            // 
            // textBoxReplaceFontChunksPath
            // 
            this.textBoxReplaceFontChunksPath.Location = new System.Drawing.Point(12, 250);
            this.textBoxReplaceFontChunksPath.Name = "textBoxReplaceFontChunksPath";
            this.textBoxReplaceFontChunksPath.Size = new System.Drawing.Size(394, 22);
            this.textBoxReplaceFontChunksPath.TabIndex = 16;
            this.textBoxReplaceFontChunksPath.Text = global::Heartache.HeartacheSettings.Default.ReplaceFontChunksPath;
            this.textBoxReplaceFontChunksPath.TextChanged += new System.EventHandler(this.textBoxReplaceFontChunksPath_TextChanged);
            // 
            // textBoxTranslationCSVPath
            // 
            this.textBoxTranslationCSVPath.Location = new System.Drawing.Point(12, 151);
            this.textBoxTranslationCSVPath.Name = "textBoxTranslationCSVPath";
            this.textBoxTranslationCSVPath.Size = new System.Drawing.Size(394, 22);
            this.textBoxTranslationCSVPath.TabIndex = 13;
            this.textBoxTranslationCSVPath.Text = global::Heartache.HeartacheSettings.Default.TranslationCSVPath;
            this.textBoxTranslationCSVPath.TextChanged += new System.EventHandler(this.textBoxTranslationCSVPath_TextChanged);
            // 
            // textBoxTranslatedDataWinPath
            // 
            this.textBoxTranslatedDataWinPath.Location = new System.Drawing.Point(12, 388);
            this.textBoxTranslatedDataWinPath.Name = "textBoxTranslatedDataWinPath";
            this.textBoxTranslatedDataWinPath.Size = new System.Drawing.Size(394, 22);
            this.textBoxTranslatedDataWinPath.TabIndex = 8;
            this.textBoxTranslatedDataWinPath.Text = global::Heartache.HeartacheSettings.Default.TranslatedDataWinPath;
            this.textBoxTranslatedDataWinPath.TextChanged += new System.EventHandler(this.textBoxTranslatedDataWinPath_TextChanged);
            // 
            // textBoxDisassembledDataPath
            // 
            this.textBoxDisassembledDataPath.Location = new System.Drawing.Point(12, 109);
            this.textBoxDisassembledDataPath.Name = "textBoxDisassembledDataPath";
            this.textBoxDisassembledDataPath.Size = new System.Drawing.Size(394, 22);
            this.textBoxDisassembledDataPath.TabIndex = 5;
            this.textBoxDisassembledDataPath.Text = global::Heartache.HeartacheSettings.Default.DisassembledDataPath;
            this.textBoxDisassembledDataPath.TextChanged += new System.EventHandler(this.textBoxDisassembledDataPath_TextChanged);
            // 
            // textBoxDataWinPath
            // 
            this.textBoxDataWinPath.Location = new System.Drawing.Point(12, 24);
            this.textBoxDataWinPath.Name = "textBoxDataWinPath";
            this.textBoxDataWinPath.Size = new System.Drawing.Size(394, 22);
            this.textBoxDataWinPath.TabIndex = 4;
            this.textBoxDataWinPath.Text = global::Heartache.HeartacheSettings.Default.DataWinPath;
            this.textBoxDataWinPath.TextChanged += new System.EventHandler(this.textBoxDataWinPath_TextChanged);
            // 
            // Heartache
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 422);
            this.Controls.Add(this.buttonOpenReplaceFontChunksPath);
            this.Controls.Add(this.textBoxReplaceFontChunksPath);
            this.Controls.Add(this.labelReplaceFontChunksPath);
            this.Controls.Add(this.buttonOpenTranslationCSV);
            this.Controls.Add(this.textBoxTranslationCSVPath);
            this.Controls.Add(this.labelTranslationCSV);
            this.Controls.Add(this.buttonOpenTranslatedDataWinPath);
            this.Controls.Add(this.buttonOpenDisassembledDataPath);
            this.Controls.Add(this.buttonOpenDataWin);
            this.Controls.Add(this.textBoxTranslatedDataWinPath);
            this.Controls.Add(this.labelTranslatedDataWinPath);
            this.Controls.Add(this.labelDisassembledDataPath);
            this.Controls.Add(this.textBoxDisassembledDataPath);
            this.Controls.Add(this.textBoxDataWinPath);
            this.Controls.Add(this.labelDataWinPath);
            this.Controls.Add(this.buttonBuildFontChunk);
            this.Controls.Add(this.buttonDisassemble);
            this.Controls.Add(this.buttonAssemble);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Heartache";
            this.Text = "MainForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAssemble;
        private System.Windows.Forms.Button buttonDisassemble;
        private System.Windows.Forms.Button buttonBuildFontChunk;
        private System.Windows.Forms.Label labelDataWinPath;
        private System.Windows.Forms.TextBox textBoxDataWinPath;
        private System.Windows.Forms.TextBox textBoxDisassembledDataPath;
        private System.Windows.Forms.Label labelDisassembledDataPath;
        private System.Windows.Forms.Label labelTranslatedDataWinPath;
        private System.Windows.Forms.TextBox textBoxTranslatedDataWinPath;
        private System.Windows.Forms.Button buttonOpenDataWin;
        private System.Windows.Forms.Button buttonOpenDisassembledDataPath;
        private System.Windows.Forms.Button buttonOpenTranslatedDataWinPath;
        private System.Windows.Forms.Label labelTranslationCSV;
        private System.Windows.Forms.TextBox textBoxTranslationCSVPath;
        private System.Windows.Forms.Button buttonOpenTranslationCSV;
        private System.Windows.Forms.Label labelReplaceFontChunksPath;
        private System.Windows.Forms.TextBox textBoxReplaceFontChunksPath;
        private System.Windows.Forms.Button buttonOpenReplaceFontChunksPath;
    }
}