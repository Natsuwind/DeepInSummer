﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace Ywen.ValidationCodeRobot
{
    public partial class PreForm : Form
    {
        Image numPic;
        public PreForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = cbbxImageUrl.Text.Trim();
            HttpWebRequest objImgRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            objImgRequest.Method = "GET";
            objImgRequest.CookieContainer = new CookieContainer();
            WebResponse wr2 = objImgRequest.GetResponse();
            System.IO.Stream s = wr2.GetResponseStream();
            numPic = System.Drawing.Image.FromStream(s);// 得到验证码图片
            pictureBox1.Image = numPic;
            textBox1.Text = numPic.Width.ToString();
            textBox2.Text = numPic.Height.ToString();
            textBox3.Text = "0";
            textBox4.Text = "0";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //从文件获取原始图片，并使用流中嵌入的颜色管理信息
            Image sourceImage = this.numPic.Clone() as Image;
            int pickedImageWidth = Convert.ToInt32(textBox1.Text);
            int pickedImageHeigh = Convert.ToInt32(textBox2.Text);
            int pickedX = Convert.ToInt32(textBox3.Text);
            int pickedY = Convert.ToInt32(textBox4.Text);

            Image pickedImage = CutImage(
                sourceImage,
                pickedImageWidth,
                pickedImageHeigh,
                pickedX,
                pickedY
                );


            pictureBox1.Image = pickedImage.Clone() as Image;
            //释放资源
            sourceImage.Dispose();
            pickedImage.Dispose();
        }

        private static Image CutImage(Image sourceImage, int pickedImageWidth, int pickedImageHeigh, int pickedX, int pickedY)
        {
            //裁剪对象实例化
            Image cutImage = new System.Drawing.Bitmap(pickedImageWidth, pickedImageHeigh);
            Graphics cutGraphics = System.Drawing.Graphics.FromImage(cutImage);
            //定位
            Rectangle fromR = new Rectangle(0, 0, 0, 0);//原图裁剪定位
            Rectangle toR = new Rectangle(0, 0, 0, 0);//目标定位
            //裁剪源定位
            fromR.X = pickedX;
            fromR.Y = pickedY;
            fromR.Width = pickedImageWidth;
            fromR.Height = pickedImageHeigh;
            //裁剪目标定位
            toR.X = 0;
            toR.Y = 0;
            toR.Width = pickedImageWidth;
            toR.Height = pickedImageHeigh;
            //设置质量
            cutGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            cutGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //裁剪
            cutGraphics.DrawImage(sourceImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);

            cutGraphics.Dispose();
            return cutImage;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<UnCodeInfo> codeList = new List<UnCodeInfo>();
            codeList.Add(new UnCodeInfo("2", "111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111000000111111111100000000001111111100000000001111111000001110000111111000011110000111111111111110000111111111111100000111111111111000001111111111110000011111111111100000111111111111000001111111111110000011111111111100000111111111111000001111111111111000000000000111111000000000000111111000000000000111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"));
            codeList.Add(new UnCodeInfo("7", "111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111000000000001111111000000000001111111000000000001111111111111100001111111111111000011111111111111000011111111111111000011111111111110000111111111111110000111111111111110000111111111111100001111111111111100001111111111111000011111111111111000011111111111111000011111111111110000111111111111110000111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"));

            codeList.Add(new UnCodeInfo("5", "111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111100000000001111111100000000001111111100000000001111111100011111111111111000011111111111111000011111111111111000000000011111111000000000001111111000000000001111111000011110000111111111111110000111111111111110000111111111111110000111111000011110000111111000001100001111111100000000011111111110000000111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"));
            codeList.Add(new UnCodeInfo("0", "111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111000001111111111110000000011111111100000000011111111100011100001111111000011100001111111000011100001111111000011110000111111000011110000111111000011110000111111000011110000111111000011110000111111000011100000111111000011100001111111000011100001111111100001000001111111100000000011111111111000000111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"));
            codeList.Add(new UnCodeInfo("1", "111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111000111111111111100000111111111110000000111111111110000000111111111110000000111111111110011000111111111111111000111111111111111000111111111111111000111111111111111000111111111111111000111111111111111000111111111111111000111111111111111000111111111111111000111111111111111000111111111111111000111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"));
            codeList.Add(new UnCodeInfo("6", "111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111000000111111111110000000001111111100000000001111111100001100001111111000011110001111111000011111111111111000010000111111111000000000011111111000000000001111111000011100000111111000011110000111111000011110000111111000011110000111111000011110000111111100001100001111111100000000001111111110000000111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"));
            codeList.Add(new UnCodeInfo("3", "111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111000000111111111100000000011111111100000000001111111000001100000111111000011110000111111111111110000111111111111100001111111111100000011111111111100000111111111111100000001111111111111100001111111111111110000111111000011110000111111000011100000111111000001100001111111100000000001111111110000000111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"));
            codeList.Add(new UnCodeInfo("8", "111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111000000111111111100000000011111111000000000001111111000011110000111111000011110000111111000011110001111111000001100001111111100000000011111111110000000111111111100000000001111111000011100001111111000011110000111111000111110000111111000011110000111111000001100001111111100000000001111111110000000111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"));
            codeList.Add(new UnCodeInfo("4", "111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111000011111111111111000011111111111110000011111111111100000011111111111100000011111111111000000011111111110000000011111111110001000011111111100011000011111111000011000011111111000111000011111111000000000000011111000000000000011111000000000000111111111111000011111111111111000011111111111111000011111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"));
            codeList.Add(new UnCodeInfo("9", "111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111000000111111111100000000011111111000000000001111111000011100001111110000111100001111110000111110001111110000011100001111111000011100000111111000000000000111111100000000000111111111000010001111111111111110001111111111111100001111111000011100001111111000011000011111111000000000011111111100000000111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"));
            
            Image i1 = CutImage(this.numPic, 18, 40, 52, 2);
            Image i2 = CutImage(this.numPic, 18, 40, 88, 2);
            this.pictureBox2.Image = i1;
            this.pictureBox3.Image = i2;
            //MessageBox.Show("aaaaa");

            //Image i1i2 = new Bitmap(i1.Width + i2.Width, 40);
            //Graphics g = Graphics.FromImage(i1i2);
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //g.DrawImage(i1i2, new Point(0, 0));
            //g.DrawImage(i1i2, new Point(i1.Width, 0));
            //g.Dispose();

            UnCode uc = new UnCode(i1 as Bitmap);
            label1.Text = uc.getPicnum(codeList);

            uc = new UnCode(i2 as Bitmap);
            label2.Text = uc.getPicnum(codeList);
        }
    }
}
