using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SQLite;


namespace WatchingSystemFiles
{

    public class WatchSystemFile
    {
        SqliteLibrary.SqliteHelper db = new SqliteLibrary.SqliteHelper("Data Source=" + System.Environment.CurrentDirectory.ToString() + @"\record.db");
        getIP getipclass = new getIP();
        //private FileSystemWatcher _watcher = null;
        private FileSystemWatcher _watcher = new FileSystemWatcher("C:\\");
        private string _path = "C:\\";   //监视目录
        private string _filter = "*.txt|*.doc|*.jpg|*.bmp|*.doc|*.docx";   //设置监控文件的类型  
        private bool _isWatch = false;


        /// <summary>
        /// 监控是否正在运行
        /// </summary>
        public bool IsWatch
        {
            get
            {
                return _isWatch;
            }
        }

        public void Open()
        {
            _watcher.Filter = _filter;
            _watcher.Path = _path;
            
            _watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size;
            _watcher.Created += new FileSystemEventHandler(fsWatcher_Created);
            _watcher.Changed += new FileSystemEventHandler(fsWatcher_Changed);
            _watcher.Deleted += new FileSystemEventHandler(fsWatcher_Deleted);
            _watcher.IncludeSubdirectories = true;
            _watcher.EnableRaisingEvents = true;
            _isWatch = true;
        }
        public void watchingFile()
        {
            FileSystemWatcher fsWatcher = new FileSystemWatcher();
            fsWatcher.Filter = "*.txt|*.doc|*.jpg|*.bmp";   //设置监控文件的类型  
            fsWatcher.IncludeSubdirectories = true;//是否监视子目录
            fsWatcher.Path = "C:\\";   //设置监控的文件目录;//监视的目录
            
            fsWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size; 
            //文件变更时触发以下操作
            fsWatcher.Created += new FileSystemEventHandler(this.fsWatcher_Created);
            fsWatcher.Changed += new FileSystemEventHandler(this.fsWatcher_Changed);
            fsWatcher.Deleted += new FileSystemEventHandler(this.fsWatcher_Deleted);
            fsWatcher.EnableRaisingEvents = true;//是否可用
            _isWatch = true;
        }
        protected void fsWatcher_Created(object sender, FileSystemEventArgs e)
        {
            //定义新建文件时触发事件
            string insertstr="insert into log(recordType,detail) values('新增','文件名："+e.Name+",路径："+e.FullPath+",事件类型："+e.ChangeType+"')";
            db.ExecuteNonQuery(insertstr, System.Data.CommandType.Text, null);
        }
        protected void fsWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            //定义变更文件时触发事件
            string insertstr = "insert into log(recordType,detail) values('修改','文件名：" + e.Name + ",路径：" + e.FullPath + ",事件类型：" + e.ChangeType + "')";
            db.ExecuteNonQuery(insertstr, System.Data.CommandType.Text, null);
        }
        protected void fsWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            //定义删除文件时触发事件
            string insertstr = "insert into log(recordType,detail) values('删除','文件名：" + e.Name + ",路径：" + e.FullPath + ",事件类型：" + e.ChangeType + "')";
            db.ExecuteNonQuery(insertstr, System.Data.CommandType.Text, null);
        }
    }
}
