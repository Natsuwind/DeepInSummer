using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace MultiFileUpload.Classes
{
    /// <summary>
    /// 文件集合管理类
    /// 注：ObservableCollection是个泛型集合类，往其中添加或去除条目时（或者其中的条目实现了INotifyPropertyChanged的话，在属性变动时），
    /// 它会发出变化通知事件（先执行集合类中的同名属性）。这在做数据绑定时会非常方便，因为UI控件可以使用这些通知来知道自动刷新它们的值，而不用开发人员编写代码来
    /// 显式地这么做。
    /// </summary>
    public class FileCollection : ObservableCollection<UserFile>
    {
        /// <summary>
        /// 已上传的累计（多文件）字节数
        /// </summary>
        private double _bytesUploaded = 0;
        /// <summary>
        /// 已上传字符数占全部字节数的百分比
        /// </summary>
        private int _percentage = 0;
        /// <summary>
        /// 当前正在上传的文件序号
        /// </summary>
        private int _currentUpload = 0;
        /// <summary>
        /// 上传初始化参数，详情如下：
        /// MaxFileSizeKB: 	File size in KBs.
        /// MaxUploads: 	Maximum number of simultaneous uploads
        /// FileFilter:	File filter, for example ony jpeg use: FileFilter=Jpeg (*.jpg) |*.jpg
        /// CustomParam: Your custom parameter, anything here will be available in the WCF webservice
        /// DefaultColor: The default color for the control, for example: LightBlue
        /// </summary>
        private string _customParams;
        /// <summary>
        /// 最大上传字节数
        /// </summary>
        private int _maxUpload;
        
        /// <summary>
        /// 已上传的累计（多文件）字节数,该字段的修改事件通知会发给page.xmal中的TotalKB
        /// </summary>
        public double BytesUploaded
        {
            get { return _bytesUploaded; }
            set
            {
                _bytesUploaded = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("BytesUploaded"));
            }
        }

        /// <summary>
        /// 已上传字符数占全部字节数的百分比,该字段的修改事件通知会发给page.xmal中的TotalProgress
        /// </summary>
        public int Percentage
        {
            get { return _percentage; }
            set
            {
                _percentage = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("Percentage"));
            }
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="customParams"></param>
        /// <param name="maxUploads"></param>
        public FileCollection(string customParams, int maxUploads)
        {
            _customParams = customParams;
            _maxUpload = maxUploads;

            this.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(FileCollection_CollectionChanged);
        }

       
        /// <summary>
        /// 依次加入所选的上传文件信息
        /// </summary>
        /// <param name="item"></param>
        public new void Add(UserFile item)
        {
            item.PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);            
            base.Add(item);
        }

        /// <summary>
        /// 单个上传文件属性改变时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //当属性变化为“从上传列表中移除”
            if (e.PropertyName == "IsDeleted")
            {
                UserFile file = (UserFile)sender;

                if (file.IsDeleted)
                {
                    if (file.State == Constants.FileStates.Uploading)
                    {
                        _currentUpload--;
                        UploadFiles();
                    }

                    this.Remove(file);

                    file = null;
                }
            }
            //当属性变化为“开始上传”
            else if (e.PropertyName == "State")
            {
                UserFile file = (UserFile)sender;
                //此时file.State状态为ploading
                if (file.State == Constants.FileStates.Finished || file.State == Constants.FileStates.Error)
                {
                    _currentUpload--;
                    UploadFiles();
                }
            }
            //当属性变化为“上传进行中”
            else if (e.PropertyName == "BytesUploaded")
            {
                //重新计算上传数据
                RecountTotal();
            }
        }
     

        /// <summary>
        /// 上传文件
        /// </summary>
        public void UploadFiles()
        {
            lock (this)
            {
                foreach (UserFile file in this)
                {   //当上传文件未被移除（IsDeleted）或是暂停时
                    if (!file.IsDeleted && file.State == Constants.FileStates.Pending && _currentUpload < _maxUpload)
                    {
                        file.Upload(_customParams);
                        _currentUpload++;
                    }
                }
            }

        }

        /// <summary>
        /// 重新计算数据
        /// </summary>
        private void RecountTotal()
        {
            //Recount total
            double totalSize = 0;
            double totalSizeDone = 0;

            foreach (UserFile file in this)
            {
                totalSize += file.FileSize;
                totalSizeDone += file.BytesUploaded;
            }

            double percentage = 0;

            if (totalSize > 0)
                percentage = 100 * totalSizeDone / totalSize;

            BytesUploaded = totalSizeDone; 

            Percentage = (int)percentage;
        }

        /// <summary>
        /// 当添加或取消上传文件时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FileCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //当集合信息变化时（添加或删除项）时，则重新计算数据 
            RecountTotal();
        }

    
    }
}
