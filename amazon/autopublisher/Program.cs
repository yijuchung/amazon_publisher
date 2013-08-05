using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;


namespace parser
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Form.CheckForIllegalCrossThreadCalls = false;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //Form1.CheckForIllegalCrossThreadCalls = false;
            
        }
    }
}
