<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="template.aspx.cs" Inherits="iTCA.Yuwen.Web.Admin.template" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>模板生成</title>
    <link href="Main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:CheckBoxList ID="cbxlTemplateFileList" runat="server">
        </asp:CheckBoxList></div>
        <asp:Button ID="btnCreateAll" runat="server" OnClick="btnCreateAll_Click" Text="全部生成" />
    </form>
</body>
</html>
