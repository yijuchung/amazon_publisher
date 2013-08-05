using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Net;
using System.Reflection;
using System.IO;
using System.Web;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using WindowsInput;

namespace parser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            printText("請確認 csv 檔沒有被其他程式開啟，再開始執行。");
            printText("若要執行多帳號請先勾選在選取 csv。");

            //string regex = "[" + Path.GetInvalidFileNameChars() + "]";
            removeInvalidChars = new Regex(@"[?:*""<>|]"); 
            
        }

        Regex removeInvalidChars;

        public class ThreadInput
        {
            public void InputString()
            {
                System.Threading.Thread.Sleep(800);
                KeyboardSimulator kb = new KeyboardSimulator();
                kb.TextEntry(this.inputstring);
                kb.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
            }
            
            public string inputstring;
        }
        
        string sInipath;
        string sBookError;
        string sBookLog;
        string sBookNameForLog;
        bool bCheckLogin;

        Encoding enc;
        List<string[]> lData = new List<string[]>();
        List<string[]> userData = new List<string[]>();

        int iUserNum = 0;
        
        private void startLogin_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://kdp.amazon.com/self-publishing/signin/ap");

            waitForPage();
            
            HtmlDocument doc = webBrowser1.Document;

            while (doc.All["ap_email"] == null || doc.All["ap_password"] == null)
            {
                System.Windows.Forms.Application.DoEvents();
            }
            
            doc.All["ap_email"].SetAttribute("value", this.username.Text);
            doc.All["ap_password"].SetAttribute("value", this.password.Text);
            doc.All["signInSubmit"].InvokeMember("click");

            doc = webBrowser1.Document;

            while (doc.All["addItemForm"] == null)
                System.Windows.Forms.Application.DoEvents();

            bCheckLogin = true;
        }

        private void printText(string output)
        {
            textBox1.AppendText(output + Environment.NewLine);
            textBox1.ScrollToCaret();
            this.textBox1.Focus();
            this.textBox1.Select(this.textBox1.TextLength, 0);
            this.textBox1.ScrollToCaret();
        }

        private void waitForPage()
        {
            while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
            {
                System.Windows.Forms.Application.DoEvents();
            }
        }

        private void openFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFile = new OpenFileDialog();

            oFile.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            oFile.FilterIndex = 1;
            oFile.RestoreDirectory = true;

            if (oFile.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(oFile.FileName,Encoding.Default);
                sInipath = Path.GetDirectoryName(oFile.FileName);

                enc = Encoding.Default;
                string sTitle = sr.ReadLine();
                
                printText("Start parsing CSV pls wait.");

                // loading csv
                
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] sArray_temp = line.Split(new Char[] {','});
                    string[] sArray = new string[10];

                    int iArrTemp = sArray_temp.Count();
                    
                    int iTemp = 0;
                    while(iTemp < iArrTemp)
                    {
                        sArray[iTemp] = sArray_temp[iTemp];
                        iTemp++;
                    }

                    lData.Add(sArray);
                }
                printText("Parsing CSV is done.");
                //MessageBox.Show(lData.Count.ToString());
                sr.Close();
            }

            if (multiUser.Checked == true)
                multiUserFunc();
        }

        private void multiUserFunc()
        {
            if (!File.Exists(sInipath + "\\"+"config.txt"))
            {
                printText("Multiuser's config.txt is missing");
                return ;
            }

            StreamReader sr = new StreamReader(sInipath + "\\" + "config.txt", Encoding.Default);
            //string sTitle = sr.ReadLine();
            
            string sRow;
            while ((sRow = sr.ReadLine()) != null)
            {
                string[] sArray_temp = sRow.Split(new Char[] { ',' });
                string[] sArray = new string[5];

                int iArrTemp = sArray_temp.Count();

                int iTemp = 0;
                while (iTemp < iArrTemp)
                {
                    sArray[iTemp] = sArray_temp[iTemp];
                    iTemp++;
                }

                userData.Add(sArray);
            }
            sr.Close();
        }

        private void UserGoNext()
        {
            if (iUserNum >= userData.Count)
            {
                printText("多帳號處理完成 !!");
                return;
            }

            this.username.Text = userData[iUserNum][0];
            this.password.Text = userData[iUserNum][1];

            printText("NOW user is "+this.username.Text+" !!");

            bCheckLogin = false;
            startLogin_Click(this,null);

            while (!bCheckLogin)
                System.Windows.Forms.Application.DoEvents();
            //MessageBox.Show("ok");
            lData.Clear();

            StreamReader sr = new StreamReader(sInipath + "\\" + userData[iUserNum][2], Encoding.Default);
            printText("Start parsing CSV pls wait.");
            string sTitle = sr.ReadLine();

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] sArray_temp = line.Split(new Char[] { ',' });
                string[] sArray = new string[10];

                int iArrTemp = sArray_temp.Count();

                int iTemp = 0;
                while (iTemp < iArrTemp)
                {
                    sArray[iTemp] = sArray_temp[iTemp];
                    iTemp++;
                }

                lData.Add(sArray);
            }
            printText("Parsing CSV is done.");
            sr.Close();

            this.startNum.Text = userData[iUserNum][3];
            this.totalBooks.Text = userData[iUserNum][4];

            iUserNum++;

            publish_Click(this, null);
        }

        private void publish_Click(object sender, EventArgs e)
        {
            int iIndexStart = Convert.ToInt32(this.startNum.Text) - 1;
            int iTemp = 0;

            System.IO.Directory.CreateDirectory(sInipath + "\\log\\");
            System.IO.Directory.CreateDirectory(sInipath + "\\log\\success\\");
            System.IO.Directory.CreateDirectory(sInipath + "\\log\\success\\" + this.username.Text + "\\");
            System.IO.Directory.CreateDirectory(sInipath + "\\log\\error\\");
            System.IO.Directory.CreateDirectory(sInipath + "\\log\\error\\" + this.username.Text + "\\");

            string CurrTime = DateTime.Now.ToString("yyyyMMdd");

            HtmlDocument doc;

            while (iTemp < Convert.ToInt32(this.totalBooks.Text))
            {
                if (iTemp + iIndexStart >= lData.Count)
                    break;

                webBrowser1.Navigate("https://kdp.amazon.com/self-publishing/dashboard");
                waitForPage();
                
                doc = webBrowser1.Document;
                doc.All["addItemForm"].InvokeMember("submit");

                sBookError = "";
                sBookLog = "";
                sBookNameForLog = "";
                bool check = false;
                try
                {
                    check = fillForm(iTemp + iIndexStart);
                }
                catch (Exception elog)
                {
                    printText("發生未知的錯誤 (" + iTemp.ToString() + ") : " + sBookNameForLog + " 錯誤碼 : " + elog);
                }

                iTemp++;

                if (check)
                {
                    printText("Book " + iTemp.ToString() + " finished !!");
                    StreamWriter swSuc = new StreamWriter(sInipath + "\\log\\success\\" + this.username.Text + "\\" + removeInvalidChars.Replace(sBookNameForLog, "") + "_" + CurrTime + "_" + this.username.Text + ".txt", false, enc);
                    swSuc.Write(sBookLog);
                    swSuc.Close();
                }
                else
                {
                    printText("Book " + iTemp.ToString() + " ERROR !!");
                    StreamWriter swErr = new StreamWriter(sInipath + "\\log\\error\\" + this.username.Text + "\\" + removeInvalidChars.Replace(sBookNameForLog, "") + "_" + CurrTime + "_" + this.username.Text + "_error.txt", false, enc);
                    swErr.Write(sBookError);
                    swErr.Close();
                }

                System.Threading.Thread.Sleep(Convert.ToInt32(timeInterval.Text)*1000);
            }

            //MessageBox.Show("上傳完畢!!請看 log 視窗是否有發生錯誤，並請重新檢查後，上傳有錯誤的書籍。");
            System.IO.Directory.CreateDirectory(sInipath + "\\log\\" + this.username.Text + "\\");

            StreamWriter swLog = new StreamWriter(sInipath + "\\log\\" + this.username.Text + "\\" + CurrTime + "_" + this.username.Text + "_log.txt", false, enc);
            swLog.Write(this.textBox1.Text);
            swLog.Close();

            if (multiUser.Checked == true)
                UserGoNext();
        }
        
        private bool fillForm(int i)
        {
            //-------------------load file
            string CurrTime = DateTime.Now.ToString("yyyyMMdd");
            string sBookName;
            if (!Directory.Exists(sInipath + "\\" + lData[i][3] + "\\"))
            {
                sBookName = lData[i][6];
                printText("Book " + (i + 1) + " directory is wrong !!");
                sBookError += "Book " + (i + 1) + " directory is wrong !!" + Environment.NewLine;
                return false;
            }

            StreamReader srFile = new StreamReader(sInipath + "\\" + lData[i][3] + "\\" + lData[i][0], enc);
            sBookName = srFile.ReadLine().Trim(new char[] { '\r','\t' });
            sBookNameForLog = sBookName;

            if (!File.Exists(sInipath + "\\" + lData[i][3] + "\\" + lData[i][0]))
            {
                printText("Book " + (i + 1) + " txt doesn't exist !!");
                sBookError += "Book " + (i + 1) + " txt doesn't exist !!" + Environment.NewLine;
                srFile.Close();
                return false;
            }

            Encoding infEnc = Encoding.Default;

            //MessageBox.Show(sInipath + "\\" + lData[i][3] + "\\" + removeInvalidChars.Replace(sBookName,"") + "_" + CurrTime + "_" + this.username.Text + ".txt");

            StreamWriter swLog = new StreamWriter(sInipath + "\\" + lData[i][3] + "\\" + removeInvalidChars.Replace(sBookName,"") + "_" + CurrTime + "_" + this.username.Text + ".txt", false, infEnc);
            StreamWriter swError = new StreamWriter(sInipath + "\\" + lData[i][3] + "\\" + removeInvalidChars.Replace(sBookName, "") + "_" + CurrTime + "_" + this.username.Text + "_error.txt", false, infEnc);

            string sBookSubName = srFile.ReadLine();
            string sBookKeys = srFile.ReadLine();

            if (lData[i].Length > 6)
            {
                if (!(lData[i][6] == null || lData[i][6] == ""))
                    sBookName = lData[i][6];
                
                if (!(lData[i][7] == null || lData[i][7] == ""))
                    sBookSubName = lData[i][7];

                if (!(lData[i][8] == null || lData[i][8] == ""))
                    sBookKeys = lData[i][8].Replace("@", ",");
            }

            string sBookCateL1 = srFile.ReadLine();
            string sBookCateL2 = srFile.ReadLine();
            string sBookIntro = SubString(srFile.ReadToEnd(),0,3998);
            srFile.Close();

            string[] sBookCate1 = sBookCateL1.Split(new Char[] { '>' });
            string[] sBookCate2 = sBookCateL2.Split(new Char[] { '>' });

            string[] ImgTemp = Directory.GetFiles(sInipath + "\\" + lData[i][3].Replace("@",",") + "\\", "*.jpg");
            string sBookImg = ImgTemp[0];

            if (lData[i][9] != "")
                sBookImg = lData[i][9];

            string[] BookTemp = Directory.GetFiles(sInipath + "\\" + lData[i][3].Replace("@", ",") + "\\", "*.pdf");
            string sBookPDF = BookTemp[0];

            string sRoyalty = "35_PERCENT";
            if (lData[i][4] == "70%")
                sRoyalty = "70_PERCENT";
            
            //-------------------random string choosing

            if(bookNameRand.Checked)
                sBookName = RandomString(sBookName, bookNameRand.Checked);

            swLog.WriteLine(sBookName);
            sBookLog += sBookName +Environment.NewLine;

            sBookSubName = RandomString(sBookSubName, subNameRand.Checked);
            swLog.WriteLine(sBookSubName);
            sBookLog += sBookSubName + Environment.NewLine;
            
            sBookKeys = RandomKeywords(sBookKeys, keyWordRand.Checked);
            swLog.WriteLine(sBookKeys);
            sBookLog += sBookKeys + Environment.NewLine;
            swLog.WriteLine(sBookCateL1);
            swLog.WriteLine(sBookCateL2);
            sBookLog += sBookCateL1 + Environment.NewLine;
            sBookLog += sBookCateL2 + Environment.NewLine;
            sBookIntro = RandomString(sBookIntro, introRand.Checked);
            swLog.WriteLine(sBookIntro);
            sBookLog += sBookIntro + Environment.NewLine;
            swLog.Close();
            //return;
            //-------------------
            
            HtmlDocument doc = webBrowser1.Document;

            while (doc.All["drm-yes"] == null || doc.All["drm-no"] == null)
            {
                System.Windows.Forms.Application.DoEvents();
            }
              
            doc.All["book-title-box"].SetAttribute("value", sBookName+","+sBookSubName);
            doc.All["desc-area"].InnerText = sBookIntro;

            doc = webBrowser1.Document;
            doc.All["bookLanguageSelect"].InvokeMember("change");
            doc.All["bookLanguageSelect"].SetAttribute("selectedIndex", "1");
            // ------------category
            
            doc = webBrowser1.Document;
            doc.All["add-categories-but"].InvokeMember("click");
            
            HtmlElementCollection cate;
            doc = webBrowser1.Document;
            cate = doc.All["categories-tree-view"].GetElementsByTagName("li");
            while (cate.Count == 0)
            {
                System.Windows.Forms.Application.DoEvents();
                doc = webBrowser1.Document;
                cate = doc.All["categories-tree-view"].GetElementsByTagName("li");
            }
            
            // -----------fetch category
            /*
            int iFirstLevel = cate.Count;

            StreamWriter sw = new StreamWriter("c:\\log\\catefull.txt");

            for (int k = 0; k < cate.Count; k++)
            {
                this.textBox1.Text += "[" + k + "] " + cate[k].InnerText + Environment.NewLine;
                sw.WriteLine("["+k+"] "+cate[k].InnerText);
                sw.Flush();
                if (k == 51)
                    break;

                cate[k].InvokeMember("click");
                HtmlElement cate2 = cate[k].Children[2];
                while (cate2.Children.Count <= 1)
                {
                    System.Windows.Forms.Application.DoEvents();
                    doc = webBrowser1.Document;
                }

                int j = 0;
                while (j < cate2.Children.Count)
                {
                    if (cate2.Children[j].GetAttribute("className") != "expandable")
                    {
                        sw.WriteLine(" - " + cate2.Children[j].InnerText);
                        sw.Flush();
                    }
                    else
                    {
                        sw.WriteLine(" - " + cate2.Children[j].InnerText);
                        sw.Flush();
                        
                        cate2.Children[j].InvokeMember("click");
                        HtmlElement cate3 = cate2.Children[j].Children[2];
                        while (cate3.Children[0].GetAttribute("className") == "last")
                        {
                            System.Windows.Forms.Application.DoEvents();
                            doc = webBrowser1.Document;
                        }

                        int l = 0;
                        while (l < cate3.Children.Count)
                        {
                            if (cate3.Children[l].GetAttribute("className") != "expandable")
                            {
                                sw.WriteLine(" --- " + cate3.Children[l].InnerText);
                                sw.Flush();
                            }
                            else
                            {
                                sw.WriteLine(" --- " + cate3.Children[l].InnerText);
                                sw.Flush();

                                cate3.Children[l].InvokeMember("click");
                                HtmlElement cate4 = cate3.Children[l].Children[2];
                                while (cate4.Children[0].GetAttribute("className") == "last")
                                {
                                    System.Windows.Forms.Application.DoEvents();
                                    doc = webBrowser1.Document;
                                }
                                int m = 0;
                                while (m < cate4.Children.Count)
                                {
                                    sw.WriteLine(" ----- " + cate4.Children[m].InnerText);
                                    sw.Flush();
                                    m++;
                                }
                            }
                            l++;
                        }
                    }
                    j++;
                }
            }

            sw.Close();
            */
            
            int iLoc1;
            int iLoc2;

            iLoc1 = -1;
            iLoc2 = -1;

            int iMaxLevel = sBookCate1.Count();
            
            for (int k = 0; k < cate.Count; k++)
            {
                System.Windows.Forms.Application.DoEvents();
                if (cate[k].GetElementsByTagName("span")[0].InnerText.Trim(new char[] { '\n' }).Trim() == sBookCate1[0].Trim().ToUpper())
                {
                    iLoc1 = k;
                    cate[k].GetElementsByTagName("div")[0].InvokeMember("click");
                    break;
                }
            }

            if (iLoc1 < 0)
            {
                printText("Category 1-1 " + sBookCate1[0].Trim() + " setting error !! pls check ~");
                swError.WriteLine("Category 1-1 " + sBookCate1[0].Trim() + " setting error !! pls check ~");
                sBookError += "Category 1-1 " + sBookCate1[0].Trim() + " setting error !! pls check ~" + Environment.NewLine;
                return false;
            }

            do
            {
                System.Windows.Forms.Application.DoEvents();
                doc = webBrowser1.Document;

                if (doc.All["categories-tree-view"].GetElementsByTagName("li")[iLoc1].GetElementsByTagName("ul").Count > 0)
                    cate = doc.All["categories-tree-view"].GetElementsByTagName("li")[iLoc1].GetElementsByTagName("ul")[0].GetElementsByTagName("li");
            }while (cate.Count < 2);

            for (int k = 0; k < cate.Count; k++)
            {
                System.Windows.Forms.Application.DoEvents();
                if (cate[k].GetElementsByTagName("span")[0].InnerText.Trim(new char[] { '\n' }).Trim() == sBookCate1[1].Trim())
                {
                    iLoc2 = k;
                    cate[k].InvokeMember("click");
                    break;
                }
            }

            if (iMaxLevel > 2)
            {
                if (iLoc2 < 0)
                {
                    printText("Category 1-2 " + sBookCate1[1].Trim() + " setting error !! pls check ~");
                    swError.WriteLine("Category 1-2 " + sBookCate1[1].Trim() + " setting error !! pls check ~");
                    sBookError += "Category 1-2 " + sBookCate1[1].Trim() + " setting error !! pls check ~" + Environment.NewLine;
                    return false;
                }

                do
                {
                    System.Windows.Forms.Application.DoEvents();
                    doc = webBrowser1.Document;
                    if (doc.All["categories-tree-view"].GetElementsByTagName("li")[iLoc1].GetElementsByTagName("ul").Count > 0)
                        cate = doc.All["categories-tree-view"].GetElementsByTagName("li")[iLoc1].GetElementsByTagName("ul")[0].GetElementsByTagName("li")[iLoc2].GetElementsByTagName("ul")[0].GetElementsByTagName("li");
                } while (cate.Count < 2);

                for (int k = 0; k < cate.Count; k++)
                {
                    System.Windows.Forms.Application.DoEvents();
                    if (cate[k].GetElementsByTagName("span")[0].InnerText.Trim(new char[] { '\n' }).Trim() == sBookCate1[2].Trim())
                    {
                        //iLoc2 = k;
                        cate[k].InvokeMember("click");
                        break;
                    }
                }
            }

            iMaxLevel = sBookCate2.Count();

            iLoc1 = -1;
            iLoc2 = -1;

            doc = webBrowser1.Document;

            cate = doc.All["categories-tree-view"].GetElementsByTagName("li");
            for (int k = 0; k < cate.Count; k++)
            {
                System.Windows.Forms.Application.DoEvents();

                if (cate[k].GetElementsByTagName("span")[0].InnerText.Trim(new char[] { '\n' }).Trim() == sBookCate2[0].Trim().ToUpper())
                {
                    //printText(cate[k].GetElementsByTagName("span")[0].InnerText.Trim(new char[] { '\n' }).Trim() + " " + sBookCate2[0].Trim()+" "+k.ToString());
                    iLoc1 = k;
                    cate[k].InvokeMember("click");
                    break;
                }
            }

            if (iLoc1 < 0)
            {
                printText("Category 2-1 " + sBookCate2[0].Trim() + " setting error !! pls check ~");
                swError.WriteLine("Category 2-1 " + sBookCate2[0].Trim() + " setting error !! pls check ~");
                sBookError += "Category 2-1 " + sBookCate2[0].Trim() + " setting error !! pls check ~" + Environment.NewLine;
                return false;
            }
            
            do
            {
                System.Windows.Forms.Application.DoEvents();
                doc = webBrowser1.Document;

                //printText(doc.All["categories-tree-view"].GetElementsByTagName("li")[iLoc1].InnerHtml);
                //return true;
                if( doc.All["categories-tree-view"].GetElementsByTagName("li")[iLoc1].GetElementsByTagName("ul").Count > 0)
                    cate = doc.All["categories-tree-view"].GetElementsByTagName("li")[iLoc1].GetElementsByTagName("ul")[0].GetElementsByTagName("li");
            } while (cate.Count < 2);

            for (int k = 0; k < cate.Count; k++)
            {
                System.Windows.Forms.Application.DoEvents();
                if (cate[k].GetElementsByTagName("span")[0].InnerText.Trim(new char[] { '\n' }).Trim() == sBookCate2[1].Trim())
                {
                    iLoc2 = k;
                    cate[k].InvokeMember("click");
                    break;
                }
            }

            if (iMaxLevel > 2)
            {

                if (iLoc2 < 0)
                {
                    printText("Category 2-2 " + sBookCate2[1].Trim() + " setting error !! pls check ~");
                    swError.WriteLine("Category 2-2 " + sBookCate2[1].Trim() + " setting error !! pls check ~");
                    sBookError += "Category 2-2 " + sBookCate2[1].Trim() + " setting error !! pls check ~" + Environment.NewLine;
                    return false;
                }

                do
                {
                    System.Windows.Forms.Application.DoEvents();
                    doc = webBrowser1.Document;
                    if(doc.All["categories-tree-view"].GetElementsByTagName("li")[iLoc1].GetElementsByTagName("ul")[0].GetElementsByTagName("li")[iLoc2].GetElementsByTagName("ul").Count > 0)
                        cate = doc.All["categories-tree-view"].GetElementsByTagName("li")[iLoc1].GetElementsByTagName("ul")[0].GetElementsByTagName("li")[iLoc2].GetElementsByTagName("ul")[0].GetElementsByTagName("li");
                } while (cate.Count < 2);

                for (int k = 0; k < cate.Count; k++)
                {
                    System.Windows.Forms.Application.DoEvents();
                    if (cate[k].GetElementsByTagName("span")[0].InnerText.Trim(new char[] { '\n' }).Trim() == sBookCate2[2].Trim())
                    {
                        cate[k].InvokeMember("click");
                        break;
                    }
                }
            }

            doc = webBrowser1.Document;

            doc.All["save-categories-but"].InvokeMember("click");

            doc.All["add-contributors-button"].InvokeMember("click");
            doc.All.GetElementsByName("firstName")[1].SetAttribute("value", lData[i][2]);
            doc.All.GetElementsByName("lastName")[1].SetAttribute("value", lData[i][1]);
            doc.All.GetElementsByName("type")[1].SetAttribute("selectedIndex", "1");
            doc.All.GetElementsByName("type")[1].SetAttribute("SelectedValue", "AUTHOR");
            doc.All["contributors-save-button"].InvokeMember("click");
            // -------------
            
            doc.All.GetElementsByName("searchKeywords")[0].SetAttribute("value", sBookKeys);
            doc.All.GetElementsByName("isPublicDomain")[1].InvokeMember("click");

            doc.All["drm-yes-span"].InvokeMember("click");
            doc.All["drm-yes"].InvokeMember("click");

            ThreadInput ti = new ThreadInput();
            ti.inputstring = sBookPDF;
            Thread tithread = new Thread(ti.InputString);
            tithread.Start();

            this.textBox1.Text += "Book " + (i + 1).ToString() + " Uploading";
            doc.All["content-input"].InvokeMember("keypress");
            doc.All["content-input"].InvokeMember("click");
            doc.All["upload-button"].InvokeMember("click");

                        // uploading img
            doc.All["browse-img-button"].InvokeMember("click");

            ti.inputstring = sBookImg;
            tithread = new Thread(ti.InputString);

            tithread.Start();
            doc.All["image-input"].InvokeMember("click");
            doc.All["image-upload-button"].Enabled = true;
            doc.All["image-upload-button"].InvokeMember("click");

            //wait picture uploading
            waitForPic();
            doc = webBrowser1.Document;

            while (doc.All["msg-content-uploading"].GetElementsByTagName("className").Equals("progress-module hidden") && doc.All["msg-content-converting"].GetElementsByTagName("className").Equals("progress-module hidden"))
                doc.All["upload-button"].InvokeMember("click");

            //wait book uploading
            waitForBook();
            timer1.Stop();
            bTimer = true;

            if (!bBookUploaded)
            {
                printText("...More than " + timeLimit.Text + " for uploading(Skip)");
                swError.WriteLine("More than " + timeLimit.Text + " for uploading(Skip)");
                sBookError += "More than " + timeLimit.Text + " for uploading(Skip)" + Environment.NewLine;

                bBookUploaded = true;
                return false;
            }

            printText("...Finished");
            doc.All["save-and-continue"].InvokeMember("click");
            waitForPage();

            //---------------page2
            
            doc = webBrowser1.Document;

            while (doc.All["save-and-publish"] == null)
            {
                System.Windows.Forms.Application.DoEvents();
                doc = webBrowser1.Document;
            }

            doc.All[sRoyalty].InvokeMember("click");

            //doc.All["kindlePriceUS"].InvokeMember("keypress");
            doc.All["kindlePriceUS"].SetAttribute("value", lData[i][5]);
            doc.All["kindlePriceUS"].InvokeMember("change");

            while (doc.All["priceBasedOnUSForUK"] == null)
            {
                System.Windows.Forms.Application.DoEvents();
                doc = webBrowser1.Document;
            }

            doc.All["priceBasedOnUSForUK"].InvokeMember("click");

            while (doc.All["priceConversionUK"].GetAttribute("className").Equals("hidden"))
            {
                System.Windows.Forms.Application.DoEvents();
                doc = webBrowser1.Document;
            }

            while (doc.All["priceBasedOnUSForDE"] == null)
            {
                System.Windows.Forms.Application.DoEvents();
                doc = webBrowser1.Document;
            }

            doc.All["priceBasedOnUSForDE"].InvokeMember("click");

            while (doc.All["priceConversionDE"].GetAttribute("className").Equals("hidden"))
            {
                System.Windows.Forms.Application.DoEvents();
                doc = webBrowser1.Document;
            }

            doc.All.GetElementsByName("copyright")[0].InvokeMember("keypress");
            doc.All.GetElementsByName("copyright")[0].SetAttribute("Checked", "true");
            doc = webBrowser1.Document;
            waitForStatus();

            doc.All["save-and-publish"].InvokeMember("click");

            DateTime CurTime = DateTime.Now;
            DateTime LastTime = DateTime.Now;
            DateTime DuringTime = DateTime.Now;
                        
            while ( true )
            {
                foreach (HtmlElement h in doc.All)
                {
                    if (h.GetAttribute("className").Equals("ap_content"))
                    {
                        //printText(h.FirstChild.GetAttribute("id"));
                        if (h.FirstChild.GetAttribute("id").Equals("invalidInput"))
                        {
                            foreach (HtmlElement k in doc.All)
                            {
                                if (k.GetAttribute("className").Equals("ap_close"))
                                {
                                    k.InvokeMember("click");
                                    break;
                                }
                            }
                            break;
                        }
                        else if (h.FirstChild.GetAttribute("id").Equals("successPopup"))
                        {
                            swError.Close();
                            return true;
                        }
                    }
                }

                CurTime = DateTime.Now;
                TimeSpan s = new TimeSpan(CurTime.Ticks - LastTime.Ticks);

                TimeSpan s2 = new TimeSpan(CurTime.Ticks - DuringTime.Ticks);

                if (s.Seconds > 1)
                {
                    doc.All["save-and-publish"].InvokeMember("click");
                    LastTime = CurTime;
                }

                if (s2.Seconds > 30)
                {
                    printText("More than 30 secs for publishing(Skip)");
                    swError.WriteLine("More than 30 secs for publishing(Skip)");
                    sBookError += "More than 30 secs for publishing(Skip)" + Environment.NewLine;

                    return false;
                }

                System.Windows.Forms.Application.DoEvents();
                doc = webBrowser1.Document;
            }
            
        }

        private void waitForPic()
        {
            HtmlDocument doc;
            doc = webBrowser1.Document;

            DateTime CurrTime = DateTime.Now;
            DateTime LastTime = DateTime.Now;

            while (true)
            {
                CurrTime = DateTime.Now;
                TimeSpan s = new TimeSpan(CurrTime.Ticks - LastTime.Ticks);

                if (s.Seconds > 10)
                {
                    //printText("pic upload error, reupload !!");
                    doc.All["image-upload-button"].Enabled = true;
                    doc.All["image-upload-button"].InvokeMember("click");
                    System.Windows.Forms.Application.DoEvents();
                    LastTime = CurrTime;
                }

                if (doc.GetElementById("image-upload-msg").InnerText.Equals("Uploaded successfully!"))
                {
                    for (int j = 0; j < doc.All.Count; j++)
                    {
                        if (doc.All[j].GetAttribute("className").Equals("ap_close"))
                        {
                            doc.All[j].InvokeMember("click");
                            break;
                        }
                    }
                    if (doc.All["image-upload-popup"].GetAttribute("className").Equals("hidden"))
                        break;
                }

                System.Windows.Forms.Application.DoEvents();
                doc = webBrowser1.Document;
            }
        }

        private void waitForBook()
        {
            HtmlDocument doc;
            if (Convert.ToInt32(timeLimit.Text) != 0)
            {
                timer1.Interval = Convert.ToInt32(timeLimit.Text) * 1000;
                timer1.Start();
            }
            
            while (bTimer)
            {
                doc = webBrowser1.Document;
                if (doc.GetElementById("msg-content-success").GetAttribute("className").Equals("success-module"))
                    break;
                
                if (!bBookUploaded)
                    break;
                System.Windows.Forms.Application.DoEvents();
            }
        }

        private void waitForStatus()
        {
            HtmlDocument doc;
            while (true)
            {
                doc = webBrowser1.Document;

                if (doc.All["status1-complete"] != null)
                {
                    if (doc.All["status1-complete"].GetAttribute("className").ToString().Equals("success1"))
                        break;
                }
                else if (doc.All["top-nav-step1"].GetElementsByTagName("div")[2] != null)
                {
                    if (doc.All["top-nav-step1"].GetElementsByTagName("div")[2].InnerText.Trim() == "Complete")
                        break;
                }

                System.Windows.Forms.Application.DoEvents();
            }
        }

        public static string SubString(string strData, int startIndex, int length)
        {
            int intLen = strData.Length;
            int intSubLen = intLen - startIndex;
            string strReturn;
            if (length == 0)
                strReturn = "";
            else
            {
                if (intLen <= startIndex)
                    strReturn = "";
                else
                {
                    if (length > intSubLen)
                        length = intSubLen;

                    strReturn = strData.Substring(startIndex, length);
                }
            }
            return strReturn;
        }

        bool bBookUploaded = true;
        bool bTimer = true;

        private void timer1_Tick(object sender, EventArgs e)
        {
            bBookUploaded = false;
            bTimer = false;
        }

        public string RandomString(string s, bool bRand)
        {
            Random randObj = new Random();

            string pattern = @"\{([^\}]*)\}";
            MatchCollection mc = Regex.Matches(s, pattern);
            foreach (Match match in mc)
            {
                string[] sMatch = match.Groups[1].Value.Split('|');
                
                string sRep;
                if (bRand)
                {
                    int iMatIndex = randObj.Next(0, sMatch.Length - 1);
                    sRep = sMatch[iMatIndex].Trim();
                }
                else
                {
                    sRep = sMatch[0].Trim();
                }
                s = Regex.Replace(s, Regex.Escape(match.Groups[0].Value), sRep);
            }

            mc = Regex.Matches(s, pattern);
            if (mc.Count > 0)
            {
                foreach (Match match in mc)
                {
                    string[] sMatch = match.Groups[1].Value.Split('|');

                    string sRep;
                    if (bRand)
                    {
                        int iMatIndex = randObj.Next(0, sMatch.Length - 1);
                        sRep = sMatch[iMatIndex].Trim();
                    }
                    else
                    {
                        sRep = sMatch[0].Trim();
                    }
                    s = Regex.Replace(s, Regex.Escape(match.Groups[0].Value), sRep);
                }
            }

            MatchCollection mc2 = Regex.Matches(s, pattern);

            if (mc2.Count > 0)
                s = RandomString(s, bRand);

            return s;
        }

        public string RandomKeywords(string s, bool bRand)
        {
            Random randObj = new Random();
            string[] sKey = s.Split(',');

            string[] sKeyDone = sKey.OrderBy(x => randObj.Next()).ToArray();

            string sRand = null;
            int iMax = Convert.ToInt32(numKeyword.Text);
            int iNow = 0;
            foreach (string key in sKeyDone)
            {
                iNow++;
                if (iNow > iMax)
                    break;

                sRand += key.Trim() + ",";
            }

            sRand.TrimEnd(',');

            return sRand;
        }
    }

}