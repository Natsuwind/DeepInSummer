<%@ Page language="c#" AutoEventWireup="false" EnableViewState="false" Inherits="LiteCMS.Web.showmessage" %>
<%@ Import namespace="LiteCMS.Data" %>
<%@ Import namespace="LiteCMS.Entity" %>
<script runat="server">
override protected void OnInit(EventArgs e)
{
	/*
	This is a cached-file of template("\templates\templatename\showmessage.htm"), it was created by LiteCMS.CN Template Engine.
	Please do NOT edit it.
	此文件为模板文件的缓存("\templates\模板名\showmessage.htm"),由 LiteCMS.CN 模板引擎生成.
	所以请不要编辑此文件.
	*/
	base.OnInit(e);

	templateBuilder.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n");
	templateBuilder.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n");
	templateBuilder.Append("<head>\r\n");
	templateBuilder.Append("	<link rel=\"stylesheet\" type=\"text/css\" href=\"templates/");
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
	templateBuilder.Append("				<li><a href=\"showcolumn.aspx\">新闻</a></li>\r\n");
	templateBuilder.Append("				<li><a href=\"postarticle.aspx\">投递</a></li>\r\n");
	templateBuilder.Append("            <!--<li><a href=\"#\">博客</a></li>\r\n");
	templateBuilder.Append("				<li><a href=\"bbs/\">论坛</a></li>-->\r\n");

	if (userid>0)
	{

	templateBuilder.Append("				<li><a href=\"usercontrolpanel.aspx\">用户中心[");
	templateBuilder.Append(username.ToString());
	templateBuilder.Append("]</a></li>\r\n");

	if (adminid>0)
	{

	templateBuilder.Append("				<li><a href=\"admincp.aspx\" target=\"_blank\">系统设置</a></li>\r\n");

	}	//end if

	templateBuilder.Append("				<li><a href=\"loginout.aspx\">注销</a></li>\r\n");

	}
	else
	{

	templateBuilder.Append("				<li><a href=\"login.aspx\">登录</a></li>\r\n");
	templateBuilder.Append("				<li><a href=\"register.aspx\">注册</a></li>			\r\n");

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
	templateBuilder.Append("		<div class=\"div-header\">系统提示</div>\r\n");
	templateBuilder.Append("		<div style=\"border-style: dotted; border-width: 1px; margin: 100px auto 200px; width: 60%;\">\r\n");
	templateBuilder.Append("		    <div class=\"div-header\" style=\"height:20px;\">");
	templateBuilder.Append(messageheader.ToString());
	templateBuilder.Append("</div>\r\n");
	templateBuilder.Append("		    <div style=\"padding: 10px;\">");
	templateBuilder.Append(messagebody.ToString());
	templateBuilder.Append("</div>\r\n");
	templateBuilder.Append("		    <div style=\"padding-left: 10px;\">");
	templateBuilder.Append(messagefooter.ToString());
	templateBuilder.Append("</div>		\r\n");
	templateBuilder.Append("		    <div style=\"padding: 10px;\">\r\n");

	if (isautoredirect)
	{

	templateBuilder.Append("<span id=\"autoredirect\"></span>\r\n");

	}	//end if

	templateBuilder.Append("<a href=\"");
	templateBuilder.Append(redirecturl.ToString());
	templateBuilder.Append("\">如果浏览器没有自动转向, 请点击这里.</a></div>\r\n");
	templateBuilder.Append("		</div>\r\n");
	templateBuilder.Append("	</div>\r\n");
	templateBuilder.Append("	<!--左栏结束-->\r\n");
	templateBuilder.Append("	<!--内容结束-->\r\n");

	if (isautoredirect)
	{

	templateBuilder.Append("    <script language='javascript' type='text/javascript'>\r\n");
	templateBuilder.Append("    var secs = 3; //倒计时的秒数\r\n");
	templateBuilder.Append("    var URL = '");
	templateBuilder.Append(redirecturl.ToString());
	templateBuilder.Append("';\r\n");
	templateBuilder.Append("    for(var i=secs;i>=0;i--)\r\n");
	templateBuilder.Append("    {\r\n");
	templateBuilder.Append("     window.setTimeout('doUpdate(' + i + ')', (secs-i) * 1000);\r\n");
	templateBuilder.Append("    }\r\n");
	templateBuilder.Append("    function doUpdate(num)\r\n");
	templateBuilder.Append("    {\r\n");
	templateBuilder.Append("     document.getElementById('autoredirect').innerHTML = '浏览器将在'+num+'秒后自动转向.' ;\r\n");
	templateBuilder.Append("     if(num == 0) { window.location=URL;  }\r\n");
	templateBuilder.Append("    }\r\n");
	templateBuilder.Append("    </"+ "script> \r\n");

	}	//end if


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