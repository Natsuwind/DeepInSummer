<%@ Page language="c#" AutoEventWireup="false" EnableViewState="false" Inherits="Wysky.Discuz.Plugin.QZoneLogin.Views.Main.QZoneLogin" %>
<%@ Import namespace="System.Data" %>
<%@ Import namespace="Discuz.Common" %>
<%@ Import namespace="Discuz.Forum" %>
<%@ Import namespace="Discuz.Entity" %>

<script runat="server">
override protected void OnInit(EventArgs e)
{

	/* 
		This page was created by Discuz!NT Template Engine at 2011/5/24 17:47:59.
		本页面代码由Discuz!NT模板引擎生成于 2011/5/24 17:47:59. 
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
	templateBuilder.Append("\r\n    <meta name=\"generator\" content=\"Discuz!NT 3.5.2\" />\r\n    <meta name=\"author\" content=\"Discuz!NT Team and Comsenz UI Team\" />\r\n    <meta name=\"copyright\" content=\"2001-2011 Comsenz Inc.\" />\r\n    <meta http-equiv=\"x-ua-compatible\" content=\"ie=7\" />\r\n    <link rel=\"icon\" href=\"");
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
	templateBuilder.Append("\r\n</head>");

	templateBuilder.Append("\r\n<body>\r\n");
	if (wysky_page_msg=="")
	{

	templateBuilder.Append("\r\n    <div class=\"cl\" style=\"margin: 30px auto; width: 96%;\">\r\n        <div style=\"border: 1px solid #9DCCF8; padding: 8px; width: 390px;\">\r\n            <div class=\"msgbox\" style=\"padding: 0 !important;\">\r\n                <p>\r\n                    您是第一次用QQ登录功能，请填写完整信息用户激活QQ登录</p>\r\n            </div>\r\n            <span id=\"returnregmessage\" style=\"color: #336699\">\r\n            ");
	if (page_err>0)
	{

	templateBuilder.Append("\r\n            ");
	templateBuilder.Append(msgbox_text.ToString());
	templateBuilder.Append("\r\n            ");
	}	//end if

	templateBuilder.Append("\r\n            </span>\r\n            <form id=\"formlogin\" name=\"formlogin\" method=\"post\" action=\"?callback=1&createuser=1\"\r\n            onsubmit=\"submitCheck(this);\">\r\n            <div class=\"c cl\" style=\"margin-top: 5px;\">\r\n                <div class=\"sipt lpsw\">\r\n                    <label for=\"username\">\r\n                        用户名 ：</label>\r\n                    <input type=\"text\" id=\"username\" name=\"username\" size=\"25\" maxlength=\"40\" tabindex=\"2\"\r\n                        value=\"");
	templateBuilder.Append(username.ToString());
	templateBuilder.Append("\" class=\"txt\" onkeyup=\"checkusername(this.value);\" />\r\n                </div>\r\n                <div class=\"sipt lpsw\">\r\n                    <label for=\"email\">\r\n                        Email ：</label>\r\n                    <input type=\"text\" id=\"email\" name=\"email\" size=\"25\" tabindex=\"3\" value=\"");
	templateBuilder.Append(email.ToString());
	templateBuilder.Append("\" class=\"txt\" onkeyup=\"checkemail(this.value)\" />\r\n                </div>\r\n            </div>\r\n            <p class=\"fsb pns cl\">\r\n                <input type=\"submit\" style=\"width: 0; filter: alpha(opacity=0); -moz-opacity: 0;\r\n                    opacity: 0;\" />\r\n                <button name=\"login\" type=\"submit\" id=\"login\" tabindex=\"8\" class=\"pn\">\r\n                    <span>激活QQ登录帐号</span></button>\r\n            </p>\r\n            </form>\r\n        </div>\r\n    </div>\r\n    <script type=\"text/javascript\">\r\n        document.getElementById(\"email\").focus();\r\n        function checkemail(strMail) {\r\n            var str;\r\n            if (strMail.length == 0) return false;\r\n            var objReg = new RegExp(\"[A-Za-z0-9-_]+@[A-Za-z0-9-_]+[\\.][A-Za-z0-9-_]\")\r\n            var IsRightFmt = objReg.test(strMail)\r\n            var objRegErrChar = new RegExp(\"[^a-z0-9-._@]\", \"ig\")\r\n            var IsRightChar = (strMail.search(objRegErrChar) == -1)\r\n            var IsRightLength = strMail.length <= 60\r\n            var IsRightPos = (strMail.indexOf(\"@\", 0) != 0 && strMail.indexOf(\".\", 0) != 0 && strMail.lastIndexOf(\"@\") + 1 != strMail.length && strMail.lastIndexOf(\".\") + 1 != strMail.length)\r\n            var IsNoDupChar = (strMail.indexOf(\"@\", 0) == strMail.lastIndexOf(\"@\"))\r\n            if (!(IsRightFmt && IsRightChar && IsRightLength && IsRightPos && IsNoDupChar)) {\r\n                str = \"E-mail 地址无效，请提供真实Email，用于找回密码和接收通知。\";\r\n            }\r\n            if (str != '' && str != null && str != undefined) {\r\n                $('returnregmessage').innerHTML = str;\r\n                $('returnregmessage').className = 'onerror';\r\n            }\r\n            else {\r\n                $('returnregmessage').className = '';\r\n                $('returnregmessage').innerHTML = '激活';\r\n            }\r\n        }\r\n        function htmlEncode(source, display, tabs) {\r\n            function special(source) {\r\n                var result = '';\r\n                for (var i = 0; i < source.length; i++) {\r\n                    var c = source.charAt(i);\r\n                    if (c < ' ' || c > '~') {\r\n                        c = '&#' + c.charCodeAt() + ';';\r\n                    }\r\n                    result += c;\r\n                }\r\n                return result;\r\n            }\r\n\r\n            function format(source) {\r\n                // Use only integer part of tabs, and default to 4\r\n                tabs = (tabs >= 0) ? Math.floor(tabs) : 4;\r\n\r\n                // split along line breaks\r\n                var lines = source.split(/\\r\\n|\\r|\\n/);\r\n\r\n                // expand tabs\r\n                for (var i = 0; i < lines.length; i++) {\r\n                    var line = lines[i];\r\n                    var newLine = '';\r\n                    for (var p = 0; p < line.length; p++) {\r\n                        var c = line.charAt(p);\r\n                        if (c === '\\t') {\r\n                            var spaces = tabs - (newLine.length % tabs);\r\n                            for (var s = 0; s < spaces; s++) {\r\n                                newLine += ' ';\r\n                            }\r\n                        }\r\n                        else {\r\n                            newLine += c;\r\n                        }\r\n                    }\r\n                    // If a line starts or ends with a space, it evaporates in html\r\n                    // unless it's an nbsp.\r\n                    newLine = newLine.replace(/(^ )|( $)/g, '&nbsp;');\r\n                    lines[i] = newLine;\r\n                }\r\n\r\n                // re-join lines\r\n                var result = lines.join('<br />');\r\n\r\n                // break up contiguous blocks of spaces with non-breaking spaces\r\n                result = result.replace(/  /g, ' &nbsp;');\r\n\r\n                // tada!\r\n                return result;\r\n            }\r\n\r\n            var result = source;\r\n\r\n            // ampersands (&)\r\n            result = result.replace(/\\&/g, '&amp;');\r\n\r\n            // less-thans (<)\r\n            result = result.replace(/\\</g, '&lt;');\r\n\r\n            // greater-thans (>)\r\n            result = result.replace(/\\>/g, '&gt;');\r\n\r\n            if (display) {\r\n                // format for display\r\n                result = format(result);\r\n            }\r\n            else {\r\n                // Replace quotes if it isn't for display,\r\n                // since it's probably going in an html attribute.\r\n                result = result.replace(new RegExp('\"', 'g'), '&quot;');\r\n            }\r\n\r\n            // special characters\r\n            result = special(result);\r\n\r\n            // tada!\r\n            return result;\r\n        }\r\n\r\n        var profile_username_toolong = '您的用户名超过 20 个字符，请输入一个较短的用户名。';\r\n        var profile_username_tooshort = '您输入的用户名小于3个字符, 请输入一个较长的用户名。';\r\n        var profile_username_pass = \"<img src='/templates/default/images/check_right.gif'/>\";\r\n\r\n        function checkusername(username) {\r\n            var unlen = username.replace(/[^\\x00-\\xff]/g, \"**\").length;\r\n\r\n            if (unlen < 3 || unlen > 20) {\r\n                $(\"returnregmessage\").innerHTML = (unlen < 3 ? profile_username_tooshort : profile_username_toolong);\r\n                $('returnregmessage').className = 'onerror';\r\n                return;\r\n            }\r\n            ajaxRead(\"");
	templateBuilder.Append(rooturl.ToString());
	templateBuilder.Append("tools/ajax.aspx?t=checkusername&username=\" + escape(username), \"showcheckresult(obj,'\" + escape(username) + \"');\");\r\n        }\r\n\r\n        function showcheckresult(obj, username) {\r\n            var res = obj.getElementsByTagName('result');\r\n            var result = \"\";\r\n            if (res[0] != null && res[0] != undefined) {\r\n                if (res[0].childNodes.length > 1) {\r\n                    result = res[0].childNodes[1].nodeValue;\r\n                } else {\r\n                    result = res[0].firstChild.nodeValue;\r\n                }\r\n            }\r\n            if (result == \"1\") {\r\n                var tips = \"您输入的用户名 \\\"\" + htmlEncode(unescape(username), true, 4) + \"\\\" 已经被他人使用或被系统禁用。\";\r\n                $('returnregmessage').innerHTML = tips;\r\n                $('returnregmessage').className = 'onerror';\r\n            }\r\n            else {\r\n                $('returnregmessage').className = '';\r\n                $('returnregmessage').innerHTML = '激活';\r\n            }\r\n        }\r\n        function submitCheck(regForm) {\r\n            return true;\r\n        }\r\n    </");
	templateBuilder.Append("script>\r\n");
	}
	else
	{

	templateBuilder.Append("\r\n");
	templateBuilder.Append(wysky_page_msg.ToString());
	templateBuilder.Append("\r\n");
	}	//end if

	templateBuilder.Append("\r\n</body>\r\n</html>\r\n");
	}	//end if


	Response.Write(templateBuilder.ToString());
}
</script>
