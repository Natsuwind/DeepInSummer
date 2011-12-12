<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dbconvert.aspx.cs" Inherits="LiteCMS.Web.dbconvert" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="gvPreData" runat="server">
        </asp:GridView>
        &nbsp;</div>
        <asp:Button ID="btnStart" runat="server" OnClick="btnStart_Click" Text="执行" />
    </form>
</body>
</html>
