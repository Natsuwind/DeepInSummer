<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="columnlist.aspx.cs" Inherits="iTCA.Yuwen.Web.Admin.columnlist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="Main.css" rel="stylesheet" type="text/css" />
    <title>栏目管理</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <br />
        <br />
        <asp:Label ID="lbMessage" runat="server" ForeColor="Red"></asp:Label><br />
        <table style="width:50%;">
            <tr>
                <td style="width: 61px">
                    栏目id</td>
                <td>
                    <asp:TextBox ID="tbxColumnid" runat="server" Width="61px"></asp:TextBox>非必要请勿填写.</td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="width: 61px">
                    栏目名</td>
                <td>
                    <asp:TextBox ID="tbxColumnname" runat="server" Width="83px"></asp:TextBox></td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="width: 61px">
                    父级栏目</td>
                <td>
                    <asp:DropDownList ID="dddlParentid" runat="server">
                    </asp:DropDownList></td>
                <td>
                    <asp:Button ID="btnAddNewColumn" runat="server" OnClick="btnAddNewColumn_Click" Text="添加栏目" /></td>
            </tr>
            <tr>
                <td style="width: 61px">
                </td>
                <td>
                    </td>
                <td>
                </td>
            </tr>
        </table>
    
    </div>
        <asp:GridView ID="gvColumnList" runat="server" AutoGenerateColumns="False" Width="50%" OnRowCancelingEdit="gvColumnList_RowCancelingEdit" OnRowDeleting="gvColumnList_RowDeleting" OnRowEditing="gvColumnList_RowEditing" OnRowUpdating="gvColumnList_RowUpdating">
            <Columns>
                <asp:BoundField DataField="columnid" HeaderText="栏目id" ReadOnly="True" />
                <asp:BoundField DataField="columnname" HeaderText="名称" />
                <asp:BoundField DataField="parentid" HeaderText="父级id" />
                <asp:CommandField ShowEditButton="True" />
                <asp:CommandField ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
