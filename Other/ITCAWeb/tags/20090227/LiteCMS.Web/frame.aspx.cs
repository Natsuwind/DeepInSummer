using System;
using System.Web;
using Natsuhime.Web;

namespace LiteCMS.Web
{
    public partial class frame : BasePage
    {
        protected override void Page_Show()
        {
            base.IsAdminLogined();
            if (admininfo != null)
            {
                string url = "";
                //string adminpath = YRequest.GetString("path") == string.Empty ? "admin" : YRequest.GetString("path");
                string action = YRequest.GetString("action");
                int id = YRequest.GetInt("id", 0);
                string name = YRequest.GetString("name");

                if (action == "postarticle")
                {
                    url = string.Format("{0}/postarticle.aspx", adminpath);
                }
                else if (action == "editarticle")
                {
                    url = string.Format("{0}/postarticle.aspx?id={1}&action=edit", adminpath, id);
                }
                else if (action == "deletearticle")
                {
                    url = string.Format("{0}/deleteadmin.aspx?articleid={1}", adminpath, id);
                }
                else if (action == "listarticle")
                {
                    url = string.Format("{0}/articlelist.aspx?cid={1}", adminpath, id);
                }
                else if (action == "mgrcolumn")
                {
                    url = string.Format("{0}/columnlist.aspx", adminpath);
                }
                else if (action == "mainsetting")
                {
                    url = string.Format("{0}/mainsetting.aspx", adminpath);
                }
                else if (action == "template")
                {
                    url = string.Format("{0}/template.aspx", adminpath);
                }
                else if (action == "createtemplate")
                {
                    url = string.Format("{0}/template.aspx?action=create&folder={1}", adminpath, name);
                }
                else if (action == "")
                {
                }
                else if (action == "")
                {
                }
                else if (action == "default")
                {
                    url = string.Format("{0}/articlelist.aspx?cid={1}", adminpath, id);
                }
                else
                {
                    url = "";
                }
                Server.Transfer(url);
            }
            else
            {
                currentcontext.Response.Redirect("admincp.aspx");
            }
        }
    }
}
