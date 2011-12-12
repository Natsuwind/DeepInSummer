<%@ Page language="c#" AutoEventWireup="false" EnableViewState="false" Inherits="iTCA.Yuwen.Web.ann" %>
<%@ Import namespace="iTCA.Yuwen.Data" %>
<%@ Import namespace="iTCA.Yuwen.Entity" %>
<script runat="server">
override protected void OnInit(EventArgs e)
{
	base.OnInit(e);
	Response.Write("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n");
	Response.Write("<html xmlns=\"http://www.w3.org/1999/xhtml\" >\r\n");
	Response.Write("<head>\r\n");
	Response.Write("    <title>Untitled Page</title>\r\n");
	Response.Write("</head>\r\n");
	Response.Write("<body>\r\n");
	Response.Write("iTCA公告<br />\r\n");
	Response.Write("    <ul>\r\n");

	int anninfo__loop__id=0;
	foreach(ArticleInfo anninfo in annlist)
	{
		anninfo__loop__id++;

	Response.Write("            <li>[" + anninfo.Columnname.ToString().Trim() + "]" + anninfo.Title.ToString().Trim() + "&nbsp;&nbsp;&nbsp;" + anninfo.Username.ToString().Trim() + "&nbsp;&nbsp;" + anninfo.Postdate.ToString().Trim() + "</li>\r\n");

	}	//end loop

	Response.Write("    </ul>\r\n");
	Response.Write("" + querycount.ToString() + "<br />\r\n");
	Response.Write("" + querydetail.ToString() + "<br />\r\n");
	Response.Write("" + processtime.ToString() + "秒<br />\r\n");
	Response.Write("</body>\r\n");
	Response.Write("</html>\r\n");

}
</script>