using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PollChart
{
    public class SectorData
    {
        #region Properties
        private int _id;
        private double _value;
        private string _title;
        private string _comment;

        #endregion

        #region Constructor
        public SectorData()
        {

        }
        public SectorData(int pId, string pTitle, string pComment, double pValue)
        {
            _id = pId;
            _value = pValue;
            _title = pTitle;
            _comment = pComment;
        }

        #endregion

        #region Methods
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        #endregion
    }
}
