<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="deleteadmin.aspx.cs" Inherits="LiteCMS.Web.Admin.deleteadmin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="Main.css" rel="stylesheet" type="text/css" />
    <title>删除管理</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <br />
        <br />
        <br />
        <br />
        <asp:Label ID="lbMessage" runat="server"></asp:Label><br />
        <br />
        <br />
        <asp:Button ID="btnYes" runat="server" Text="确定删除" OnClick="btnYes_Click" />&nbsp; &nbsp; &nbsp;<asp:Button ID="btnCancel" runat="server" Text="取消返回" OnClick="btnCancel_Click" />
    </div>
    </form>
</body>
</html>
