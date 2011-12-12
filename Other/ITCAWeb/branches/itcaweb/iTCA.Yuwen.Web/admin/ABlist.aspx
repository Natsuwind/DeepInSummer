<%@ page language="C#" autoeventwireup="true" inherits="ATitle, App_Web_ewwuxgiv" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>管理标题列表</title>
    <link href="ATitle.css" rel="stylesheet" type="text/css" media="all" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="container">
        <div id="menu">
        <a href="?type=1">显示未回复</a> <a href="?type=2">显示所有</a>	
        </div>
        <br />
        <br />
        <br />
        <br />
        
        <div id="list">
            <asp:repeater id="rptList" runat="server">
                <ItemTemplate>
                    <table>
                    <tr>
                            <td id="del"><a href="BDel.aspx?id=<%# DataBinder.Eval(Container.DataItem,"g_ID") %>&deltype=3">[删除]</a>
                        </td>
                            <td id="ckbox">                        
                            <asp:CheckBox ID="ckbox_single" runat="server" />                         
                            <input type="hidden" id="SelectedID" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "g_id")%>'/>
                        </td>
                            
                            <td id="title" align="left"><a href="BReply.aspx?id=<%# DataBinder.Eval(Container.DataItem,"g_ID") %>"><%# DataBinder.Eval(Container.DataItem,"g_title") %></a>
                        </td>
                            <td id="user"><a href="../book/user.aspx?name=<%# DataBinder.Eval(Container.DataItem,"g_User") %>" target="_blank"><%# DataBinder.Eval(Container.DataItem,"g_User") %></a>
                        </td>
                            <td id="time"><%# DataBinder.Eval(Container.DataItem,"g_date","{0:yyyy-MM-dd hh:mm }") %>
                        </td>
                    </tr>                
                    </table>
                </ItemTemplate>						
		    </asp:repeater>
		    <asp:Button id="bt_show" runat="server" Text="删除所选" OnClick="bt_show_Click" Width="72px" ></asp:Button>
           
        </div>
        <div class="pager">
            <table>
			    <tr>
				    <td style="height: 17px">共<asp:label id="lbTotalPage" runat="server"></asp:label>页|<asp:hyperlink id="hlkFirstPage" runat="server">首页</asp:hyperlink>|<asp:hyperlink id="hlkPrevPage" runat="server">上一页</asp:hyperlink>|<asp:hyperlink id="hlkNextPage" runat="server">下一页</asp:hyperlink>|<asp:hyperlink id="hlkLastPage" runat="server">末页</asp:hyperlink>|第<asp:label id="lbCurrentPage" runat="server"></asp:label>页
					</td>
				</tr>
			</table>
        </div>


    </div>
    <div>
        &nbsp;</div>
    </form>
</body>
</html>
