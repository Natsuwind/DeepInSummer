<%@ page language="C#" autoeventwireup="true" inherits="admin_AUlist, App_Web_ewwuxgiv" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
body {
	font:12px Tahoma;
	margin:0px;
	text-align:left;	
	background:#FFF;	
}
a:link,a:hover,a:visited{
font-size:12px;text-decoration:none;color:#000;
}
#container {	
	margin:10px auto; /*页面上、下边距为10个像素，并且居中显示*/
}
#list {
	
    margin:8px auto; /*居中*/
	padding:0 40px 0 40px; /*利用padding:20px 20px 0 0来固定菜单位置*/
}
#title {
	width:60%;
}

.time {
	width:20%;
}
.center{
	text-align:center;
}

</style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div id="list">
            <asp:repeater id="rptList" runat="server">
                <HeaderTemplate>
                    <table>
                    <tr>
                    <td>删除</td>
                    <td>选中</td>
                    <td>用户名</td>
                    <td>编辑用户</td>
                    <td>最后活动</td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    
                    <tr>
                            <td id="del"><a href="BDel.aspx?id=<%# DataBinder.Eval(Container.DataItem,"id") %>&deltype=user">[删除]</a>
                        </td>
                            <td id="ckbox">                        
                            <asp:CheckBox ID="ckbox_single" runat="server" />                         
                            <input type="hidden" id="SelectedID" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "id")%>'/>
                        </td>
                            
                            <td id="name" align="left"><a href="../book/user.aspx?uid=<%# DataBinder.Eval(Container.DataItem,"id") %>" target="_blank"><%# DataBinder.Eval(Container.DataItem,"u_name") %></a>
                        </td>
                            <td id="edit"><a href="AUcpm.aspx?uid=<%# DataBinder.Eval(Container.DataItem,"id") %>">编辑</a>
                        </td>
                            <td id="time"><%# DataBinder.Eval(Container.DataItem,"u_date") %>&nbsp;<%# DataBinder.Eval(Container.DataItem,"u_posttime") %>
                        </td>
                    </tr> 
                </ItemTemplate>		
                <FooterTemplate></table></FooterTemplate>				
		    </asp:repeater> 
		    <br />
	<asp:Button id="bt_del" runat="server" Text="删除所选" Width="72px" OnClick="bt_del_Click" ></asp:Button>
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
    </form>
</body>
</html>
