using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Natsuhime
{
    public class TinyRandom
    {
        private static char[] constant =
{
'0','1','2','3','4','5','6','7','8','9',
'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'

};
        public enum RandomType
        {
            All,
            Number,
            Uppercased,
            Lowercased,
            NumberAndUppercased,
            NumberAndLowercased,
            UppercasedAndLowercased,
        }
        public static string GenerateRandom(int Length, RandomType rt)
        {
            int initsize = 0;
            int beginsize = 0;
            int endsize = 0;
            Boolean IsCross = false;
            #region 判断是哪种类型
            switch (rt)
            {
                case RandomType.All:
                    {
                        initsize = 62;
                        beginsize = 1;
                        endsize = 62;
                        //IsCross = false;
                        break;
                    }
                case RandomType.Lowercased:
                    {
                        initsize = 26;
                        beginsize = 11;
                        endsize = 36;
                        //IsCross = false;
                        break;
                    }
                case RandomType.Uppercased:
                    {
                        initsize = 26;
                        beginsize = 37;
                        endsize = 62;
                        // IsCross = false;
                        break;
                    }
                case RandomType.Number:
                    {
                        initsize = 10;
                        beginsize = 1;
                        endsize = 10;
                        //IsCross = false;
                        break;
                    }
                case RandomType.UppercasedAndLowercased:
                    {
                        initsize = 52;
                        beginsize = 11;
                        endsize = 62;
                        //IsCross = false;
                        break;
                    }
                case RandomType.NumberAndLowercased:
                    {
                        initsize = 36;
                        beginsize = 1;
                        endsize = 36;
                        //IsCross = false;
                        break;
                    }
                case RandomType.NumberAndUppercased:
                    {
                        IsCross = true;
                        break;
                    }
            }
            #endregion

            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(initsize);
            Random rd = new Random();
            if (!IsCross)
            {
                for (int i = 0; i < Length; i++)
                {
                    newRandom.Append(constant[rd.Next(beginsize, endsize)]);
                }
            }
            else
            {
                for (int i = 0; i < Length; i++)
                {
                    newRandom.Append(constant[rd.Next(1, 10)]);
                    newRandom.Append(constant[rd.Next(37, 62)]);
                }
            }

            return newRandom.ToString();
        } 

        //随机数
        //private static char[] constant =  
        //{  
        //'0','1','2','3','4','5','6','7','8','9',  
        //'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',  
        //'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'  
        //};
        public static string GenerateRandom(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(62)]);
            }
            return newRandom.ToString();
        }
    }
}
