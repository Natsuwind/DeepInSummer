<%@ Page language="c#" AutoEventWireup="false" EnableViewState="false" Inherits="iTCA.Yuwen.Web.showarticle" %>
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


	Response.Write("<div id=\"pagebody\">\r\n");
	Response.Write("    <div id=\"content\" style=\"background-image: url(images/showback.jpg)\">\r\n");
	Response.Write("        <div class=\"title\">\r\n");
	Response.Write("            <strong style=\"font-size: 16px\">" + articleinfo.Title.ToString().Trim() + "</strong>\r\n");
	Response.Write("        </div>\r\n");
	Response.Write("        <div class=\"information\" style=\"float: right; padding-right: 40px\">\r\n");
	Response.Write("            发布人:" + articleinfo.Username.ToString().Trim() + " &nbsp; 发布时间: " + articleinfo.Postdate.ToString().Trim() + "\r\n");
	Response.Write("        </div>\r\n");
	Response.Write("        <br />\r\n");
	Response.Write("        <div class=\"content\" style=\"text-align: left; padding: 10px 40px\">\r\n");
	Response.Write("            " + articleinfo.Content.ToString().Trim() + "</div>\r\n");
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