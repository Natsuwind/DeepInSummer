<%@ Page language="c#" AutoEventWireup="false" EnableViewState="false" Inherits="LiteCMS.Web.admincp" %>
<script runat="server">
override protected void OnInit(EventArgs e)
{
	/*
	This is a cached-file of template("\templates\templatename\admincp.htm"), it was created by LiteCMS.CN Template Engine.
	Please do NOT edit it.
	此文件为模板文件的缓存("\templates\模板名\admincp.htm"),由 LiteCMS.CN 模板引擎生成.
	所以请不要编辑此文件.
	*/
	base.OnInit(e);
	templateBuilder.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n");
	templateBuilder.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\" >\r\n");
	templateBuilder.Append("<head>\r\n");
	templateBuilder.Append("    <title>Administrator's Control Panel - Power by LiteCMS</title>\r\n");

	if (admininfo!=null)
	{

	templateBuilder.Append("    <style type=\"text/css\">\r\n");
	templateBuilder.Append("    * \r\n");
	templateBuilder.Append("    {\r\n");
	templateBuilder.Append("        margin:0pt;\r\n");
	templateBuilder.Append("        padding:0pt;\r\n");
	templateBuilder.Append("    }\r\n");
	templateBuilder.Append("    h1, h2, h3, h4, h5, h6, p, blockquote \r\n");
	templateBuilder.Append("    {\r\n");
	templateBuilder.Append("        margin:0pt;\r\n");
	templateBuilder.Append("        padding:10px;\r\n");
	templateBuilder.Append("    }\r\n");
	templateBuilder.Append("    body\r\n");
	templateBuilder.Append("        {\r\n");
	templateBuilder.Append("            font:12px Tahoma;\r\n");
	templateBuilder.Append("	        text-align:center;\r\n");
	templateBuilder.Append("	        background:#EEEEFF;\r\n");
	templateBuilder.Append("	        width:100%;\r\n");
	templateBuilder.Append("        }\r\n");
	templateBuilder.Append("    ul \r\n");
	templateBuilder.Append("        {\r\n");
	templateBuilder.Append("            list-style:none;\r\n");
	templateBuilder.Append("            /*float:left;*/\r\n");
	templateBuilder.Append("            width:100%;\r\n");
	templateBuilder.Append("            font-size:16px;\r\n");
	templateBuilder.Append("            margin-left: 0px;\r\n");
	templateBuilder.Append("            margin-top:10px;\r\n");
	templateBuilder.Append("        }\r\n");
	templateBuilder.Append("    ul li \r\n");
	templateBuilder.Append("        {\r\n");
	templateBuilder.Append("            float:left;\r\n");
	templateBuilder.Append("            width:100%;\r\n");
	templateBuilder.Append("            line-height:20px; \r\n");
	templateBuilder.Append("            text-align:center\r\n");
	templateBuilder.Append("        }\r\n");
	templateBuilder.Append("    ul li:hover\r\n");
	templateBuilder.Append("        {\r\n");
	templateBuilder.Append("            background-color:Silver; color:Red;\r\n");
	templateBuilder.Append("        }\r\n");
	templateBuilder.Append("    ul li a\r\n");
	templateBuilder.Append("        {\r\n");
	templateBuilder.Append("            display:block;width:100%;text-decoration:none;\r\n");
	templateBuilder.Append("        }\r\n");
	templateBuilder.Append("    ul li a:link,a:visited\r\n");
	templateBuilder.Append("        {\r\n");
	templateBuilder.Append("            color:#000;\r\n");
	templateBuilder.Append("        }\r\n");
	templateBuilder.Append("    ul li a:hover\r\n");
	templateBuilder.Append("        {\r\n");
	templateBuilder.Append("            color:Red;\r\n");
	templateBuilder.Append("        }\r\n");
	templateBuilder.Append("/*  ul li a:visited\r\n");
	templateBuilder.Append("        {\r\n");
	templateBuilder.Append("            color:#000;\r\n");
	templateBuilder.Append("        }*/\r\n");
	templateBuilder.Append("    </style>\r\n");

	}
	else
	{

	templateBuilder.Append("    <link rel=\"stylesheet\" href=\"templates/sample/admincp.css\" type=\"text/css\" media=\"all\" />\r\n");

	}	//end if

	templateBuilder.Append("</head>\r\n");
	templateBuilder.Append("<body>\r\n");
	templateBuilder.Append("<script language=\"javascript\" type=\"text/javascript\">\r\n");
	templateBuilder.Append("	if(self.parent.frames.length != 0) {\r\n");
	templateBuilder.Append("		self.parent.location=document.location;\r\n");
	templateBuilder.Append("	}\r\n");
	templateBuilder.Append("</"+ "script>\r\n");

	if (admininfo!=null)
	{

	templateBuilder.Append("    <div id=\"Banner\" style=\"height:70px; width:100%; background-color:Gray\">\r\n");
	templateBuilder.Append("        Banner\r\n");
	templateBuilder.Append("    </div>\r\n");
	templateBuilder.Append("    <div id=\"Main\" style=\"margin:10px auto 0 auto; border:solid 1px gray; text-align:left\">欢迎 ");
	templateBuilder.Append(admininfo.Name.ToString().Trim());
	templateBuilder.Append(" - [");
	templateBuilder.Append(username.ToString());
	templateBuilder.Append("] 登录后台管理,点击<a href=\"../\" target=\"_self\">回到前台</a>。\r\n");
	templateBuilder.Append("    </div>\r\n");
	templateBuilder.Append("    <div id=\"Menu\" style=\"background-color:#FFF;width:13%; float:left; border:solid 1px gray; margin:10px 5px\">\r\n");
	templateBuilder.Append("        <ul>\r\n");
	templateBuilder.Append("            <li><a href=\"frame.aspx?action=listarticle&id=0\" target=\"MainIFR\">后台首页</a></li>\r\n");
	templateBuilder.Append("            <li><a href=\"frame.aspx?action=postarticle\" target=\"MainIFR\">发布文章</a></li>\r\n");
	templateBuilder.Append("            <li><a href=\"frame.aspx?action=listarticle&id=0\" target=\"MainIFR\">文章管理</a></li>\r\n");
	templateBuilder.Append("            <li><a href=\"frame.aspx?action=mgrcolumn\" target=\"MainIFR\">栏目管理</a></li>\r\n");
	templateBuilder.Append("            <li><a href=\"#\" target=\"MainIFR\">留言管理</a></li>\r\n");
	templateBuilder.Append("            <li><a href=\"#\" target=\"MainIFR\">用户管理</a></li>\r\n");
	templateBuilder.Append("            <li><a href=\"#\" target=\"MainIFR\">管理设置</a></li>\r\n");
	templateBuilder.Append("            <li><a href=\"#\" target=\"MainIFR\">友情管理</a></li>\r\n");
	templateBuilder.Append("            <li><a href=\"frame.aspx?action=mainsetting\" target=\"MainIFR\">系统设置</a></li>\r\n");
	templateBuilder.Append("            <li><a href=\"frame.aspx?action=template\" target=\"MainIFR\">模板生成</a></li>\r\n");
	templateBuilder.Append("            <li><a href=\"admincp.aspx?action=logout\" target=\"_parent\">退出后台</a></li>\r\n");
	templateBuilder.Append("        </ul>\r\n");
	templateBuilder.Append("    </div>\r\n");
	templateBuilder.Append("    <iframe id=\"MainIFR\" name=\"MainIFR\" frameborder=\"0\" scrolling=\"yes\" src=\"");
	templateBuilder.Append(url.ToString());
	templateBuilder.Append("\" width=\"85%\" height=\"490px\" style=\"margin:10px auto auto auto; border:solid 1px gray;\">\r\n");
	templateBuilder.Append("    </iframe>\r\n");

	}
	else
	{

	templateBuilder.Append("<table class=\"logintb\">\r\n");
	templateBuilder.Append("    <tbody>\r\n");
	templateBuilder.Append("        <tr>\r\n");
	templateBuilder.Append("            <td class=\"login\">\r\n");
	templateBuilder.Append("                <h1>LiteCMS Administrator's Control Panel</h1>\r\n");
	templateBuilder.Append("                <p>LiteCMS 是一个采用 C# 和 SQLite 等多种数据库构建的高效内容管理系统解决方案.</p>\r\n");
	templateBuilder.Append("            </td>\r\n");
	templateBuilder.Append("            <td>\r\n");
	templateBuilder.Append("            <form method=\"post\" name=\"login\" id=\"loginform\" action=\"\">\r\n");
	templateBuilder.Append("                <input type=\"hidden\" name=\"sid\" value=\"vxbppo\">\r\n");
	templateBuilder.Append("                <input type=\"hidden\" name=\"frames\" value=\"yes\">\r\n");
	templateBuilder.Append("                <p class=\"logintitle\">用户名: </p>\r\n");
	templateBuilder.Append("                <p class=\"loginform\"><input name=\"loginname\" tabindex=\"1\" type=\"text\" class=\"txt\"></p>\r\n");
	templateBuilder.Append("                <p class=\"logintitle\">密　码:</p>\r\n");
	templateBuilder.Append("                <p class=\"loginform\"><input name=\"password\" tabindex=\"2\" type=\"password\" class=\"txt\"></p>\r\n");
	templateBuilder.Append("                <p class=\"logintitle\">目　录:</p>\r\n");
	templateBuilder.Append("                <p class=\"loginform\"><input name=\"path\" tabindex=\"3\" type=\"text\" class=\"txt\"></p>\r\n");
	templateBuilder.Append("                <p class=\"loginnofloat\"><input name=\"submit\" value=\"提交\" tabindex=\"3\" type=\"submit\" class=\"btn\"></p>\r\n");
	templateBuilder.Append("                </form>\r\n");
	templateBuilder.Append("                <script type=\"text/javascript\">document.getelementbyid('loginform').loginname.focus();</"+ "script>\r\n");
	templateBuilder.Append("            </td>            \r\n");
	templateBuilder.Append("        </tr>\r\n");
	templateBuilder.Append("        <tr>\r\n");
	templateBuilder.Append("            <td colspan=\"2\" class=\"footer\">\r\n");
	templateBuilder.Append("                <div class=\"copyright\">\r\n");
	templateBuilder.Append("                    <p>Powered by <a href=\"http://www.litecms.cn/\" target=\"_blank\">LiteCMS</a> 0.1.1314.1 </p>\r\n");
	templateBuilder.Append("                    <p>&copy; 2008, <a href=\"http://www.52dnt.cn/\" target=\"_blank\">");
	templateBuilder.Append(config.Websitename.ToString().Trim());
	templateBuilder.Append("</a> inc.</p>\r\n");
	templateBuilder.Append("                </div>\r\n");
	templateBuilder.Append("            </td>\r\n");
	templateBuilder.Append("        </tr>\r\n");
	templateBuilder.Append("    </tbody>\r\n");
	templateBuilder.Append("</table>\r\n");

	}	//end if

	templateBuilder.Append("</body>\r\n");
	templateBuilder.Append("</html>\r\n");

	Response.Write(templateBuilder.ToString());
}
</script>