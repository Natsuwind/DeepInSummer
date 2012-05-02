<%@ Page Language="c#" Inherits="Discuz.Web.Admin.cloudset"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>Discuz云平台设置</title>
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <link href="../styles/dntmanager.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../js/modalpopup.js"></script>
    <script type="text/javascript" src="../../javascript/jquery.js"></script>
    <style type="text/css">
        .sideinfo li{border-bottom:1px dashed #EEE;padding:8px 0;color:#000;}
	    .sideinfo li span{float:left;display:block;width:120px;font-weight:700;}
    </style>
</head>
<body>
    <script type="text/javascript">
        var ajaxtarget = "global_cloudset.aspx";
    </script>
    <div class="ManagerForm">
        <fieldset>
        <%if (config.Cloudenabled == 0)
          {%>
            <legend style="background:url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">开通Discuz!云平台</legend>
            <table width="100%">
                <tr>
                    <td id="cloudtiptitle" class="item_title"></td>
                </tr>
                <tr>
                    <td id="cloudtips" class="vtop">
                    </td>
                </tr>
                <tr>
                    <td id="clouderrtitle" class="item_title"></td>
                </tr>
                <tr>
                    <td id="clouderrtips" style=" background-color:#eee" class="vtop"></td>
                </tr>
                <tr>
                    <td style="padding-top: 10px; color: #003366;">
                        <input type="checkbox" checked="checked" onclick="agreeprotocalcontrol(this);" name="finished" id="agreeprotocal" style="margin-right: 10px;vertical-align: middle;"/>
                        我已经仔细阅读并同意<a id="protocalurl" href="###" target="_blank">《Discuz!云平台服务使用协议》</a>
                    </td>
                </tr>
                <tr>
                    <td style="padding: 10px 0;">
                        <button id="regcloudbtn" onclick="regcloud();" class="ManagerButton" type="button"><img src="../images/submit.gif">我要开通</button>
                    </td>
                </tr>
	        </table>
<%--
            <div>
                <input id="regcloudbtn" type="button" onclick="regcloud();" value="我要开通" />
            </div>--%>
            <script type="text/javascript">
                var reged = "<%=config.Cloudsiteid %>" == "";

                function agreeprotocalcontrol(obj) {
                    if (obj.checked) {
                        $('#regcloudbtn').attr('disabled', false);
                        $('#regcloudbtn').css('color', '#000');
                    } else {
                        $('#regcloudbtn').attr('disabled', true);
                        $('#regcloudbtn').css('color', '#aaa');
                    }
                }

                function regcloud() {
                    if (reged) {
                        jQuery.get(ajaxtarget, { 'action': 'reg', 't': Math.random() },
                        function (data) {
                            if (data == "OK") {
                                redirecttobind();
                            }
                            else {
                                $('#clouderrtitle').html('云平台开通错误');
                                $('#clouderrtips').html(data);
                            }
                        }
                    );
                    }
                    else {
                        redirecttobind();
                    }
                }

                function redirecttobind() {
                    jQuery.get(ajaxtarget, { 'action': 'bind', 't': Math.random() },
                        function (data) {
                            var callback = eval("(" + data + ")");
                            parent.parent.window.location = callback.url;
                        }
                    );
                }

                function dealHandle(json) {
                    $('#cloudtiptitle').append(json.cloudIntroduction.open_title);
                    $('#cloudtips').append(json.cloudIntroduction.open_content);
                    $('#protocalurl').attr('href', json.protocalUrl);
                }
            </script>
            <script type="text/javascript" src="http://cp.discuz.qq.com/cloud/introduction/"></script>
        <%}
        else
          { %>
            <legend style="background:url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">站点信息</legend>
	        <table width="100%">
		        <tr><td class="item_title">提示技巧</td></tr>
		        <tr>
			        <td class="vtop">
				        <ul>
					        <li>如果站点名称或者站点URL有变动，请点击"同步站点信息"按钮。</li>
					        <li>站点KEY是站点与云平台通信的验证密钥，若近期有危险操作泄漏站点KEY等消息,请点击"更换站点KEY"按钮.<span style="color:#B23D53">请谨慎使用此功能</span></li>
				        </ul>
			        </td>
		        </tr>
		        <tr><td class="item_title">站点信息</td></tr>
		        <tr>
			        <td class="vtop">
				        <ul class="sideinfo">
					        <li><span>站点ID</span><%=config.Cloudsiteid%></li>
                            <li><span>站点KEY</span><%=SecritySiteKey(config.Cloudsitekey)%>(此处为了安全考虑不显示站点key的完全信息)</li>
				        </ul>
			        </td>
		        </tr>
                <tr>
                    <td id="controltips" style="background-color:#eee">
                    </td>
                </tr>
		        <tr>
                    <td style="padding:10px 0;">
                        <button id="syncsite" class="ManagerButton" type="button" onclick="syncsiteinfo();"><img alt="" src="../images/submit.gif"/>同步站点信息</button>
                        <button id="changekey" class="ManagerButton" type="button" onclick="resetkey();"><img alt=""  src="../images/submit.gif"/>更换站点KEY</button>
                        <button id="setip" class="ManagerButton" type="button"><img alt=""  src="../images/submit.gif"/>设置云平台接口IP</button>
                    </td>
                </tr>
	        </table>
            <script type="text/javascript">
                function syncsiteinfo() {
                    jQuery.get(ajaxtarget, { 'action': 'sync', 't': Math.random() },
                        function (data) {
                            if (data == "OK") {
                                $('#controltips').html('站点信息已同步');
                            }
                            else {
                                $('#controltips').html(data);
                            }
                        }
                    );
                }

                function resetkey() {
                    if (confirm("确定要更换站点KEY")) {
                        jQuery.get(ajaxtarget, { 'action': 'resetkey', 't': Math.random() },
                        function (data) {
                            if (data == "OK") {
                                alert("站点KEY已更新");
                                location.reload();
                            }
                            else {
                                $('#controltips').html(data);
                            }
                        }
                        );
                    }
                }
            </script>
        <%} %>
        </fieldset>
    </div>



<%--    <%if (config.Cloudenabled == 1)
      {%>
    <iframe frameborder="0" width="810px" scrolling="no" height="810px" src="<%=iframeUrl %>">
    </iframe><%} %>--%>
</body>
</html>
