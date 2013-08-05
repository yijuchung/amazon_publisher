namespace csvchecker
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.logtext = new System.Windows.Forms.TextBox();
            this.choose = new System.Windows.Forms.Button();
            this.includeall = new System.Windows.Forms.CheckBox();
            this.check = new System.Windows.Forms.Button();
            this.cateBut = new System.Windows.Forms.Button();
            this.imgText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // logtext
            // 
            this.logtext.Location = new System.Drawing.Point(129, 13);
            this.logtext.Multiline = true;
            this.logtext.Name = "logtext";
            this.logtext.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logtext.Size = new System.Drawing.Size(448, 349);
            this.logtext.TabIndex = 0;
            // 
            // choose
            // 
            this.choose.Location = new System.Drawing.Point(13, 13);
            this.choose.Name = "choose";
            this.choose.Size = new System.Drawing.Size(89, 23);
            this.choose.TabIndex = 1;
            this.choose.Text = "choose CSV";
            this.choose.UseVisualStyleBackColor = true;
            this.choose.Click += new System.EventHandler(this.choose_Click);
            // 
            // includeall
            // 
            this.includeall.AutoSize = true;
            this.includeall.Location = new System.Drawing.Point(13, 43);
            this.includeall.Name = "includeall";
            this.includeall.Size = new System.Drawing.Size(110, 18);
            this.includeall.TabIndex = 2;
            this.includeall.Text = "包含同目錄底下";
            this.includeall.UseVisualStyleBackColor = true;
            // 
            // check
            // 
            this.check.Location = new System.Drawing.Point(13, 339);
            this.check.Name = "check";
            this.check.Size = new System.Drawing.Size(75, 23);
            this.check.TabIndex = 3;
            this.check.Text = "check";
            this.check.UseVisualStyleBackColor = true;
            this.check.Click += new System.EventHandler(this.check_Click);
            // 
            // cateBut
            // 
            this.cateBut.Location = new System.Drawing.Point(13, 68);
            this.cateBut.Name = "cateBut";
            this.cateBut.Size = new System.Drawing.Size(89, 23);
            this.cateBut.TabIndex = 4;
            this.cateBut.Text = "choose cate";
            this.cateBut.UseVisualStyleBackColor = true;
            this.cateBut.Click += new System.EventHandler(this.cateBut_Click);
            // 
            // imgText
            // 
            this.imgText.Location = new System.Drawing.Point(13, 98);
            this.imgText.Name = "imgText";
            this.imgText.Size = new System.Drawing.Size(100, 22);
            this.imgText.TabIndex = 5;
            this.imgText.Text = "(large)";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 374);
            this.Controls.Add(this.imgText);
            this.Controls.Add(this.cateBut);
            this.Controls.Add(this.check);
            this.Controls.Add(this.includeall);
            this.Controls.Add(this.choose);
            this.Controls.Add(this.logtext);
            this.Name = "Form1";
            this.Text = "CSV Checker by YJChung";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox logtext;
        private System.Windows.Forms.Button choose;
        private System.Windows.Forms.CheckBox includeall;
        private System.Windows.Forms.Button check;
        private System.Windows.Forms.Button cateBut;
        private System.Windows.Forms.TextBox imgText;
    }
}

