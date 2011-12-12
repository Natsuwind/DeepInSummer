using System;
using System.Collections.Generic;
using System.Text;
using BallotAiying2;
using System.Drawing;

namespace Ywen.ValidationCodeRobot
{
    internal class UnCodeInfo
    {
        /// <summary>
        /// 实际字符串
        /// </summary>
        private string m_Code;
        /// <summary>
        /// 实际支付传
        /// </summary>
        public string Code
        {
            get { return m_Code; }
            set { m_Code = value; }
        }

        /// <summary>
        /// 图片码
        /// </summary>
        private string m_ImgCode;
        public string ImgCode
        {
            get { return m_ImgCode; }
            set { m_ImgCode = value; }
        }

        public UnCodeInfo(string code, string imgCode)
        {
            this.m_ImgCode = imgCode;
            this.m_Code = code;
        }
    }
    internal class UnCode : UnCodebase
    {
        public UnCode(Bitmap pic)
            : base(pic)
        {
        }

        public string getPicnum(List<UnCodeInfo> unCodeList)
        {
            GrayByPixels(); //灰度处理
            GetPicValidByValue(128, 4); //得到有效空间
            Bitmap[] pics = GetSplitPics(1, 1);     //分割

            if (pics.Length != 1)
            {
                return ""; //分割错误
            }
            else  // 重新调整大小
            {
                pics[0] = GetPicValidByValue(pics[0], 128);
            }

            string result = "";

            for (int i = 0; i < 1; i++)
            {
                string imgCode = GetSingleBmpCode(pics[i], 128);   //得到代码串

                foreach (UnCodeInfo objUnCode in unCodeList)
                {
                    if (objUnCode.ImgCode == imgCode)
                    {
                        result += objUnCode.Code;
                        break;
                    }
                }
            }

            return result;
        }
    }
}
