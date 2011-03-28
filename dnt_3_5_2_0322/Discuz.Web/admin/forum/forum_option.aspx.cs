using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Common;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    public partial class option : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadConfigInfo();
                quickforward.Items[0].Attributes.Add("onclick", "setStatus(true)");
                quickforward.Items[1].Attributes.Add("onclick", "setStatus(false)");
            }
        }

        public void LoadConfigInfo()
        {
            #region ����������Ϣ

            GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
            fullmytopics.SelectedValue = configInfo.Fullmytopics.ToString();
            modworkstatus.SelectedValue = configInfo.Modworkstatus.ToString();
            userstatusby.SelectedValue = (configInfo.Userstatusby.ToString() != "0") ? "1" : "0";
            guestcachepagetimeout.Text = configInfo.Guestcachepagetimeout.ToString();
            topiccachemark.Text = configInfo.Topiccachemark.ToString();

            if (configInfo.TopicQueueStats == 1)
            {
                Topicqueuestats_1.Checked = true;
                Topicqueuestats_0.Checked = false;
                topicqueuestatscount.AddAttributes("style", "visibility:visible;");
            }
            else
            {
                Topicqueuestats_0.Checked = true;
                Topicqueuestats_1.Checked = false;
                topicqueuestatscount.AddAttributes("style", "visibility:hidden;");
            }

            topicqueuestatscount.Text = configInfo.TopicQueueStatsCount.ToString();
            losslessdel.Text = configInfo.Losslessdel.ToString();
            edittimelimit.Text = configInfo.Edittimelimit.ToString();
            deletetimelimit.Text = configInfo.Deletetimelimit.ToString();
            editedby.SelectedValue = configInfo.Editedby.ToString();
            //defaulteditormode.SelectedValue = configInfo.Defaulteditormode.ToString();
            allowswitcheditor.SelectedValue = configInfo.Allowswitcheditor.ToString();
            reasonpm.SelectedValue = configInfo.Reasonpm.ToString();
            hottopic.Text = configInfo.Hottopic.ToString();
            starthreshold.Text = configInfo.Starthreshold.ToString();
            fastpost.SelectedValue = configInfo.Fastpost.ToString();
            tpp.Text = configInfo.Tpp.ToString();
            ppp.Text = configInfo.Ppp.ToString();
            //allowhtmltitle.SelectedValue = configInfo.Htmltitle.ToString();
            enabletag.SelectedValue = configInfo.Enabletag.ToString();
            string[] ratevalveset = configInfo.Ratevalveset.Split(',');
            ratevalveset1.Text = ratevalveset[0];
            ratevalveset2.Text = ratevalveset[1];
            ratevalveset3.Text = ratevalveset[2];
            ratevalveset4.Text = ratevalveset[3];
            ratevalveset5.Text = ratevalveset[4];
            statstatus.SelectedValue = configInfo.Statstatus.ToString();
            statscachelife.Text = configInfo.Statscachelife.ToString();
            //pvfrequence.Text = __configinfo.Pvfrequence.ToString();
            hottagcount.Text = configInfo.Hottagcount.ToString();
            oltimespan.Text = configInfo.Oltimespan.ToString();
            maxmodworksmonths.Text = configInfo.Maxmodworksmonths.ToString();
            disablepostad.SelectedValue = configInfo.Disablepostad.ToString();
            disablepostad.Items[0].Attributes.Add("onclick", "$('" + postadstatus.ClientID + "').style.display='';");
            disablepostad.Items[1].Attributes.Add("onclick", "$('" + postadstatus.ClientID + "').style.display='none';");
            disablepostadregminute.Text = configInfo.Disablepostadregminute.ToString();
            disablepostadpostcount.Text = configInfo.Disablepostadpostcount.ToString();
            disablepostadregular.Text = configInfo.Disablepostadregular.ToString();
            replynotificationstatus.SelectedValue = configInfo.Replynotificationstatus.ToString();
            replyemailstatus.SelectedValue = configInfo.Replyemailstatus.ToString();
            allowforumindexposts.SelectedValue = configInfo.Allwoforumindexpost.ToString();
            swfupload.SelectedValue = configInfo.Swfupload.ToString();
            quickforward.SelectedValue = configInfo.Quickforward.ToString();
            viewnewtopicminute.Text = configInfo.Viewnewtopicminute.ToString();
            rssstatus.SelectedValue = configInfo.Rssstatus.ToString();
            msgforwardlist.Text = configInfo.Msgforwardlist.Replace(",", "\r\n");
            if (configInfo.Disablepostad == 0)
            {
                postadstatus.Attributes.Add("style", "display:none");
            }

            #endregion
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region ����������Ϣ
            string[][] inputrule = new string[2][];
            inputrule[0] = new string[] { losslessdel.Text ,tpp.Text, ppp.Text, starthreshold.Text, hottopic.Text, 
                guestcachepagetimeout.Text,disablepostadregminute.Text,disablepostadpostcount.Text};
            inputrule[1] = new string[] { "ɾ����������ʱ��" ,"ÿҳ������", "ÿҳ������", "����������ֵ", "���Ż����������",
                "�����ο�ҳ���ʧЧʱ��","���û����ǿ������ע�������","���û����ǿ�����η�����" };
            for (int j = 0; j < inputrule[0].Length; j++)
            {
                if (!Utils.IsInt(inputrule[0][j].ToString()))
                {
                    base.RegisterStartupScript("", "<script>alert('�������:" + inputrule[1][j].ToString() + ",ֻ����0����������');window.location.href='forum_option.aspx';</script>");
                    return;
                }
            }
            if (Convert.ToInt32(losslessdel.Text) > 9999 || Convert.ToInt32(losslessdel.Text) < 0)
            {
                base.RegisterStartupScript("", "<script>alert('ɾ����������ʱ������ֻ����0-9999֮��');window.location.href='forum_option.aspx';</script>");
                return;
            }

            if (TypeConverter.StrToInt(edittimelimit.Text) > 9999999 || TypeConverter.StrToInt(edittimelimit.Text) < -1)
            {
                base.RegisterStartupScript("", "<script>alert('�༭����ʱ������ֻ����-1-9999999֮��');window.location.href='forum_option.aspx';</script>");
                return;
            }

            if (TypeConverter.StrToInt(deletetimelimit.Text) > 9999999 || TypeConverter.StrToInt(deletetimelimit.Text) < -1)
            {
                base.RegisterStartupScript("", "<script>alert('ɾ������ʱ������ֻ����-1-9999999֮��');window.location.href='forum_option.aspx';</script>");
                return;
            }

            if (Convert.ToInt16(tpp.Text) > 100 || Convert.ToInt16(tpp.Text) <= 0)
            {
                base.RegisterStartupScript("", "<script>alert('ÿҳ������ֻ����1-100֮��');window.location.href='forum_option.aspx';</script>");
                return;
            }

            if (Convert.ToInt16(ppp.Text) > 100 || Convert.ToInt16(ppp.Text) <= 0)
            {
                base.RegisterStartupScript("", "<script>alert('ÿҳ������ֻ����1-100֮��');window.location.href='forum_option.aspx';</script>");
                return;
            }
            if (Convert.ToInt16(starthreshold.Text) > 9999 || Convert.ToInt16(starthreshold.Text) < 0)
            {
                base.RegisterStartupScript("", "<script>alert('����������ֵֻ����0-9999֮��');window.location.href='forum_option.aspx';</script>");
                return;
            }

            if (Convert.ToInt16(hottopic.Text) > 9999 || Convert.ToInt16(hottopic.Text) < 0)
            {
                base.RegisterStartupScript("", "<script>alert('���Ż����������ֻ����0-9999֮��');window.location.href='forum_option.aspx';</script>");
                return;
            }

            if (Convert.ToInt16(hottagcount.Text) > 60 || Convert.ToInt16(hottagcount.Text) < 0)
            {
                base.RegisterStartupScript("", "<script>alert('��ҳ���ű�ǩ(Tag)����ֻ����0-60֮��');window.location.href='forum_option.aspx';</script>");
            }

            if (TypeConverter.StrToInt(viewnewtopicminute.Text) > 14400 || (TypeConverter.StrToInt(viewnewtopicminute.Text) < 5))
            {
                base.RegisterStartupScript("", "<script>alert('�鿴���������ñ�����5-14400֮��');window.location.href='global_uiandshowstyle.aspx';</script>");
                return;
            }
            if (!ValidateRatevalveset(ratevalveset1.Text)) return;
            if (!ValidateRatevalveset(ratevalveset2.Text)) return;
            if (!ValidateRatevalveset(ratevalveset3.Text)) return;
            if (!ValidateRatevalveset(ratevalveset4.Text)) return;
            if (!ValidateRatevalveset(ratevalveset5.Text)) return;
            if (!(Convert.ToInt16(ratevalveset1.Text) < Convert.ToInt16(ratevalveset2.Text) &&
                  Convert.ToInt16(ratevalveset2.Text) < Convert.ToInt16(ratevalveset3.Text) &&
                  Convert.ToInt16(ratevalveset3.Text) < Convert.ToInt16(ratevalveset4.Text) &&
                  Convert.ToInt16(ratevalveset4.Text) < Convert.ToInt16(ratevalveset5.Text)))
            {
                base.RegisterStartupScript("", "<script>alert('���ַ�ֵ���ǵ���ȡֵ');window.location.href='forum_option.aspx';</script>");
                return;
            }
            if (disablepostad.SelectedValue == "1" && disablepostadregular.Text == "")
            {
                base.RegisterStartupScript("", "<script>alert('���û����ǿ������������ʽΪ��');window.location.href='forum_option.aspx';</script>");
                return;
            }
            if (this.CheckCookie())
            {
                GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
                configInfo.Fullmytopics = Convert.ToInt16(fullmytopics.SelectedValue);
                configInfo.Modworkstatus = Convert.ToInt16(modworkstatus.SelectedValue);
                configInfo.Userstatusby = Convert.ToInt16(userstatusby.SelectedValue);
                if (Topicqueuestats_1.Checked == true)
                {
                    configInfo.TopicQueueStats = 1;
                }
                else
                {
                    configInfo.TopicQueueStats = 0;
                }
                configInfo.TopicQueueStatsCount = Convert.ToInt32(topicqueuestatscount.Text);
                configInfo.Guestcachepagetimeout = Convert.ToInt16(guestcachepagetimeout.Text);
                configInfo.Topiccachemark = Convert.ToInt16(topiccachemark.Text);
                configInfo.Losslessdel = Convert.ToInt16(losslessdel.Text);
                configInfo.Edittimelimit = TypeConverter.StrToInt(edittimelimit.Text);
                configInfo.Deletetimelimit = TypeConverter.StrToInt(deletetimelimit.Text);
                configInfo.Editedby = Convert.ToInt16(editedby.SelectedValue);
                //configInfo.Defaulteditormode = Convert.ToInt16(defaulteditormode.SelectedValue);
                configInfo.Allowswitcheditor = Convert.ToInt16(allowswitcheditor.SelectedValue);
                configInfo.Reasonpm = Convert.ToInt16(reasonpm.SelectedValue);
                configInfo.Hottopic = Convert.ToInt16(hottopic.Text);
                configInfo.Starthreshold = Convert.ToInt16(starthreshold.Text);
                configInfo.Fastpost = Convert.ToInt16(fastpost.SelectedValue);
                configInfo.Tpp = Convert.ToInt16(tpp.Text);
                configInfo.Ppp = Convert.ToInt16(ppp.Text);
                //configInfo.Htmltitle = Convert.ToInt32(allowhtmltitle.SelectedValue);
                configInfo.Enabletag = Convert.ToInt32(enabletag.SelectedValue);
                configInfo.Ratevalveset = ratevalveset1.Text + "," + ratevalveset2.Text + "," + ratevalveset3.Text + "," + ratevalveset4.Text + "," + ratevalveset5.Text;
                configInfo.Statstatus = Convert.ToInt16(statstatus.SelectedValue);
                configInfo.Statscachelife = Convert.ToInt16(statscachelife.Text);
                //__configinfo.Pvfrequence = Convert.ToInt16(pvfrequence.Text);
                configInfo.Hottagcount = Convert.ToInt16(hottagcount.Text);
                configInfo.Oltimespan = Convert.ToInt16(oltimespan.Text);
                configInfo.Maxmodworksmonths = Convert.ToInt16(maxmodworksmonths.Text);
                configInfo.Disablepostad = Convert.ToInt16(disablepostad.SelectedValue);
                configInfo.Disablepostadregminute = Convert.ToInt16(disablepostadregminute.Text);
                configInfo.Disablepostadpostcount = Convert.ToInt16(disablepostadpostcount.Text);
                configInfo.Disablepostadregular = disablepostadregular.Text;
                configInfo.Replynotificationstatus = Convert.ToInt16(replynotificationstatus.SelectedValue);
                configInfo.Replyemailstatus = Convert.ToInt16(replyemailstatus.SelectedValue);
                configInfo.Allwoforumindexpost = Convert.ToInt16(allowforumindexposts.SelectedValue);
                configInfo.Swfupload = Convert.ToInt16(swfupload.SelectedValue);
                configInfo.Viewnewtopicminute = TypeConverter.StrToInt(viewnewtopicminute.Text);
                configInfo.Quickforward = TypeConverter.StrToInt(quickforward.SelectedValue);
                configInfo.Msgforwardlist = msgforwardlist.Text.Replace("\r\n", ",");
                configInfo.Rssstatus = Convert.ToInt16(rssstatus.SelectedValue);
                GeneralConfigs.Serialiaze(configInfo, Server.MapPath("../../config/general.config"));
                Discuz.Forum.TopicStats.SetQueueCount();
                Caches.ReSetConfig();
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "��̳���ܳ���ѡ������", "");
                base.RegisterStartupScript("PAGE", "window.location.href='forum_option.aspx';");
            }
            #endregion
        }

        private bool ValidateRatevalveset(string val)
        {
            #region ��ֵ֤
            if (!Utils.IsNumeric(val))
            {
                base.RegisterStartupScript("", "<script>alert('���ָ��ֵֻ��������');window.location.href='forum_option.aspx';</script>");
                return false;
            }
            if (Convert.ToInt16(val) > 999 || Convert.ToInt16(val) < 1)
            {
                base.RegisterStartupScript("", "<script>alert('���ָ��ֵֻ����1-999֮��');window.location.href='forum_option.aspx';</script>");
                return false;
            }
            else
                return true;
            #endregion
        }

        #region Web ������������ɵĴ���

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
        }

        #endregion
    }
}
