using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;

namespace csvchecker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            removeInvalidChars = new Regex(@"[?:*""<>|]"); 
        }

        string sInipath;
        string sCurCSV;
        Encoding enc;
        Regex removeInvalidChars;
        List<string[]> lData = new List<string[]>();

        Dictionary<string, int> dCateRoot = new Dictionary<string, int>();
        Dictionary<int, string[]> dCateRootToSec = new Dictionary<int, string[]>();
        //List<string[]> lCateRootToSec = new List<string[]>();
        Dictionary<int, string> dCateSec = new Dictionary<int, string>();
        Dictionary<int, string[]> dCateSecToThird = new Dictionary<int, string[]>();
        //List<string[]> lCateSecToThird = new List<string[]>();
        Dictionary<int, string> dCateThird = new Dictionary<int, string>();
        Dictionary<int, string[]> dCateThirdToFour = new Dictionary<int, string[]>();
        //List<string[]> lCateThirdToFour = new List<string[]>();

        private void printText(string output)
        {
            this.logtext.AppendText(output + Environment.NewLine);
            this.logtext.ScrollToCaret();
            this.logtext.Focus();
            this.logtext.Select(this.logtext.TextLength, 0);
            this.logtext.ScrollToCaret();
        }

        private void choose_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFile = new OpenFileDialog();

            oFile.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            oFile.FilterIndex = 1;
            oFile.RestoreDirectory = true;

            if (oFile.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(oFile.FileName, Encoding.Default);
                sInipath = Path.GetDirectoryName(oFile.FileName);
                sCurCSV = Path.GetFileNameWithoutExtension(oFile.FileName);

                enc = Encoding.Default;
                string sTitle = sr.ReadLine();
                
                string line;
                int iCSV = 2;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] sArray = line.Split(new Char[] { ',' });
                    //string[] sArray = new string[10];

                    int iArrTemp = sArray.Count();

                    if (iArrTemp != 10)
                    {
                        printText(oFile.FileName + " 行數:" + iCSV.ToString() + " 請檢查逗點是否正常。");
                    }else
                        lData.Add(sArray);

                    iCSV++;
                }
                sr.Close();
            }
        }

        private void cateBut_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFile = new OpenFileDialog();

            oFile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            oFile.FilterIndex = 1;
            oFile.RestoreDirectory = true;

            if (oFile.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(oFile.FileName, Encoding.Default);
                
                enc = Encoding.Default;
                
                string line;
                
                string sRoot = "";
                string sSec = "";
                string sThird = "";
                string sFour = "";

                int iRoot = 0;
                int iSec = 0;
                int iThird = 0;
                
                while ((line = sr.ReadLine()) != null)
                {
                    if (!line.Substring(0, 1).Equals("-"))
                    {
                        //iSec = 0;
                        //iThird = 0;
                        sRoot = line.Trim();
                        //printText(sRoot);
                        dCateRoot.Add(sRoot, iRoot);
                        iRoot++;
                        continue;
                    }

                    if (line.Substring(0, 2).Equals("- "))
                    {
                        //iThird = 0;
                        sSec = line.Substring(2).Trim();
                        //printText(sSec);
                        dCateSec.Add(iSec,sSec);
                        iSec++;

                        if (dCateRootToSec.ContainsKey(iRoot-1))
                        {
                            string[] sTemp = dCateRootToSec[iRoot-1];
                            string[] sNew = new string[sTemp.Length + 1];
                            sNew[sTemp.Length] = sSec;
                            sTemp.CopyTo(sNew, 0);
                            dCateRootToSec[iRoot-1] = sNew;
                        }else
                        {
                            string[] sTemp = new string[1];
                            sTemp[0] = sSec;
                            dCateRootToSec.Add(iRoot-1, sTemp);
                        }
                        
                        continue;
                    }

                    if (line.Substring(0, 4).Equals("--- "))
                    {
                        sThird = line.Substring(4).Trim();
                        //printText(sThird);
                        dCateThird.Add(iThird, sThird);
                        iThird++;

                        if (dCateSecToThird.ContainsKey(iSec-1))
                        {
                            string[] sTemp = dCateSecToThird[iSec-1];
                            string[] sNew = new string[sTemp.Length + 1];
                            sNew[sTemp.Length] = sThird;
                            sTemp.CopyTo(sNew, 0);
                            dCateSecToThird[iSec-1] = sNew;
                        }
                        else
                        {
                            string[] sTemp = new string[1];
                            sTemp[0] = sThird;
                            dCateSecToThird.Add(iSec-1,sTemp);
                        }
                        continue;
                    }

                    if (line.Substring(0, 6).Equals("----- "))
                    {
                        sFour = line.Substring(6).Trim();
                        //printText(sFour);
                        if (dCateThirdToFour.ContainsKey(iThird-1))
                        {
                            string[] sTemp = dCateThirdToFour[iThird-1];
                            string[] sNew = new string[sTemp.Length + 1];
                            sNew[sTemp.Length] = sFour;
                            sTemp.CopyTo(sNew, 0);
                            dCateThirdToFour[iThird-1] = sNew;
                        }
                        else
                        {
                            string[] sTemp = new string[1];
                            sTemp[0] = sFour;
                            dCateThirdToFour.Add(iThird-1,sTemp);
                        }
                        continue;
                    }
                }
                sr.Close();
                /*
                foreach (KeyValuePair<int,string[]> d in dCateSecToThird)
                {
                    printText(d.Key.ToString());
                    foreach (string str in d.Value)
                    {
                        printText(str);
                    }
                }
                */
                
                //MessageBox.Show("ok");
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

        private bool checkbook(int i)
        {
            //-------------------load file
            string sBookName;

            lData[i][3] = lData[i][3].Replace("@", ",");
            
            if (!Directory.Exists(sInipath + "\\" + lData[i][3] + "\\"))
            {
                printText("Book : "+ (i + 1) + " directory is wrong !!");
                return false;
            }

            if (!File.Exists(sInipath + "\\" + lData[i][3] + "\\" + lData[i][0]))
            {
                printText("書名無法判別 : " + (i + 1) + " "+ sInipath + "\\" + lData[i][3] + "\\" + lData[i][0] + " txt doesn't exist !!");
                return false;
            }

            StreamReader srFile = new StreamReader(sInipath + "\\" + lData[i][3] + "\\" + lData[i][0], enc);
            sBookName = srFile.ReadLine().Trim(new char[] { '\r', '\t' });

            Encoding infEnc = Encoding.Default;

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
            string sBookIntro = SubString(srFile.ReadToEnd(), 0, 3998);
            srFile.Close();

            string[] sBookCate1 = sBookCateL1.Split(new Char[] { '>' });
            string[] sBookCate2 = sBookCateL2.Split(new Char[] { '>' });

            string[] ImgTemp = Directory.GetFiles(sInipath + "\\" + lData[i][3] + "\\", "*.jpg");

            if (ImgTemp.Count() < 1)
            {
                printText("Book : " + (i + 1) + " " + sInipath + "\\" + lData[i][3] + "\\ img doesn't exist !!");
                return false;
            }

            string sBookImg = ImgTemp[0];

            if (!sBookImg.ToLower().EndsWith(this.imgText.Text.ToLower()+ ".jpg"))
            {
                printText("Book : " + (i + 1) + " " +sBookImg+ " should with "+this.imgText.Text);
                return false;
            }

            if (lData[i][9] != "")
                sBookImg = lData[i][9];

            string[] BookTemp = Directory.GetFiles(sInipath + "\\" + lData[i][3] + "\\", "*.pdf");
            string sBookPDF = BookTemp[0];

            int iMaxLevel = sBookCate1.Count();
            int currLevel = 0;
            int iRoot = 0;
            int iSec = 0;
            int iThird = 0;
            bool bCheck = true;
            while ((currLevel < iMaxLevel) && bCheck)
            {
                sBookCate1[currLevel] = sBookCate1[currLevel].Trim();
               // printText("原始" + sBookCate1[currLevel]);
                switch (currLevel)
                {
                    case 0:
                        if (!dCateRoot.ContainsKey(sBookCate1[currLevel]))
                        {
                            printText(sBookName + " : " + (i + 1) + " cate1_1 " + sBookCate1[currLevel]+" error !!");
                            bCheck = false;
                            break;
                        }
                        iRoot = dCateRoot[sBookCate1[currLevel]];
                        //printText(iRoot.ToString());
                        break;
                    case 1:
                        if (!dCateRootToSec.ContainsKey(iRoot))
                        {
                            printText(sBookName + " : " + (i + 1) + " cate1_2 " + sBookCate1[currLevel] + " error !!");
                            bCheck = false;
                            break;
                        }

                        string[] sTemp = dCateRootToSec[iRoot];
                        //MessageBox.Show("ok");
                        if (!sTemp.Contains(sBookCate1[currLevel]))
                        {
                            printText(sBookName + " : " + (i + 1) + " cate1_2 " + sBookCate1[currLevel] + " error !!");
                            bCheck = false;
                            break;
                        }
                        bool bSec = false;
                        foreach( KeyValuePair<int, string> d in dCateSec)
                        {
                            if (d.Value == sBookCate1[currLevel])
                            {
                                bSec = true;
                                iSec = d.Key;
                                break;
                            }
                        }

                        if (!bSec)
                            bCheck = false;
                        break;
                    case 2:

                        if (!dCateSecToThird.ContainsKey(iSec))
                        {
                            printText(sBookName + " : " + (i + 1) + " cate1_3 " + sBookCate1[currLevel] + " error !!");
                            bCheck = false;
                            break;
                        }

                        sTemp = dCateSecToThird[iSec];

                        if (!sTemp.Contains(sBookCate1[currLevel]))
                        {
                            printText(sBookName + " : " + (i + 1) + " cate1_3 " + sBookCate1[currLevel] + " error !!");
                            bCheck = false;
                            break;
                        }

                        foreach (KeyValuePair<int, string> d in dCateThird)
                        {
                            if (d.Value == sBookCate1[currLevel])
                            {
                                iThird = d.Key;
                                break;
                            }
                        }
                        break;
                    case 3:
                        if (!dCateThirdToFour.ContainsKey(iThird))
                        {
                            printText(sBookName + " : " + (i + 1) + " cate1_4 " + sBookCate1[currLevel] + " error !!");
                            bCheck = false;
                            break;
                        }

                        sTemp = dCateThirdToFour[iThird];

                        if (!sTemp.Contains(sBookCate1[currLevel]))
                        {
                            printText(sBookName + " : " + (i + 1) + " cate1_4 " + sBookCate1[currLevel] + " error !!");
                            bCheck = false;
                            break;
                        }
                        break;
                }
                currLevel++;
            }

            iMaxLevel = sBookCate2.Count();
            currLevel = 0;
            iRoot = 0;
            iSec = 0;
            iThird = 0;
            bCheck = true;

            while ((currLevel < iMaxLevel) && bCheck)
            {
                sBookCate2[currLevel] = sBookCate2[currLevel].Trim();
                switch (currLevel)
                {
                    case 0:
                        if (!dCateRoot.ContainsKey(sBookCate2[currLevel]))
                        {
                            printText(sBookName + " : " + (i + 1) + " cate2_1 " + sBookCate2[currLevel] + " error !!");
                            bCheck = false;
                            break;
                        }
                        iRoot = dCateRoot[sBookCate2[currLevel]];
                        //printText(iRoot.ToString());
                        break;
                    case 1:
                        if (!dCateRootToSec.ContainsKey(iRoot))
                        {
                            printText(sBookName + " : " + (i + 1) + " cate2_2 " + sBookCate2[currLevel] + " error !!");
                            bCheck = false;
                            break;
                        }

                        string[] sTemp = dCateRootToSec[iRoot];
                        //MessageBox.Show("ok");
                        if (!sTemp.Contains(sBookCate2[currLevel]))
                        {
                            printText(sBookName + " : " + (i + 1) + " cate2_2 " + sBookCate2[currLevel] + " error !!");
                            bCheck = false;
                            break;
                        }
                        bool bSec = false;
                        foreach (KeyValuePair<int, string> d in dCateSec)
                        {
                            if (d.Value == sBookCate2[currLevel])
                            {
                                bSec = true;
                                iSec = d.Key;
                                break;
                            }
                        }

                        if (!bSec)
                            bCheck = false;
                        break;
                    case 2:

                        if (!dCateSecToThird.ContainsKey(iSec))
                        {
                            printText(sBookName + " : " + (i + 1) + " cate2_3 " + sBookCate2[currLevel] + " error !!");
                            bCheck = false;
                            break;
                        }

                        sTemp = dCateSecToThird[iSec];

                        if (!sTemp.Contains(sBookCate2[currLevel]))
                        {
                            printText(sBookName + " : " + (i + 1) + " cate2_3 " + sBookCate2[currLevel] + " error !!");
                            bCheck = false;
                            break;
                        }

                        foreach (KeyValuePair<int, string> d in dCateThird)
                        {
                            if (d.Value == sBookCate2[currLevel])
                            {
                                iThird = d.Key;
                                break;
                            }
                        }
                        break;
                    case 3:
                        if (!dCateThirdToFour.ContainsKey(iThird))
                        {
                            printText(sBookName + " : " + (i + 1) + " cate2_4 " + sBookCate2[currLevel] + " error !!");
                            bCheck = false;
                            break;
                        }

                        sTemp = dCateThirdToFour[iThird];

                        if (!sTemp.Contains(sBookCate2[currLevel]))
                        {
                            printText(sBookName + " : " + (i + 1) + " cate2_4 " + sBookCate2[currLevel] + " error !!");
                            bCheck = false;
                            break;
                        }
                        break;
                }
                currLevel++;
            }

            return true;
        }

        private void check_Click(object sender, EventArgs e)
        {
            if (!includeall.Checked)
            {
                multiCSV();
                MessageBox.Show("檢測完成 !!");
            }
            else
            {
                string[] csvTemp = Directory.GetFiles(sInipath + "\\", "*.csv");
                foreach( string csvPath in csvTemp )
                {
                    lData.Clear();

                    StreamReader sr = new StreamReader(csvPath, Encoding.Default);
                    //sInipath = Path.GetDirectoryName(oFile.FileName);
                    sCurCSV = Path.GetFileNameWithoutExtension(csvPath);

                    printText("檢查 -> "+csvPath);

                    enc = Encoding.Default;
                    string sTitle = sr.ReadLine();

                    string line;
                    int iCSV = 2;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] sArray = line.Split(new Char[] { ',' });
                        //string[] sArray = new string[10];

                        int iArrTemp = sArray.Count();

                        if (iArrTemp != 10)
                        {
                            printText(csvPath + " 行數:" + iCSV.ToString() + " 請檢查逗點是否正常。");
                        }
                        else
                            lData.Add(sArray);

                        iCSV++;
                    }
                    sr.Close();

                    multiCSV();

                    printText("檢查完成");
                    printText("生成 log 檔 -> "+sInipath + "\\" + sCurCSV + ".txt");
                }
                MessageBox.Show("檢測完成 !!");
            }
        }

        private void multiCSV()
        {
            int iBook = 0;
            while (iBook < lData.Count)
            {
                try
                {
                    checkbook(iBook);
                }
                catch (Exception e)
                {
                    printText("發生未知錯誤 ("+iBook.ToString()+") : "+e+"。");
                }
                iBook++;
                System.Windows.Forms.Application.DoEvents();
            }
            StreamWriter sw = new StreamWriter(sInipath + "\\" + sCurCSV + ".txt");
            sw.Write(logtext.Text);
            sw.Close();
        }
    }
}
