<%@ Page Language="C#" AutoEventWireup="true" Codebehind="postarticle.aspx.cs" Inherits="LiteCMS.Web.Admin.postarticle" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Main.css" rel="stylesheet" type="text/css" />
    <title>文章发布</title>
    <!-- tinyMCE -->

    <script language="javascript" type="text/javascript" src="../editor/tiny_mce.js"></script>

    <script language="javascript" type="text/javascript">
        // Notice: The simple theme does not use all options some of them are limited to the advanced theme
        tinyMCE.init({
        mode : "textareas",theme : "advanced",language : "zh",
        theme_advanced_toolbar_location : "top",theme_advanced_toolbar_align : "left"
        });
    </script>

    <!-- /tinyMCE -->
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="width:100%;">
                <tr>
                    <td style="width: 96px">
                        <asp:DropDownList ID="ddlColumns" runat="server" Width="96px"></asp:DropDownList></td>
                    <td>
                        <asp:TextBox ID="tbxTitle" runat="server" Width="557px"></asp:TextBox><asp:DropDownList ID="ddlHightlight" runat="server" Width="117px">
                            <asp:ListItem Value="">默认</asp:ListItem>
                            <asp:ListItem Value="color:#FF0000">红色</asp:ListItem>
                            <asp:ListItem Value="color:#FFA500">橙色</asp:ListItem>
                            <asp:ListItem Value="color:#FFFF00">黄色</asp:ListItem>
                            <asp:ListItem Value="color:#008000">绿色</asp:ListItem>
                            <asp:ListItem Value="color:#00FFFF">青色</asp:ListItem>
                            <asp:ListItem Value="color:#0000FF">蓝色</asp:ListItem>
                            <asp:ListItem Value="color:#800080">紫色</asp:ListItem>
                            <asp:ListItem Value="color:#808080">灰色</asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="tbxSummary" runat="server" TextMode="MultiLine" Height="70px" Width="790px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="tbxContent" runat="server" TextMode="MultiLine" Height="366px" Width="790px"></asp:TextBox></td>
                </tr>
                <tr>
                    
                    <td>上传文件:</td>
                    <td>
                        <asp:FileUpload ID="fuUploader" runat="server" />
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                        <asp:Button ID="btnUpload" runat="server" Text="上传" OnClick="btnUpload_Click" /><asp:Label
                            ID="lbMessage" runat="server" ForeColor="Red"></asp:Label></td>
                </tr>
                <tr>
                    
                    <td>推荐:</td>
                    <td>
                        <asp:CheckBox ID="ckbxRecommand" runat="server" Text="推荐主题" />
                    </td>
                </tr>
                <tr>                    
                    <td align="center" colspan="2">
                        <asp:Button ID="btnSubmit" runat="server" Text="提交" OnClick="btnSubmit_Click" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" Text="取消" /></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
