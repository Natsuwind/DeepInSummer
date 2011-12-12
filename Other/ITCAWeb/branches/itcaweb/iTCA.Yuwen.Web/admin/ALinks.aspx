<%@ page language="C#" autoeventwireup="true" inherits="admin_links, App_Web_ru5tvrvv" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>友情链接</title>
    <link href="ATitle.css" rel="stylesheet" type="text/css" media="all" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align:center">
        <table style="width: 538px">
            <tr>
                <td>
                    Logo地址</td>
                <td>
                    网站名字</td>
                <td style="width: 100px">
                    网站地址</td>
                <td style="width: 174px">
                    网站描述</td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="tb_logo" runat="server" Width="124px"></asp:TextBox></td>
                <td>
                    <asp:TextBox ID="tb_name" runat="server" Width="124px"></asp:TextBox></td>
                <td style="width: 100px">
                    <asp:TextBox ID="tb_link" runat="server" Width="124px"></asp:TextBox></td>
                <td style="width: 174px">
                    <asp:TextBox ID="tb_desc" runat="server" Width="124px"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Button ID="bt_addlink" runat="server" OnClick="bt_addlink_Click" Text="增加" /></td>
            </tr>
        </table>
        </div>
    <div>
        <asp:DataGrid ID="dg" runat="server" AutoGenerateColumns="False" OnCancelCommand="dg_CancelCommand" OnEditCommand="dg_EditCommand" OnUpdateCommand="dg_UpdateCommand" BorderStyle="None" HorizontalAlign="Center" OnDeleteCommand="dg_DeleteCommand">
            <Columns>
                <asp:TemplateColumn HeaderText="序号">
                    <ItemTemplate>
                        <asp:Label ID="id" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.l_id") %>'></asp:Label> 
                    </ItemTemplate>                    
                </asp:TemplateColumn>
            
                <asp:TemplateColumn HeaderText="Logo">
                    <ItemTemplate>
                        <img src="<%# DataBinder.Eval(Container, "DataItem.l_img") %>" alt="<%# DataBinder.Eval(Container, "DataItem.l_img") %>" height="31px" width="88px" />                        
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" ID="strIMG" Text='<%# DataBinder.Eval(Container, "DataItem.l_img") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="网站名字">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.l_name") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" ID="strNAME" Text='<%# DataBinder.Eval(Container, "DataItem.l_name") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="网站地址">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.l_link") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" ID="strLINK" Text='<%# DataBinder.Eval(Container, "DataItem.l_link") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="描述">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.l_desc") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" ID="strDESC" Text='<%# DataBinder.Eval(Container, "DataItem.l_desc") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:LinkButton runat="server" CausesValidation="false" CommandName="Edit" Text="编辑"></asp:LinkButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton runat="server" CommandName="Update" Text="更新"></asp:LinkButton>
                        <asp:LinkButton runat="server" CausesValidation="false" CommandName="Cancel" Text="取消"></asp:LinkButton>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:LinkButton runat="server" CausesValidation="false" CommandName="Delete" Text="删除"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid></div>
        
        <div class="pager" style="text-align:center">
            <table>
			    <tr>
				    <td style="height: 17px">共<asp:label id="lbTotalPage" runat="server"></asp:label>页|<asp:hyperlink id="hlkFirstPage" runat="server">首页</asp:hyperlink>|<asp:hyperlink id="hlkPrevPage" runat="server">上一页</asp:hyperlink>|<asp:hyperlink id="hlkNextPage" runat="server">下一页</asp:hyperlink>|<asp:hyperlink id="hlkLastPage" runat="server">末页</asp:hyperlink>|第<asp:label id="lbCurrentPage" runat="server"></asp:label>页
					</td>
				</tr>
			</table>
        </div>
        
    </form>
</body>
</html>
