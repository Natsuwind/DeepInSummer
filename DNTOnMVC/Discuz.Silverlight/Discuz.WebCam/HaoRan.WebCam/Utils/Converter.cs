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
using System.Windows.Data;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace HaoRan.WebCam
{
    /// <summary>
    /// 字节转换
    /// </summary>
    public class ByteConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string size = "0 KB";

            if (value != null)
            {
                double byteCount = (double)value;
                
                if (byteCount >= 1073741824)
                    size = String.Format("{0:##.##}", byteCount / 1073741824) + " GB";
                else if (byteCount >= 1048576)
                    size = String.Format("{0:##.##}", byteCount / 1048576) + " MB";
                else if (byteCount >= 1024)
                    size = String.Format("{0:##.##}", byteCount / 1024) + " KB";
                else if (byteCount > 0 && byteCount < 1024)
                    size = "1 KB";    //Bytes are unimportant ;)            

            }

            return size;
        }

        //We only use one-way binding, so we don't implement this.
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// 状态转换
    /// </summary>
    public class StateConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string state = "0 KB";

            if (value != null)
            {
                switch ((Constants.FileStates)value)
                {
                    case Constants.FileStates.Pending: state = "暂停"; break;
                    case Constants.FileStates.Uploading: state = "上传中..."; break;
                    case Constants.FileStates.Finished: state = "结束"; break;
                    case Constants.FileStates.Deleted: state = "移除"; break;
                    case Constants.FileStates.Error: state = "错误"; break;
                    default: state = "未知"; break;
                }
            }

            return state;
        }

        //We only use one-way binding, so we don't implement this.
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
