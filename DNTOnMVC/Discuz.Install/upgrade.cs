using System;
using System.Collections.Generic;
using System.Text;

using Discuz.Common;
using Discuz.Config;
using Discuz.Config.Provider;
using System.Threading;
using Discuz.Data;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using Discuz.Forum;
using System.Xml;

namespace Discuz.Install
{
    public class Upgrade : System.Web.UI.Page
    {
        protected string step = DNTRequest.GetString("step");

        protected int stepNum = 0;
        /// <summary>
        /// 服务器环境检测结果json
        /// </summary>
        protected string testResult = "";
        protected BaseConfigInfo baseconfig = BaseConfigs.GetBaseConfig();
        /// <summary>
        /// 获取升级脚本的文件夹
        /// </summary>
        private string upgradeSqlScriptPath = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            upgradeSqlScriptPath = string.Format("sqlscript/{0}/", baseconfig.Dbtype.ToString().Trim());
            switch (step)
            {
                case "servertest":
                    testResult = InstallUtils.InitialSystemValidCheck();
                    stepNum = 1;
                    break;
                case "upgrade":
                    stepNum = 2;
                    break;
            }
        }

        /// <summary>
        /// 检测数据库链接
        /// </summary>
        protected void CheckConnection()
        {
            try
            {
                DbHelper.ExecuteNonQuery("SELECT 1");
                Response.Write("{\"Result\":true,\"Message\":\"数据库连接检测通过\"}");
            }
            catch (SqlException)
            {
                Response.Write("{\"Result\":false,\"Message\":\"在与 SQL Server 建立连接时出现与网络相关的或特定于实例的错误,请检查dnt.config中的连接字符串是否正确\"}");
            }
        }

        /// <summary>
        /// 获得论坛版本
        /// </summary>
        /// <returns></returns>
        private string GetForumVersion()
        {
            string filePath = Server.MapPath("../config/forumversion.config");
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            else
                return "";
        }

        private void SetForumVersion()
        {
            string filePath = Server.MapPath("../config/forumversion.config");
            File.WriteAllText(filePath, Discuz.Common.Utils.ASSEMBLY_VERSION);
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="message"></param>
        private void ShowMessage(string message)
        {
            Response.Write("<script type='text/javascript'>showmessage('" + message + "');</script>");
            Response.Flush();
        }

        private void ShowErrorMessage(string message)
        {
            message = message.Replace("'", "\\\'").Replace("\r\n", "");
            Response.Write("<script type='text/javascript'>showerrormessage('" + message + "');</script>");
            Response.Flush();
            Response.End();
        }

        private string[] GetScript(string scriptPath)
        {
            return File.ReadAllText(scriptPath, Encoding.UTF8).Trim().Replace("dnt_", baseconfig.Tableprefix)
                .Split(new string[] { "GO\r\n", "go\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 执行存储过程sql脚本
        /// </summary>
        private void UpgradeProcedure()
        {

            string upgradeProcedureScriptFileName = Server.MapPath(string.Format("{0}upgradeprocedure_2000.sql", upgradeSqlScriptPath));
            if (DbHelper.ExecuteScalar(CommandType.Text, "SELECT @@VERSION").ToString().Substring(20, 24).IndexOf("2000") == -1)    //查询SQLSERVER的版本
                upgradeProcedureScriptFileName = Server.MapPath(string.Format("{0}upgradeprocedure_2005.sql", upgradeSqlScriptPath));

            //升级脚本不存在
            if (!File.Exists(upgradeProcedureScriptFileName))
            {
                ShowErrorMessage("存储过程升级脚本不存在，请重新部署升级脚本再重新升级！");
                return;
            }
            string[] sqlArray = GetScript(upgradeProcedureScriptFileName);
            foreach (string sql in sqlArray)
            {
                if (sql.Trim() == string.Empty)
                    continue;
                try
                {
                    DbHelper.ExecuteNonQuery(CommandType.Text, sql);
                }
                catch (SqlException e)
                {
                    ShowErrorMessage("脚本运行出现异常:" + e.Message);
                    return;
                }
                catch
                {
                    ;
                }
            }
            DatabaseProvider.GetInstance().UpdatePostSP();
            ShowMessage("升级存储过程完成");
        }

        private void ResponseResult(string sql)
        {
            MatchCollection mc;
            RegexOptions options = RegexOptions.IgnoreCase;
            if (Regex.IsMatch(sql, @"CREATE\s+TABLE"))
            {
                ShowMessage(string.Format("创建表 \"{0}\" 已完成", Regex.Matches(sql, @"CREATE\s+TABLE\s+\[(.+?)\]")[0].Groups[1].Value));
            }
            else if (Regex.IsMatch(sql, @"DROP\s+TABLE"))
            {
                ShowMessage(string.Format("删除表 \"{0}\" 已完成", Regex.Matches(sql, @"DROP\s+TABLE\s+\[(.+?)\]")[0].Groups[1].Value));
            }
            else if (Regex.IsMatch(sql, @"ALTER\s+TABLE"))
            {
                if (Regex.IsMatch(sql, @"\]\s+ADD\s+\["))
                {
                    mc = Regex.Matches(sql, @"ALTER\s+TABLE\s+\[(.+?)\]\s+ADD\s+\[(.+?)\]", options);
                    ShowMessage(string.Format("为表 \"{0}\" 增加字段 \"{1}\" 已完成", mc[0].Groups[1].Value, mc[0].Groups[2].Value));
                }
                else if (Regex.IsMatch(sql, @" DROP\s+COLUMN "))
                {
                    mc = Regex.Matches(sql, @"ALTER\s+TABLE\s+\[(.+?)\]\s+DROP\s+COLUMN\s+\[(.+?)\]", options);
                    ShowMessage(string.Format("删除表 \"{0}\" 的字段 \"{1}\" 已完成", mc[0].Groups[1].Value, mc[0].Groups[2].Value));
                }
                else if (Regex.IsMatch(sql, @".+ADD\s+CONSTRAINT\s+"))
                {
                    mc = Regex.Matches(sql, @"ALTER\s+TABLE\s+\[(.+?)\].+ADD\s+CONSTRAINT\s+\[(.+?)\].+FOR\s+\[(.+?)\]", options);
                    ShowMessage(string.Format("为表 \"{0}\" 的 \"{1}\" 字段增加约束 \"{2}\" 已完成", mc[0].Groups[1].Value, mc[0].Groups[3].Value, mc[0].Groups[2].Value));
                }
                else if (Regex.IsMatch(sql, @" DROP\s+CONSTRAINT "))
                {
                    mc = Regex.Matches(sql, @"ALTER\s+TABLE\s+\[(.+?)\]\s+DROP\s+CONSTRAINT\s+\[(.+?)\]", options);
                    ShowMessage(string.Format("删除表 \"{0}\" 的约束 \"{1}\" 已完成", mc[0].Groups[1].Value, mc[0].Groups[2].Value));
                }
            }
            else if (Regex.IsMatch(sql, @"CREATE\s+INDEX"))
            {
                mc = Regex.Matches(sql, @"CREATE\s+INDEX \[(.+?)\]\s+ON\s+\[(.+?)\]", options);
                ShowMessage(string.Format("为表 \"{0}\" 建立索引 \"{1}\" 已完成", mc[0].Groups[2].Value, mc[0].Groups[1].Value));
            }
            else if (Regex.IsMatch(sql, @"DROP\s+INDEX"))
            {
                mc = Regex.Matches(sql, @"DROP\s+INDEX\s+\[(.+?)\]\.\[(.+?)\]", options);
                ShowMessage(string.Format("删除表 \"{0}\" 的索引 \"{1}\" 已完成", mc[0].Groups[1].Value, mc[0].Groups[2].Value));
            }
            else if (Regex.IsMatch(sql, @"EXEC\s+sp_rename"))
            {
                mc = Regex.Matches(sql, @"EXEC\s+sp_rename\s+'\[(.+?)\].\[(.+?)\]'\s*,\s*'(.+?)'", options);
                ShowMessage(string.Format("将表 \"{0}\" 的字段 \"{1}\" 重命名为 \"{2}\" 已完成", mc[0].Groups[1].Value, mc[0].Groups[3].Value, mc[0].Groups[2].Value));
            }
        }

        private void UpgradeTable()
        {
            string forumVersion = GetForumVersion();
            if (forumVersion == "")
                forumVersion = "2.5";
            int forumMajorVersion;
            forumMajorVersion = int.TryParse(forumVersion.Split('.')[0], out forumMajorVersion) ? forumMajorVersion : 2;
            string upgradeTableScriptFileName = Server.MapPath(string.Format("{0}upgradetable{1}x.sql", upgradeSqlScriptPath, forumMajorVersion));
            //升级脚本起始脚本是否存在
            if (!File.Exists(upgradeTableScriptFileName))
            {
                ShowErrorMessage("数据表升级脚本不存在，请重新部署升级脚本再重新升级！");
                return;
            }
            //设置标志，如果在当前升级脚本中找到对应的版本号
            bool isFindVersion = false;
            while (true)
            {
                upgradeTableScriptFileName = Server.MapPath(string.Format("{0}upgradetable{1}x.sql", upgradeSqlScriptPath, forumMajorVersion));
                //判断当前要运行的脚本是否存在，如果存在则执行，如果不存在则说明所有脚本已经执行完毕
                if (!File.Exists(upgradeTableScriptFileName))
                    break;
                string[] sqlArray = GetScript(upgradeTableScriptFileName);
                foreach (string sql in sqlArray)
                {
                    if (sql.Trim() != string.Empty && (isFindVersion || sql.Contains("--" + forumVersion + "\r\n")))
                    {
                        isFindVersion = true;
                        try
                        {
                            DbHelper.ExecuteNonQuery(CommandType.Text, sql);
                            ResponseResult(sql);
                        }
                        catch (SqlException e)
                        {
                            ShowErrorMessage("脚本运行出现异常:" + e.Message);
                        }
                        catch
                        {
                            ;
                        }
                    }
                }
                forumMajorVersion++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpgradeDetachTable()
        {
            string detachTableFilePath = Server.MapPath(string.Format("{0}detachtable.sql", upgradeSqlScriptPath));
            if (!File.Exists(detachTableFilePath))
            {
                ShowErrorMessage("分表脚本不存在!");
                return;
            }
            string sqlContent = File.ReadAllText(detachTableFilePath, Encoding.UTF8).Trim();
            foreach (DataRow dr in DatabaseProvider.GetInstance().GetDatechTableIds())
            {
                string tableId = dr["id"].ToString();
                string[] sqlArray = sqlContent.Replace("dnt_posts1", BaseConfigs.GetTablePrefix + "posts" + tableId).Replace("_dnt_posts_",string.Format("_{0}posts_",BaseConfigs.GetTablePrefix))
                    .Split(new string[] { "GO\r\n", "go\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string sql in sqlArray)
                {
                    if (sql.Trim() != string.Empty)
                    {
                        try
                        {
                            DbHelper.ExecuteNonQuery(CommandType.Text, sql);
                            ResponseResult(sql);
                        }
                        catch (SqlException e)
                        {
                            ShowErrorMessage("脚本运行出现异常:" + e.Message);
                        }
                        catch
                        {
                            ;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 升级
        /// </summary>
        protected void UpgradeProcess()
        {
            //当数据库不是Sql Server不允许升级
            if (baseconfig.Dbtype != "SqlServer")
            {
                ShowErrorMessage("你的论坛不是Sql Server版本，不能用此程序升级");
                return;
            }
            try
            {
                //将首页默认定为论坛首页
                GeneralConfigs.GetConfig().Forumurl = "forumindex.aspx";
                GeneralConfigs.Serialiaze(GeneralConfigs.GetConfig(), Server.MapPath("../config/general.config"));
                if (Utils.ASSEMBLY_VERSION != GetForumVersion())
                {
                    //升级表
                    UpgradeTable();
                    //升级分表
                    UpgradeDetachTable();
                    //升级存储过程
                    UpgradeProcedure();
                    //数据库升级完毕后工作
                    UpgradeConfig();
                    MoveCommonUpgradeIniConfig();
                    //生成后台菜单
                    MenuManage.CreateMenuJson();
                    //重新设置最新的论坛版本
                    SetForumVersion();
                    ShowMessage("安装成功,点击\"完成\"进入首页......");
                }
                else
                {
                    ShowMessage("您的论坛已经最新版本，无需要升级，点击\"完成\"进入首页......");
                }
                //升级完成
                Succeed();
            }
            catch (ThreadAbortException e)
            {
                ShowErrorMessage("升级过程中出现异常，升级将程序终止。请参照错误信息提示修改错误再刷新重试。如果无法解决，请<a href=\'http://nt.discuz.net/support/\' target=\'_blank\' style=\'color:red;\'>点击这里</a>进行反馈。");
                return;
            }
            catch (Exception e)
            {
                ShowErrorMessage("升级过程中出现异常:" + e.Message);
                return;
            }
        }

        private void Succeed()
        {
            Response.Write("<script type=\"text/javascript\">$('#successlink').attr('class', 'next').attr('href', '../index.aspx');</script>");
            Response.Flush();
        }

        /// <summary>
        /// 升级配置文件修改
        /// </summary>
        private void UpgradeConfig()
        {
            //Discuz!NT 2.5升级到Discuz!NT 2.6
            UpgradeAdminMenu_V25(); //增加2.5需要的菜单
            UpgradeScoreSet();

            //Discuz!NT 2.6升级到Discuz!NT 3.0
            UpgradeAdminMenu_V26();

            //Discuz!NT 3.0升级到Discuz!NT 3.1
            UpgradeAdminMenu_V30();
            UpgradeGeneralConfig();
            CreateUpdateUserCreditsProcedure(); //重新按积分规则生成'dnt_updateusercredits'存储过程
            CreateInvitationSchedule();
            UpgradeScoresetForInvitation();

            //插件菜单升级
            UpgradeAdminMenu_V35();
            UpgradeAdminMenu_V36();
        }

        /// <summary>
        /// 移动commonupgradeini.config文件到Config目录
        /// </summary>
        private void MoveCommonUpgradeIniConfig()
        {
            string fileName = Utils.GetMapPath("commonupgradeini.config");
            if (File.Exists(fileName))
            {
                if (File.Exists(Utils.GetMapPath("../config/commonupgradeini.config")))
                    File.Delete(Utils.GetMapPath("../config/commonupgradeini.config"));
                File.Move(fileName, Utils.GetMapPath("../config/commonupgradeini.config"));
            }
        }

        #region 各版本升级配置文件的方法

        /// <summary>
        /// Discuz!NT 2.5升级更新后台管理菜单
        /// </summary>
        private void UpgradeAdminMenu_V25()
        {
            MenuManage.NewMenuItem(1020, "导航菜单管理", "global/global_navigationmanage.aspx");
            MenuManage.NewMenuItem(6010, "公共消息管理", "global/global_announceprivatemessage.aspx");
            MenuManage.DeleteMenuItem("全 局", "常规选项", "“我的”菜单");
        }

        /// <summary>
        /// 更新积分设置
        /// </summary>
        private void UpgradeScoreSet()
        {
            string configPath = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/scoreset.config");
            XmlDocument doc = new XmlDocument();
            doc.Load(configPath);
            if (doc.SelectSingleNode("/scoreset/formula/bonuscreditstrans") == null)   //如果节点存在就不必再增加
            {
                XmlElement bonuscreditstrans = doc.CreateElement("bonuscreditstrans");
                bonuscreditstrans.InnerText = "0";
                doc.SelectSingleNode("/scoreset/formula").InsertAfter(bonuscreditstrans, doc.SelectSingleNode("/scoreset/formula/creditstrans"));
            }
            if (doc.SelectSingleNode("/scoreset/formula/topicattachcreditstrans") == null)
            {
                XmlElement topicattachcreditstrans = doc.CreateElement("topicattachcreditstrans");
                topicattachcreditstrans.InnerText = "0";
                doc.SelectSingleNode("/scoreset/formula").InsertAfter(topicattachcreditstrans, doc.SelectSingleNode("/scoreset/formula/creditstrans"));
            }
            foreach (XmlNode node in doc.SelectNodes("/scoreset/record/name"))
            {
                node.InnerText = node.InnerText.Replace("(＋)", "").Replace("(－)", "");
            }
            doc.Save(configPath);
        }

        /// <summary>
        /// Discuz!NT 2.6升级更新后台管理菜单
        /// </summary>
        private void UpgradeAdminMenu_V26()
        {
            MenuManage.DeleteMenuItem("其 他", "其它设置", "论坛头像列表");
            MenuManage.DeleteMenuItem("工 具", "数据库", "数据库优化");
        }

        /// <summary>
        /// Discuz!NT 3.0升级更新后台管理菜单
        /// </summary>
        private void UpgradeAdminMenu_V30()
        {
            MenuManage.EditMenuItem("论 坛", "论坛聚合", "推荐版块", "推荐版块", "aggregation/aggregation_recommendtopic.aspx");
            MenuManage.NewMenuItem(2030, "论坛热帖", "aggregation/aggregation_edithottopic.aspx");
        }


        private void UpgradeAdminMenu_V35()
        {
            GeneralConfigInfo config = GeneralConfigs.GetConfig();
            if (Discuz.Plugin.Space.SpacePluginProvider.GetInstance() != null)
            {
                MenuManage.ImportPluginMenu(Utils.GetMapPath("space.xml"));
            }

            if (Discuz.Plugin.Album.AlbumPluginProvider.GetInstance() != null)
            {
                MenuManage.ImportPluginMenu(Utils.GetMapPath("album.xml"));
            }
        }

        /// <summary>
        /// Discuz!NT 3.5升级更新后台管理菜单
        /// </summary>
        private void UpgradeAdminMenu_V36()
        {
            MenuManage.NewMenuItem(2210, "论坛数据调用", "global/global_forumdatacall.aspx");
        }

        /// <summary>
        /// 修改分享站点列表
        /// </summary>
        private void UpgradeGeneralConfig()
        {
            GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
            if (!configInfo.Sharelist.Contains("百度收藏"))
                configInfo.Sharelist += ",5|qq|qq书签|1,6|google|google书签|1,7|vivi|爱问收藏|1,8|live|live收藏|1,9|favorite|收藏夹|1,10|baidu|百度收藏|1";
            GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../config/general.config"));
        }

        /// <summary>
        /// 建立用户积分存储过程
        /// </summary>
        private void CreateUpdateUserCreditsProcedure()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Server.MapPath("../config/scoreset.config"));
            Discuz.Forum.AdminForums.CreateUpdateUserCreditsProcedure(xmlDoc.SelectSingleNode("/scoreset/formula/formulacontext").InnerText, false);
        }

        /// <summary>
        /// 建立邀请计划任务
        /// </summary>
        private void CreateInvitationSchedule()
        {
            ScheduleConfigInfo sci = ScheduleConfigs.GetConfig();
            //检查该事件是否存在
            foreach (Discuz.Config.Event ev1 in sci.Events)
            {
                if (ev1.Key == "InvitationEvent")
                    return;
            }

            //建立新的邀请计划任务
            Discuz.Config.Event ev = new Discuz.Config.Event();
            ev.Key = "InvitationEvent";
            ev.Enabled = true;
            ev.IsSystemEvent = true;
            ev.ScheduleType = "Discuz.Event.InvitationEvent, Discuz.Event";
            ev.TimeOfDay = 2;
            ev.Minutes = 1;
            ScheduleConfigs.SaveConfig(sci);
        }

        /// <summary>
        /// 增加邀请注册的积分规则
        /// </summary>
        private void UpgradeScoresetForInvitation()
        {
            DataSet scoreset = new DataSet();
            scoreset.ReadXml(Server.MapPath("../config/scoreset.config"));
            if (scoreset.Tables[0].Select("id='18' AND name='邀请注册'").Length != 0)
                return;
            DataRow dr = scoreset.Tables[0].NewRow();
            dr["id"] = "18";
            dr["name"] = "邀请注册";
            dr["extcredits1"] = "5";
            dr["extcredits2"] = "5";
            dr["extcredits3"] = "0";
            dr["extcredits4"] = "0";
            dr["extcredits5"] = "0";
            dr["extcredits6"] = "0";
            dr["extcredits7"] = "0";
            dr["extcredits8"] = "0";
            scoreset.Tables[0].Rows.Add(dr);
            scoreset.WriteXml(Server.MapPath("../config/scoreset.config"));
        }

        #endregion
    }
}
