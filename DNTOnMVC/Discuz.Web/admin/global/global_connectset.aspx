<%@ Page Language="c#" Inherits="Discuz.Web.Admin.connectset"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
    <head>
        <title>QQ登录设置</title>
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
    <%if (upload == 0)
      { %>
        <div class="ManagerForm">
            <fieldset>
                <legend style="background:url(../images/icons/legendimg.jpg) no-repeat 6px 50%;">QQ登录设置</legend>
                <form id="connectset" method="post" action="global_connectset.aspx?save=1">
                    <table width="100%">
                        <tr><td class="item_title" colspan="2">是否开启QQ注册</td></tr>
                        <tr>
                            <td class="vtop rowform">
                                <span class="buttonlist" id="enableqqreg">
                                    <input name="enablereg" id="enablereg_1" type="radio" value="1"/>
                                    <label for="enablereg_1">是</label>
                                    <input name="enablereg" id="enablereg_0" type="radio" value="0"/>
                                    <label for="enablereg_0">否</label>
                                </span>
                            </td>
                            <td class="vtop">选择"是"将允许首次使用QQ登录的用户自动注册一个论坛帐号和QQ关联</td>
                        </tr>
                        <tr><td class="item_title" colspan="2">是否允许论坛读取QQ用户的Qzone头像</td></tr>
                        <tr>
                            <td class="vtop rowform">
                                <span class="buttonlist" id="enablecatchqzavatar">
                                    <input name="enablecatchqzavatar" id="enablecatchqzavatar_1" type="radio" value="1"/>
                                    <label for="enablecatchqzavatar_1">是</label>
                                    <input name="enablecatchqzavatar" id="enablecatchqzavatar_0" type="radio" value="0"/>
                                    <label for="enablecatchqzavatar_0">否</label>
                                </span>
                            </td>
                            <td class="vtop">选择"是"将允许用户将论坛的头像设置为Qzone的头像,因为该功能需要抓取远程图像资源,可能会有少许性能损耗</td>
                        </tr>
                        <tr><td class="item_title" colspan="2">允许每个QQ号最多注册的用户数量</td></tr>
                        <tr>
                            <td class="vtop rowform">
                                <input name="maxbindcount" id="maxbindcount" type="text" value="<%=config.Maxuserbindcount %>" size="3"/>
                            </td>
                            <td class="vtop">该设置可以限制某QQ号码多次解绑并重复注册新的论坛用户次数,0为不限制</td>
                        </tr>
                        <tr><td class="item_title" colspan="2">上传站点LOGO</td></tr>
                        <tr>
                            <td class="vtop rowform">
                                <a href="global_connectset.aspx?upload=1">上传logo</a>
                            </td>
                            <td class="vtop">点击链接进入上传站点logo的页面</td>
                        </tr>
                    </table>
                    <script type="text/javascript">
                        var enablereg = <%=config.Allowconnectregister %>;
                        var enablecatchqzavatar = <%=config.Allowuseqzavater %>;

                        $('#enablereg_' + enablereg).attr('checked','checked');
                        $('#enablecatchqzavatar_' + enablecatchqzavatar).attr('checked','checked');
                    </script>
                    <div class="Navbutton">
                        <button class="ManagerButton" id="saveset" type="submit"><img alt="" src="../images/submit.gif"/>提交</button>
                    </div>
                </form>
            </fieldset>
        </div>
        <%}
      else
      { %>
      <iframe  frameborder="0" width="810px" scrolling="no" height="810px" src="<%=uploadLogoUrl %>"></iframe>
        <%} %>
    </body>
</html>