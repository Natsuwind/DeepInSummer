<%@ page language="C#" autoeventwireup="true" inherits="Acpm, App_Web_ru5tvrvv" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>�����޸�</title>
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
            �û���</td>
                    <td style="width: 400px; height: 21px">
                        <asp:TextBox ID="tb_name" runat="server" Width="139px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 105px">
                        �� ��</td>
                    <td style="width: 400px">
                        <asp:Button ID="bt_chg" runat="server" CausesValidation="False" OnClick="bt_chg_Click"
                            Text="�޸�����" Width="143px" />
                        <asp:Label ID="lb_chgmessage" runat="server" ForeColor="Red" Visible="False"></asp:Label><br />
                        <asp:Panel ID="pl_chg" runat="server" Visible="False">
                            <table style="width: 331px">
                                <tr>
                                    <td style="width: 46px; height: 21px">
                                        �� ��</td>
                                    <td style="width: 274px; height: 21px">
                                        <asp:TextBox ID="tb_pwd" runat="server" TextMode="Password" Visible="False"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tb_pwd"
                                            Display="Dynamic" ErrorMessage="����ѡ��"></asp:RequiredFieldValidator></td>
                                </tr>
                                <tr>
                                    <td style="width: 46px">
                        У ��</td>
                                    <td style="width: 274px">
                                        <asp:TextBox ID="tb_pwd2" runat="server" TextMode="Password" Visible="False"></asp:TextBox>
                        <asp:CompareValidator ID="CV_pwd" runat="server" ControlToCompare="tb_pwd" ControlToValidate="tb_pwd2"
                            ErrorMessage="2�����벻ƥ��" Display="Dynamic" Visible="False"></asp:CompareValidator><asp:RequiredFieldValidator
                                ID="RequiredFieldValidator2" runat="server" ControlToValidate="tb_pwd2" Display="Dynamic"
                                ErrorMessage="����ѡ��"></asp:RequiredFieldValidator></td>
                                </tr>
                                <tr>
                                    <td style="width: 46px">
                                    </td>
                                    <td style="width: 274px">
                                        <asp:Button ID="bt_chgpwd" runat="server" OnClick="bt_chgpwd_Click" Text="�޸�" Visible="False"
                                            Width="76px" /></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 105px; height: 21px">
                        ����</td>
                    <td style="width: 400px; height: 21px">
                        <asp:TextBox ID="tb_truename" runat="server" Width="115px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tb_truename"
                            ErrorMessage="����ѡ��"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td style="width: 105px; height: 21px">
                        Ժϵ</td>
                    <td style="width: 400px; height: 21px">
                        <asp:DropDownList ID="DDL_pro" runat="server" Width="122px">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>����ѧԺ</asp:ListItem>
                            <asp:ListItem>������ó��ѧԺ</asp:ListItem>
                            <asp:ListItem>Ӧ�ü���ѧԺ</asp:ListItem>
                            <asp:ListItem>�������ѧ�빤��ѧԺ</asp:ListItem>
                            <asp:ListItem>����ѧԺ</asp:ListItem>
                            <asp:ListItem>��������ѧѧԺ</asp:ListItem>
                            <asp:ListItem>���ѧԺ</asp:ListItem>
                            <asp:ListItem>������Ϣ���Զ���ѧԺ</asp:ListItem>
                            <asp:ListItem>���̹���ѧԺ</asp:ListItem>
                            <asp:ListItem>���Ͽ�ѧ�빤��ѧԺ</asp:ListItem>
                            <asp:ListItem>���﹤��ѧԺ</asp:ListItem>
                            <asp:ListItem>�����ѧԺ</asp:ListItem>
                            <asp:ListItem>��ó��ϢѧԺ</asp:ListItem>
                            <asp:ListItem>���˽���ѧԺ</asp:ListItem>
                            <asp:ListItem>����</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="DDL_pro"
                            ErrorMessage="����ѡ��"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td style="width: 105px; height: 21px">
                        ѧ��</td>
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
                            Display="Dynamic" ErrorMessage="��ʽ����" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator></td>
                </tr>
                <tr>
                    <td style="width: 105px">
                        ������ʾ����</td>
                    <td style="width: 400px">
                        <asp:TextBox ID="tb_ask" runat="server" Width="249px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 105px; height: 21px">
                        ������ʾ��</td>
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
                            Display="Dynamic" ErrorMessage="��������ȷ��QQ����" ValidationExpression="^\d{5,11}"></asp:RegularExpressionValidator></td>
                </tr>
                <tr>
                    <td style="width: 105px">
            MSN</td>
                    <td style="width: 400px">
                        <asp:TextBox ID="tb_msn" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tb_msn"
                            Display="Dynamic" ErrorMessage="��ʽ����" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator></td>
                </tr>
                <tr>
                    <td style="width: 105px">
        ���˽���</td>
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
            <asp:Button ID="bt_sub"   runat="server" OnClick="bt_ziliao_Click" Text="�ύ" Width="86px" /></td>
                </tr>
            </table>
        </center>
        </form>
    </div>
            
            <asp:Label ID="lb1" runat="server"></asp:Label><asp:Label ID="lb2" runat="server"></asp:Label>
    
</body>
</html>
