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
using System.ComponentModel;
using System.IO;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace MultiFileUpload.Classes
{
    /// <summary>
    /// 上传文件信息类
    /// </summary>
    public class UserFile : INotifyPropertyChanged
    {
        /// <summary>
        /// 上传文件名称
        /// </summary>
        private string _fileName;
        /// <summary>
        /// 是否取消上传该文件
        /// </summary>
        private bool _isDeleted = false;      
        /// <summary>
        /// 上传文件的流信息
        /// </summary>
        private Stream _fileStream;
        /// <summary>
        /// 浏览的流信息
        /// </summary>
        private Stream _viewStream;
        /// <summary>
        /// 当前上传文件状态
        /// </summary>
        private Constants.FileStates _state = Constants.FileStates.Pending;
        /// <summary>
        /// 当前已上传的字节数（这里与FileCollection中的同名属性意义不同，FileCollection中的是已上传的所有文件的字节总数）
        /// </summary>
        private double _bytesUploaded = 0;
        /// <summary>
        /// 当前文件大小
        /// </summary>
        private double _fileSize = 0;
        /// <summary>
        /// 已上传文件的百分比
        /// </summary>
        private int _percentage = 0;
        /// <summary>
        /// 上传文件操作类
        /// </summary>
        private FileUploader _fileUploader;

        /// <summary>
        /// 上传文件名称
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                NotifyPropertyChanged("FileName");
            }
        }

        /// <summary>
        /// 当前上传文件的状态，注意这时使用了NotifyPropertyChanged来通知FileRowControl控件中的FileRowControl_PropertyChanged事件
        /// </summary>
        public Constants.FileStates State
        {
            get { return _state; }
            set
            {
                _state = value;
                NotifyPropertyChanged("State");
            }
        }

        /// <summary>
        /// 当前上传文件是否已被移除，注意这时使用了NotifyPropertyChanged来通知FileCollection类中的item_PropertyChanged事件
        /// </summary>
        public bool IsDeleted
        {
            get { return _isDeleted; }
            set
            {
                _isDeleted = value;

                if (_isDeleted)
                    CancelUpload();

                NotifyPropertyChanged("IsDeleted");
            }
        }

        /// <summary>
        /// 上传文件的流信息
        /// </summary>
        public Stream FileStream
        {
            get { return _fileStream; }
            set
            {
                _fileStream = value;

                if (_fileStream != null)
                    _fileSize = _fileStream.Length;
            }
        }

        /// <summary>
        /// 浏览的流信息
        /// </summary>
        public Stream ViewStream
        {
            get { return _viewStream; }
            set
            {
                _viewStream = value;
            }
        }
        

        /// <summary>
        /// 当前文件大小
        /// </summary>
        public double FileSize
        {
            get { return _fileSize; }
        }

        /// <summary>
        /// 当前已上传的字节数（这里与FileCollection中的同名属性意义不同，FileCollection中的是已上传的所有文件的字节总数）
        /// </summary>
        public double BytesUploaded
        {
            get { return _bytesUploaded; }
            set
            {
                _bytesUploaded = value;
                NotifyPropertyChanged("BytesUploaded");
                Percentage = (int)((value * 100) / _fileStream.Length);
            }
        }

        /// <summary>
        /// 已上传文件的百分比（这里与FileCollection中的同名属性意义不同，FileCollection中的是已上传字符数占全部字节数的百分比,该字段的修改事件通知会发给page.xmal中的TotalProgress）
        /// </summary>
        public int Percentage
        {
            get { return _percentage; }
            set
            {
                _percentage = value;
                NotifyPropertyChanged("Percentage");
            }
        }

      
        /// <summary>
        /// 上传当前文件
        /// </summary>
        /// <param name="initParams"></param>
        public void Upload(string initParams)
        {
            this.State = Constants.FileStates.Uploading;

            _fileUploader = new FileUploader(this);            
            _fileUploader.UploadAdvanced(initParams);
            _fileUploader.UploadFinished += new EventHandler(fileUploader_UploadFinished);            
        }

        /// <summary>
        /// 取消上传，注：该文件仅在本类中的IsDeleted属性中使用
        /// </summary>
        public void CancelUpload()
        {
            if (_fileUploader != null && this.State == Constants.FileStates.Uploading)
                _fileUploader.CancelUpload();
        }

        /// <summary>
        /// 当前文件上传完成时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void fileUploader_UploadFinished(object sender, EventArgs e)
        {
            _fileUploader = null;
            this.State = Constants.FileStates.Finished;
        }


        #region INotifyPropertyChanged Members

        private void NotifyPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
