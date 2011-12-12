<%@ page language="C#" validaterequest="false" autoeventwireup="true" inherits="AShow, App_Web_ewwuxgiv" %>
<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>显示和回复</title>
    <link href="../itca.css" rel="stylesheet" type="text/css" media="all" />
</head>
<body style="background-color:White">



    <form id="form1" runat="server">
    <center>
        <table style="width: 621px; height: 136px; table-layout: auto; border-collapse: collapse;">
            <tr>
                <td style="height: 29px; width: 15%;" align="left">
                    <asp:HyperLink ID="hl_name" runat="server" Target="_blank">[hl_name]</asp:HyperLink></td>
                <td width="85%" style="height: 29px" align="left">
                    标题:<asp:Label ID="lb_title" runat="server"></asp:Label></td>
                <td style="height: 29px">
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 17px;text-align:left">
                    <asp:Label ID="lb_content" runat="server"></asp:Label></td>
                <td style="height: 17px">
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 17px; text-align: left">
                    留言日期<asp:Label ID="lb_posttime" runat="server"></asp:Label></td>
                <td style="height: 17px">
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 17px; text-align: left; background-color:#9ebef5">
                    回复日期<asp:Label ID="lb_replytime" runat="server"></asp:Label></td>
                <td style="height: 17px">
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 21px; background-color:#9ebef5" align="left">
                    <asp:HyperLink ID="hl_admin" runat="server">[hl_admin]</asp:HyperLink></td>
                <td style="height: 21px;" align="left">
                </td>
            </tr>
            <tr>
                <td colspan="2" style="background-color:#9ebef5">
                    &nbsp;<ftb:FreeTextBox ID="tb_replycontent" Width="623px" Height="187px" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                <asp:Button ID="bt_submit" runat="server" OnClick="bt_submit_Click" Text="提交" Width="237px" /></td>
                <td>
                </td>
            </tr>
        </table>
    </center>        
    </form>
</body>
</html>
