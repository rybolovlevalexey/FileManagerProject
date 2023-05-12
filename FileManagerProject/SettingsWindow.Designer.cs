
namespace FileManagerProject
{
    partial class SettingsWindow
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
            this.label1 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.color_buttons_btn = new System.Windows.Forms.Button();
            this.black_theme_btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(15, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Размер шрифта:";
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(238, 30);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(171, 45);
            this.trackBar1.TabIndex = 1;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(181, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Мелкий";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(415, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "Крупный";
            // 
            // color_buttons_btn
            // 
            this.color_buttons_btn.BackColor = System.Drawing.SystemColors.ControlDark;
            this.color_buttons_btn.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.color_buttons_btn.Location = new System.Drawing.Point(12, 114);
            this.color_buttons_btn.Name = "color_buttons_btn";
            this.color_buttons_btn.Size = new System.Drawing.Size(457, 65);
            this.color_buttons_btn.TabIndex = 4;
            this.color_buttons_btn.Text = "Покраска кнопок";
            this.color_buttons_btn.UseVisualStyleBackColor = false;
            this.color_buttons_btn.Click += new System.EventHandler(this.color_buttons_btn_Click);
            // 
            // black_theme_btn
            // 
            this.black_theme_btn.BackColor = System.Drawing.SystemColors.ControlDark;
            this.black_theme_btn.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.black_theme_btn.Location = new System.Drawing.Point(12, 235);
            this.black_theme_btn.Name = "black_theme_btn";
            this.black_theme_btn.Size = new System.Drawing.Size(457, 68);
            this.black_theme_btn.TabIndex = 5;
            this.black_theme_btn.Text = "Тёмная тема";
            this.black_theme_btn.UseVisualStyleBackColor = false;
            this.black_theme_btn.Click += new System.EventHandler(this.black_theme_btn_Click);
            // 
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.Controls.Add(this.black_theme_btn);
            this.Controls.Add(this.color_buttons_btn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.label1);
            this.Name = "SettingsWindow";
            this.Text = "SettingsWindow";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button color_buttons_btn;
        private System.Windows.Forms.Button black_theme_btn;
    }
}