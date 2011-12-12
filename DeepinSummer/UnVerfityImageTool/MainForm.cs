using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BallotAiying2;
using System.Net;
using System.IO;

namespace UnVerfityImageTool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnClearSource_Click(object sender, EventArgs e)
        {
            int verfitycodecharcount = Convert.ToInt32(this.tbxVerfityCodeCharCount.Text);

            UnCodebase uc = new UnCodebase((Bitmap)pbVerfityImageSource.Image);
            uc.GrayByPixels(); //灰度处理
            uc.GetPicValidByValue(128, verfitycodecharcount); //得到有效空间
            Bitmap[] pics = uc.GetSplitPics(verfitycodecharcount, 1);     //分割

            if (pics.Length != verfitycodecharcount)
            {
                MessageBox.Show("分割错误,数量不是预期的" + verfitycodecharcount + "个"); //分割错误
                return;
            }
            else  // 重新调整大小
            {
                pics[0] = uc.GetPicValidByValue(pics[0], 128);
                pics[1] = uc.GetPicValidByValue(pics[1], 128);
                pics[2] = uc.GetPicValidByValue(pics[2], 128);
                pics[3] = uc.GetPicValidByValue(pics[3], 128);
            }


            int newbitmaphight = 0;
            int newbitmapwith = 0;

            int lasty = 0;
            foreach (Bitmap bmp in pics)
            {
                newbitmapwith += bmp.Width;
                if (bmp.Height > newbitmaphight)
                {
                    newbitmaphight = bmp.Height;
                }

                PictureBox pb = new PictureBox();
                pb.Image = (Image)bmp;
                pb.Location = new Point(0, lasty);
                pb.Size = new Size(bmp.Width, bmp.Height);
                this.plClearedImageList.Controls.Add(pb);

                Label lb = new Label();
                lb.Text = uc.GetSingleBmpCode(bmp, 128);
                lb.Location = new Point(66, lasty);
                lb.Size = new Size(500, bmp.Height + 5);
                this.plClearedImageList.Controls.Add(lb);

                TextBox tb = new TextBox();
                tb.Location = new Point(lb.Location.X + lb.Size.Width + 10, lasty);
                tb.Size = new Size(20, bmp.Height + 5);
                this.plClearedImageList.Controls.Add(tb);

                lasty += bmp.Height + 5;
            }

            Bitmap newBitmap = new Bitmap(newbitmapwith, newbitmaphight);

            Graphics g = Graphics.FromImage(newBitmap);
            g.Clear(System.Drawing.Color.White);
            int x = 0;
            foreach (Bitmap newbmp in pics)
            {
                g.DrawImage(newbmp, x, newbmp.Height);
                x += newbmp.Width;
            }
            pbVerfityImageClear.Image = (Image)newBitmap;
            //newBitmap.Dispose();
            //g.Dispose();

            //      if (!textBoxInput.Text.Equals(""))
            //string result = "";
            //char singleChar = ' ';
            //{
            //    for (int i = 0; i < 4; i++)
            //    {
            //        string code = GetSingleBmpCode(pics[i], 128);   //得到代码串

            //        for (int arrayIndex = 0; arrayIndex < CodeArray.Length; arrayIndex++)
            //        {
            //            if (CodeArray[arrayIndex].Equals(code))  //相等
            //            {
            //                if (arrayIndex < 10)   // 0..9
            //                    singleChar = (char)(48 + arrayIndex);
            //                else if (arrayIndex < 36) //A..Z
            //                    singleChar = (char)(65 + arrayIndex - 10);
            //                else
            //                    singleChar = (char)(97 + arrayIndex - 36);
            //                result = result + singleChar;
            //            }
            //        }
            //    }
            //}
            //return result;
        }

        private void btnGetVerfityImage_Click(object sender, EventArgs e)
        {
            string verfityimageurl = cbbxVerfityImageUrl.Text.Trim();
            Image verfityimage;
            try
            {
                Uri imguri = new Uri(verfityimageurl);
                if (imguri.Scheme == Uri.UriSchemeFile)
                {
                    verfityimage = Image.FromFile(verfityimageurl);
                }
                else
                {
                    HttpWebRequest objRequest = (HttpWebRequest)HttpWebRequest.Create(verfityimageurl);
                    objRequest.Method = "GET";
                    WebResponse wr2 = objRequest.GetResponse();
                    Stream s = wr2.GetResponseStream();
                    verfityimage = Image.FromStream(s);
                    s.Close();
                    s.Dispose();
                }
                pbVerfityImageSource.Image = verfityimage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
