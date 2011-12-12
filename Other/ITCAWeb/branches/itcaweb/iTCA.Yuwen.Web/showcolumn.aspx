<%@ Page language="c#" AutoEventWireup="false" EnableViewState="false" Inherits="iTCA.Yuwen.Web.showcolumn" %>
<%@ Import namespace="iTCA.Yuwen.Data" %>
<%@ Import namespace="iTCA.Yuwen.Entity" %>
<script runat="server">
override protected void OnInit(EventArgs e)
{
	base.OnInit(e);

	Response.Write("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n");
	Response.Write("<html xmlns=\"http://www.w3.org/1999/xhtml\" >\r\n");
	Response.Write("<head runat=\"server\">\r\n");
	Response.Write("    <title>" + pagetitle.ToString() + "</title>\r\n");
	Response.Write("    <link href=\"templates/css/itca.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n");
	Response.Write("</head>\r\n");
	Response.Write("<body>\r\n");
	Response.Write("    <form id=\"form1\" runat=\"server\">\r\n");
	Response.Write("    <div id=\"container\">\r\n");
	Response.Write("     <div id = \"header\">\r\n");
	Response.Write("	     <div id=\"menu\">\r\n");
	Response.Write("		 <ul>\r\n");
	Response.Write("			<li><a id=\"index\" href=\"index.aspx\">首 页</a></li>\r\n");
	Response.Write("			<li class=\"menudiv\"></li>\r\n");
	Response.Write("			<li><a id=\"ann\" href=\"showcolumn-1-1.aspx\">公 告</a></li>\r\n");
	Response.Write("			<li class=\"menudiv\"></li>\r\n");
	Response.Write("			<li><a id=\"news\" href=\"news.aspx\">新 闻</a></li>\r\n");
	Response.Write("			<li class=\"menudiv\"></li>\r\n");
	Response.Write("			<li><a id=\"act\" href=\"showcolumn-5-1.aspx\">活 动</a></li>\r\n");
	Response.Write("			<li class=\"menudiv\"></li>\r\n");
	Response.Write("			<li><a id=\"book\" href=\"book/Index.aspx?cid=1\">留 言 板</a></li>\r\n");
	Response.Write("			<li class=\"menudiv\"></li>\r\n");
	Response.Write("			<li><a id=\"other\" href=\"other/Index.aspx\">其他相关</a></li>\r\n");
	Response.Write("		 </ul>\r\n");
	Response.Write("		 </div>         \r\n");
	Response.Write("     </div>\r\n");
	Response.Write("     <div id=\"Div1\" style=\"clear:both; background:url(images/index/banner1.gif); height:29px\"></div>\r\n");
	Response.Write("       <div>\r\n");


	Response.Write("<div id=\"pagebody\" style=\"clear: both\">\r\n");
	Response.Write("    <!--main-->\r\n");
	Response.Write("    <div id=\"maincenter\" style=\"clear: both\">\r\n");
	Response.Write("        <center>\r\n");
	Response.Write("            <div id=\"articlelist\">\r\n");
	Response.Write("                <br />\r\n");
	Response.Write("                <table>\r\n");
	Response.Write("                    <tr>\r\n");
	Response.Write("                        <td background=\"images/cms/side07.jpg\" width=\"25px\">\r\n");
	Response.Write("                        </td>\r\n");
	Response.Write("                        <td>\r\n");
	Response.Write("                            <table width=\"500px\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">\r\n");
	Response.Write("                                <tr>\r\n");
	Response.Write("                                    <td id=\"title\" align=\"left\">\r\n");
	Response.Write("                                        <img src=\"images/cms/cms01.jpg\" height=\"22\" align=\"right\" /></a><!--width=\"337\"-->\r\n");
	Response.Write("                                    </td>\r\n");
	Response.Write("                                    <td class=\"time\" align=\"left\">\r\n");
	Response.Write("                                        <img src=\"images/cms/cms02.jpg\" width=\"150\" height=\"22\" align=\"left\" />\r\n");
	Response.Write("                                    </td>\r\n");
	Response.Write("                                </tr>\r\n");

	int newinfo__loop__id=0;
	foreach(ArticleInfo newinfo in newslist)
	{
		newinfo__loop__id++;

	Response.Write("                                <tr>\r\n");
	Response.Write("                                    <td id=\"Td1\" align=\"left\">\r\n");
	Response.Write("                                        &nbsp;[" + newinfo.Columnname.ToString().Trim() + "]&nbsp;<a href=\"showarticle-" + newinfo.Articleid.ToString().Trim() + ".aspx\">" + newinfo.Title.ToString().Trim() + "</a>\r\n");
	Response.Write("                                    </td>\r\n");
	Response.Write("                                    <td class=\"time\" align=\"center\">\r\n");
	Response.Write("                                        " + newinfo.Postdate.ToString().Trim() + "\r\n");
	Response.Write("                                    </td>\r\n");
	Response.Write("                                </tr>\r\n");

	}	//end loop

	Response.Write("                            </table>\r\n");
	Response.Write("                        </td>\r\n");
	Response.Write("                </table>\r\n");
	Response.Write("            </div>\r\n");
	Response.Write("            <img src=\"images/cms/cms04.jpg\" height=\"3\" />\r\n");
	Response.Write("            <div id=\"pager\">页码:" + pagecounthtml.ToString() + "</div>\r\n");
	Response.Write("        </center>\r\n");
	Response.Write("    </div>\r\n");
	Response.Write("</div>\r\n");

	Response.Write("</div>\r\n");
	Response.Write("      <div id = \"footer\" title=\"执行时间:" + processtime.ToString() + ":查询数:" + querycount.ToString() + "\">&copy;iTCA 重庆工学院计算机协会</div>\r\n");
	Response.Write("    </div>\r\n");
	Response.Write("    </form>\r\n");
	Response.Write("</body>\r\n");
	Response.Write("</html>\r\n");


	Response.Write("<!--" + querycount.ToString() + "<br />\r\n");
	Response.Write("" + querydetail.ToString() + "<br />\r\n");
	Response.Write("" + processtime.ToString() + "秒<br />-->\r\n");

}
</script>