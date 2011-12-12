<%@ Page Language="C#" AutoEventWireup="true" Inherits="itca.WebPages.other.Links,iTCA.Yuwen.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>友情链接</title>
      <style type="text/css">
        body {
	        font:12px Tahoma;
	        margin:0px;
	        text-align:left;	
	        background:#000;	
        }
        a:link,a:hover,a:visited{
        font-size:12px;text-decoration:none;color:#000;
        }
        #container{
        margin:15px auto;
        }
        #page {
        width:658px; height:auto; background:#FFF;height:484px;
        }
     </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="page" style="height:600px">    
     <div id="container" style="padding-left:20px; padding-top:0; margin-top:0">    
       <div id="list">
            <asp:repeater id="rptList" runat="server">
                <HeaderTemplate>
                <table>
                    <tr>                                                        
                            <td style="width:90px; text-align:center">网站LOGO
                        </td>                            
                            <td style="width:200px;text-align:center">网站名字
                        </td>
                            <td style="width:300px; text-align:center">网站简述
                        </td>
                    </tr>           
                </HeaderTemplate>
                <ItemTemplate>
                    
                    <tr>                                                        
                            <td align="left" style="height:31px;overflow:hidden"><img src="<%# DataBinder.Eval(Container.DataItem,"l_img") %>" alt="LOGO" height="31px" width="88px" />
                        </td>                           
                            <td align="center"><a href="<%# DataBinder.Eval(Container.DataItem,"l_link") %>" target="_blank"><%# DataBinder.Eval(Container.DataItem,"l_name") %></a>
                        </td>
                            <td><%# DataBinder.Eval(Container.DataItem,"l_desc") %>
                        </td>
                    </tr>                
                    
                </ItemTemplate>			
                <FooterTemplate>
                </table>
                </FooterTemplate>			
		    </asp:repeater> 
		    </div>
		    
		    <div class="pager" style="text-align:center; padding-top:10px">
            共<asp:label id="lbTotalPage" runat="server"></asp:label>页|<asp:hyperlink id="hlkFirstPage" runat="server">首页</asp:hyperlink>|<asp:hyperlink id="hlkPrevPage" runat="server">上一页</asp:hyperlink>|<asp:hyperlink id="hlkNextPage" runat="server">下一页</asp:hyperlink>|<asp:hyperlink id="hlkLastPage" runat="server">末页</asp:hyperlink>|第<asp:label id="lbCurrentPage" runat="server"></asp:label>页				
            </div>
    </div>
    </div>
    </form>
</body>
</html>
