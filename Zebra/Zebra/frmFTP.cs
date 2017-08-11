using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Zebra
{
    public partial class frmFTP : Form
    {
        public string dir = @"ftp://192.168.58.128/";
        List<FileStruct> ls = new List<FileStruct>();
        FTPHelper ftp = new FTPHelper("TT", "citic");
        public frmFTP()
        {
            InitializeComponent();
        }

        private void frmFTP_Load(object sender, EventArgs e)
        {
            
            ls = ftp.ListDirectories(dir);
            show();

        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = listView1.HitTest(e.X, e.Y);
            if (info.Item != null)
            {
                //MessageBox.Show(info.Item.Text);
                for (int i = 0; i < ls.Count; i++)
                {
                    if (ls[i].Name == info.Item.Text)
                    {
                        if (ls[i].IsDirectory == true)
                        {
                            dir = dir + @"/" + info.Item.Text;
                            ls = ftp.ListDirectories(dir);
                            show();
                        }
                        else
                        {
                            MessageBox.Show(info.Item.Text + "不是路径");
                        }
                    }
                }
                
            }
        }

        private void show()
        {
            int index = 0;
            List<ListViewItem> listBuffer = new List<ListViewItem>();
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(10, 20);// 设置行高 20 //分别是宽和高  
            listView1.LargeImageList = imgList; //这里设置listView的SmallImageList ,用imgList将其撑大  
            foreach (var item in ls)
            {
                ListViewItem li = new ListViewItem();
                li.ImageIndex = 0;
                li.SubItems[0].Text = item.Name;
                li.Tag = item;
                //li.SubItems=
                li.ForeColor = item.IsDirectory == true ? Color.Green : Color.Blue;
                listBuffer.Add(li);

                if (index++ % 1000 == 0)
                {
                    listView1.Items.AddRange(listBuffer.ToArray());
                    listBuffer.Clear();
                }
                if (index % 50 == 0)
                {
                    Application.DoEvents();
                }

            }
            listView1.Items.AddRange(listBuffer.ToArray());
        }
    }
}
