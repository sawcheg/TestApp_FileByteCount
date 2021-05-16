
namespace WinFormTestApp_FileByteCount
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.tbPath = new System.Windows.Forms.TextBox();
            this.btnSetDirectory = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbPath
            // 
            this.tbPath.Location = new System.Drawing.Point(12, 28);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(558, 20);
            this.tbPath.TabIndex = 1;
            // 
            // btnSetDirectory
            // 
            this.btnSetDirectory.Location = new System.Drawing.Point(568, 26);
            this.btnSetDirectory.Name = "btnSetDirectory";
            this.btnSetDirectory.Size = new System.Drawing.Size(25, 23);
            this.btnSetDirectory.TabIndex = 2;
            this.btnSetDirectory.Text = "...";
            this.btnSetDirectory.UseVisualStyleBackColor = true;
            this.btnSetDirectory.Click += new System.EventHandler(this.btnSetDirectory_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(599, 25);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(133, 23);
            this.btnRun.TabIndex = 3;
            this.btnRun.Text = "Выполнить";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // tbResult
            // 
            this.tbResult.Location = new System.Drawing.Point(12, 54);
            this.tbResult.Multiline = true;
            this.tbResult.Name = "tbResult";
            this.tbResult.Size = new System.Drawing.Size(720, 180);
            this.tbResult.TabIndex = 4;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 320);
            this.Controls.Add(this.tbResult);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnSetDirectory);
            this.Controls.Add(this.tbPath);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "Тестовое приложение";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.Button btnSetDirectory;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TextBox tbResult;
    }
}

