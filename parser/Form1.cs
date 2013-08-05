using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Net;
using System.IO;
using System.Web;
using System.Threading;
using HtmlAgilityPack;

namespace parser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static ManualResetEvent[] mrEvents;
        public static ManualResetEvent[] mrpEvents;
        public static int iBase = 2;
        public char[] cParse = new char[] { '/', '.' };
        private static Mutex mut = new Mutex();

        public void PrintLog(string log)
        {
            this.textBox1.Text += log;
            this.textBox1.Update();
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load("page/"+iNow+"_page1.txt", Encoding.UTF8);
            HtmlAgilityPack.HtmlDocument content = new HtmlAgilityPack.HtmlDocument();
            content.LoadHtml(doc.DocumentNode.SelectSingleNode("/html/body/*[@id='mainContent']/div[3]/div[1]").InnerHtml);
            HtmlNodeCollection nList =
         content.DocumentNode.SelectNodes("./div[@class='sw1'] | ./div[@class='sw2']");

            String baseurl = "http://big5.qidian.com";
            StreamWriter sw = new StreamWriter("list/"+iNow+"_1.txt");
            
            //this.textBox1.Text += "First page analyze ok!! now waiting for 100 books loading...";
            PrintLog("First page start analize " + Environment.NewLine);
            WebClient webClient = new WebClient();

            foreach (HtmlNode ent in nList)
            {
                int iID = Convert.ToInt32(ent.SelectSingleNode("./div[1]").InnerText.ToString());
                String sTag = ent.SelectSingleNode("./div[2]").InnerText.ToString();
                String sName = ent.SelectSingleNode("./div[3]/*[@class='swbt']").InnerText.ToString();
                String sUrl = baseurl + ent.SelectSingleNode("./div[3]/*[@class='swbt']//a[1]").Attributes["href"].Value.ToString();
                string sWord = ent.SelectSingleNode("./div[4]//ul/li[1]").InnerText.ToString();
                uint iWord = Convert.ToUInt32(sWord);
                String sAuthor = ent.SelectSingleNode("./div[5]").InnerText.ToString();
                String sUpdateTime = ent.SelectSingleNode("./div[6]").InnerText.ToString();
                string[] sRes = sUrl.Split(cParse);

                if (!File.Exists("book/" + sRes[6] + ".txt"))
                {
                    PrintLog("Book " + sRes[6] + "(" + sName + ") Need Deeper Search ~" + Environment.NewLine);
                }

                sw.WriteLine(String.Format("{0},{1},{2},{3},{4},{5},{6},{7}", sRes[6], iID, sTag, sName, sUrl, iWord, sAuthor, sUpdateTime));

            }

            String sLastUrl = content.DocumentNode.SelectSingleNode("./div[103]//a[13]").Attributes["href"].Value.ToString();
            System.Uri uri = new System.Uri(sLastUrl);
            iLast = Convert.ToInt32(HttpUtility.ParseQueryString(uri.Query).Get("PageIndex"));
            sw.Close();

            PrintLog("First Page Done ~"+Environment.NewLine);
            PrintLog("[This Category needs to download "+iLast+" pages for next step]" + Environment.NewLine);

            doc = null;
            content = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void DownThread(object d)
        {
            int iPage = Convert.ToInt32(d);
            //WebClient webClient = new WebClient();
            //webClient.DownloadFile(new Uri("http://big5.qidian.com/book/bookstore.aspx?ChannelId=" + iNow + "&SubCategoryId=-1&Tag=all&Size=-1&Action=-1&OrderId=9&P=all&PageIndex=" + (iPage + iBase) + "&update=-1&Vip=-1&Boutique=-1&SignStatus=-1"), @"page/"+iNow+"_page" + (iPage + iBase) + @".txt");

            WebRequest request = WebRequest.Create(new Uri("http://big5.qidian.com/book/bookstore.aspx?ChannelId=" + iNow + "&SubCategoryId=-1&Tag=all&Size=-1&Action=-1&OrderId=9&P=all&PageIndex=" + (iPage + iBase) + "&update=-1&Vip=-1&Boutique=-1&SignStatus=-1"));
            //request.Proxy();
            WebResponse response = request.GetResponse();
            //Stream resStream = ;
            StreamReader sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
            //FileStream fs = File.Create("page/" + iNow + "_page" + (iPage + iBase) + @".txt");
            StreamWriter sw = new StreamWriter("page/" + iNow + "_page" + (iPage + iBase) + @".txt");
            sw.Write(sr.ReadToEnd());
            sr.Close();
            sw.Close();
            //= sr.ReadToEnd();
            //resStream.Close();
            //sr.Close();

            mrEvents[iPage].Set();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.textBox1.Clear();
            int iMaxThread = 5;
            WaitCallback waitCallback = new WaitCallback(DownThread);

            mrEvents = new ManualResetEvent[iMaxThread];
            ThreadPool.SetMaxThreads(10, iMaxThread);
            
            bool bDone = false;

            while (true)
            {
                for (int i = 0; i < iMaxThread; i++)
                {
                    ThreadPool.QueueUserWorkItem(waitCallback, i);
                    mrEvents[i] = new ManualResetEvent(false);
                    
                    if ((i + iBase) >= iLast)
                    {
                        bDone = true;
                        break;
                    }

                    PrintLog("Start Download Thread for page " + (i + iBase) + Environment.NewLine);
                }

                if (bDone)
                    break;

                WaitHandle.WaitAll(mrEvents);

                iBase += iMaxThread;
            }
            PrintLog("DownLoad All Pages Done !!"+Environment.NewLine);
        }

        private void parse_page(object d)
        {
            int iPage = Convert.ToInt32(d as string);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load("page/"+iNow+"_page" + iPage + ".txt", Encoding.UTF8);

            HtmlAgilityPack.HtmlDocument content = new HtmlAgilityPack.HtmlDocument();
            content.LoadHtml(doc.DocumentNode.SelectSingleNode("/html/body/*[@id='mainContent']/div[3]/div[1]").InnerHtml);
            StreamWriter sw = new StreamWriter("list/" + iNow + "_" + iPage + ".txt");

            HtmlNodeCollection nList =
         content.DocumentNode.SelectNodes("./div[@class='sw1'] | ./div[@class='sw2']");

            //this.textBox1.Text += "Page " + iPage + " analyze ok!!" + Environment.NewLine;
            PrintLog("Page " + iPage + " analyze ok!!" + Environment.NewLine);
            if (nList == null)
            {
                PrintLog("Page " + iPage + " error !!" + Environment.NewLine);
                sw.Close();
                doc = null;
                content = null;
                return;
            }
            //mut.ReleaseMutex();
            String baseurl = "http://big5.qidian.com";
            foreach (HtmlNode ent in nList)
            {
                int iID = Convert.ToInt32(ent.SelectSingleNode("./div[1]").InnerText.ToString());
                String sTag = ent.SelectSingleNode("./div[2]").InnerText.ToString();
                String sName = ent.SelectSingleNode("./div[3]/*[@class='swbt']").InnerText.ToString();
                String sUrl = baseurl + ent.SelectSingleNode("./div[3]/*[@class='swbt']//a[1]").Attributes["href"].Value.ToString();
                int iWord = Convert.ToInt32(ent.SelectSingleNode("./div[4]").InnerText.ToString());
                String sAuthor = ent.SelectSingleNode("./div[5]").InnerText.ToString();
                String sUpdateTime = ent.SelectSingleNode("./div[6]").InnerText.ToString();

                string[] sRes = sUrl.Split(cParse);
                if (!File.Exists("book/" + sRes[6] + ".txt"))
                {
                    //webClient.DownloadFile(new Uri(sUrl), @"book/" + sRes[6] + @".txt");
                    //webClient.Dispose();
                    PrintLog("Book " + sRes[6] + "(" + sName + ") Need Deeper Search ~" + Environment.NewLine);
                }

                sw.WriteLine(String.Format("{0},{1},{2},{3},{4},{5},{6},{7}", sRes[6], iID, sTag, sName, sUrl, iWord, sAuthor, sUpdateTime));
            }
            //mut.WaitOne();
            //this.textBox1.Text += "Page " + iPage + "Done !!" + Environment.NewLine;
            PrintLog("Page " + iPage + "Done !!" + Environment.NewLine);
            //mut.ReleaseMutex();
            sw.Close();
            doc = null;
            content = null;
        }

        private void analyzebutton_Click(object sender, EventArgs e)
        {
            this.textBox1.Clear();
            int iMaxThread = 30;
            WaitCallback waitCallback = new WaitCallback(parse_page);

            mrpEvents = new ManualResetEvent[iMaxThread];
            ThreadPool.SetMaxThreads(10, iMaxThread);

            bool bDone = false;
            int temp = 2;
            while (true)
            {
                for (int i = 0; i < iMaxThread; i++)
                {
                    ThreadPool.QueueUserWorkItem(waitCallback, (i+temp));
                    mrpEvents[i] = new ManualResetEvent(false);

                    if (i + temp >= iLast)
                    {
                        bDone = true;
                        break;
                    }

                    PrintLog("Start Analyze Thread for page " + (i + temp) + Environment.NewLine);
                }

                if (bDone)
                    break;

                    WaitHandle.WaitAll(mrpEvents);

                temp += iMaxThread;
            }
            PrintLog("DownLoad All Pages Done !!" + Environment.NewLine);
        }
    }
}
