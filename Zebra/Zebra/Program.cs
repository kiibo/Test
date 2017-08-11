using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Data;

namespace Zebra
{

    static class Program
    {
     
        
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            SqliteLibrary.SqliteHelper db = new SqliteLibrary.SqliteHelper("Data Source=" + System.Environment.CurrentDirectory.ToString() + @"\record.db");
            DataTable dt=db.GetDataTable("select * from log");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Zebra());

        }

    }
}
