<%@ Page language="c#" AutoEventWireup="false" EnableViewState="false" Inherits="iTCA.Yuwen.Web.index" %>
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


	Response.Write("<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">\r\n");
	Response.Write("    <tr height=\"113\">\r\n");
	Response.Write("        <td background=\"images/index/default01.jpg\" style=\"height: 113px\">\r\n");
	Response.Write("        </td>\r\n");
	Response.Write("        <td background=\"images/index/default02.jpg\" style=\"height: 113px\">\r\n");
	Response.Write("        </td>\r\n");
	Response.Write("    </tr>\r\n");
	Response.Write("    <tr>\r\n");
	Response.Write("        <td width=\"71%\">\r\n");
	Response.Write("            <table width=\"568\" height=\"261\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">\r\n");
	Response.Write("                <tr>\r\n");
	Response.Write("                    <td style=\"height: 40px; width: 116px; background: url(images/index/def_left01.jpg) no-repeat;\r\n");
	Response.Write("                        background-color: #F7F7F7\">\r\n");
	Response.Write("                    </td>\r\n");
	Response.Write("                    <td style=\"height: 40px; width: 452px; background: url(images/index/def_mid01.jpg) no-repeat;\">\r\n");
	Response.Write("                    </td>\r\n");
	Response.Write("                </tr>\r\n");
	Response.Write("                <tr>\r\n");
	Response.Write("                    <td id=\"as\" width=\"116\" height=\"75\" bgcolor=\"f7f7f7\" background=\"images/index/def_left03.jpg\">\r\n");
	Response.Write("                        <a href=\"other/\" title=\"协会简介\" target=\"_blank\">重庆工学院计算机<br />\r\n");
	Response.Write("                            协会成立于 1998<br />\r\n");
	Response.Write("                            年，经过数次变革<br />\r\n");
	Response.Write("                            之后形成了现在的<br />\r\n");
	Response.Write("                            iTCA……&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<br />\r\n");
	Response.Write("                        </a>\r\n");
	Response.Write("                    </td>\r\n");
	Response.Write("                    <td background=\"images/index/def_mid02.jpg\" width=\"452\" height=\"75\" align=\"left\">\r\n");
	Response.Write("                        <table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">\r\n");

	int anninfo__loop__id=0;
	foreach(ArticleInfo anninfo in annlist)
	{
		anninfo__loop__id++;

	Response.Write("                            <tr>\r\n");
	Response.Write("                                <td>\r\n");
	Response.Write("                                    <div class=\"i_title\">\r\n");
	Response.Write("                                        [" + anninfo.Columnname.ToString().Trim() + "]&nbsp;<a href=\"showarticle-" + anninfo.Articleid.ToString().Trim() + ".aspx\">" + anninfo.Title.ToString().Trim() + "</a></div>\r\n");
	Response.Write("                                </td>\r\n");
	Response.Write("                                <td>\r\n");
	Response.Write("                                    <div class=\"i_date\" style=\"float: right\">\r\n");
	Response.Write("                                        " + anninfo.Postdate.ToString().Trim() + "</div>\r\n");
	Response.Write("                                    </li>\r\n");
	Response.Write("                                </td>\r\n");
	Response.Write("                            </tr>\r\n");

	}	//end loop

	Response.Write("                        </table>\r\n");
	Response.Write("                    </td>\r\n");
	Response.Write("                </tr>\r\n");
	Response.Write("                <tr>\r\n");
	Response.Write("                    <td style=\"height: 25px; width: 116px; background: url(images/index/def_left02.jpg) no-repeat;\r\n");
	Response.Write("                        background-color: #F7F7F7\">\r\n");
	Response.Write("                    </td>\r\n");
	Response.Write("                    <td style=\"height: 25px; width: 116px; background: url(images/index/def_mid03.jpg) no-repeat;\">\r\n");
	Response.Write("                    </td>\r\n");
	Response.Write("                </tr>\r\n");
	Response.Write("                <tr>\r\n");
	Response.Write("                    <td bgcolor=\"f7f7f7\" width=\"116\" height=\"121\" background=\"images/index/def_left04.jpg\">\r\n");
	Response.Write("                        <div id=\"standard\">\r\n");
	Response.Write("                            <marquee scrollamount=\"2\" width=\"100\" height=\"88\" scrolldelay=\"100\" direction=\"up\"\r\n");
	Response.Write("                                onmouseover=\"this.stop()\" onmouseout=\"this.start()\">\r\n");
	Response.Write("			            <a href=\"http://www.cqit.edu.cn/\" target=\"_blank\">重庆工学院</a><br />\r\n");
	Response.Write("			            <a href=\"http://cs.cqit.edu.cn/\" target=\"_blank\">计算机学院</a><br />\r\n");
	Response.Write("			            <a href=\"http://nicweb.cqit.edu.cn/\" target=\"_blank\">网络信息中心</a><br />\r\n");
	Response.Write("			            <a href=\"http://jsjzx.cqit.edu.cn/\" target=\"_blank\">计算机中心</a><br />\r\n");
	Response.Write("			            <a href=\"http://www.intel.com/cn\" target=\"_blank\">Intel(中国)</a><br />\r\n");
	Response.Write("			     </marquee>\r\n");
	Response.Write("                        </div>\r\n");
	Response.Write("                    </td>\r\n");
	Response.Write("                    <td background=\"images/index/def_mid04.jpg\" width=\"452\" height=\"121\" align=\"left\">\r\n");
	Response.Write("                        <div class=\"mainbodyfrm\">\r\n");
	Response.Write("                            <table height=\"88%\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">\r\n");

	int newsinfo__loop__id=0;
	foreach(ArticleInfo newsinfo in newslist)
	{
		newsinfo__loop__id++;

	Response.Write("                                <tr>\r\n");
	Response.Write("                                    <td>\r\n");
	Response.Write("                                        <div class=\"i_title\">\r\n");
	Response.Write("                                            [" + newsinfo.Columnname.ToString().Trim() + "]&nbsp;<a href=\"showarticle-" + newsinfo.Articleid.ToString().Trim() + ".aspx\">" + newsinfo.Title.ToString().Trim() + "</a>\r\n");
	Response.Write("                                        </div>\r\n");
	Response.Write("                                    </td>\r\n");
	Response.Write("                                    <td>\r\n");
	Response.Write("                                        <div class=\"i_date\" style=\"float: right\">\r\n");
	Response.Write("                                            " + newsinfo.Postdate.ToString().Trim() + "</div>\r\n");
	Response.Write("                                    </td>\r\n");
	Response.Write("                                </tr>\r\n");

	}	//end loop

	Response.Write("                            </table>\r\n");
	Response.Write("                        </div>\r\n");
	Response.Write("                        <br />\r\n");
	Response.Write("                    </td>\r\n");
	Response.Write("                </tr>\r\n");
	Response.Write("            </table>\r\n");
	Response.Write("        </td>\r\n");
	Response.Write("        <td background=\"images/index/def_right.jpg\" height=\"261\" width=\"233\">\r\n");
	Response.Write("        </td>\r\n");
	Response.Write("    </tr>\r\n");
	Response.Write("</table>\r\n");

	Response.Write("</div>\r\n");
	Response.Write("      <!--footer-->\r\n");
	Response.Write("	<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">\r\n");
	Response.Write("	<tr height=\"91\">\r\n");
	Response.Write("	<td width=\"800\" background=\"images/index/def_foot01.jpg\">\r\n");
	Response.Write("	</td>\r\n");
	Response.Write("	</tr>\r\n");
	Response.Write("	</table>\r\n");
	Response.Write("	<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">\r\n");
	Response.Write("	<tr height=\"73\">\r\n");
	Response.Write("	<td align=\"center\" style=\"background:#3C458A; width:800px; color:#FFF\">\r\n");
	Response.Write("	       <span class=\"menudiv STYLE4\" title=\"执行时间:" + processtime.ToString() + ":查询数:" + querycount.ToString() + "\">版权所有 重庆工学院计算机协会<br />\r\n");
	Response.Write("      Copyright &copy;2007 All Rights Reserved<br />\r\n");
	Response.Write("      推荐使用IE6.0在1024*768分辨率下浏览</span>	</td>\r\n");
	Response.Write("	</tr>\r\n");
	Response.Write("	</table>\r\n");
	Response.Write("    </div>\r\n");
	Response.Write("    </form>\r\n");
	Response.Write("</body>\r\n");
	Response.Write("</html>\r\n");


	Response.Write("<!--" + querycount.ToString() + "<br />\r\n");
	Response.Write("" + querydetail.ToString() + "<br />\r\n");
	Response.Write("" + processtime.ToString() + "秒<br />-->\r\n");

}
</script>