<%@ Page Language="C#" MasterPageFile="~/include/itca.Master" AutoEventWireup="true" Inherits="itca.WebPages.other.index,iTCA.Yuwen.Web" Title="关于我们" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


<div id="leftcolumn" style="width:142px; height:500px; float:left; background:url(../images/other/left.jpg) no-repeat; background-color:White"><!--height:600px;-->
<table width="100%" border="0" style="text-align:center">
  <tr> 
    <td style="height: 30px; font-size: 11pt; color:#3c458a; text-align:center; background:url(../images/other/about.gif) no-repeat"><strong><b>关于我们</b></strong>
    </td>
  </tr>
  <tr> 
    <td style="height: 25px"><div style="text-align:center"><a href="intr.html" target="mainFrame" style="color:#3c458a;">iTCA简介</a></div></td>
  </tr>
  <tr> 
    <td style="height: 25px"><div style="text-align:center"><a href="cul.html" target="mainFrame" style="color:#3c458a;">iTCA文化</a></div></td>
  </tr>
  <tr> 
    <td style="height: 25px"><div style="text-align:center"><a href="orgshow.html" target="mainFrame" style="color:#3c458a;">组织结构</a></div></td>
  </tr>
  <tr> 
    <td style="height: 25px"><div style="text-align:center"><a href="events.html" target="mainFrame" style="color:#3c458a;">大事记</a></div></td>
  </tr>
  <tr> 
    <td style="height: 30px; text-align:center; background:url(../images/other/links.gif) no-repeat"><a href="links.aspx" target="mainFrame" style="font-size: 11pt; color:#3c458a;"><strong><b>友情链接</b></strong></a></td>
  </tr>
  </table>
  <br />  <br />  <br />  <br />  <br />    <br />    <br />    <br />    <br />    <br />    <br />    <br />    <br />    <br />    <br />  <br />  <br />
  
  <table width="100%" border="0" style="text-align:center">
  <tr> 
    <td style="height: 30px; font-size: 11pt; color:#3c458a; text-align:center; background:url(../images/other/tel.gif) no-repeat"><a href="Connection.html" target="mainFrame" style="color:#3c458a;"><strong><b>联系我们</b></strong>
    </td>
  </tr>
  </table>
 </div>
<div id="rightpic" style="height:30px; float:right; width:658px; background:url(../images/other/itca_ban.jpg) no-repeat"></div>
<iframe id="mainFrame" name="mainFrame" src="intr.html" style="width:658px; height:470px; float:right; border:0" scrolling="no" frameborder="0"></iframe><!--IE居然只认frameborder...-->


</asp:Content>
