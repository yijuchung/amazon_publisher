namespace parser
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
            this.components = new System.ComponentModel.Container();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.openFile = new System.Windows.Forms.Button();
            this.startLogin = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.publish = new System.Windows.Forms.Button();
            this.username = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timeLimit = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.timeInterval = new System.Windows.Forms.TextBox();
            this.subNameRand = new System.Windows.Forms.CheckBox();
            this.introRand = new System.Windows.Forms.CheckBox();
            this.keyWordRand = new System.Windows.Forms.CheckBox();
            this.numKeyword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.bookNameRand = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.totalBooks = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.startNum = new System.Windows.Forms.TextBox();
            this.multiUser = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(165, 399);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(655, 181);
            this.textBox1.TabIndex = 0;
            // 
            // openFile
            // 
            this.openFile.Location = new System.Drawing.Point(74, 12);
            this.openFile.Name = "openFile";
            this.openFile.Size = new System.Drawing.Size(75, 23);
            this.openFile.TabIndex = 1;
            this.openFile.Text = "OpenFile";
            this.openFile.UseVisualStyleBackColor = true;
            this.openFile.Click += new System.EventHandler(this.openFile_Click);
            // 
            // startLogin
            // 
            this.startLogin.Location = new System.Drawing.Point(15, 152);
            this.startLogin.Name = "startLogin";
            this.startLogin.Size = new System.Drawing.Size(38, 38);
            this.startLogin.TabIndex = 3;
            this.startLogin.Text = "Login";
            this.startLogin.UseVisualStyleBackColor = true;
            this.startLogin.Click += new System.EventHandler(this.startLogin_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(165, 13);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(21, 21);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(655, 380);
            this.webBrowser1.TabIndex = 4;
            // 
            // publish
            // 
            this.publish.Location = new System.Drawing.Point(57, 152);
            this.publish.Name = "publish";
            this.publish.Size = new System.Drawing.Size(51, 38);
            this.publish.TabIndex = 5;
            this.publish.Text = "Publish";
            this.publish.UseVisualStyleBackColor = true;
            this.publish.Click += new System.EventHandler(this.publish_Click);
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(15, 40);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(145, 22);
            this.username.TabIndex = 6;
            this.username.Text = "username";
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(15, 69);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(145, 22);
            this.password.TabIndex = 7;
            this.password.Text = "pass";
            this.password.UseSystemPasswordChar = true;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timeLimit
            // 
            this.timeLimit.Location = new System.Drawing.Point(15, 123);
            this.timeLimit.Name = "timeLimit";
            this.timeLimit.Size = new System.Drawing.Size(145, 22);
            this.timeLimit.TabIndex = 8;
            this.timeLimit.Text = "300";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "Time Limit (s, 0 = 無限)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 195);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "Time interval";
            // 
            // timeInterval
            // 
            this.timeInterval.Location = new System.Drawing.Point(15, 213);
            this.timeInterval.Name = "timeInterval";
            this.timeInterval.Size = new System.Drawing.Size(145, 22);
            this.timeInterval.TabIndex = 11;
            this.timeInterval.Text = "30";
            // 
            // subNameRand
            // 
            this.subNameRand.AutoSize = true;
            this.subNameRand.Checked = true;
            this.subNameRand.CheckState = System.Windows.Forms.CheckState.Checked;
            this.subNameRand.Location = new System.Drawing.Point(15, 242);
            this.subNameRand.Name = "subNameRand";
            this.subNameRand.Size = new System.Drawing.Size(60, 16);
            this.subNameRand.TabIndex = 12;
            this.subNameRand.Text = "副標題";
            this.subNameRand.UseVisualStyleBackColor = true;
            // 
            // introRand
            // 
            this.introRand.AutoSize = true;
            this.introRand.Checked = true;
            this.introRand.CheckState = System.Windows.Forms.CheckState.Checked;
            this.introRand.Location = new System.Drawing.Point(15, 264);
            this.introRand.Name = "introRand";
            this.introRand.Size = new System.Drawing.Size(48, 16);
            this.introRand.TabIndex = 13;
            this.introRand.Text = "摘要";
            this.introRand.UseVisualStyleBackColor = true;
            // 
            // keyWordRand
            // 
            this.keyWordRand.AutoSize = true;
            this.keyWordRand.Checked = true;
            this.keyWordRand.CheckState = System.Windows.Forms.CheckState.Checked;
            this.keyWordRand.Location = new System.Drawing.Point(97, 264);
            this.keyWordRand.Name = "keyWordRand";
            this.keyWordRand.Size = new System.Drawing.Size(60, 16);
            this.keyWordRand.TabIndex = 14;
            this.keyWordRand.Text = "關鍵字";
            this.keyWordRand.UseVisualStyleBackColor = true;
            // 
            // numKeyword
            // 
            this.numKeyword.Location = new System.Drawing.Point(65, 287);
            this.numKeyword.Name = "numKeyword";
            this.numKeyword.Size = new System.Drawing.Size(43, 22);
            this.numKeyword.TabIndex = 15;
            this.numKeyword.Text = "7";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 289);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 12);
            this.label3.TabIndex = 16;
            this.label3.Text = "Keyword";
            // 
            // bookNameRand
            // 
            this.bookNameRand.AutoSize = true;
            this.bookNameRand.Location = new System.Drawing.Point(97, 242);
            this.bookNameRand.Name = "bookNameRand";
            this.bookNameRand.Size = new System.Drawing.Size(48, 16);
            this.bookNameRand.TabIndex = 17;
            this.bookNameRand.Text = "書名";
            this.bookNameRand.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(63, 311);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 12);
            this.label4.TabIndex = 18;
            this.label4.Text = "Max";
            // 
            // totalBooks
            // 
            this.totalBooks.Location = new System.Drawing.Point(65, 328);
            this.totalBooks.Name = "totalBooks";
            this.totalBooks.Size = new System.Drawing.Size(41, 22);
            this.totalBooks.TabIndex = 19;
            this.totalBooks.Text = "40";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 311);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 12);
            this.label5.TabIndex = 20;
            this.label5.Text = "Start";
            // 
            // startNum
            // 
            this.startNum.Location = new System.Drawing.Point(15, 328);
            this.startNum.Name = "startNum";
            this.startNum.Size = new System.Drawing.Size(43, 22);
            this.startNum.TabIndex = 21;
            this.startNum.Text = "1";
            // 
            // multiUser
            // 
            this.multiUser.AutoSize = true;
            this.multiUser.Location = new System.Drawing.Point(15, 16);
            this.multiUser.Name = "multiUser";
            this.multiUser.Size = new System.Drawing.Size(60, 16);
            this.multiUser.TabIndex = 22;
            this.multiUser.Text = "多帳號";
            this.multiUser.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 592);
            this.Controls.Add(this.multiUser);
            this.Controls.Add(this.startNum);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.totalBooks);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.bookNameRand);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numKeyword);
            this.Controls.Add(this.keyWordRand);
            this.Controls.Add(this.introRand);
            this.Controls.Add(this.subNameRand);
            this.Controls.Add(this.timeInterval);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.timeLimit);
            this.Controls.Add(this.password);
            this.Controls.Add(this.username);
            this.Controls.Add(this.publish);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.startLogin);
            this.Controls.Add(this.openFile);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Auto Publishing by YJC";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button openFile;
        private System.Windows.Forms.Button startLogin;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button publish;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox timeLimit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox timeInterval;
        private System.Windows.Forms.CheckBox subNameRand;
        private System.Windows.Forms.CheckBox introRand;
        private System.Windows.Forms.CheckBox keyWordRand;
        private System.Windows.Forms.TextBox numKeyword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox bookNameRand;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox totalBooks;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox startNum;
        private System.Windows.Forms.CheckBox multiUser;
    }
}

