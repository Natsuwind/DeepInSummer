using System;
namespace LiteCMS.Entity
{
    public class ColumnInfo
    {
        private int m_columnid;

        public int Columnid
        {
            get { return m_columnid; }
            set { m_columnid = value; }
        }
        private string m_columnname;

        public string Columnname
        {
            get { return m_columnname; }
            set { m_columnname = value; }
        }

        private int m_parentid;

        public int Parentid
        {
            get { return m_parentid; }
            set { m_parentid = value; }
        }
        private string m_description;

        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }
        private string m_allowpost;

        public string Allowpost
        {
            get { return m_allowpost; }
            set { m_allowpost = value; }
        }
        private string m_allowedit;

        public string Allowedit
        {
            get { return m_allowedit; }
            set { m_allowedit = value; }
        }
        private string m_allowdel;

        public string Allowdel
        {
            get { return m_allowdel; }
            set { m_allowdel = value; }
        }
        private int m_shenghe;

        public int Shenghe
        {
            get { return m_shenghe; }
            set { m_shenghe = value; }
        }
    }
}
