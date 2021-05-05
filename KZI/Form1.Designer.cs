namespace KZI
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonStart = new System.Windows.Forms.Button();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
            this.buttonDecode = new System.Windows.Forms.Button();
            this.buttonFilePath = new System.Windows.Forms.Button();
            this.textBoxFilePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonKeyPath = new System.Windows.Forms.Button();
            this.buttonPathIV = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(445, 164);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(132, 49);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Кодировать файл";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.Location = new System.Drawing.Point(52, 54);
            this.textBoxStatus.Multiline = true;
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxStatus.Size = new System.Drawing.Size(236, 259);
            this.textBoxStatus.TabIndex = 2;
            // 
            // buttonDecode
            // 
            this.buttonDecode.Location = new System.Drawing.Point(445, 219);
            this.buttonDecode.Name = "buttonDecode";
            this.buttonDecode.Size = new System.Drawing.Size(132, 45);
            this.buttonDecode.TabIndex = 3;
            this.buttonDecode.Text = "Декодировать файл";
            this.buttonDecode.UseVisualStyleBackColor = true;
            this.buttonDecode.Click += new System.EventHandler(this.buttonDecode_Click);
            // 
            // buttonFilePath
            // 
            this.buttonFilePath.Location = new System.Drawing.Point(485, 82);
            this.buttonFilePath.Name = "buttonFilePath";
            this.buttonFilePath.Size = new System.Drawing.Size(146, 24);
            this.buttonFilePath.TabIndex = 4;
            this.buttonFilePath.Text = "Выбрать файл";
            this.buttonFilePath.UseVisualStyleBackColor = true;
            this.buttonFilePath.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxFilePath
            // 
            this.textBoxFilePath.Location = new System.Drawing.Point(435, 54);
            this.textBoxFilePath.Name = "textBoxFilePath";
            this.textBoxFilePath.Size = new System.Drawing.Size(238, 22);
            this.textBoxFilePath.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(329, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Путь к файлу:";
            // 
            // buttonKeyPath
            // 
            this.buttonKeyPath.Location = new System.Drawing.Point(503, 355);
            this.buttonKeyPath.Name = "buttonKeyPath";
            this.buttonKeyPath.Size = new System.Drawing.Size(188, 23);
            this.buttonKeyPath.TabIndex = 7;
            this.buttonKeyPath.Text = "Выбрать файл ключа";
            this.buttonKeyPath.UseVisualStyleBackColor = true;
            this.buttonKeyPath.Click += new System.EventHandler(this.buttonKeyPath_Click);
            // 
            // buttonPathIV
            // 
            this.buttonPathIV.Location = new System.Drawing.Point(230, 355);
            this.buttonPathIV.Name = "buttonPathIV";
            this.buttonPathIV.Size = new System.Drawing.Size(248, 23);
            this.buttonPathIV.TabIndex = 8;
            this.buttonPathIV.Text = "Выбрать вектор инициализации";
            this.buttonPathIV.UseVisualStyleBackColor = true;
            this.buttonPathIV.Click += new System.EventHandler(this.buttonPathIV_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 407);
            this.Controls.Add(this.buttonPathIV);
            this.Controls.Add(this.buttonKeyPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxFilePath);
            this.Controls.Add(this.buttonFilePath);
            this.Controls.Add(this.buttonDecode);
            this.Controls.Add(this.textBoxStatus);
            this.Controls.Add(this.buttonStart);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.TextBox textBoxStatus;
        private System.Windows.Forms.Button buttonDecode;
        private System.Windows.Forms.Button buttonFilePath;
        private System.Windows.Forms.TextBox textBoxFilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonKeyPath;
        private System.Windows.Forms.Button buttonPathIV;
    }
}

