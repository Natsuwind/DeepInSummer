using System;

namespace LiteCMS.Entity
{
    public class ArticleInfo
    {
        private int m_Articleid;

        public int Articleid
        {
            get { return m_Articleid; }
            set { m_Articleid = value; }
        }
        private int m_Columnid;

        public int Columnid
        {
            get { return m_Columnid; }
            set { m_Columnid = value; }
        }
        private string m_ColumnName;

        public string Columnname
        {
            get { return m_ColumnName; }
            set { m_ColumnName = value; }
        }
        private string m_Title;

        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }
        private string m_Summary;

        public string Summary
        {
            get { return m_Summary; }
            set { m_Summary = value; }
        }
        private int m_Uid;

        public int Uid
        {
            get { return m_Uid; }
            set { m_Uid = value; }
        }
        private string m_Username;

        public string Username
        {
            get { return m_Username; }
            set { m_Username = value; }
        }
        private string m_Postdate;

        public string Postdate
        {
            get { return m_Postdate; }
            set { m_Postdate = value; }
        }
        private int m_Commentcount;

        public int Commentcount
        {
            get { return m_Commentcount; }
            set { m_Commentcount = value; }
        }
        private int m_Viewcount;

        public int Viewcount
        {
            get { return m_Viewcount; }
            set { m_Viewcount = value; }
        }
        private int m_Sort;

        public int Sort
        {
            get { return m_Sort; }
            set { m_Sort = value; }
        }
        private string m_Ip;

        public string Ip
        {
            get { return m_Ip; }
            set { m_Ip = value; }
        }
        private string m_Image;

        public string Image
        {
            get { return m_Image; }
            set { m_Image = value; }
        }
        private int m_Del;

        public int Del
        {
            get { return m_Del; }
            set { m_Del = value; }
        }
        private string m_Content;

        public string Content
        {
            get { return m_Content; }
            set { m_Content = value; }
        }
        private int m_Goodcount;

        public int Goodcount
        {
            get { return m_Goodcount; }
            set { m_Goodcount = value; }
        }
        private int m_Badcount;

        public int Badcount
        {
            get { return m_Badcount; }
            set { m_Badcount = value; }
        }
        private string m_Highlight = "";

        public string Highlight
        {
            get { return m_Highlight; }
            set { m_Highlight = value; }
        }

        public int Recommend { get; set; }
    }
}
