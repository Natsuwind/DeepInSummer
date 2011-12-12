<%@ page language="C#" autoeventwireup="true" inherits="Acpm, App_Web_ru5tvrvv" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>资料修改</title>
</head>
<body>
    <div>
        <form id="form1" runat="server">
            <center>
                &nbsp;</center>
        <center>
            &nbsp;<table style="vertical-align: middle; width: 497px; text-align: left">
                <tr>
                    <td style="width: 105px; height: 21px">
            用户名</td>
                    <td style="width: 400px; height: 21px">
                        <asp:TextBox ID="tb_name" runat="server" Width="139px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 105px">
                        密 码</td>
                    <td style="width: 400px">
                        <asp:Button ID="bt_chg" runat="server" CausesValidation="False" OnClick="bt_chg_Click"
                            Text="修改密码" Width="143px" />
                        <asp:Label ID="lb_chgmessage" runat="server" ForeColor="Red" Visible="False"></asp:Label><br />
                        <asp:Panel ID="pl_chg" runat="server" Visible="False">
                            <table style="width: 331px">
                                <tr>
                                    <td style="width: 46px; height: 21px">
                                        密 码</td>
                                    <td style="width: 274px; height: 21px">
                                        <asp:TextBox ID="tb_pwd" runat="server" TextMode="Password" Visible="False"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tb_pwd"
                                            Display="Dynamic" ErrorMessage="必填选项"></asp:RequiredFieldValidator></td>
                                </tr>
                                <tr>
                                    <td style="width: 46px">
                        校 验</td>
                                    <td style="width: 274px">
                                        <asp:TextBox ID="tb_pwd2" runat="server" TextMode="Password" Visible="False"></asp:TextBox>
                        <asp:CompareValidator ID="CV_pwd" runat="server" ControlToCompare="tb_pwd" ControlToValidate="tb_pwd2"
                            ErrorMessage="2次输入不匹配" Display="Dynamic" Visible="False"></asp:CompareValidator><asp:RequiredFieldValidator
                                ID="RequiredFieldValidator2" runat="server" ControlToValidate="tb_pwd2" Display="Dynamic"
                                ErrorMessage="必填选项"></asp:RequiredFieldValidator></td>
                                </tr>
                                <tr>
                                    <td style="width: 46px">
                                    </td>
                                    <td style="width: 274px">
                                        <asp:Button ID="bt_chgpwd" runat="server" OnClick="bt_chgpwd_Click" Text="修改" Visible="False"
                                            Width="76px" /></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 105px; height: 21px">
                        姓名</td>
                    <td style="width: 400px; height: 21px">
                        <asp:TextBox ID="tb_truename" runat="server" Width="115px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tb_truename"
                            ErrorMessage="必填选项"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td style="width: 105px; height: 21px">
                        院系</td>
                    <td style="width: 400px; height: 21px">
                        <asp:DropDownList ID="DDL_pro" runat="server" Width="122px">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>数理学院</asp:ListItem>
                            <asp:ListItem>经济与贸易学院</asp:ListItem>
                            <asp:ListItem>应用技术学院</asp:ListItem>
                            <asp:ListItem>计算机科学与工程学院</asp:ListItem>
                            <asp:ListItem>汽车学院</asp:ListItem>
                            <asp:ListItem>人文社会科学学院</asp:ListItem>
                            <asp:ListItem>会计学院</asp:ListItem>
                            <asp:ListItem>电子信息与自动化学院</asp:ListItem>
                            <asp:ListItem>工商管理学院</asp:ListItem>
                            <asp:ListItem>材料科学与工程学院</asp:ListItem>
                            <asp:ListItem>生物工程学院</asp:ListItem>
                            <asp:ListItem>外国语学院</asp:ListItem>
                            <asp:ListItem>商贸信息学院</asp:ListItem>
                            <asp:ListItem>成人教育学院</asp:ListItem>
                            <asp:ListItem>其他</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="DDL_pro"
                            ErrorMessage="必填选项"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td style="width: 105px; height: 21px">
                        学号</td>
                    <td style="width: 400px; height: 21px">                    
                        <asp:Label ID="tb_no" runat="server"></asp:Label>
                        </td>
                </tr>
                <tr>
                    <td style="width: 105px; height: 21px">
            E_mail</td>
                    <td style="width: 400px; height: 21px">
                        <asp:TextBox ID="tb_email" runat="server" Width="250px"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="REV_email" runat="server" ControlToValidate="tb_email"
                            Display="Dynamic" ErrorMessage="格式错误" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator></td>
                </tr>
                <tr>
                    <td style="width: 105px">
                        密码提示问题</td>
                    <td style="width: 400px">
                        <asp:TextBox ID="tb_ask" runat="server" Width="249px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 105px; height: 21px">
                        密码提示答案</td>
                    <td style="width: 400px; height: 21px">
                        <asp:TextBox ID="tb_ans" runat="server" Width="249px"></asp:TextBox>
                        <input type="hidden" id="Hidden1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 105px; height: 26px">
            QQ</td>
                    <td style="width: 400px; height: 26px">
                        <asp:TextBox ID="tb_qq" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="tb_qq"
                            Display="Dynamic" ErrorMessage="请输入正确的QQ号码" ValidationExpression="^\d{5,11}"></asp:RegularExpressionValidator></td>
                </tr>
                <tr>
                    <td style="width: 105px">
            MSN</td>
                    <td style="width: 400px">
                        <asp:TextBox ID="tb_msn" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tb_msn"
                            Display="Dynamic" ErrorMessage="格式错误" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator></td>
                </tr>
                <tr>
                    <td style="width: 105px">
        个人介绍</td>
                    <td style="width: 400px">
                        <asp:TextBox ID="tb_message" runat="server" TextMode="MultiLine" Height="57px" Width="269px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 105px">
        </td>
                    <td style="width: 400px">
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
            <asp:Button ID="bt_sub"   runat="server" OnClick="bt_ziliao_Click" Text="提交" Width="86px" /></td>
                </tr>
            </table>
        </center>
        </form>
    </div>
            
            <asp:Label ID="lb1" runat="server"></asp:Label><asp:Label ID="lb2" runat="server"></asp:Label>
    
</body>
</html>
