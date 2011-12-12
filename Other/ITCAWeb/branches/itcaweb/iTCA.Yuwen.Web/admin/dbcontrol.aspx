<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dbcontrol.aspx.cs" Inherits="iTCA.Yuwen.Web.Admin.dbcontrol" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="Main.css" rel="stylesheet" type="text/css" />
    <title>数据管理</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="tbxSql" runat="server" Height="133px" TextMode="MultiLine" Width="420px"></asp:TextBox>&nbsp;
        <br />
        <br />
        <asp:Button ID="btnExecute" runat="server" Text="执行" OnClick="btnExecute_Click" /><br />
        <br />
        <asp:Label ID="lbMessage" runat="server" ForeColor="Red"></asp:Label></div>
    </form>
</body>
</html>
