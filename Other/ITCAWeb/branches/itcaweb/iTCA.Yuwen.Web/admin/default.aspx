<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="iTCA.Yuwen.Web.Admin.index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>后台管理-重庆工学院计算机协会(iTCA)</title>
    <style type="text/css">
    * 
    {
        margin:0pt;
        padding:0pt;
    }
    h1, h2, h3, h4, h5, h6, p, blockquote 
    {
        margin:0pt;
        padding:10px;
    }
    body
        {
            font:12px Tahoma;
	        text-align:center;
	        background:#FFF;
	        width:960px;
        }
    ul 
        {
            list-style:none;
            /*float:left;*/
            width:100%;
            font-size:16px;
            margin-left: 0px;
            margin-top:10px;
        }
    ul li 
        {
            float:left;
            width:100%;
            line-height:20px; 
            text-align:center
        }
    ul li:hover
        {
            background-color:Silver; color:Red;
        }
    ul li a
        {
            display:block;width:100%;text-decoration:none;
        }
    ul li a:link,a:visited
        {
            color:#000;
        }
    ul li a:hover
        {
            color:Red;
        }
/*  ul li a:visited
        {
            color:#000;
        }*/
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="Banner" style="height:70px; width:100%; background-color:Gray">
        Banner
    </div>
    <div id="Main" style="margin:10px auto 0 auto; border:solid 1px gray; text-align:left">欢迎 <asp:Label ID="lbLoginName" runat="server"></asp:Label> 登录后台管理,点击<a href="../" target="_blank">回到前台</a>。
    </div>
    <div id="Menu" style="width:130px; float:right; border:solid 1px gray; margin:10px 5px">
        <ul>
            <li><a href="articlelist.aspx" target="MainIFR">后台首页</a></li>
            <li><a href="postarticle.aspx" target="MainIFR">发布文章</a></li>
            <li><a href="articlelist.aspx" target="MainIFR">文章管理</a></li>
            <li><a href="columnlist.aspx" target="MainIFR">栏目管理</a></li>
            <li><a href="ABlist.aspx" target="MainIFR">留言管理</a></li>
            <li><a href="AUlist.aspx" target="MainIFR">用户管理</a></li>
            <li><a href="AEditadmin.aspx" target="MainIFR">管理设置</a></li>
            <li><a href="ALinks.aspx" target="MainIFR">友情管理</a></li>
            <!--<li><a href="#" target="MainIFR">系统设置</a></li>-->
            <li><a href="AOthEdit.aspx" target="MainIFR">其他管理</a></li>
            <li><a href="template.aspx" target="MainIFR">模板生成</a></li>
            <li><a href="ALoginout.aspx" target="_parent">退出后台</a></li>
        </ul>
    </div>
    
    <iframe id="MainIFR" name="MainIFR" frameborder="0" scrolling="no" src="articlelist.aspx" width="800px" height="490px" style="margin:10px auto auto auto; border:solid 1px gray;">
    </iframe>
    </form>
</body>
</html>
