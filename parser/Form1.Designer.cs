namespace parser
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        public int iLast;
        public int iNow;
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.startB = new System.Windows.Forms.TextBox();
            this.endB = new System.Windows.Forms.TextBox();
            this.grabBut = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.anaBut = new System.Windows.Forms.Button();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 13);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(888, 480);
            this.textBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 509);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Input Start Book ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 541);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Input End Book ID";
            // 
            // startB
            // 
            this.startB.Location = new System.Drawing.Point(189, 509);
            this.startB.Name = "startB";
            this.startB.Size = new System.Drawing.Size(100, 22);
            this.startB.TabIndex = 3;
            this.startB.Text = "1";
            // 
            // endB
            // 
            this.endB.Location = new System.Drawing.Point(189, 546);
            this.endB.Name = "endB";
            this.endB.Size = new System.Drawing.Size(100, 22);
            this.endB.TabIndex = 4;
            this.endB.Text = "10";
            // 
            // grabBut
            // 
            this.grabBut.Location = new System.Drawing.Point(17, 578);
            this.grabBut.Name = "grabBut";
            this.grabBut.Size = new System.Drawing.Size(75, 23);
            this.grabBut.TabIndex = 5;
            this.grabBut.Text = "Start";
            this.grabBut.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(109, 578);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(180, 23);
            this.progressBar1.TabIndex = 6;
            // 
            // anaBut
            // 
            this.anaBut.Location = new System.Drawing.Point(17, 617);
            this.anaBut.Name = "anaBut";
            this.anaBut.Size = new System.Drawing.Size(75, 23);
            this.anaBut.TabIndex = 7;
            this.anaBut.Text = "Analyze";
            this.anaBut.UseVisualStyleBackColor = true;
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(109, 617);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(180, 23);
            this.progressBar2.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 676);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.anaBut);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.grabBut);
            this.Controls.Add(this.endB);
            this.Controls.Add(this.startB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox startB;
        private System.Windows.Forms.TextBox endB;
        private System.Windows.Forms.Button grabBut;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button anaBut;
        private System.Windows.Forms.ProgressBar progressBar2;
    }
}

