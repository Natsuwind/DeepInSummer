using System;
using System.Drawing;
using System.Text;
using BallotAiying2;
using System.Collections.Generic;
using Jyi.Entity;


namespace Jyi.Utility
{
    class UnCode : UnCodebase
    {        
        public UnCode(Bitmap pic)
            : base(pic)
        {
        }

        public string getPicnum(List<UnCodeInfo> unCodeList)
        {
            GrayByPixels(); //灰度处理
            GetPicValidByValue(128, 4); //得到有效空间
            Bitmap[] pics = GetSplitPics(4, 1);     //分割

            if (pics.Length != 4)
            {
                return ""; //分割错误
            }
            else  // 重新调整大小
            {
                pics[0] = GetPicValidByValue(pics[0], 128);
                pics[1] = GetPicValidByValue(pics[1], 128);
                pics[2] = GetPicValidByValue(pics[2], 128);
                pics[3] = GetPicValidByValue(pics[3], 128);
            }

            //      if (!textBoxInput.Text.Equals(""))
            string result = "";
            char singleChar = ' ';

            for (int i = 0; i < 4; i++)
            {
                string code = GetSingleBmpCode(pics[i], 128);   //得到代码串

                foreach (UnCodeInfo objUnCode in unCodeList)
                {
                    if (objUnCode.ImgCode == code)
                    {
                        result += objUnCode.Code;
                    }
                }
            }

            return result;
        }
    }
}
