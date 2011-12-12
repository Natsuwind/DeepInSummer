<%@ Page language="c#" AutoEventWireup="false" EnableViewState="false" Inherits="LiteCMS.Web.postarticle" %>
<%@ Import namespace="LiteCMS.Data" %>
<%@ Import namespace="LiteCMS.Entity" %>
<script runat="server">
override protected void OnInit(EventArgs e)
{
	/*
	This is a cached-file of template("\templates\templatename\postarticle.htm"), it was created by LiteCMS.CN Template Engine.
	Please do NOT edit it.
	此文件为模板文件的缓存("\templates\模板名\postarticle.htm"),由 LiteCMS.CN 模板引擎生成.
	所以请不要编辑此文件.
	*/
	base.OnInit(e);

	templateBuilder.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n");
	templateBuilder.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n");
	templateBuilder.Append("<head>\r\n");
	templateBuilder.Append("	<link rel=\"stylesheet\" type=\"text/css\" href=\"../templates/");
	templateBuilder.Append(config.Templatefolder.ToString().Trim());
	templateBuilder.Append("/main.css\" />\r\n");
	templateBuilder.Append("	<meta content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\" />\r\n");

	if (pagetitle=="")
	{

	templateBuilder.Append("<title>");
	templateBuilder.Append(config.Websitename.ToString().Trim());
	templateBuilder.Append(" - ");
	templateBuilder.Append(config.Seotitle.ToString().Trim());
	templateBuilder.Append(" - Powered by LiteCMS</title>\r\n");

	}
	else
	{

	templateBuilder.Append("<title>");
	templateBuilder.Append(pagetitle.ToString());
	templateBuilder.Append(" - ");
	templateBuilder.Append(config.Websitename.ToString().Trim());
	templateBuilder.Append(" - ");
	templateBuilder.Append(config.Seotitle.ToString().Trim());
	templateBuilder.Append(" - Powered by LiteCMS</title>\r\n");

	}	//end if

	templateBuilder.Append("</head>\r\n");
	templateBuilder.Append("<body>\r\n");
	templateBuilder.Append("<div id=\"wrap\">\r\n");
	templateBuilder.Append("	<!--头部开始-->\r\n");
	templateBuilder.Append("	<div id=\"header\">\r\n");
	templateBuilder.Append("		<div id=\"main-menu\">\r\n");
	templateBuilder.Append("			<ul>\r\n");
	templateBuilder.Append("				<li><a href=\"index.aspx\">首页</a></li>\r\n");
	templateBuilder.Append("				<li><a href=\"/cms/index.aspx\">文章</a></li>\r\n");
	templateBuilder.Append("				<li><a href=\"/cms/showcolumn.aspx\">新闻</a></li>\r\n");
	templateBuilder.Append("				<li><a href=\"/cms/postarticle.aspx\">投递</a></li>\r\n");
	templateBuilder.Append("                <li><a href=\"/space/index.aspx\">博客</a></li>\r\n");
	templateBuilder.Append("				<!--<li><a href=\"bbs/\">论坛</a></li>-->\r\n");

	if (userid>0)
	{

	templateBuilder.Append("				<li><a href=\"/member/usercontrolpanel.aspx\">用户中心[");
	templateBuilder.Append(username.ToString());
	templateBuilder.Append("]</a></li>\r\n");

	if (adminid>0)
	{

	templateBuilder.Append("				<li><a href=\"/admincp.aspx\" target=\"_blank\">系统设置</a></li>\r\n");

	}	//end if

	templateBuilder.Append("				<li><a href=\"/member/loginout.aspx\">注销</a></li>\r\n");

	}
	else
	{

	templateBuilder.Append("				<li><a href=\"/member/login.aspx\">登录</a></li>\r\n");
	templateBuilder.Append("				<li><a href=\"/member/register.aspx\">注册</a></li>			\r\n");

	}	//end if

	templateBuilder.Append("			</ul>\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("	<!--头部结束-->\r\n");


	templateBuilder.Append("	<!--内容开始-->\r\n");

	templateBuilder.Append("	<!--右栏开始-->\r\n");
	templateBuilder.Append("	<div id=\"right-side\">\r\n");
	templateBuilder.Append("		<div class=\"div-header\">站内搜索</div>\r\n");
	templateBuilder.Append("		<div id=\"search\"><form action=\"search.aspx\" method=\"get\">标题搜索&nbsp;:&nbsp;<input type=\"text\" id=\"searchkey\" name=\"searchkey\" />&nbsp;&nbsp;<input type=\"submit\" value=\"搜索\" /></form></div>	\r\n");
	templateBuilder.Append("		<div id=\"hot-article\" class=\"right-list\">\r\n");

	if (config.Urlrewrite==1)
	{

	templateBuilder.Append("			<div class=\"div-header\"><a href=\"showcolumn-hot");
	templateBuilder.Append(config.Urlrewriteextname.ToString().Trim());
	templateBuilder.Append("\">热门新闻</a></div>\r\n");

	}
	else
	{

	templateBuilder.Append("			<div class=\"div-header\"><a href=\"showcolumn.aspx?type=hot\">热门新闻</a></div>\r\n");

	}	//end if

	templateBuilder.Append("			<ul>\r\n");

	int hotarticleinfo__loop__id=0;
	foreach(ArticleInfo hotarticleinfo in hotarticlelist)
	{
		hotarticleinfo__loop__id++;


	if (config.Urlrewrite==1)
	{

	templateBuilder.Append("				<li><h2><a href=\"showarticle-");
	templateBuilder.Append(hotarticleinfo.Articleid.ToString().Trim());
	templateBuilder.Append(config.Urlrewriteextname.ToString().Trim());
	templateBuilder.Append("\">");
	templateBuilder.Append(hotarticleinfo.Title.ToString().Trim());
	templateBuilder.Append("</a></h2></li>\r\n");

	}
	else
	{

	templateBuilder.Append("				<li><h2><a href=\"showarticle.aspx?id=");
	templateBuilder.Append(hotarticleinfo.Articleid.ToString().Trim());
	templateBuilder.Append("\">");
	templateBuilder.Append(hotarticleinfo.Title.ToString().Trim());
	templateBuilder.Append("</a></h2></li>\r\n");

	}	//end if


	}	//end loop

	templateBuilder.Append("			</ul>\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("		<div id=\"latest-comment\" class=\"right-list\">\r\n");
	templateBuilder.Append("			<div class=\"div-header\">最新评论</div>\r\n");
	templateBuilder.Append("			<ul>\r\n");

	int latestcommentinfo__loop__id=0;
	foreach(CommentInfo latestcommentinfo in latestcommentlist)
	{
		latestcommentinfo__loop__id++;

	templateBuilder.Append("				<li>\r\n");
	templateBuilder.Append("					<span class=\"content\">");
	templateBuilder.Append(latestcommentinfo.Content.ToString().Trim());
	templateBuilder.Append("</span>\r\n");
	templateBuilder.Append("					<div class=\"comment-info\"><span class=\"comment-author\">");
	templateBuilder.Append(latestcommentinfo.Username.ToString().Trim());
	templateBuilder.Append(" </span>对 <span class=\"from-article\">\"<a href=\"\r\n");

	if (config.Urlrewrite==1)
	{

	templateBuilder.Append("showarticle-");
	templateBuilder.Append(latestcommentinfo.Articleid.ToString().Trim());
	templateBuilder.Append(config.Urlrewriteextname.ToString().Trim());
	templateBuilder.Append("\r\n");

	}
	else
	{

	templateBuilder.Append("showarticle.aspx?id=");
	templateBuilder.Append(latestcommentinfo.Articleid.ToString().Trim());
	templateBuilder.Append("\r\n");

	}	//end if

	templateBuilder.Append("\">");
	templateBuilder.Append(latestcommentinfo.Articletitle.ToString().Trim());
	templateBuilder.Append("</a>\"</span> 的评论</div>\r\n");
	templateBuilder.Append("				</li>\r\n");

	}	//end loop

	templateBuilder.Append("			</ul>\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("		<div id=\"hot-comment\" class=\"right-list\">\r\n");
	templateBuilder.Append("			<div class=\"div-header\">热门评论</div>\r\n");
	templateBuilder.Append("			<ul>\r\n");

	int mostgradecommentinfo__loop__id=0;
	foreach(CommentInfo mostgradecommentinfo in mostgradecommentlist)
	{
		mostgradecommentinfo__loop__id++;

	templateBuilder.Append("				<li>\r\n");
	templateBuilder.Append("					<span class=\"content\">");
	templateBuilder.Append(mostgradecommentinfo.Content.ToString().Trim());
	templateBuilder.Append("</span>\r\n");
	templateBuilder.Append("					<div class=\"comment-info\"><span class=\"comment-author\">");
	templateBuilder.Append(mostgradecommentinfo.Username.ToString().Trim());
	templateBuilder.Append(" </span>对 <span class=\"from-article\"><a href=\"\r\n");

	if (config.Urlrewrite==1)
	{

	templateBuilder.Append("showarticle-");
	templateBuilder.Append(mostgradecommentinfo.Articleid.ToString().Trim());
	templateBuilder.Append(config.Urlrewriteextname.ToString().Trim());
	templateBuilder.Append("\r\n");

	}
	else
	{

	templateBuilder.Append("showarticle.aspx?id=");
	templateBuilder.Append(mostgradecommentinfo.Articleid.ToString().Trim());
	templateBuilder.Append("\r\n");

	}	//end if

	templateBuilder.Append("\">");
	templateBuilder.Append(mostgradecommentinfo.Articletitle.ToString().Trim());
	templateBuilder.Append("</a></span> 的评论</div>\r\n");
	templateBuilder.Append("				</li>\r\n");

	}	//end loop

	templateBuilder.Append("			</ul>\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("	<!--右栏结束-->	\r\n");


	templateBuilder.Append("	<!--左栏开始-->\r\n");
	templateBuilder.Append("	<div id=\"left-side\">\r\n");
	templateBuilder.Append("		<!--文章开始-->\r\n");
	templateBuilder.Append("		<div class=\"div-header\">投递文章</div>\r\n");
	templateBuilder.Append("		<!-- tinyMCE -->\r\n");
	templateBuilder.Append("        <script language=\"javascript\" type=\"text/javascript\" src=\"../editor/tiny_mce.js\"></"+ "script>\r\n");
	templateBuilder.Append("        <script language=\"javascript\" type=\"text/javascript\">\r\n");
	templateBuilder.Append("            // Notice: The simple theme does not use all options some of them are limited to the advanced theme\r\n");
	templateBuilder.Append("            tinyMCE.init({\r\n");
	templateBuilder.Append("            mode : \"textareas\",theme : \"advanced\",language : \"zh\",\r\n");
	templateBuilder.Append("            theme_advanced_toolbar_location : \"top\",theme_advanced_toolbar_align : \"left\"\r\n");
	templateBuilder.Append("            });\r\n");
	templateBuilder.Append("         </"+ "script>\r\n");
	templateBuilder.Append("        <!-- /tinyMCE -->\r\n");
	templateBuilder.Append("        <div>\r\n");
	templateBuilder.Append("        <form method=\"post\" action=\"\">\r\n");
	templateBuilder.Append("        <table style=\"width:100%;\">\r\n");
	templateBuilder.Append("                <tr>\r\n");
	templateBuilder.Append("                    <td style=\"width: 96px\">\r\n");
	templateBuilder.Append("                        <select name=\"columnid\" id=\"columns\" style=\"width:96px;\">\r\n");

	int columninfo__loop__id=0;
	foreach(ColumnInfo columninfo in columnlist)
	{
		columninfo__loop__id++;

	templateBuilder.Append("	                        <option value=\"");
	templateBuilder.Append(columninfo.Columnid.ToString().Trim());
	templateBuilder.Append("\">");
	templateBuilder.Append(columninfo.Columnname.ToString().Trim());
	templateBuilder.Append("</option>\r\n");

	}	//end loop

	templateBuilder.Append("                        </select>\r\n");
	templateBuilder.Append("                    </td>\r\n");
	templateBuilder.Append("                    <td>\r\n");
	templateBuilder.Append("                        <input name=\"title\" type=\"text\" style=\"width:100%;\" />\r\n");
	templateBuilder.Append("                    </td>\r\n");
	templateBuilder.Append("                </tr>\r\n");
	templateBuilder.Append("                <tr>\r\n");
	templateBuilder.Append("                    <td colspan=\"2\">\r\n");
	templateBuilder.Append("                        <textarea name=\"summary\" rows=\"2\" cols=\"20\" id=\"summary\" style=\"height:70px;width:100%;\"></textarea>\r\n");
	templateBuilder.Append("                    </td>\r\n");
	templateBuilder.Append("                </tr>\r\n");
	templateBuilder.Append("                <tr>\r\n");
	templateBuilder.Append("                    <td colspan=\"2\">\r\n");
	templateBuilder.Append("                        <textarea name=\"content\" rows=\"2\" cols=\"20\" id=\"content\" style=\"height:366px;width:100%;\"></textarea>\r\n");
	templateBuilder.Append("                    </td>\r\n");
	templateBuilder.Append("                </tr>\r\n");
	templateBuilder.Append("                <tr>\r\n");
	templateBuilder.Append("                    <td>上传文件:</td>\r\n");
	templateBuilder.Append("                    <td>                       \r\n");
	templateBuilder.Append("                        <input type=\"file\" name=\"upfile\" id=\"upfile\" />\r\n");
	templateBuilder.Append("                        <button class=\"button\" id=\"buttonUpload\" onclick=\"return ajaxFileUpload();\">上传文件</button>                          \r\n");
	templateBuilder.Append("                        <script src=\"javascript/jquery-1.2.6.js\" type=\"text/javascript\" language=\"javascript\"></"+ "script>\r\n");
	templateBuilder.Append("                        <script src=\"javascript/ajaxfileupload.js\" type=\"text/javascript\"></"+ "script>\r\n");
	templateBuilder.Append("                        <script type=\"text/javascript\">\r\n");
	templateBuilder.Append("                            function ajaxFileUpload()\r\n");
	templateBuilder.Append("                            {\r\n");
	templateBuilder.Append("                                //starting setting some animation when the ajax starts and completes\r\n");
	templateBuilder.Append("                                $(\"#loading\")\r\n");
	templateBuilder.Append("                                .ajaxStart(function(){\r\n");
	templateBuilder.Append("                                    $(this).show();\r\n");
	templateBuilder.Append("                                })\r\n");
	templateBuilder.Append("                                .ajaxComplete(function(){\r\n");
	templateBuilder.Append("                                    $(this).hide();\r\n");
	templateBuilder.Append("                                });\r\n");
	templateBuilder.Append("                                /*\r\n");
	templateBuilder.Append("                                    prepareing ajax file upload\r\n");
	templateBuilder.Append("                                    url: the url of script file handling the uploaded files\r\n");
	templateBuilder.Append("                                                fileElementId: the file type of input element id and it will be the index of  $_FILES Array()\r\n");
	templateBuilder.Append("                                    dataType: it support json, xml\r\n");
	templateBuilder.Append("                                    secureuri:use secure protocol\r\n");
	templateBuilder.Append("                                    success: call back function when the ajax complete\r\n");
	templateBuilder.Append("                                    error: callback function when the ajax failed\r\n");
	templateBuilder.Append("                                        */\r\n");
	templateBuilder.Append("                                $.ajaxFileUpload\r\n");
	templateBuilder.Append("                                (\r\n");
	templateBuilder.Append("                                    {\r\n");
	templateBuilder.Append("                                        url:'uploadfile.aspx',\r\n");
	templateBuilder.Append("                                        secureuri:false,\r\n");
	templateBuilder.Append("                                        fileElementId:'upfile',\r\n");
	templateBuilder.Append("                                        dataType: 'json',\r\n");
	templateBuilder.Append("                                        success: function (data, status)\r\n");
	templateBuilder.Append("                                        {\r\n");
	templateBuilder.Append("                                            if(typeof(data.Filepath) != 'undefined')\r\n");
	templateBuilder.Append("                                            {\r\n");
	templateBuilder.Append("                                                alert('上传成功!');                                                \r\n");
	templateBuilder.Append("                                                tinyMCE.getInstanceById('content').getBody().innerHTML += '<img src=\"' + data.Filepath + '\" alt=\"\" />';\r\n");
	templateBuilder.Append("                                            }\r\n");
	templateBuilder.Append("//                                            if(typeof(data.error) != 'undefined')\r\n");
	templateBuilder.Append("//                                            {\r\n");
	templateBuilder.Append("//                                                if(data.error != '')\r\n");
	templateBuilder.Append("//                                                {\r\n");
	templateBuilder.Append("//                                                    alert(data.error);\r\n");
	templateBuilder.Append("//                                                }else\r\n");
	templateBuilder.Append("//                                                {\r\n");
	templateBuilder.Append("//                                                    alert(data.msg);\r\n");
	templateBuilder.Append("//                                                }\r\n");
	templateBuilder.Append("//                                            }\r\n");
	templateBuilder.Append("                                        },\r\n");
	templateBuilder.Append("                                        error: function (data, status, e)\r\n");
	templateBuilder.Append("                                        {\r\n");
	templateBuilder.Append("                                            alert('上传失败,请检查扩展名是否被允许.');\r\n");
	templateBuilder.Append("                                        }\r\n");
	templateBuilder.Append("                                    }\r\n");
	templateBuilder.Append("                                )\r\n");
	templateBuilder.Append("                                return false;\r\n");
	templateBuilder.Append("                            }  \r\n");
	templateBuilder.Append("                        </"+ "script>\r\n");
	templateBuilder.Append("                    </td>\r\n");
	templateBuilder.Append("                </tr>\r\n");
	templateBuilder.Append("                <tr>                    \r\n");
	templateBuilder.Append("                    <td align=\"center\" colspan=\"2\">\r\n");
	templateBuilder.Append("                        <input type=\"submit\" value=\"提交\" />&nbsp;&nbsp;&nbsp;<input type=\"submit\" name=\"btnCancel\" value=\"取消\" id=\"btnCancel\" />\r\n");
	templateBuilder.Append("                    </td>\r\n");
	templateBuilder.Append("                </tr>\r\n");
	templateBuilder.Append("            </table>\r\n");
	templateBuilder.Append("        </form>\r\n");
	templateBuilder.Append("        </div>\r\n");
	templateBuilder.Append("		<!--文章结束-->\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("	<!--左栏结束-->\r\n");
	templateBuilder.Append("	<!--内容结束-->\r\n");

	templateBuilder.Append("	<div id=\"footer\">\r\n");
	templateBuilder.Append("	    <div>\r\n");
	templateBuilder.Append("		    <ul>\r\n");
	templateBuilder.Append("			    <li><a href=\"#\">关于本站</a></li>\r\n");
	templateBuilder.Append("			    <li><a href=\"#\">联系我们</a></li>		\r\n");
	templateBuilder.Append("			    <li><a href=\"#\">广告服务</a></li>	\r\n");
	templateBuilder.Append("			    <li>版权所有 © 2004-2008 <a href=\"#\">");
	templateBuilder.Append(config.Websitename.ToString().Trim());
	templateBuilder.Append("</a></li>\r\n");
	templateBuilder.Append("			    <li title=\"执行时间:");
	templateBuilder.Append(processtime.ToString());
	templateBuilder.Append(",查询数:");
	templateBuilder.Append(querycount.ToString());
	templateBuilder.Append("\">Powered by <a href=\"http://www.litecms.cn/\">LiteCMS</a> 0.1.1314.1</li>\r\n");
	templateBuilder.Append("		    </ul>\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("		<br />\r\n");
	templateBuilder.Append("		<div id=\"FriendLink\" style=\"float:right; \">\r\n");
	templateBuilder.Append("		    <ul>\r\n");
	templateBuilder.Append("		        <li>友情连接:<a href=\"http://phprimer.com/\" title=\"PHP/Ruby/交互设计/Ajax/RIA/CSS 技术宝典\" target=\"_blank\">PHPrimer</a> | <a href=\"#\">更多</a>.</li>\r\n");
	templateBuilder.Append("		        <li>合作伙伴：<a target=\"_blank\" href=\"http://www.microsoft.com/\">微软</a> | <a target=\"_blank\" href=\"http://www.infoq.com/cn/\">InfoQ中文站</a>.</li>\r\n");
	templateBuilder.Append("		        <li>合作出版社：<a target=\"_blank\" href=\"http://www.broadview.com.cn/\">博文视点</a> | <a target=\"_blank\" href=\"http://www.turingbook.com/\">图灵</a>.</li>\r\n");
	templateBuilder.Append("		    </ul>\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("		");
	templateBuilder.Append(config.Analyticscode.ToString().Trim());
	templateBuilder.Append("\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("</div>\r\n");
	templateBuilder.Append("</body>\r\n");
	templateBuilder.Append("</html>\r\n");



	Response.Write(templateBuilder.ToString());
}
</script>