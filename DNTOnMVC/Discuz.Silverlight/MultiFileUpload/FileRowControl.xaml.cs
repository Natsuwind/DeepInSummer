using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MultiFileUpload.Classes;
using System.Windows.Media.Imaging;
/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace MultiFileUpload
{
    public partial class FileRowControl : UserControl
    {

        private UserFile UserFile
        {
            get
            {
                if (this.DataContext != null)
                    return ((UserFile)this.DataContext);
                else
                    return null;
            }
        }

        public FileRowControl()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(FileRowControl_Loaded);
        }


        void FileRowControl_Loaded(object sender, RoutedEventArgs e)
        {
            //定制UserFile的PropertyChanged 属性，如BytesUploaded，Percentage，IsDeleted
            UserFile.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(FileRowControl_PropertyChanged);     
        }

        void FileRowControl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "State")
            {
                //当前文件上传完毕后显示灰字
                if (this.UserFile.State == Constants.FileStates.Finished)
                {
                    GreyOutText();
                    ShowValidIcon();
                    if (FileUploadFinish != null)
                    {
                        FileUploadFinish(this, new UploadArgs()
                        {
                            FileInfo = this.UserFile
                        });
                    }
                }

                //如上传失败显示错误信息
                if (this.UserFile.State == Constants.FileStates.Error)
                {
                    ErrorMsgTextBlock.Visibility = Visibility.Visible;

                    if (FileCancelUpload != null)
                    {
                        FileCancelUpload(this, new UploadArgs()
                        {
                            FileInfo = this.UserFile
                        });
                    }
                }
            }

        }

        private void ShowValidIcon()
        {
            PercentageProgress.Visibility = Visibility.Collapsed;
            ValidUploadIcon.Visibility = Visibility.Visible;
            CancelUpload.Visibility = Visibility.Collapsed;         
        }

        private void GreyOutText()
        {
            SolidColorBrush grayBrush = new SolidColorBrush(Colors.Gray);

            FileNameTextBlock.Foreground = grayBrush;
            StateTextBlock.Foreground = grayBrush;
            FileSizeTextBlock.Foreground = grayBrush;           
        }


        public event EventHandler<UploadArgs> FileCancelUpload;

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            UserFile file = (UserFile)((Button)e.OriginalSource).DataContext;

            file.IsDeleted = true;
            
            this.Visibility = Visibility.Collapsed;

            if (FileCancelUpload != null)
            {
                FileCancelUpload(this, new UploadArgs()
                {
                    FileInfo = file
                });
            }
        }

        public event EventHandler<FileMouseEventArgs> FileMouseEnter;

        private void FileNameTextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            if (FileMouseEnter != null)
            {
                FileMouseEnter(this, new FileMouseEventArgs()
                {
                    X = e.GetPosition(null).X,
                    Y = e.GetPosition(null).Y, 
                    FileInfo = (UserFile)((TextBlock)e.OriginalSource).DataContext
                });
            }
        }

        public event EventHandler<FileMouseEventArgs> FileMouseLeave;

        private void FileNameTextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            if (FileMouseLeave != null)
            {
                FileMouseLeave(this, new FileMouseEventArgs()
                {
                    X = e.GetPosition(null).X,
                    Y = e.GetPosition(null).Y, 
                    FileInfo = (UserFile)((TextBlock)e.OriginalSource).DataContext
                });
            }
        }

        public event EventHandler<UploadArgs> FileUploadFinish;

    }

    public class UploadArgs : System.EventArgs
    {
        public UserFile FileInfo { set; get; }
    }


    public class FileMouseEventArgs : UploadArgs
    {
        public double X {set;get;}

        public double Y {set;get;}
    }     
}
