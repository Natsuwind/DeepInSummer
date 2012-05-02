using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Mall.Data;

using Discuz.Mall;
using Discuz.Entity;
using Discuz.Web.Admin;

namespace Discuz.Mall.Admin
{
    /// <summary>
    /// 商品分类管理
    /// </summary>
    
#if NET1
    public class mall_goodscategoriesmanage : AdminPage
#else
    public partial class mall_goodscategoriesmanage : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected System.Web.UI.WebControls.Label ShowTreeLabel;
        #endregion
#endif

        #region 图标信息变量声明

        private string T_rootpic = "<img src=../images/lines/tplus.gif align=absmiddle>";
        private string L_rootpic = "<img src=../images/lines/lplus.gif align=absmiddle>";
        private string L_TOP_rootpic = "<img src=../images/lines/rplus.gif align=absmiddle>";
        private string I_rootpic = "<img src=../images/lines/dashplus.gif align=absmiddle>";
        private string T_nodepic = "<img src=../images/lines/tminus.gif align=absmiddle>";
        private string L_nodepic = "<img src=../images/lines/lminus.gif align=absmiddle>";
        private string I_nodepic = "<img src=../images/lines/i.gif align=absmiddle>";
        private string No_nodepic = "<img src=../images/lines/noexpand.gif align=absmiddle>";

        #endregion

        public string str = "";
        public int noPicCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (DNTRequest.GetString("currentfid") != "")
                MoveCategory();

            if (DNTRequest.GetString("method") == "new")
                NewChildCategory();

            if (DNTRequest.GetString("method") == "edit")
                EditCategory();

            if (DNTRequest.GetString("method") == "newrootnode")
                NewRootCategory();

            if (DNTRequest.GetString("method") == "del")
                DeleteNode();

            if (DNTRequest.GetString("method") == "updateall")
                UpdateAll();

            if(DNTRequest.GetString("method") == "update")
                UpdateCategoryGoodsCount();

            if (DNTRequest.GetString("highlevel") == "set")
            {
                string ishighlevel = DNTRequest.GetString("ishighlevel");
                SetHighLevel(ishighlevel);
            }
            
            LoadCategoryTree();
            LoadForumsTree();
        }

        private void UpdateCategoryGoodsCount()
        {
            string id = DNTRequest.GetString("id").Trim();
            Goodscategoryinfo goodscategoryinfo = GoodsCategories.GetGoodsCategoryInfoById(int.Parse(id));
            GoodsCategories.UpdateCategoryGoodsCount(goodscategoryinfo);
        }

        private void UpdateAll()
        {
            GoodsCategories.UpdateCategoryGoodsCount();
        }

        private void LoadCategoryTree()
        {
            DataTable dt = DbProvider.GetInstance().GetAllCategoriesTable();
            ViewState["dt"] = dt;

            if (dt.Rows.Count == 0)
            {
                str = "<script type=\"text/javascript\">\r\n  obj = [];\r\n newtree = new tree(\"newtree\",obj,\"reSetTree\");</script>";
            }
            else
            {
                AddTree(0, dt.Select("layer=0 AND [parentid]=0"), "");
                str = "<script type=\"text/javascript\">\r\n  obj = [" + str;
                str = str.Substring(0, str.Length - 3);
                str += "];\r\n newtree = new tree(\"newtree\",obj,\"reSetTree\");";
                str += "</script>";
            }
            ShowTreeLabel.Text = str;
        }

        private void LoadForumsTree()
        {
            DataTable dt = Discuz.Data.DatabaseProvider.GetInstance().GetAllOpenForum();
            
            string strForumsTree = "\r\n<script type='text/javascript'>\r\nforumstree = [";
            foreach (DataRow dr in dt.Rows)
            {
                strForumsTree += "\r\n{fid:" + dr["fid"].ToString() + ",parentid:" + dr["parentid"].ToString() + ",name:\"" + Utils.RemoveHtml(dr["name"].ToString().Replace("\\","\\\\").Replace("'","\\'").Trim()) + "\"},";
            }
            if (dt.Rows.Count != 0)
                strForumsTree = strForumsTree.TrimEnd(',');
            strForumsTree += "]\r\n</script>";
            ForumsTreeLabel.Text = strForumsTree;
        }

        private void MoveCategory()
        {
            int currentfid = DNTRequest.GetInt("currentfid",0);
            int targetfid = DNTRequest.GetInt("targetfid",0);
            string isaschildnode = DNTRequest.GetString("isaschildnode");
            Goodscategoryinfo gc = GoodsCategories.GetGoodsCategoryInfoById(currentfid);
            int oldparentid = gc.Parentid;
            Goodscategoryinfo parentgc = GoodsCategories.GetGoodsCategoryInfoById(targetfid);
            gc.Fid = 0;
            gc.Parentid = targetfid;
            gc.Parentidlist = parentgc.Parentidlist + "," + parentgc.Categoryid;
            gc.Layer = parentgc.Layer + 1;
            gc.Pathlist = parentgc.Pathlist + string.Format("<a href=\"showgoodslist-{0}.aspx\">{1}</a>", gc.Categoryid, gc.Categoryname);
            GoodsCategories.UpdateGoodsCategory(gc);
            parentgc.Haschild = 1;
            GoodsCategories.UpdateGoodsCategory(parentgc);
            if (gc.Haschild == 0)
                return;
            DataTable dt = DbProvider.GetInstance().GetAllCategoriesTable();
            MoveSubCategory(gc, dt);
            Goodscategoryinfo oldparentgc = GoodsCategories.GetGoodsCategoryInfoById(oldparentid);
            oldparentgc.Haschild = (dt.Select("parentid=" + oldparentid) == null ? 0 : 1);
            GoodsCategories.UpdateGoodsCategory(oldparentgc);
            ResetStatus();
            this.RegisterStartupScript("PAGE", "window.location='mall_goodscategoriesmanage.aspx';");
        }

        private void NewChildCategory()
        {
            int parentid = DNTRequest.GetInt("parentcategoryid", 0);
            int fid = DNTRequest.GetInt("forumtreelist",0);
            Goodscategoryinfo parentgc = GoodsCategories.GetGoodsCategoryInfoById(parentid);
            Goodscategoryinfo gc = new Goodscategoryinfo();
            gc.Categoryname = Utils.HtmlEncode(DNTRequest.GetString("categoryname").Trim());
            gc.Parentid = parentid;
            gc.Layer = parentgc.Layer + 1;
            if (parentgc.Parentidlist.Trim() == "0")
            {
                gc.Parentidlist = parentgc.Categoryid.ToString();
                gc.Fid = fid;
            }
            else
            {
                gc.Parentidlist = parentgc.Parentidlist + "," + parentgc.Categoryid;
                gc.Fid = parentgc.Fid;
            }
            gc.Pathlist = "";
            gc.Goodscount = 0;
            gc.Haschild = 0;
            int newcategoryid = GoodsCategories.CreateGoodsCategory(gc);
            gc.Categoryid = newcategoryid;
            if (parentgc.Fid != 0)
            {
                gc.Pathlist = parentgc.Pathlist;
            }
            gc.Pathlist += string.Format("<a href=\"showgoodslist-{0}.aspx\">{1}</a>", newcategoryid, gc.Categoryname);
            GoodsCategories.UpdateGoodsCategory(gc);
            parentgc.Haschild = 1;
            GoodsCategories.UpdateGoodsCategory(parentgc);
            SetForumsTrade(gc.Fid);
            OpenMall(fid);
            ResetStatus();
            this.RegisterStartupScript("PAGE", "window.location='mall_goodscategoriesmanage.aspx';");
        }

        private void EditCategory()
        {
            int categoryid = DNTRequest.GetInt("categoryid",0);
            string categoryname = DNTRequest.GetString("categoryname").Trim();
            int fid = DNTRequest.GetInt("forumtreelist",0);
            DataTable dt = DbProvider.GetInstance().GetAllCategoriesTable();
            Goodscategoryinfo gc = GoodsCategories.GetGoodsCategoryInfoById(categoryid);
            int oldfid = gc.Fid;
            if(gc.Parentidlist.IndexOf(",") == -1)
                gc.Fid = fid;

            Goodscategoryinfo parentgc = GoodsCategories.GetGoodsCategoryInfoById(gc.Parentid);
            gc.Categoryname = categoryname;
            if(parentgc == null || parentgc.Fid == 0)
                gc.Pathlist = string.Format("<a href=\"showgoodslist-{0}.aspx\">{1}</a>", gc.Categoryid, gc.Categoryname);
            else
                gc.Pathlist = parentgc.Pathlist + string.Format("<a href=\"showgoodslist-{0}.aspx\">{1}</a>", gc.Categoryid, gc.Categoryname);
            GoodsCategories.UpdateGoodsCategory(gc);
            EditSubCategory(gc, dt);
            if (oldfid != fid)
            {
                if (fid != 0)
                    SetForumsTrade(fid); 
                else if (oldfid != 0)
                    SetForumsTrade(oldfid); 
            }
            OpenMall(fid);
            ResetStatus();
            this.RegisterStartupScript("PAGE", "window.location='mall_goodscategoriesmanage.aspx';");
        }

        private void OpenMall(int fid)
        {
            GeneralConfigInfo configInfo = GeneralConfigs.Deserialize(Server.MapPath("../../config/general.config"));
            if(fid != 0 && configInfo.Enablemall == 0)
            {
                configInfo.Enablemall = 1;
                GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../../config/general.config"));
            }
        }

        private void EditSubCategory(Goodscategoryinfo parentgc,DataTable dt)
        {
            DataRow[] datarow = dt.Select("parentid=" + parentgc.Categoryid.ToString());
            if(datarow.Length == 0)
                return;
            foreach (DataRow dr in datarow)
            {
                Goodscategoryinfo gc = GoodsCategories.GetGoodsCategoryInfoById(int.Parse(dr["categoryid"].ToString()));
                gc.Pathlist = parentgc.Pathlist + string.Format("<a href=\"showgoodslist-{0}.aspx\">{1}</a>", gc.Categoryid, gc.Categoryname);
                gc.Fid = parentgc.Fid;
                GoodsCategories.UpdateGoodsCategory(gc);
                EditSubCategory(gc, dt);
            }
        }

        private void MoveSubCategory(Goodscategoryinfo parentgc, DataTable dt)
        {
            DataRow[] datarow = dt.Select("parentid=" + parentgc.Categoryid.ToString());
            if (datarow.Length == 0)
                return;
            foreach (DataRow dr in datarow)
            {
                Goodscategoryinfo gc = GoodsCategories.GetGoodsCategoryInfoById(int.Parse(dr["categoryid"].ToString()));
                gc.Parentidlist = parentgc.Parentidlist + "," + parentgc.Categoryid;
                gc.Pathlist = parentgc.Pathlist + string.Format("<a href=\"showgoodslist-{0}.aspx\">{1}</a>", gc.Categoryid, gc.Categoryname);
                gc.Layer = parentgc.Layer + 1;
                GoodsCategories.UpdateGoodsCategory(gc);
                MoveSubCategory(gc, dt);
            }
        }

        private void NewRootCategory()
        {
            Goodscategoryinfo gc = new Goodscategoryinfo();
            gc.Categoryname = Utils.HtmlEncode(DNTRequest.GetString("categoryname").Trim());
            int fid = DNTRequest.GetInt("forumtreelist", 0);
            gc.Fid = fid;
            gc.Parentid = 0;
            gc.Layer = 0;
            gc.Parentidlist = "0";
            gc.Pathlist = "";
            gc.Goodscount = 0;
            gc.Haschild = 0;
            int newcategoryid = GoodsCategories.CreateGoodsCategory(gc);
            gc.Categoryid = newcategoryid;
            gc.Pathlist = string.Format("<a href=\"showgoodslist-{0}.aspx\">{1}</a>", newcategoryid, gc.Categoryname);
            GoodsCategories.UpdateGoodsCategory(gc);
            SetForumsTrade(gc.Fid);
            OpenMall(fid);
            ResetStatus();
            this.RegisterStartupScript("PAGE", "window.location='mall_goodscategoriesmanage.aspx';");
        }

        private void DeleteNode()
        {
            string id = DNTRequest.GetString("id").Trim();
            Goodscategoryinfo gc = GoodsCategories.GetGoodsCategoryInfoById(int.Parse(id));
            GoodsCategories.DeleteGoodsCategory(int.Parse(id));
            DataTable dt = DbProvider.GetInstance().GetAllCategoriesTable();
            if (gc.Parentid != 0)
            {
                Goodscategoryinfo parentgc = GoodsCategories.GetGoodsCategoryInfoById(gc.Parentid);
                parentgc.Haschild = dt.Select("parentid=" + gc.Parentid).Length > 0 ? 1 : 0;
                GoodsCategories.UpdateGoodsCategory(parentgc);
            }
            SetForumsTrade(gc.Fid);
            ResetStatus();
            this.RegisterStartupScript("PAGE", "window.location='mall_goodscategoriesmanage.aspx';");
        }

        private void AddTree(int layer, DataRow[] drs, string currentnodestr)
        {
            DataTable dt = (DataTable)ViewState["dt"];
            if (layer == 0)
            {
                #region 作为根分类

                for (int n = 0; n < drs.Length; n++)
                {
                    string mystr = "";
                    if (drs.Length == 1)
                    {
                        mystr += I_rootpic; //
                        currentnodestr = No_nodepic;
                    }
                    else
                    {
                        if (n == 0)
                        {
                            mystr += L_TOP_rootpic; //
                            currentnodestr = I_nodepic;
                        }
                        else
                        {
                            if ((n > 0) && (n < (drs.Length - 1)))
                            {
                                mystr += T_rootpic; //
                                currentnodestr = I_nodepic;
                            }
                            else
                            {
                                mystr += L_rootpic;
                                currentnodestr = No_nodepic;
                            }
                        }
                    }

                    str += "{fid:" + drs[n]["categoryid"] + ",name:\"" +
                           Utils.HtmlEncode(drs[n]["categoryname"].ToString().Trim().Replace("\\", "\\\\\\\\").Replace("'","\\\\'")) + "\",subject:\" " +
                           mystr + " <img src=../images/folders.gif align=\\\"absmiddle\\\" >";
                    if(drs[n]["fid"].ToString() != "0" && int.Parse(drs[n]["layer"].ToString()) < 2)
                        str += "<input type=\\\"checkbox\\\" name=\\\"ishighlevel\\\" value=\\\"" + drs[n]["categoryid"] + "\\\">";
                    str += Utils.HtmlEncode(drs[n]["categoryname"].ToString().Trim().Replace("\\", "\\\\ ")) + "\",linetitle:\"" +
                           mystr + "\",parentidlist:0,layer:" + drs[n]["layer"] + ",subforumcount:" +
                           (Convert.ToBoolean(drs[n]["haschild"].ToString()) ? 1 : 0) + ",addfid:" + drs[n]["fid"] + 
                           ",editfid:" + (drs[n]["fid"].ToString() == "0" ? GetParentFid(drs[n]["parentid"].ToString(),dt) : drs[n]["fid"].ToString()) +
                           ",cfid:" + drs[n]["fid"] + "},\r\n";
                    if (Convert.ToBoolean(drs[n]["haschild"].ToString()))
                    {
                        int mylayer = Convert.ToInt32(drs[n]["layer"].ToString());
                        string selectstr = "layer=" + (++mylayer) + " AND parentid=" + drs[n]["categoryid"];
                        AddTree(mylayer, dt.Select(selectstr), currentnodestr);
                    }
                }

                #endregion
            }
            else
            {
                #region 作为子分类

                for (int n = 0; n < drs.Length; n++)
                {
                    string mystr = "";
                    mystr += currentnodestr;
                    string temp = currentnodestr;

                    if ((n >= 0) && (n < (drs.Length - 1)))
                    {
                        mystr += T_nodepic; //
                        temp += I_nodepic;
                    }
                    else
                    {
                        mystr += L_nodepic;
                        noPicCount++;
                        temp += No_nodepic;
                    }

                    str += "{fid:" + drs[n]["categoryid"] + ",name:\"" +
                          Utils.HtmlEncode(drs[n]["categoryname"].ToString().Trim().Replace("\\", "\\\\\\\\").Replace("'", "\\\\'")) + "\",subject:\" " +
                          mystr + " <img src=../images/folder.gif align=\\\"absmiddle\\\" >";
                    if (drs[n]["fid"].ToString() != "0" && int.Parse(drs[n]["layer"].ToString()) < 2)
                        str += "<input type=\\\"checkbox\\\" name=\\\"ishighlevel\\\" value=\\\"" + drs[n]["categoryid"] + "\\\">";
                    str += Utils.HtmlEncode(drs[n]["categoryname"].ToString().Trim().Replace("\\", "\\\\ ")) + "\",linetitle:\"" +
                          mystr + "\",parentidlist:\"" + drs[n]["parentidlist"].ToString().Trim() + "\",layer:" +
                          drs[n]["layer"] + ",subforumcount:" +
                          (Convert.ToBoolean(drs[n]["haschild"].ToString()) ? 1 : 0) + ",addfid:" + drs[n]["fid"] +
                          ",editfid:" + (drs[n]["layer"].ToString() == "1" ? GetParentFid(drs[n]["parentid"].ToString(), dt) : drs[n]["fid"].ToString()) + 
                          ",cfid:" + drs[n]["fid"] + "},\r\n";
                    if (Convert.ToBoolean(drs[n]["haschild"].ToString()))
                    {
                        int mylayer = Convert.ToInt32(drs[n]["layer"].ToString());
                        string selectstr = "layer=" + (++mylayer) + " AND parentid=" + drs[n]["categoryid"];
                        AddTree(mylayer, dt.Select(selectstr), temp);
                    }
                }

                #endregion
            }
        }

        private string GetParentFid(string parentid,DataTable dt)
        {
            string fid = "0";
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["categoryid"].ToString() == parentid)
                    fid = dr["fid"].ToString();
            }
            return fid;
        }

        private void SetHighLevel(string ishighlevel)
        {
            if (ishighlevel == "")
                return;
            DataTable dt = DbProvider.GetInstance().GetAllCategoriesTable();
            foreach (string highlevelid in ishighlevel.Split(','))
            {
                int fid = 0;
                Goodscategoryinfo gc = GoodsCategories.GetGoodsCategoryInfoById(int.Parse(highlevelid));
                fid = gc.Fid;
                gc.Fid = 0;
                GoodsCategories.UpdateGoodsCategory(gc);
                SetSubCategoryFid(dt,gc.Categoryid);
                SetForumsTrade(fid);
            }
            ResetStatus();
        }

        private void SetSubCategoryFid(DataTable dt, int parentid)
        {
            DataRow[] datarow = dt.Select("parentid=" + parentid);
            if (datarow == null)
                return;
            foreach (DataRow dr in datarow)
            {
                Goodscategoryinfo gc = GoodsCategories.GetGoodsCategoryInfoById(int.Parse(dr["categoryid"].ToString()));
                gc.Fid = 0;
                GoodsCategories.UpdateGoodsCategory(gc);
                SetSubCategoryFid(dt,gc.Categoryid);
            }
        }

        private void SetForumsTrade(int fid)
        {
            ForumInfo foruminfo = AdminForums.GetForumInfo(fid);
            int isexist = DbProvider.GetInstance().GetCategoriesFidCount(fid) != 0 ? 1 : 0;
            if (foruminfo.Istrade != isexist)
            {
                foruminfo.Istrade = isexist;
                AdminForums.UpdateForumInfo(foruminfo);
            }
        }

        private void ResetStatus()
        {
            GoodsCategories.GetInstance.WriteJsonFile();
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Mall/MallSetting/GoodsCategories");
        }


        #region 把VIEWSTATE写入容器

        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            base.DiscuzForumSavePageState(viewState);
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            return base.DiscuzForumLoadPageState();
        }

        #endregion

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            //this.Load += new EventHandler(this.Page_Load);

        }
        #endregion
    }
}