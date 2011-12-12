<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>

<%@ Page Language="C#" MasterPageFile="~/include/itca.Master" AutoEventWireup="true"
    Inherits="book_read, App_Web_s0wfswid" Title="阅读留言 - 留言板 - iTCA 重庆工学院计算机协会" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <center>
    <div class="table" style="height: 230px">
        <table style="text-align: left; width: 80%">
            <tr>
                <td>
                    <strong>标题:<asp:Label ID="lb_title" runat="server"></asp:Label></strong></td>
            </tr>
            <tr>
                <td>
                    内容:<br />
                    <asp:Label ID="lb_content" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    留言时间:<asp:Label ID="lb_time" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <br />
    <div class="table" style="height: 230px">
        <table style="text-align: left; width: 80%" bgcolor="#d5d5d5">
            <tr>
                <td>
                    回复时间:<asp:Label ID="lb_replytime" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    回复人:<asp:HyperLink ID="hl_admin" runat="server">[hl_admin]</asp:HyperLink></td>
            </tr>
            <tr>
                <td>
                    内容:<br />
                    <asp:Label ID="lb_replycontent" runat="server"></asp:Label></td>
            </tr>
        </table>
    </div>
    </center>
</asp:Content>
