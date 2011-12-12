<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mainSetting.aspx.cs" Inherits="LiteCMS.Web.Admin.mainsetting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>网站设置</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table>
        <tr>
            <th>网站名称  : </th><td><asp:TextBox ID="tbxWebSiteName" runat="server" Width="178px"></asp:TextBox></td>
        </tr>
        <tr>
            <th>标题SEO   : </th><td><asp:TextBox ID="tbxSEOTitle" runat="server" Height="111px" 
                        TextMode="MultiLine" Width="256px"></asp:TextBox></td>
        </tr>
        <tr>
            <th>页面伪静态: </th><td><asp:CheckBox ID="ckbxUrlRewrite" runat="server" Text="启用" /></td>
        </tr>
        <tr>
            <th>伪静态后缀: </th><td><asp:TextBox ID="tbxUrlRewriteExtName" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <th>总缓存时间:</th><td><asp:TextBox ID="tbxGlobalCacheTimeOut" runat="server"></asp:TextBox>(单位:分钟)</td>
        </tr>
        <tr>
            <th>统计代码  :</th><td><asp:TextBox ID="tbxAnalyticsCode" TextMode="MultiLine" 
                        runat="server" Height="130px" Width="401px"></asp:TextBox></td>
        </tr>
        <tr>
            <th></th><td></td>
        </tr>
        <tr>
            <th></th><td></td>
        </tr>
        <tr>
            <th></th><td></td>
        </tr>
        <tr>
            <th></th><td><asp:Button ID="btnSubmit" runat="server" Text="提交" /></td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
