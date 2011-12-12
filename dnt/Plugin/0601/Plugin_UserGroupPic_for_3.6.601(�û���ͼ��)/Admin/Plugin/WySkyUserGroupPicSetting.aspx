<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WySkyUserGroupPicSetting.aspx.cs"
    Inherits="Wysky.Discuz.Plugin.UserGroupPic.Views.Admin.Plugin.WySkyUserGroupPicSetting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用户组图标设置</title>
    <link href="../styles/datagrid.css" type="text/css" rel="stylesheet" />
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
    <link href="../styles/modelpopup.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/modalpopup.js"></script>
    <script type="text/javascript" src="../js/common.js"></script>
</head>
<body>
    
    <% if (isInstall != 1)
       {
    %>
    <div class="ManagerForm">
        <fieldset>
            <legend style="background: url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">QQ
                用户组图标设置</legend>
            <div style="background: url('../images/hint.gif') no-repeat scroll 20px 15px #FDFFF2;
                border: 1px dotted #DBDDD3; clear: both; margin: 10px 0; padding: 15px 10px 10px 56px;
                text-align: left">
                上传的图标将在帖子查看页左栏显示。修改图标后，如果前台无变化，请 Ctrl+F5 强制刷新前台页面以更新浏览器缓存图片。</div>
            <asp:Repeater ID="rptGroupList" runat="server">
                <HeaderTemplate>
                    <table width="100%">
                        <tbody>
                        <tr>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="vtop rowform">
                        <%# DataBinder.Eval(Container.DataItem, "Grouptitle")%>
                        </td>
                        <td class="vtop">
                            <img src="../../upload/plugin/wyskyusergrouppic/<%# DataBinder.Eval(Container.DataItem, "groupid")%>.gif?<%=DateTime.Now.ToString("hhmmss")%>>" />
                        </td>
                        <td>
                        <form id="form_gourid_<%# DataBinder.Eval(Container.DataItem, "groupid")%>" name="form_gourid_<%# DataBinder.Eval(Container.DataItem, "groupid")%>" action="WySkyUserGroupPicSetting.aspx" enctype="multipart/form-data" method="post">
                        <input type="hidden" id="groupid_<%# DataBinder.Eval(Container.DataItem, "groupid")%>" name="groupid" value="<%# DataBinder.Eval(Container.DataItem, "groupid")%>" />
                        <input type="file" id="groupid_<%# DataBinder.Eval(Container.DataItem, "groupid")%>" name="groupid_<%# DataBinder.Eval(Container.DataItem, "groupid")%>" value="<%# DataBinder.Eval(Container.DataItem, "groupid")%>" />
                        <input type="submit" value="上传" />
                        </form>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody> </table>
                </FooterTemplate>
            </asp:Repeater>
            <br />
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
        </fieldset>
    </div>
    <%} %>
</body>
</html>
