using System;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;
using LiteCMS.Core;
using LiteCMS.Entity;
using Natsuhime.Web;

namespace LiteCMS.Web
{
    public partial class uploadfile : BasePage
    {
        protected override void Page_Show()
        {
            UserInfo userinfo = GetUserInfo();
            if (userinfo == null)
            {
                ShowError("上传文件", "请登录后再上传文件,谢谢~", "", "login.aspx");
            }
            if (ispost)
            {
                int filecount = System.Web.HttpContext.Current.Request.Files.Count;
                for (int i = 0; i < filecount; i++)
                {
                    System.Web.HttpPostedFile postedfile = System.Web.HttpContext.Current.Request.Files[i];
                    if (postedfile.FileName != string.Empty)
                    {
                        string fileext = Path.GetExtension(postedfile.FileName).ToLower();
                        string savepath = Path.Combine("upload", DateTime.Now.ToString("yyMM"));
                        string filename = string.Format("{0}{1}{2}", DateTime.Now.ToString("yyMMddhhmm"), Guid.NewGuid().ToString(), fileext);
                        string fullsavename = Path.Combine(savepath, filename);

                        bool canUpload = false;
                        string[] allowedextensions = { ".gif", ".png", ".jpeg", ".jpg", ".zip", ".rar" };
                        foreach (string allowextname in allowedextensions)
                        {
                            if (fileext == allowextname)
                            {
                                canUpload = true;
                                break;
                            }
                        }

                        if (canUpload == true)
                        {

                            YRequest.SaveRequestFile(System.Web.HttpContext.Current.Request.Files[i], Server.MapPath("~/" + fullsavename));

                            AttachmentInfo info = new AttachmentInfo();
                            info.Filename = filename;
                            info.Filepath = fullsavename;
                            info.Filetype = 0;
                            info.Posterid = userinfo.Uid;
                            info.Description = "";
                            Attachments.CreateAttachment(info);

                            string result = JavaScriptConvert.SerializeObject(info);
                            currentcontext.Response.Write(result);
                            currentcontext.Response.End();
                        }
                    }
                }
                //System.Web.HttpContext.Current.Response.Redirect("uploadfile.aspx?filename=" + uploadedfilename.Trim(','));
            }
        }
    }
}
