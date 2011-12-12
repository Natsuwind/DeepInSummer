<%@ Page Language="C#" MasterPageFile="~/include/itca.Master" AutoEventWireup="true"
    Inherits="reg, App_Web_s0wfswid" Title="注册 - 留言板 - iTCA 重庆工学院计算机协会" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <center>
        <asp:Panel ID="Pl_agree" runat="server">
            <table style="width: 497px; height: 450px; text-align: left">
                <tr style="height: 10%">
                    <td style="width: 493px; height: 2px; text-align: center">
                        <strong style="font-size: larger">注册协议</strong></td>
                </tr>
                <tr>
                    <td style="width: 493px; height: 80%; vertical-align: top">
                        <iframe src="agree.aspx" style="width: 490px; height: 380px"></iframe>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; width: 493px; height: 10%">
                        <asp:Button ID="bt_gree" runat="server" OnClick="bt_gree_Click" Text="我同意" Width="86px" />
                        <asp:Button ID="bt_cancel" runat="server" OnClick="bt_cancel_Click" Text="我不同意" Width="86px" /></td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pl_reg" runat="server" Visible="False">
            <table style="vertical-align: middle; width: 497px; height: 450px; text-align: left">
                <tr>
                    <td style="width: 105px; height: 21px">
                        验证码</td>
                    <td style="width: 400px; height: 21px">
                        <asp:TextBox ID="tb_code" runat="server" Width="115px" ToolTip="区分大小写"></asp:TextBox><img
                            src="../code.aspx" alt="区分大小写" /></td>
                </tr>
                <tr>
                    <td style="width: 105px; height: 21px">
                        用户名</td>
                    <td style="width: 400px; height: 21px">
                        <asp:TextBox ID="tb_name" runat="server" Width="115px"></asp:TextBox>
                        *<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                            ControlToValidate="tb_name" Display="Dynamic" ErrorMessage="重新输入,4-16字母(中文2-8字)"
                            ValidationExpression="^\w{4,16}|^\W{2,8}"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tb_name"
                            ErrorMessage="必填选项"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td style="width: 105px; height: 28px;">
                        密 码</td>
                    <td style="width: 400px; height: 28px;">
                        <asp:TextBox ID="tb_pwd" runat="server" TextMode="Password" Width="115px"></asp:TextBox>
                        *<asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                            ControlToValidate="tb_pwd" Display="Dynamic" ErrorMessage="密码长度6-18字符" ValidationExpression="^\w{6,18}"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tb_pwd"
                            ErrorMessage="必填选项"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td style="width: 105px; height: 28px">
                        重复密码</td>
                    <td style="width: 400px; height: 28px">
                        <asp:TextBox ID="tb_pwd2" runat="server" TextMode="Password" Width="115px"></asp:TextBox>
                        *<asp:CompareValidator ID="CV_pwd" runat="server" ControlToCompare="tb_pwd" ControlToValidate="tb_pwd2"
                            ErrorMessage="2次输入不匹配" Display="Dynamic"></asp:CompareValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tb_pwd2"
                            ErrorMessage="必填选项"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td style="width: 105px; height: 21px">
                        姓名</td>
                    <td style="width: 400px; height: 21px">
                        <asp:TextBox ID="tb_truename" runat="server" Width="115px" MaxLength="4"></asp:TextBox>
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
                        E_mail</td>
                    <td style="width: 400px; height: 21px">
                        <asp:TextBox ID="tb_email" runat="server" Width="229px"></asp:TextBox>
                        *<asp:RegularExpressionValidator ID="REV_email" runat="server" ControlToValidate="tb_email"
                            Display="Dynamic" ErrorMessage="格式错误" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="RFV4" runat="server" ControlToValidate="tb_email"
                            ErrorMessage="E_mail必填"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td style="width: 105px; height: 26px;">
                        密码提示问题</td>
                    <td style="width: 400px; height: 26px;">
                        <asp:TextBox ID="tb_ask" runat="server" Width="229px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RFV5" runat="server" ControlToValidate="tb_ask" ErrorMessage="请填写提示问题"
                            Enabled="False" Visible="False"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td style="width: 105px; height: 21px">
                        密码提示答案</td>
                    <td style="width: 400px; height: 21px">
                        <asp:TextBox ID="tb_ans" runat="server" Width="229px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RFV6" runat="server" ControlToValidate="tb_ans" ErrorMessage="请填写提示答案"
                            Enabled="False" Visible="False"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td style="width: 105px; height: 26px">
                        QQ</td>
                    <td style="width: 400px; height: 26px">
                        <asp:TextBox ID="tb_qq" runat="server" Width="145px"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="tb_qq"
                            Display="Dynamic" ErrorMessage="请输入正确的QQ号码" ValidationExpression="^\d{5,11}"></asp:RegularExpressionValidator></td>
                </tr>
                <tr>
                    <td style="width: 105px">
                        MSN</td>
                    <td style="width: 400px">
                        <asp:TextBox ID="tb_msn" runat="server" Width="145px"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tb_msn"
                            Display="Dynamic" ErrorMessage="格式错误" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator></td>
                </tr>
                <tr>
                    <td style="width: 105px; height: 73px">
                        个人介绍</td>
                    <td style="width: 400px; height: 73px">
                        <asp:TextBox ID="tb_message" runat="server" TextMode="MultiLine" Height="68px" Width="250px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="height: 16px;" colspan="2">
                        为了便于网站管理,请输入<span style="color: #ff0000">数字化校园帐号</span>进行验证</td>
                </tr>
                <tr>
                    <td style="width: 105px; height: 16px">
                        帐号</td>
                    <td style="width: 400px; height: 16px">
                        <asp:TextBox ID="tb_dcname" runat="server" MaxLength="12" Width="115px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 105px; height: 16px">
                        密码</td>
                    <td style="width: 400px; height: 16px">
                        <asp:TextBox ID="tb_dcpwd" runat="server" MaxLength="12" TextMode="Password" Width="115px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="bt_reg" runat="server" OnClick="bt_reg_Click" Text="注册" Width="86px" /></td>
                </tr>
            </table>
        </asp:Panel>
    </center>
</asp:Content>
