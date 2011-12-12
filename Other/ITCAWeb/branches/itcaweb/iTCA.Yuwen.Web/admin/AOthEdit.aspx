<%@ page language="C#" validaterequest="false" autoeventwireup="true" inherits="admin_AddAnn, App_Web_ewwuxgiv" %>
<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>内容管理</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:DropDownList ID="DDList" runat="server" Width="84px" AutoPostBack="True" OnTextChanged="DDList_TextChanged">
            <asp:ListItem>注册协议</asp:ListItem>            
            <asp:ListItem>协会简介</asp:ListItem>
            <asp:ListItem>协会文化</asp:ListItem>
            <asp:ListItem>组织结构</asp:ListItem>
            <asp:ListItem>大事记</asp:ListItem>
        </asp:DropDownList>
    <ftb:FreeTextBox ID="FTB" Width="550px" Height="230px" runat="server" />        
        <asp:Button ID="bt_sub" runat="server" OnClick="bt_sub_Click" Text="提交" Width="118px" /></div>
    </form>
</body>
</html>
