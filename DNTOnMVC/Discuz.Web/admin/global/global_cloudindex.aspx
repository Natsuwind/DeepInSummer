<%@ Page Language="c#" Inherits="Discuz.Web.Admin.cloudindex" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>云平台首页</title>
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/modalpopup.js"></script>
    <script type="text/javascript" src="../../javascript/jquery.js"></script>
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <style>
        .platform_error
        {
            border-top:2px solid #eee;
            border-bottom:2px solid #eee;
            background:#FCFCFC;
            padding:20px 0;
            text-align:center;
            color:#AA0408;
            font-weight:700;
            margin:20px 0;
        }
    </style>
</head>
<body>
    <div class="ManagerForm">
    <%if(config.Cloudenabled==0){ %>
    <fieldset>
	    <legend style="background:url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">Discuz!NT提示</legend>
	    <div class="platform_error">
		    云平台服务未开启,点击<a href="global_cloudset.aspx">开通云平台</a>
	    </div>
    </fieldset>
    <%}else{ %>
    <div id="cloudindex">
        <iframe frameborder="0" width="810px" scrolling="no" height="810px" src="<%=iFrameUrl %>"></iframe>
    </div>
    <%} %>
    </div>
</body>
</html>
