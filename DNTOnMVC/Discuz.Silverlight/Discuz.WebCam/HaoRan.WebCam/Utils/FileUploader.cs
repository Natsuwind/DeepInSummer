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
using System.ServiceModel;
using System.IO;
using HaoRan.WebCam.MixObjectsSoapClient;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace HaoRan.WebCam
{
    /// <summary>
    /// 文件上传类
    /// </summary>
    public class FileUploader
    {
        private UserFile _file;
        private long _dataLength;
        private long _dataSent;
        private UploadAvatarSoapClient _client;
        private string _initParams;
        private bool _firstChunk = true;
        private bool _lastChunk = false;
        

        public FileUploader(UserFile file)
        {
            _file = file;

            _dataLength = _file.FileStream.Length;
            _dataSent = 0;

            _client = Utils.CreateServiceClient(); //new MixObjectsSoapClient("", "http://localhost/services/MixObjects.asmx");
            //事件绑定
            _client.SaveAvatarCompleted += new EventHandler<SaveAvatarCompletedEventArgs>(_client_StoreFileAdvancedCompleted);
            _client.ChannelFactory.Closed += new EventHandler(ChannelFactory_Closed);
        }

        #region
        /// <summary>
        /// 关闭ChannelFactory事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ChannelFactory_Closed(object sender, EventArgs e)
        {
            ChannelIsClosed();
        }

        void _client_CancelUploadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            //当取消上传完成后关闭Channel
            _client.ChannelFactory.Close();
        }

        /// <summary>
        /// Channel被关闭
        /// </summary>
        private void ChannelIsClosed()
        {
            if (!_file.IsDeleted)
            {
                if (UploadFinished != null)
                    UploadFinished(this, null);
            }
        }

        /// <summary>
        /// 取消上传
        /// </summary>
        //public void CancelUpload()
        //{
        //    _client.CancelUploadAsync(Utils.GetCredentialInfo(), _file.FileName, _initParams);
        //}
        #endregion

        /// <summary>
        /// 上传完成事件处理对象声明
        /// </summary>
        public event EventHandler UploadFinished;

        public void UploadAdvanced(string initParams)
        {
            _initParams = initParams;

            UploadAdvanced();
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        private void UploadAdvanced()
        {            
            byte[] buffer = new byte[4 * 4096];
            int bytesRead = _file.FileStream.Read(buffer, 0, buffer.Length);
            //MessageBox.Show("int" + bytesRead);
            //文件是否上传完毕?
            if (bytesRead != 0)
            {
                _dataSent += bytesRead;

                if (_dataSent == _dataLength)
                    _lastChunk = true;//是否是最后一块数据，这样WCF会在服务端根据该信息来决定是否对临时文件重命名

                //上传当前数据块
                _client.SaveAvatarAsync(_file.FileName, buffer, bytesRead, _initParams, _firstChunk, _lastChunk, Utils.GetCredentialInfo(), _file.GrabPoint, _file.GrabSize);

                 //在第一条消息之后一直为false
                _firstChunk = false;

                //通知上传进度修改
                OnProgressChanged();
            }
            else
            {
                //当上传完毕后
                _file.FileStream.Dispose();
                _file.FileStream.Close();
                _client.ChannelFactory.Close();          
            }
        }

        /// <summary>
        /// 修改进度属性
        /// </summary>
        private void OnProgressChanged()
        {
            _file.BytesUploaded = _dataSent;//注：此处会先调用FileCollection中的同名属性，然后才是_file.BytesUploaded属性绑定
        }

        void _client_StoreFileAdvancedCompleted(object sender, SaveAvatarCompletedEventArgs e)
        {
            //检查WEB服务是否存在错误
            if (e.Error != null)
            {
                //当错误时放弃上传
                _file.State = Constants.FileStates.Error;
            }
            else
            {
                //如果文件未取消上传的话，则继续上传
                if (!_file.IsDeleted)
                {
                    //if (e.Result != null && e.Result.Aid > 0)
                    {
                        //返回了已上传的附件信息，并添加到已上传附件列表中
                        //Page.AttachmentList.Add(e.Result);
                    }
                    UploadAdvanced();
                }
           }           
        }
    }
}
