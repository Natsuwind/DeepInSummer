using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Entity
{
    /// <summary>
    /// 辩论主题的扩展类
    /// </summary>
    public class DebateInfo
    {
        #region 私有字段
        private int _tid;        
        private string _positiveopinion;        
        private string _negativeopinion;        
        //private string _positivecolor;        
        //private string _negativecolor;        
        private DateTime _terminaltime;
        private int _positivediggs;
        private int _negativediggs;
        private int _positivevote;
        private string _positivevoterids;
        private int _negativevote;
        private string _negativevoterids;
        //private string _positivebordercolor;       
        //private string _negativebordercolor;        
        #endregion

        #region 属性

        /// <summary>
        /// 主题Id
        /// </summary>
        public int Tid
        {
            get { return _tid; }
            set { _tid = value; }
        }

        /// <summary>
        /// 正方观点
        /// </summary>
        public string Positiveopinion
        {
            get { return _positiveopinion; }
            set { _positiveopinion = value; }
        }

        /// <summary>
        /// 反方观点
        /// </summary>
        public string Negativeopinion
        {
            get { return _negativeopinion; }
            set { _negativeopinion = value; }
        }

        ///// <summary>
        ///// 正方颜色
        ///// </summary>
        //public string Positivecolor
        //{
        //    get { return _positivecolor; }
        //    set { _positivecolor = value; }
        //}

        ///// <summary>
        ///// 反方颜色
        ///// </summary>
        //public string Negativecolor
        //{
        //    get { return _negativecolor; }
        //    set { _negativecolor = value; }
        //}

        /// <summary>
        /// 辩论结束时间
        /// </summary>
        public DateTime Terminaltime
        {
            get { return _terminaltime; }
            set { _terminaltime = value; }
        }

        /// <summary>
        /// 正方辩手数
        /// </summary>
        public int Positivediggs
        {
            get { return _positivediggs; }
            set { _positivediggs = value; }
        }
        
        /// <summary>
        /// 反方辩手数
        /// </summary>
        public int Negativediggs
        {
            get { return _negativediggs; }
            set { _negativediggs = value; }
        }

        /// <summary>
        /// 正方支持数
        /// </summary>
        public int Positivevote
        {
            get { return _positivevote; }
            set { _positivevote = value; }
        }

        /// <summary>
        /// 正方支持人Uid，例：uid1,uid2……
        /// </summary>
        public string Positivevoterids
        {
            get { return _positivevoterids; }
            set { _positivevoterids = value; }
        }

        /// <summary>
        /// 反方支持数
        /// </summary>
        public int Negativevote
        {
            get { return _negativevote; }
            set { _negativevote = value; }
        }

        /// <summary>
        /// 反方支持人Uid，例：uid1,uid2……
        /// </summary>
        public string Negativevoterids
        {
            get { return _negativevoterids; }
            set { _negativevoterids = value; }
        }

        ///// <summary>
        ///// 正方边框颜色
        ///// </summary>
        //public string Positivebordercolor
        //{
        //    get { return _positivebordercolor; }
        //    set { _positivebordercolor = value; }
        //}

        ///// <summary>
        ///// 反方边框颜色
        ///// </summary>
        //public string Negativebordercolor
        //{
        //    get { return _negativebordercolor; }
        //    set { _negativebordercolor = value; }
        //}
        #endregion
    }
}
