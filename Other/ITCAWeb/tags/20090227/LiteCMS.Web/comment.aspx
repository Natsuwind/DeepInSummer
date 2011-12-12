<%@ Page language="c#" AutoEventWireup="false" EnableViewState="false" Inherits="LiteCMS.Web.comment" %>
<%@ Import namespace="LiteCMS.Data" %>
<%@ Import namespace="LiteCMS.Entity" %>
<script runat="server">
override protected void OnInit(EventArgs e)
{
	/*
	This is a cached-file of template("\templates\templatename\comment.htm"), it was created by LiteCMS.CN Template Engine.
	Please do NOT edit it.
	此文件为模板文件的缓存("\templates\模板名\comment.htm"),由 LiteCMS.CN 模板引擎生成.
	所以请不要编辑此文件.
	*/
	base.OnInit(e);

	Response.Write(templateBuilder.ToString());
}
</script>