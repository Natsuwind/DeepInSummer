<%@ page language="C#" autoeventwireup="true" inherits="AEditadmin, App_Web_ru5tvrvv" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>管理设置</title>
    <link href="ATitle.css" rel="stylesheet" type="text/css" media="all" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align:left">
        <asp:repeater id="rptList" runat="server">
                <HeaderTemplate>
                    <table style="text-align:left">
                    <tr>
                        <td>删除</td>
                        <td>登陆名</td>
                        <td>管理权</td>
                        <td>最后登陆</td>
                        <td>登陆IP</td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                            <td id="del"><a href="BDel.aspx?id=<%# DataBinder.Eval(Container.DataItem,"a_id") %>&deltype=admin">[删除]</a>
                        </td>
                            <td id="user"><a href="../book/user.aspx?name=<%# DataBinder.Eval(Container.DataItem,"a_name") %>" target="_blank"><%# DataBinder.Eval(Container.DataItem,"a_name") %></a>
                        </td>
                            <td id="power"><%# DataBinder.Eval(Container.DataItem,"a_areaname") %>
                        </td>
                        <td id="lastlogin"><%# DataBinder.Eval(Container.DataItem,"a_date") %>&nbsp;<%# DataBinder.Eval(Container.DataItem,"a_time") %>
                        </td>
                        <td id="ip"><%# DataBinder.Eval(Container.DataItem,"a_ip") %>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate></table></FooterTemplate>				
		    </asp:repeater>		    
            <table>
			    <tr>
				    <td style="height: 17px">共<asp:label id="lbTotalPage" runat="server"></asp:label>页|<asp:hyperlink id="hlkFirstPage" runat="server">首页</asp:hyperlink>|<asp:hyperlink id="hlkPrevPage" runat="server">上一页</asp:hyperlink>|<asp:hyperlink id="hlkNextPage" runat="server">下一页</asp:hyperlink>|<asp:hyperlink id="hlkLastPage" runat="server">末页</asp:hyperlink>|第<asp:label id="lbCurrentPage" runat="server"></asp:label>页
					</td>
				</tr>
			</table>
        <br />
        <div style="text-align:left">
        帐号<asp:TextBox ID="tb_addadmin" runat="server"></asp:TextBox>
        <br />
        密码<asp:TextBox ID="tb_pwd" runat="server" TextMode="Password" Width="149px"></asp:TextBox>
            <br />
            密码<asp:TextBox ID="tb_pwd2" runat="server" TextMode="Password" Width="149px"></asp:TextBox>
            <asp:CompareValidator ID="CV_pwd" runat="server" ControlToCompare="tb_pwd" ControlToValidate="tb_pwd2"
                Display="Dynamic" ErrorMessage="2次输入不匹配"></asp:CompareValidator><br />
        权限<asp:DropDownList ID="DDL" runat="server">
            <asp:ListItem Value="yabadie">总管理员</asp:ListItem>
            <asp:ListItem Value="cmsadmin">新闻公告管理员</asp:ListItem>
            <asp:ListItem Value="bookadmin">留言板管理员</asp:ListItem>
        </asp:DropDownList><br />
        <asp:Button ID="bt_addadmin" runat="server" Text="添加" OnClick="bt_addadmin_Click" />
        </div>
        <asp:DataGrid ID="dg" runat="server" AutoGenerateColumns="False" Enabled="False" Visible="False">
            <Columns>
                <asp:TemplateColumn HeaderText="登陆名">
                    <ItemTemplate>
                        <asp:Label runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="密码">
                    <ItemTemplate>
                        <asp:Label runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="权限">
                    <ItemTemplate>
                        <asp:Label runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                    <asp:DropDownList runat="server"></asp:DropDownList>
                    <asp:ListItem Value="yabadie">总管理员</asp:ListItem>
                    <asp:ListItem Value="cmsadmin">新闻公告管理员</asp:ListItem>
                    <asp:ListItem Value="bookadmin">留言板管理员</asp:ListItem>                      
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="编辑">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" CausesValidation="false" CommandName="Edit" Text="编辑"></asp:LinkButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton runat="server" CommandName="Update" Text="更新"></asp:LinkButton>
                        <asp:LinkButton runat="server" CausesValidation="false" CommandName="Cancel" Text="取消"></asp:LinkButton>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="删除">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" CausesValidation="false" CommandName="Delete" Text="删除"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid><div class="pager">
        </div>
        </div>
    </form>
</body>
</html>
