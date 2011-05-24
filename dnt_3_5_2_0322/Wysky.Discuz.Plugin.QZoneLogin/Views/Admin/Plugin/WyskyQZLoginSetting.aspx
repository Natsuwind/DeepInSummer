<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WyskyQZLoginSetting.aspx.cs"
    Inherits="Wysky.Discuz.Plugin.QZoneLogin.Views.Admin.Plugin.WyskyQZLoginSetting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>QQ 登录设置</title>
    <link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
    <link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/modalpopup.js"></script>
    <script type="text/javascript" src="../js/common.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <% if (isInstall != 1)
       { %>
    <div class="ManagerForm">
        <fieldset>
            <legend style="background: url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">QQ
                登录插件设置</legend>
            <div style="background: url('../images/hint.gif') no-repeat scroll 20px 15px #FDFFF2;
                border: 1px dotted #DBDDD3; clear: both; margin: 10px 0; padding: 15px 10px 10px 56px;
                text-align: left">
                请从QQ官方申请API使用权（<a href="http://connect.opensns.qq.com/apply" target="_blank">http://connect.opensns.qq.com/apply</a>）</div>
            <br />
            <table width="100%">
                <tbody>
                    <tr>
                        <td class="item_title" colspan="2">
                            启用 QQ 登录:
                        </td>
                    </tr>
                    <tr>
                        <td class="vtop rowform">
                            <asp:RadioButton ID="rbtnEnabled" runat="server" Text="启用" GroupName="1" />
                            <asp:RadioButton ID="rbtnDisabled" runat="server" Text="关闭" GroupName="1" />
                        </td>
                        <td class="vtop">
                            关闭后，将不会显示QQ登录图标
                        </td>
                    </tr>
                    <tr>
                        <td class="item_title" colspan="2">
                            APP ID :
                        </td>
                    </tr>
                    <tr>
                        <td class="vtop rowform">
                            <asp:TextBox ID="tbxAppId" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvAppId" runat="server" 
                                ControlToValidate="tbxAppId" Display="Dynamic" ErrorMessage="*不能为空"></asp:RequiredFieldValidator>
                        </td>
                        <td class="vtop">
                            从QQ登录官方申请，请填写数值类型。</td>
                    </tr>
                    <tr>
                        <td class="item_title" colspan="2">
                            APP KEY:
                        </td>
                    </tr>
                    <tr>
                        <td class="vtop rowform">
                            <asp:TextBox ID="tbxAppKey" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvAppKey" runat="server" 
                                ControlToValidate="tbxAppKey" Display="Dynamic" ErrorMessage="*不能为空"></asp:RequiredFieldValidator>
                        </td>
                        <td class="vtop">
                            从QQ登录官方申请，请填写字符串类型。</td>
                    </tr>
                </tbody>
            </table>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
            <br />
            <asp:Button ID="btnSaveSetting" runat="server" Text="保存设置" OnClick="btnSaveSetting_Click" />
        </fieldset>
    </div>
    <%} %>
    </form>
</body>
</html>
