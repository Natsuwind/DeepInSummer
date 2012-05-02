<%@ Page language="c#" AutoEventWireup="false" EnableViewState="false" Inherits="Discuz.Web.connect" %>
<%@ Import namespace="System.Data" %>
<%@ Import namespace="Discuz.Common" %>
<%@ Import namespace="Discuz.Forum" %>
<%@ Import namespace="Discuz.Entity" %>
<%@ Import namespace="Discuz.Config" %>

<script runat="server">
override protected void OnInit(EventArgs e)
{

	/* 
		This page was created by Discuz!NT Template Engine at 2011/9/13 14:15:05.
		本页面代码由Discuz!NT模板引擎生成于 2011/9/13 14:15:05. 
	*/

	base.OnInit(e);

	templateBuilder.Capacity = 220000;



	if (infloat!=1)
	{

	templateBuilder.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n    ");
	if (pagetitle=="首页")
	{

	templateBuilder.Append("\r\n        <title>");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append(" ");
	templateBuilder.Append(config.Seotitle.ToString().Trim());
	templateBuilder.Append(" - Powered by Discuz!NT</title>\r\n    ");
	}
	else
	{

	templateBuilder.Append("\r\n        <title>");
	templateBuilder.Append(pagetitle.ToString());
	templateBuilder.Append(" - ");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append(" ");
	templateBuilder.Append(config.Seotitle.ToString().Trim());
	templateBuilder.Append(" - Powered by Discuz!NT</title>\r\n    ");
	}	//end if

	templateBuilder.Append("\r\n    ");
	templateBuilder.Append(meta.ToString());
	templateBuilder.Append("\r\n    <meta name=\"generator\" content=\"Discuz!NT 3.9.913\" />\r\n    <meta name=\"author\" content=\"Discuz!NT Team and Comsenz UI Team\" />\r\n    <meta name=\"copyright\" content=\"2001-2011 Comsenz Inc.\" />\r\n    <meta http-equiv=\"x-ua-compatible\" content=\"ie=7\" />\r\n    <link rel=\"icon\" href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("favicon.ico\" type=\"image/x-icon\" />\r\n    <link rel=\"shortcut icon\" href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("favicon.ico\" type=\"image/x-icon\" />\r\n    ");
	if (pagename!="website.aspx")
	{

	templateBuilder.Append("\r\n        <link rel=\"stylesheet\" href=\"");
	templateBuilder.Append(cssdir.ToString());
	templateBuilder.Append("/dnt.css\" type=\"text/css\" media=\"all\" />\r\n    ");
	}	//end if

	templateBuilder.Append("\r\n    <link rel=\"stylesheet\" href=\"");
	templateBuilder.Append(cssdir.ToString());
	templateBuilder.Append("/float.css\" type=\"text/css\" />\r\n    ");
	if (isnarrowpage)
	{

	templateBuilder.Append("\r\n        <link type=\"text/css\" rel=\"stylesheet\" href=\"");
	templateBuilder.Append(cssdir.ToString());
	templateBuilder.Append("/widthauto.css\" id=\"css_widthauto\" />\r\n    ");
	}	//end if

	templateBuilder.Append("\r\n    ");
	templateBuilder.Append(link.ToString());
	templateBuilder.Append("\r\n    <script type=\"text/javascript\">\r\n        var creditnotice='");
	templateBuilder.Append(Scoresets.GetValidScoreNameAndId().ToString().Trim());
	templateBuilder.Append("';	\r\n        var forumpath = \"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("\";\r\n    </");
	templateBuilder.Append("script>\r\n    <script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/common.js\"></");
	templateBuilder.Append("script>\r\n    <script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_report.js\"></");
	templateBuilder.Append("script>\r\n    <script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_utils.js\"></");
	templateBuilder.Append("script>\r\n    <script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/ajax.js\"></");
	templateBuilder.Append("script>\r\n    <script type=\"text/javascript\">\r\n	    var aspxrewrite = ");
	templateBuilder.Append(config.Aspxrewrite.ToString().Trim());
	templateBuilder.Append(";\r\n	    var IMGDIR = '");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("';\r\n        var disallowfloat = '");
	templateBuilder.Append(config.Disallowfloatwin.ToString().Trim());
	templateBuilder.Append("';\r\n	    var rooturl=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("\";\r\n	    var imagemaxwidth='");
	templateBuilder.Append(Templates.GetTemplateWidth(templatepath).ToString().Trim());
	templateBuilder.Append("';\r\n	    var cssdir='");
	templateBuilder.Append(cssdir.ToString());
	templateBuilder.Append("';\r\n    </");
	templateBuilder.Append("script>\r\n    ");
	templateBuilder.Append(script.ToString());
	templateBuilder.Append("\r\n    <script type=\"text/javascript\" src=\"");
	templateBuilder.Append(config.Jqueryurl.ToString().Trim());
	templateBuilder.Append("\"></");
	templateBuilder.Append("script>\r\n    <script type=\"text/javascript\">jQuery.noConflict();</");
	templateBuilder.Append("script>\r\n</head>");

	templateBuilder.Append("\r\n<body onkeydown=\"if(event.keyCode==27) return false;\">\r\n<div id=\"append_parent\"></div><div id=\"ajaxwaitid\"></div>\r\n");
	if (headerad!="")
	{

	templateBuilder.Append("\r\n	<div id=\"ad_headerbanner\">");
	templateBuilder.Append(headerad.ToString());
	templateBuilder.Append("</div>\r\n");
	}	//end if

	templateBuilder.Append("\r\n<div id=\"hd\">\r\n	<div class=\"wrap\">\r\n		<div class=\"head cl\">\r\n			<h2><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("index.aspx\" title=\"");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append("\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/logo.png\" alt=\"");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append("\"/></a></h2>\r\n			");
	if (userid==-1)
	{


	if (pagename!="login.aspx"&&pagename!="register.aspx")
	{

	templateBuilder.Append("\r\n			<form onsubmit=\"if ($('ls_username').value == '' || $('ls_username').value == '用户名/Email') showWindow('login', '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("login.aspx');hideWindow('register');return\" action=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("login.aspx?referer=");
	templateBuilder.Append(pagename.ToString());
	templateBuilder.Append("\" id=\"lsform\" autocomplete=\"off\" method=\"post\">\r\n				<div class=\"fastlg c1\">\r\n					<div class=\"y pns\">\r\n						<p>\r\n							<label for=\"ls_username\">帐号</label> <input type=\"text\" tabindex=\"901\" value=\"用户名/Email\" id=\"ls_username\" name=\"username\" class=\"txt\" onblur=\"if(this.value == '') this.value = '用户名/Email';\" onfocus=\"if(this.value == '用户名/Email') this.value = '';\"/><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("register.aspx\" onClick=\"showWindow('register', '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("register.aspx');hideWindow('login');\" style=\"margin-left: 7px;\" class=\"xg2\">注册</a>							\r\n						</p>\r\n						<p>\r\n							<label for=\"ls_password\">密码</label> <input type=\"password\" onfocus=\"lsShowmore();innerVcode();\" tabindex=\"902\" autocomplete=\"off\" id=\"ls_password\" name=\"password\"  class=\"txt\"/>\r\n							&nbsp;<input type=submit style=\"width:0px;filter:alpha(opacity=0);-moz-opacity:0;opacity:0;display:none;\"/><button class=\"pn\" type=\"submit\"><span>登录</span></button>\r\n						</p>\r\n					</div>\r\n                    ");
	if (isopenconnect)
	{

	templateBuilder.Append("\r\n					<div class=\"fastlg_fm y\">\r\n						<p><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("connect.aspx\"><img alt=\"QQ登录\" class=\"vm\" src=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("images/common/qq_login.gif\"/></a></p>\r\n						<p class=\"hm xg1\">只需一步，快速开始</p>\r\n					</div>\r\n                    ");
	}	//end if

	templateBuilder.Append("\r\n				</div>\r\n                <div id=\"ls_more\" style=\"position:absolute;display:none;\">\r\n                <h3 class=\"cl\"><em class=\"y\"><a href=\"###\" class=\"flbc\" title=\"关闭\" onclick=\"closeIsMore();return false;\">关闭</a></em>安全选项</h3>\r\n                ");
	if (isLoginCode)
	{

	templateBuilder.Append("\r\n                    <div id=\"vcode_header\"></div>\r\n                    <script type=\"text/javascript\" reload=\"1\">\r\n                        if (typeof vcodeimgid == 'undefined') {\r\n                            var vcodeimgid = 1;\r\n                        }\r\n                        else\r\n                            vcodeimgid++;\r\n                        var secclick = new Array();\r\n                        var seccodefocus = 0;\r\n                        var optionVcode = function (id, type) {\r\n                            id = vcodeimgid;\r\n                            if ($('vcode')) {\r\n                                $('vcode').parentNode.removeChild($('vcode'));\r\n                            }\r\n\r\n                            if (!secclick['vcodetext_header' + id]) {\r\n                                if ($('vcodetext_header' + id) != null)\r\n                                    $('vcodetext_header' + id).value = '';\r\n                                secclick['vcodetext_header' + id] = 1;\r\n                                if (type)\r\n                                    $('vcodetext_header' + id + '_menu').style.top = parseInt($('vcodetext_header' + id + '_menu').style.top) - parseInt($('vcodetext_header' + id + '_menu').style.height) + 'px';\r\n                            }\r\n                            $('vcodetext_header' + id + '_menu').style.display = '';\r\n                            $('vcodetext_header' + id).unselectable = 'off';\r\n                            $('vcodeimg' + id).src = '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/VerifyImagePage.aspx?id=");
	templateBuilder.Append(olid.ToString());
	templateBuilder.Append("&time=' + Math.random();\r\n                        }\r\n\r\n                        function innerVcode() {\r\n                            if ($('vcodetext_header1') == null) {\r\n                                $('vcode_header').innerHTML = '<input name=\"vcodetext\" tabindex=\"903\" size=\"20\" onkeyup=\"changevcode(this.form, this.value);\" class=\"txt\" style=\"width:50px;\" id=\"vcodetext_header' + vcodeimgid + '\" value=\"\" autocomplete=\"off\"/>' +\r\n                                                            '<span><a href=\"###\" onclick=\"vcodeimg' + vcodeimgid + '.src=\\'");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/VerifyImagePage.aspx?id=");
	templateBuilder.Append(olid.ToString());
	templateBuilder.Append("&time=\\' + Math.random();return false;\" style=\"margin-left: 7px;\">看不清</a></span>' + '<p style=\"margin:6px 0\">输入下图中的字符</p>' +\r\n	                                                        '<div  style=\"cursor: pointer;width: 124px; height: 44px;top:256px;z-index:10009;padding:0;\" id=\"vcodetext_header' + vcodeimgid + '_menu\" onmouseout=\"seccodefocus = 0\" onmouseover=\"seccodefocus = 1\"><img src=\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/VerifyImagePage.aspx?time=");
	templateBuilder.Append(Processtime.ToString());
	templateBuilder.Append("\" class=\"cursor\" id=\"vcodeimg' + vcodeimgid + '\" onclick=\"this.src=\\'");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/VerifyImagePage.aspx?id=");
	templateBuilder.Append(olid.ToString());
	templateBuilder.Append("&time=\\' + Math.random();\"/></div>';\r\n                                optionVcode();\r\n                            }\r\n                        }\r\n\r\n                        function changevcode(form, value) {\r\n                            if (!$('vcode')) {\r\n                                var vcode = document.createElement('input');\r\n                                vcode.id = 'vcode';\r\n                                vcode.name = 'vcode';\r\n                                vcode.type = 'hidden';\r\n                                vcode.value = value;\r\n                                form.appendChild(vcode);\r\n                            } else {\r\n                                $('vcode').value = value;\r\n                            }\r\n                        }\r\n                    </");
	templateBuilder.Append("script>\r\n                ");
	}
	else
	{

	templateBuilder.Append("\r\n                    <script type=\"text/javascript\">\r\n                        function innerVcode() {\r\n                        }\r\n                    </");
	templateBuilder.Append("script>\r\n                ");
	}	//end if


	if (config.Secques==1)
	{

	templateBuilder.Append("\r\n			    <div id=\"floatlayout_login\" class=\"pbm\">\r\n					<select style=\"width:156px;margin-bottom:8px;\" id=\"question\" name=\"question\" name=\"question\" onchange=\"displayAnswer();\" tabindex=\"904\">\r\n						<option id=\"question\" value=\"0\" selected=\"selected\">安全提问(未设置请忽略)</option>\r\n						<option id=\"question\" value=\"1\">母亲的名字</option>\r\n						<option id=\"question\" value=\"2\">爷爷的名字</option>\r\n						<option id=\"question\" value=\"3\">父亲出生的城市</option>\r\n						<option id=\"question\" value=\"4\">您其中一位老师的名字</option>\r\n						<option id=\"question\" value=\"5\">您个人计算机的型号</option>\r\n						<option id=\"question\" value=\"6\">您最喜欢的餐馆名称</option>\r\n						<option id=\"question\" value=\"7\">驾驶执照的最后四位数字</option>\r\n					</select>\r\n					<input type=\"text\" tabindex=\"905\" class=\"txt\" size=\"20\" autocomplete=\"off\" style=\"width:140px;display:none;\"  id=\"answer\" name=\"answer\"/>\r\n		        </div>\r\n                ");
	}	//end if

	templateBuilder.Append("\r\n                <script type=\"text/javascript\">\r\n                    function closeIsMore() {\r\n                        $('ls_more').style.display = 'none';\r\n                    }\r\n                    function displayAnswer() {\r\n                        if ($(\"question\").value > 0)\r\n                            $(\"answer\").style.display = \"\";\r\n                        else\r\n                            $(\"answer\").style.display = \"none\";\r\n                    }\r\n                </");
	templateBuilder.Append("script>\r\n				<div class=\"ptm cl\" style=\"border-top:1px dashed #CDCDCD;\">\r\n					<a class=\"y xg2\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("getpassword.aspx\" onclick=\"hideWindow('register');hideWindow('login');showWindow('getpassword', this.href);\">找回密码</a>\r\n					<label class=\"z\" for=\"ls_cookietime\"><input type=\"checkbox\" tabindex=\"906\" value=\"2592000\" id=\"ls_cookietime\" name=\"expires\" checked=\"checked\" tabindex=\"906\"><span title=\"下次访问自动登录\">记住我</span></label>\r\n				</div>\r\n            </div>\r\n			</form>\r\n            ");
	}	//end if


	}
	else
	{

	templateBuilder.Append("\r\n			<div id=\"um\">\r\n				<div class=\"avt y\"><a alt=\"用户名称\" target=\"_blank\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercp.aspx\"><img src=\"");
	templateBuilder.Append(useravatar.ToString());
	templateBuilder.Append("\" onerror=\"this.onerror=null;this.src='");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("images/common/noavatar_small.gif';\" /></a></div>\r\n				<p>\r\n					<strong><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("userinfo.aspx?userid=");
	templateBuilder.Append(userid.ToString());
	templateBuilder.Append("\" class=\"vwmy");
	if (isbindconnect)
	{

	templateBuilder.Append(" qq");
	}	//end if

	templateBuilder.Append("\">");
	templateBuilder.Append(username.ToString());
	templateBuilder.Append("</a></strong><span class=\"xg1\">在线</span><span class=\"pipe\">|</span>\r\n					");
	if (isopenconnect&&!isbindconnect)
	{

	templateBuilder.Append(" <a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("connect.aspx?action=bindscript\"><img alt=\"QQ绑定\" class=\"vm qq_bind\" src=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("images/common/qq_bind_small.gif\"></a><span class=\"pipe\">|</span>");
	}	//end if

	string linktitle = "";
	
	string showoverflow = "";
	

	if (oluserinfo.Newpms>0)
	{


	if (oluserinfo.Newpms>=1000)
	{

	 showoverflow = "大于";
	

	}	//end if

	 linktitle = "您有"+showoverflow+oluserinfo.Newpms+"条新短消息";
	

	}
	else
	{

	 linktitle = "您没有新短消息";
	

	}	//end if

	templateBuilder.Append("\r\n					<a id=\"pm_ntc\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpinbox.aspx\" title=\"");
	templateBuilder.Append(linktitle.ToString());
	templateBuilder.Append("\">短消息\r\n                    ");
	if (oluserinfo.Newpms>0 && oluserinfo.Newpms<=1000)
	{

	templateBuilder.Append("\r\n                                (");
	templateBuilder.Append(oluserinfo.Newpms.ToString().Trim());
	if (oluserinfo.Newpms>1000)
	{

	templateBuilder.Append("1000+");
	}	//end if

	templateBuilder.Append(")\r\n                    ");
	}	//end if

	templateBuilder.Append("</a>\r\n                    <span class=\"pipe\">|</span>\r\n                    ");	 showoverflow = "";
	

	if (oluserinfo.Newnotices>0)
	{


	if (oluserinfo.Newnotices>=1000)
	{

	 showoverflow = "大于";
	

	}	//end if

	 linktitle = "您有"+showoverflow+oluserinfo.Newnotices+"条新通知";
	

	}
	else
	{

	 linktitle = "您没有新通知";
	

	}	//end if

	templateBuilder.Append("\r\n					<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpnotice.aspx?filter=all\" title=\"");
	templateBuilder.Append(linktitle.ToString());
	templateBuilder.Append("\">\r\n                        通知");
	if (oluserinfo.Newnotices>0)
	{

	templateBuilder.Append("\r\n                                (");
	templateBuilder.Append(oluserinfo.Newnotices.ToString().Trim());
	if (oluserinfo.Newnotices>=1000)
	{

	templateBuilder.Append("+");
	}	//end if

	templateBuilder.Append(")\r\n                            ");
	}	//end if

	templateBuilder.Append("\r\n                    </a>\r\n                    <span class=\"pipe\">|</span>\r\n					<a id=\"usercenter\" class=\"drop\" onmouseover=\"showMenu(this.id);\" href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercp.aspx\">用户中心</a>\r\n				");
	if (config.Regstatus==2||config.Regstatus==3)
	{


	if (userid>0)
	{

	templateBuilder.Append("\r\n					<span class=\"pipe\">|</span><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("invite.aspx\">邀请</a>\r\n					");
	}	//end if


	}	//end if


	if (useradminid==1)
	{

	templateBuilder.Append("\r\n					<span class=\"pipe\">|</span><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("admin/index.aspx\" target=\"_blank\">系统设置</a>\r\n					");
	}	//end if

	templateBuilder.Append("\r\n					<span class=\"pipe\">|</span><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("logout.aspx?userkey=");
	templateBuilder.Append(userkey.ToString());
	templateBuilder.Append("\">退出</a>\r\n				</p>\r\n				");
	templateBuilder.Append(userinfotips.ToString());
	templateBuilder.Append("\r\n			</div> \r\n			<div id=\"pm_ntc_menu\" class=\"g_up\" style=\"display:none;\">\r\n				<div class=\"mncr\"></div>\r\n				<div class=\"crly\">\r\n					<div style=\"clear:both;font-size:0;\"></div>\r\n					<span class=\"y\"><a onclick=\"javascript:$('pm_ntc_menu').style.display='none';closenotice(");
	templateBuilder.Append(oluserinfo.Newpms.ToString().Trim());
	templateBuilder.Append(");\" href=\"javascript:;\"><img src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/delete.gif\" alt=\"关闭\"/></a></span>\r\n					<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpinbox.aspx\">您有");
	if (oluserinfo.Newpms>=1000)
	{

	templateBuilder.Append("大于");
	}	//end if
	templateBuilder.Append(oluserinfo.Newpms.ToString().Trim());
	templateBuilder.Append("条新消息</a>\r\n				</div>\r\n			</div>\r\n            <script type=\"text/javascript\">\r\n            setMenuPosition('pm_ntc', 'pm_ntc_menu', '43');\r\n            if(");
	templateBuilder.Append(oluserinfo.Newpms.ToString().Trim());
	templateBuilder.Append(" > 0 && (getcookie(\"shownotice\") != \"0\" || getcookie(\"newpms\") != ");
	templateBuilder.Append(oluserinfo.Newpms.ToString().Trim());
	templateBuilder.Append("))\r\n            {\r\n                $(\"pm_ntc_menu\").style.display='';\r\n            }            \r\n            </");
	templateBuilder.Append("script>\r\n            ");
	}	//end if

	templateBuilder.Append("\r\n		</div>\r\n		<div id=\"menubar\">\r\n			<a onMouseOver=\"showMenu(this.id, false);\" href=\"javascript:void(0);\" id=\"mymenu\">我的中心</a>\r\n            <div class=\"popupmenu_popup headermenu_popup\" id=\"mymenu_menu\" style=\"display: none\">\r\n            ");
	if (userid!=-1)
	{

	templateBuilder.Append("\r\n			<ul class=\"sel_my\">\r\n				<li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("mytopics.aspx\">我的主题</a></li>\r\n				<li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("myposts.aspx\">我的帖子</a></li>\r\n				<li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("search.aspx?posterid=current&type=digest&searchsubmit=1\">我的精华</a></li>\r\n				<li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("myattachment.aspx\">我的附件</a></li>\r\n				<li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpsubscribe.aspx\">我的收藏</a></li>\r\n			");
	if (config.Enablespace==1)
	{

	templateBuilder.Append("\r\n				<li class=\"myspace\"><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("space/\">我的空间</a></li>\r\n			");
	}	//end if


	if (config.Enablealbum==1)
	{

	templateBuilder.Append("\r\n				<li class=\"myalbum\"><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("showalbumlist.aspx?uid=");
	templateBuilder.Append(userid.ToString());
	templateBuilder.Append("\">我的相册</a></li>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n            </ul>\r\n            ");
	}
	else
	{

	templateBuilder.Append("\r\n			<p class=\"reg_tip\">\r\n				<a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("register.aspx\" onClick=\"showWindow('register', '");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("register.aspx');hideWindow('login');\" class=\"xg2\">登录或注册新用户,开通自己的个人中心</a>\r\n			</p>\r\n            ");
	}	//end if


	if (config.Allowchangewidth==1&&pagename!="website.aspx")
	{

	templateBuilder.Append("\r\n           <ul class=\"sel_mb\">\r\n				<li><a href=\"javascript:;\" onclick=\"widthauto(this,'");
	templateBuilder.Append(cssdir.ToString());
	templateBuilder.Append("')\">");
	if (isnarrowpage)
	{

	templateBuilder.Append("切换到宽版");
	}
	else
	{

	templateBuilder.Append("切换到窄版");
	}	//end if

	templateBuilder.Append("</a></li>\r\n 			</ul>\r\n        ");
	}	//end if

	templateBuilder.Append("\r\n            </div>\r\n			<ul id=\"menu\" class=\"cl\">\r\n				");
	templateBuilder.Append(mainnavigation.ToString());
	templateBuilder.Append("\r\n			</ul>\r\n		</div>\r\n	</div>\r\n</div>\r\n");
	}
	else
	{


	Response.Clear();
	Response.ContentType = "Text/XML";
	Response.Expires = 0;
	Response.Cache.SetNoStore();
	
	templateBuilder.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?><root><![CDATA[\r\n");
	}	//end if



	templateBuilder.Append("\r\n<script type=\"text/javascript\" src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_register.js\"></");
	templateBuilder.Append("script>\r\n<div class=\"wrap cl pageinfo\">\r\n    <div id=\"nav\">\r\n        <a href=\"");
	templateBuilder.Append(config.Forumurl.ToString().Trim());
	templateBuilder.Append("\">");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append("</a> &raquo; <strong>QQ Connect</strong>\r\n    </div>\r\n</div>\r\n");
	if (page_err==0)
	{


	if (ispost)
	{

	templateBuilder.Append("\r\n        <script type=\"text/javascript\">\r\n            function con_handle_response(obj) {\r\n                if (obj.errMessage != '')\r\n                    alert(obj.errMessage);\r\n            }\r\n        </");
	templateBuilder.Append("script>\r\n        ");
	templateBuilder.Append(notifyscript.ToString());
	templateBuilder.Append("\r\n        ");
	templateBuilder.Append("<div class=\"wrap s_clear\" id=\"wrap\">\r\n<div class=\"main\">\r\n	<div class=\"msgbox\">\r\n		<h1>");
	templateBuilder.Append(config.Forumtitle.ToString().Trim());
	templateBuilder.Append("　提示信息</h1>\r\n		<hr class=\"solidline\"/>\r\n		<div class=\"msg_inner\">\r\n			<p>");
	templateBuilder.Append(msgbox_text.ToString());
	templateBuilder.Append("</p>\r\n			");
	if (msgbox_url!="")
	{

	templateBuilder.Append("\r\n			<p><a href=\"");
	templateBuilder.Append(msgbox_url.ToString());
	templateBuilder.Append("\">如果浏览器没有转向, 请点击这里.</a></p>\r\n			");
	}	//end if

	templateBuilder.Append("\r\n		</div>\r\n	</div>\r\n</div>\r\n</div>");


	}
	else
	{

	templateBuilder.Append("\r\n        <div class=\"wrap cl\">\r\n            <div class=\"main cl regbox\">\r\n                ");
	if (action=="access")
	{

	templateBuilder.Append("\r\n                <div class=\"f_tab cl\">\r\n                    <ul style=\"padding-left: 100px;\">\r\n                        ");
	if (allowreg&&userid<0)
	{

	templateBuilder.Append("\r\n                        <li id=\"connect_tab_1\"><a tabindex=\"900\" onclick=\"connect_switch(1);\" href=\"javascript:;\">完善帐号信息</a></li>\r\n                        ");
	}	//end if

	templateBuilder.Append("\r\n                        <li id=\"connect_tab_2\"><a tabindex=\"900\" href=\"javascript:;\" onclick=\"connect_switch(2);\">已有帐号？ 绑定我的帐号</a></li>\r\n                    </ul>\r\n                </div>\r\n                <div class=\"blr\">\r\n                    <form id=\"form1\" action=\"connect.aspx?action=bind\" method=\"post\" name=\"form1\">\r\n                    <input type=\"hidden\" name=\"openid\" value=\"");
	templateBuilder.Append(openid.ToString());
	templateBuilder.Append("\" />\r\n                    <input type=\"hidden\" id=\"bindtype\" name=\"bind_type\" value=\"new\" />\r\n                    <div class=\"c cl\">\r\n                        <div style=\"overflow: hidden; overflow-y: auto\" class=\"lgfm\" id=\"reginfo_a\">\r\n                            <span id=\"activation_hidden_1\" style=\"display:none;\">\r\n                                <label><em><img class=\"mtn\" alt=\"QQ\" src=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("images/common/connect_qq.gif\" /></em>您将使用QQ账号注册本站，\r\n                                    <a href=\"connect.aspx\" class=\"xg2\">更换QQ账号？</a></label>\r\n                                ");
	if (config.Regstatus==3)
	{

	templateBuilder.Append("\r\n                                <label>\r\n                                    <em>邀请码:</em><input name=\"invitecode\" type=\"text\" id=\"invitecode\" size=\"20\" class=\"txt\"/>\r\n                                </label>\r\n                                ");
	}	//end if

	templateBuilder.Append("\r\n                                <label><em>用户名:</em>\r\n                                    <input name=\"");
	templateBuilder.Append(config.Antispamregisterusername.ToString().Trim());
	templateBuilder.Append("\" type=\"text\" id=\"username\" onfocus=\"showTipInfo(this);\" onblur=\"checkusername(this);\" size=\"20\" class=\"txt\" maxlength=\"20\" tabindex=\"1\" />* \r\n                                    ");
	if (usedusernames!="")
	{

	templateBuilder.Append("\r\n                                    <a href=\"###\" id=\"usedusername\" onclick=\"showMenu(this.id);\">(曾用论坛用户名)</a>\r\n                                    ");
	}	//end if

	templateBuilder.Append(" \r\n                                    <i id=\"username_tip\"></i><span id=\"username_error\"></span>\r\n                                </label>\r\n                                <label>\r\n                                    <em>Email:</em><input type=\"text\" class=\"txt\" value=\"");
	templateBuilder.Append(email.ToString());
	templateBuilder.Append("\" maxlength=\"20\" size=\"25\" autocomplete=\"off\" name=\"");
	templateBuilder.Append(config.Antispamregisteremail.ToString().Trim());
	templateBuilder.Append("\" id=\"email\" onfocus=\"showTipInfo(this);\" onblur=\"checkemail(this)\"/>\r\n                                    * <i id=\"email_tip\"></i><span id=\"email_error\"></span>\r\n                                </label>\r\n                                ");
	if (birthday!="")
	{

	templateBuilder.Append("\r\n                                <label>\r\n                                    <em>生日:</em>\r\n                                    <input name=\"bday\" type=\"text\" class=\"txt\" id=\"bday\" size=\"10\" value=\"");
	templateBuilder.Append(birthday.ToString());
	templateBuilder.Append("\" style=\"cursor:default\" onclick=\"showcalendar(event, 'bday', 'cal_startdate', 'cal_enddate', this.value);\" readonly=\"readonly\" />&nbsp;<button type=\"button\" onclick=\"$('bday').value='';\" class=\"pn\">清空</button>\r\n                                    <input type=\"hidden\" name=\"cal_startdate\" id=\"cal_startdate\" size=\"10\"  value=\"1900-01-01\" />\r\n					                <input type=\"hidden\" name=\"cal_enddate\" id=\"cal_enddate\" size=\"10\"  value=\"");
	templateBuilder.Append(nowdatetime.ToString());
	templateBuilder.Append("\" />\r\n                                </label>\r\n                                ");
	}	//end if

	templateBuilder.Append("\r\n                                <label>\r\n                                    <em>性别:</em>\r\n							        <select name=\"gender\" id=\"gender\">\r\n								        <option value=\"0\" ");
	if (gender==0)
	{

	templateBuilder.Append("selected=\"selected\"");
	}	//end if

	templateBuilder.Append(">保密</option>\r\n								        <option value=\"1\" ");
	if (gender==1)
	{

	templateBuilder.Append("selected=\"selected\"");
	}	//end if

	templateBuilder.Append(">男</option>\r\n								        <option value=\"2\" ");
	if (gender==2)
	{

	templateBuilder.Append("selected=\"selected\"");
	}	//end if

	templateBuilder.Append(">女</option>\r\n							        </select>\r\n                                    <script type=\"text/javascript\">simulateSelect('gender');</");
	templateBuilder.Append("script>\r\n                                </label>\r\n                                ");
	if (cloudconfig.Allowuseqzavater==1)
	{

	templateBuilder.Append("\r\n                                <label><em>头像:</em><img src=\"");
	templateBuilder.Append(avatarurl.ToString());
	templateBuilder.Append("\"  onerror=\"this.onerror=null;this.src='");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("images/common/noavatar_medium.gif';\"  alt=\"用户头像\" /><p style=\"padding-left: 70px; margin-top: 4px;\">\r\n                                    <input id=\"use_qzone_avatar\" class=\"pc\" type=\"checkbox\" tabindex=\"1\" checked=\"checked\" value=\"1\" name=\"use_qzone_avatar\" />使用QQ空间头像</p>\r\n                                </label>\r\n                                ");
	}	//end if

	templateBuilder.Append("\r\n                            </span>\r\n                            <span id=\"activation_hidden_2\" style=\"display:none;\">\r\n                                ");
	if (userid<0)
	{


	if (isbindoverflow)
	{

	templateBuilder.Append("\r\n                                <label>您的QQ帐号在本论坛已经注册了过多的帐号,无法再注册新的帐号.您可以绑定已有的论坛帐号</label>\r\n                                ");
	}	//end if

	templateBuilder.Append("\r\n                                <label>\r\n                                    <em>帐号:</em><input name=\"loginusername\" type=\"text\" id=\"loginusername\" size=\"20\" class=\"txt\"/>\r\n                                </label>\r\n                                <label>\r\n                                    <em>密码:</em><input name=\"password\" type=\"password\" id=\"password\" size=\"20\" class=\"txt\"/>\r\n                                </label>\r\n                                <label>\r\n                                    <em>安全提问:</em>\r\n                                    <select name=\"question\" id=\"bindquestion\" change=\"changequestion();\">\r\n						                <option value=\"0\" selected=\"selected\">安全提问</option>\r\n						                <option value=\"1\">母亲的名字</option>\r\n						                <option value=\"2\">爷爷的名字</option>\r\n						                <option value=\"3\">父亲出生的城市</option>\r\n						                <option value=\"4\">您其中一位老师的名字</option>\r\n						                <option value=\"5\">您个人计算机的型号</option>\r\n						                <option value=\"6\">您最喜欢的餐馆名称</option>\r\n						                <option value=\"7\">驾驶执照的最后四位数字</option>\r\n					                </select>\r\n					                <script type=\"text/javascript\">simulateSelect('bindquestion','214');</");
	templateBuilder.Append("script>\r\n                                </label>\r\n                                <label id=\"answerlabel\" style=\" display:none;\">\r\n                                    <em>答案:</em><input name=\"answer\" type=\"text\" id=\"answer\" size=\"20\" class=\"txt\" />\r\n                                </label>\r\n                                <script type=\"text/javascript\">\r\n                                    function changequestion() {\r\n                                        if ($('bindquestion').getAttribute(\"selecti\") != \"0\") {\r\n                                            $('answerlabel').style.display = '';\r\n                                            $('answerlabel').focus();\r\n                                        }\r\n                                        else {\r\n                                            $('answerlabel').style.display = 'none';\r\n                                        }\r\n                                    }\r\n                                </");
	templateBuilder.Append("script>\r\n                                ");
	}
	else
	{

	templateBuilder.Append("\r\n                                <label><em>当前用户:</em>");
	templateBuilder.Append(username.ToString());
	templateBuilder.Append("</label>\r\n                                ");
	}	//end if

	templateBuilder.Append("\r\n                            </span>\r\n                        </div>\r\n                    </div>\r\n                    <p class=\"fsb pns cl\">\r\n                        <span id=\"submit_b_btn_1\" style=\"display:none;\">\r\n                            <button tabindex=\"5\" value=\"true\" name=\"regsubmit\" type=\"submit\" id=\"registerformsubmit\" class=\"pn\"><span>完成，继续浏览</span></button>\r\n                        </span>\r\n                        <span id=\"submit_b_btn_2\" style=\"display:none;\">\r\n                            <button tabindex=\"5\" value=\"true\" name=\"bindsubmit\" type=\"submit\" id=\"bindsubmit\" class=\"pn\"><span>绑定帐号,继续浏览</span></button>\r\n                        </span>\r\n                    </p>\r\n                    <ul id=\"usedusername_menu\" class=\"popupmenu_popup\" style=\"display: none;\">\r\n                    </ul>\r\n                    ");
	if (usedusernames!="")
	{

	templateBuilder.Append("\r\n                    <script type=\"text/javascript\">\r\n                        var usedusername = '");
	templateBuilder.Append(usedusernames.ToString());
	templateBuilder.Append("';\r\n                        var userusernamearray = usedusername.split(',');\r\n                        if (userusernamearray.length > 0) {\r\n                            for (i = 0; i < userusernamearray.length; i++) {\r\n                                jQuery('#usedusername_menu').append('<li><a href=\"###\" onclick=\"appendvalue(this);\">' + userusernamearray[i] + '</a></li>');\r\n                                if (i + 1 == userusernamearray.length)\r\n                                    jQuery('#username').val(userusernamearray[i]);\r\n                            }\r\n                        }\r\n\r\n                        function appendvalue(obj) {\r\n                            jQuery('#username').val(obj.innerHTML);\r\n                        }\r\n                    </");
	templateBuilder.Append("script>\r\n                    ");
	}	//end if

	templateBuilder.Append("\r\n                    <script type=\"text/javascript\">\r\n                        if ($('username').value != \"\") {\r\n                            checkusername($('username'));\r\n                        }\r\n                        if ($('email').value != \"\") {\r\n                            checkemail($('email'));\r\n                        }\r\n\r\n                        function connect_switch(type) {\r\n                            jQuery('#connect_tab_1').attr('class', '');\r\n                            jQuery('#activation_hidden_1').css('display', 'none');\r\n                            jQuery('#submit_b_btn_1').css('display', 'none');\r\n                            jQuery('#connect_tab_2').attr('class', '');\r\n                            jQuery('#activation_hidden_2').css('display', 'none');\r\n                            jQuery('#submit_b_btn_2').css('display', 'none');\r\n\r\n                            jQuery('#connect_tab_' + type).attr('class', 'cur_tab');\r\n                            jQuery('#activation_hidden_' + type).css('display', '');\r\n                            jQuery('#submit_b_btn_' + type).css('display', '');\r\n                            jQuery('#bindtype').val(type == 1 ? 'new' : 'bind');\r\n                        }\r\n                        connect_switch(");
	templateBuilder.Append(connectswitch.ToString());
	templateBuilder.Append(");\r\n                    </");
	templateBuilder.Append("script>\r\n                    </form>\r\n                    ");
	if (connectswitch==1)
	{

	templateBuilder.Append("\r\n                    <div id=\"regrules_d\" style=\"display:none;\">\r\n                        <div id=\"regrules\">\r\n			                <div class=\"c cl floatwrap\">\r\n				                ");
	templateBuilder.Append(config.Rulestxt.ToString().Trim());
	templateBuilder.Append("\r\n			                </div>\r\n			                <p class=\"fsb pns cl\">\r\n				                <button type=\"submit\" id=\"btnagree\" class=\"pn pnc\"  onclick=\"hideMenu('fwin_dialog', 'dialog');\"><span>同意</span></button>\r\n				                <button name=\"cancel\" id=\"cancel\" onclick=\"window.location.href = forumpath + 'index.aspx';\" class=\"pn\"><span>不同意</span></button>\r\n			                </p>\r\n                        </div>\r\n                    </div>\r\n                    <script type=\"text/javascript\">\r\n                        showDialog($('regrules_d').innerHTML, 'info', '网站服务条款', null, 1);\r\n                    </");
	templateBuilder.Append("script>\r\n                    ");
	}	//end if

	templateBuilder.Append("\r\n                </div>\r\n                ");
	}
	else if (action=="unbind")
	{

	templateBuilder.Append("\r\n                <div class=\"blr\">\r\n                    <form id=\"unbind\" action=\"connect.aspx?action=unbind\" method=\"post\" name=\"unbind\">\r\n                        ");
	if (userconnectinfo.IsSetPassword==0)
	{

	templateBuilder.Append("\r\n                        <div class=\"c cl\">\r\n                            <div style=\"overflow: hidden; overflow-y: auto\" class=\"lgfm\" id=\"unbind_d\">\r\n                                <span>\r\n                                    <label>\r\n                                        <em>新密码:</em><input name=\"newpasswd\" type=\"password\" id=\"newpwd\" size=\"20\" class=\"txt\" onblur=\"checkpasswd(this);\" /> *\r\n                                        <i id=\"newpwd_tip\"></i><span id=\"newpwd_error\"></span>\r\n                                    </label>\r\n                                    <label id=\"passwdpower\" style=\"display:none;\"><em>密码强度</em><strong id=\"showmsg\"></strong>\r\n                                    </label>\r\n                                    <label>\r\n                                        <em>确认密码:</em><input name=\"confirmpasswd\" type=\"password\" id=\"confirmpwd\" onblur=\"checkconfirmpasswd();\" size=\"20\" class=\"txt\"/> *\r\n                                        <i id=\"confirmpwd_tip\"></i><span id=\"confirmpwd_error\"></span>\r\n                                    </label>\r\n                                </span>\r\n                            </div>\r\n                        </div>\r\n                        <script type=\"text/javascript\">\r\n                            function checkconfirmpasswd() {\r\n                                var pw1 = $('newpwd');\r\n                                var pw2 = $('confirmpwd');\r\n                                if (pw2.value.length < 6) {\r\n                                    setError(pw2, \"确认密码不得少于6个字符\");\r\n                                    return;\r\n                                }\r\n                                var str;\r\n                                if (pw1.value != pw2.value) {\r\n                                    str = \"两次输入的密码不一致\";\r\n                                }\r\n                                if (str != '' && str != null && str != undefined) {\r\n                                    setError(pw2, str);\r\n                                }\r\n                                else {\r\n                                    setError(pw2, \"\");\r\n                                }\r\n                            }\r\n                        </");
	templateBuilder.Append("script>\r\n                        ");
	}	//end if

	templateBuilder.Append("\r\n                        <p class=\"fsb pns cl\">\r\n                            <span id=\"unbind_btn\">\r\n                                <button tabindex=\"5\" value=\"true\" name=\"unbindsubmit\" type=\"submit\" id=\"unbindsubmit\" class=\"pn\"><span>解绑QQ</span></button>\r\n                            </span>\r\n                        </p>\r\n                    </form>\r\n                </div>\r\n                ");
	}	//end if

	templateBuilder.Append("\r\n            </div>\r\n        </div>\r\n        <script type=\"text/javascript\"  src=\"");
	templateBuilder.Append(jsdir.ToString());
	templateBuilder.Append("/template_calendar.js\"></");
	templateBuilder.Append("script>\r\n    ");
	}	//end if


	}
	else
	{


	templateBuilder.Append("<div class=\"wrap cl\">\r\n<div class=\"main\">\r\n	<div class=\"msgbox\">\r\n		<h1>出现了");
	templateBuilder.Append(page_err.ToString());
	templateBuilder.Append("个错误</h1>\r\n		<hr class=\"solidline\"/>\r\n		<div class=\"msg_inner error_msg\">\r\n			<p>");
	templateBuilder.Append(msgbox_text.ToString());
	templateBuilder.Append("</p>\r\n			<p class=\"errorback\">\r\n				<script type=\"text/javascript\">\r\n					if(");
	templateBuilder.Append(msgbox_showbacklink.ToString());
	templateBuilder.Append(")\r\n					{\r\n						document.write(\"<a href=\\\"");
	templateBuilder.Append(msgbox_backlink.ToString());
	templateBuilder.Append("\\\">返回上一步</a> &nbsp; &nbsp;|&nbsp; &nbsp  \");\r\n					}\r\n				</");
	templateBuilder.Append("script>\r\n				<a href=\"forumindex.aspx\">论坛首页</a>\r\n				");
	if (usergroupid==7)
	{

	templateBuilder.Append("\r\n				 &nbsp; &nbsp;|&nbsp; &nbsp; <a href=\"login.aspx\">登录</a>&nbsp; &nbsp;|&nbsp; &nbsp; <a href=\"register.aspx\">注册</a>\r\n				");
	}	//end if

	templateBuilder.Append("\r\n			</p>\r\n		</div>\r\n	</div>\r\n</div>\r\n</div>");


	}	//end if



	if (infloat!=1)
	{


	if (pagename=="website.aspx")
	{

	templateBuilder.Append("    \r\n       <div id=\"websitebottomad\"></div>\r\n");
	}
	else if (footerad!="")
	{

	templateBuilder.Append(" \r\n     <div id=\"ad_footerbanner\">");
	templateBuilder.Append(footerad.ToString());
	templateBuilder.Append("</div>   \r\n");
	}	//end if

	templateBuilder.Append("\r\n<div id=\"footer\">\r\n	<div class=\"wrap\"  id=\"wp\">\r\n		<div id=\"footlinks\">\r\n			<p><a href=\"");
	templateBuilder.Append(config.Weburl.ToString().Trim());
	templateBuilder.Append("\" target=\"_blank\">");
	templateBuilder.Append(config.Webtitle.ToString().Trim());
	templateBuilder.Append("</a> - ");
	templateBuilder.Append(config.Linktext.ToString().Trim());
	templateBuilder.Append(" - <a target=\"_blank\" href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("stats.aspx\">统计</a> - ");
	if (config.Sitemapstatus==1)
	{

	templateBuilder.Append("&nbsp;<a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("tools/sitemap.aspx\" target=\"_blank\" title=\"百度论坛收录协议\">Sitemap</a>");
	}	//end if

	templateBuilder.Append("\r\n			");
	templateBuilder.Append(config.Statcode.ToString().Trim());
	templateBuilder.Append(config.Icp.ToString().Trim());
	templateBuilder.Append("\r\n			</p>\r\n			<div>\r\n				<a href=\"http://www.comsenz.com/\" target=\"_blank\">Comsenz Technology Ltd</a>\r\n				- <a href=\"");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("archiver/index.aspx\" target=\"_blank\">简洁版本</a>\r\n			");
	if (config.Stylejump==1)
	{


	if (userid!=-1 || config.Guestcachepagetimeout<=0)
	{

	templateBuilder.Append("\r\n				- <span id=\"styleswitcher\" class=\"drop\" onmouseover=\"showMenu({'ctrlid':this.id, 'pos':'21'})\" onclick=\"window.location.href='");
	templateBuilder.Append(forumurl.ToString());
	templateBuilder.Append("showtemplate.aspx'\">界面风格</span>\r\n				");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n			</div>\r\n		</div>\r\n		<a title=\"Powered by Discuz!NT\" target=\"_blank\" href=\"http://nt.discuz.net\"><img border=\"0\" alt=\"Discuz!NT\" src=\"");
	templateBuilder.Append(imagedir.ToString());
	templateBuilder.Append("/discuznt_logo.gif\"/></a>\r\n		<p id=\"copyright\">\r\n			Powered by <strong><a href=\"http://nt.discuz.net\" target=\"_blank\" title=\"Discuz!NT\">Discuz!NT</a></strong> <em class=\"f_bold\">3.9.913 Beta</em>\r\n			");
	if (config.Licensed==1)
	{

	templateBuilder.Append("\r\n				(<a href=\"\" onclick=\"this.href='http://nt.discuz.net/certificate/?host='+location.href.substring(0, location.href.lastIndexOf('/'))\" target=\"_blank\">Licensed</a>)\r\n			");
	}	//end if

	templateBuilder.Append("\r\n				");
	templateBuilder.Append(config.Forumcopyright.ToString().Trim());
	templateBuilder.Append("\r\n		</p>\r\n		<p id=\"debuginfo\" class=\"grayfont\">\r\n		");
	if (config.Debug!=0)
	{

	templateBuilder.Append("\r\n			Processed in ");
	templateBuilder.Append(this.Processtime.ToString().Trim());
	templateBuilder.Append(" second(s)\r\n			");
	if (isguestcachepage==1)
	{

	templateBuilder.Append("\r\n				(Cached).\r\n			");
	}
	else if (querycount>1)
	{

	templateBuilder.Append("\r\n				 , ");
	templateBuilder.Append(querycount.ToString());
	templateBuilder.Append(" queries.\r\n			");
	}
	else
	{

	templateBuilder.Append("\r\n				 , ");
	templateBuilder.Append(querycount.ToString());
	templateBuilder.Append(" query.\r\n			");
	}	//end if


	}	//end if

	templateBuilder.Append("\r\n		</p>\r\n	</div>\r\n</div>\r\n<a id=\"scrolltop\" href=\"javascript:;\" style=\"display:none;\" class=\"scrolltop\" onclick=\"setScrollToTop(this.id);\">TOP</a>\r\n<ul id=\"usercenter_menu\" class=\"p_pop\" style=\"display:none;\">\r\n    <li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpprofile.aspx?action=avatar\">设置头像</a></li>\r\n    <li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpprofile.aspx\">个人资料</a></li>\r\n    <li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpnewpassword.aspx\">更改密码</a></li>\r\n    <li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercp.aspx\">用户组</a></li>\r\n    <li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpsubscribe.aspx\">收藏夹</a></li>\r\n    <li><a href=\"");
	templateBuilder.Append(forumpath.ToString());
	templateBuilder.Append("usercpcreditspay.aspx\">积分</a></li>\r\n</ul>\r\n\r\n");
	int prentid__loop__id=0;
	foreach(string prentid in mainnavigationhassub)
	{
		prentid__loop__id++;

	templateBuilder.Append("\r\n<ul class=\"p_pop\" id=\"menu_");
	templateBuilder.Append(prentid.ToString());
	templateBuilder.Append("_menu\" style=\"display: none\">\r\n");
	int subnav__loop__id=0;
	foreach(DataRow subnav in subnavigation.Rows)
	{
		subnav__loop__id++;

	bool isoutput = false;
	

	if (subnav["parentid"].ToString().Trim()==prentid)
	{


	if (subnav["level"].ToString().Trim()=="0")
	{

	 isoutput = true;
	

	}
	else
	{


	if (subnav["level"].ToString().Trim()=="1" && userid!=-1)
	{

	 isoutput = true;
	

	}
	else
	{

	bool leveluseradmindi = true;
	
	 leveluseradmindi = (useradminid==3 || useradminid==1 || useradminid==2);
	

	if (subnav["level"].ToString().Trim()=="2" &&  leveluseradmindi)
	{

	 isoutput = true;
	

	}	//end if


	if (subnav["level"].ToString().Trim()=="3" && useradminid==1)
	{

	 isoutput = true;
	

	}	//end if


	}	//end if


	}	//end if


	}	//end if


	if (isoutput)
	{

	templateBuilder.Append("\r\n    " + subnav["nav"].ToString().Trim() + "\r\n");
	}	//end if


	}	//end loop

	templateBuilder.Append("\r\n</ul>\r\n");
	}	//end loop


	if (config.Stylejump==1)
	{


	if (userid!=-1 || config.Guestcachepagetimeout<=0)
	{

	templateBuilder.Append("\r\n	<ul id=\"styleswitcher_menu\" class=\"popupmenu_popup s_clear\" style=\"display: none;\">\r\n	");
	templateBuilder.Append(templatelistboxoptions.ToString());
	templateBuilder.Append("\r\n	</ul>\r\n	");
	}	//end if


	}	//end if




	templateBuilder.Append("</body>\r\n</html>\r\n");
	}
	else
	{

	templateBuilder.Append("\r\n]]></root>\r\n");
	}	//end if




	Response.Write(templateBuilder.ToString());
}
</script>
