using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using P2PLibrary;

namespace GUIServer
{
    public partial class frmServer : Form
    {
        private Server _server;

        public frmServer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _server = new Server();
            _server.OnWriteLog += new WriteLogHandle(server_OnWriteLog);
            _server.OnUserChanged += new UserChangedHandle(OnUserChanged);
            try
            {
                _server.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //ˢ���û��б�
        private void OnUserChanged(UserCollection users)
        {
            listBox2.DisplayMember = "FullName";
            listBox2.DataSource = null;
            listBox2.DataSource = users;
        }

        //��ʾ������Ϣ
        public void server_OnWriteLog(string msg)
        {
            listBox1.Items.Add(msg);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_server != null)
                _server.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //������Ϣ�����������û�
            P2P_TalkMessage msg = new P2P_TalkMessage(textBox1.Text);
            foreach (object o in listBox2.Items)
            {
                User user = o as User;
                _server.SendMessage(msg, user.NetPoint);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }
    }
}