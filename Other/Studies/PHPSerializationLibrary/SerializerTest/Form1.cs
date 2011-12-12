using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace SerializerTest
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class Form1 : System.Windows.Forms.Form
    {
        private Conversive.PHPSerializationLibrary.Serializer serializer;

        private System.Windows.Forms.TextBox serializeTextBox;
        private System.Windows.Forms.Button serializeButton;
        private System.Windows.Forms.Button deserializeButton;
        private System.Windows.Forms.GroupBox htGroupBox;
        private System.Windows.Forms.Label ssValueLabel;
        private System.Windows.Forms.TextBox ssValueTextBox;
        private System.Windows.Forms.TextBox ssKeyTextBox;
        private System.Windows.Forms.Label ssKeyLabel;
        private System.Windows.Forms.TextBox alKeyTextBox;
        private System.Windows.Forms.Label alKeyLabel;
        private System.Windows.Forms.Label alValuesLabel;
        private System.Windows.Forms.TextBox alValue1TextBox;
        private System.Windows.Forms.TextBox alValue2TextBox;
        private System.Windows.Forms.TextBox alValue3TextBox;
        private System.Windows.Forms.TextBox intKeyTextBox;
        private System.Windows.Forms.Label intKeyLabel;
        private System.Windows.Forms.TextBox intValueTextBox;
        private System.Windows.Forms.Label intValueLabel;
        private System.Windows.Forms.GroupBox ssGroupBox;
        private System.Windows.Forms.GroupBox alGroupBox;
        private System.Windows.Forms.GroupBox intGroupBox;
        private System.Windows.Forms.Button openButton1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Button button1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public Form1()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            serializer = new Conversive.PHPSerializationLibrary.Serializer();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serializeTextBox = new System.Windows.Forms.TextBox();
            this.serializeButton = new System.Windows.Forms.Button();
            this.deserializeButton = new System.Windows.Forms.Button();
            this.ssValueLabel = new System.Windows.Forms.Label();
            this.ssValueTextBox = new System.Windows.Forms.TextBox();
            this.htGroupBox = new System.Windows.Forms.GroupBox();
            this.intGroupBox = new System.Windows.Forms.GroupBox();
            this.intValueLabel = new System.Windows.Forms.Label();
            this.intKeyLabel = new System.Windows.Forms.Label();
            this.intValueTextBox = new System.Windows.Forms.TextBox();
            this.intKeyTextBox = new System.Windows.Forms.TextBox();
            this.alGroupBox = new System.Windows.Forms.GroupBox();
            this.alValuesLabel = new System.Windows.Forms.Label();
            this.alValue1TextBox = new System.Windows.Forms.TextBox();
            this.alKeyTextBox = new System.Windows.Forms.TextBox();
            this.alValue2TextBox = new System.Windows.Forms.TextBox();
            this.alKeyLabel = new System.Windows.Forms.Label();
            this.alValue3TextBox = new System.Windows.Forms.TextBox();
            this.ssGroupBox = new System.Windows.Forms.GroupBox();
            this.ssKeyLabel = new System.Windows.Forms.Label();
            this.ssKeyTextBox = new System.Windows.Forms.TextBox();
            this.openButton1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.htGroupBox.SuspendLayout();
            this.intGroupBox.SuspendLayout();
            this.alGroupBox.SuspendLayout();
            this.ssGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // serializeTextBox
            // 
            this.serializeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.serializeTextBox.Location = new System.Drawing.Point(278, 52);
            this.serializeTextBox.Multiline = true;
            this.serializeTextBox.Name = "serializeTextBox";
            this.serializeTextBox.Size = new System.Drawing.Size(263, 271);
            this.serializeTextBox.TabIndex = 0;
            // 
            // serializeButton
            // 
            this.serializeButton.Location = new System.Drawing.Point(10, 9);
            this.serializeButton.Name = "serializeButton";
            this.serializeButton.Size = new System.Drawing.Size(105, 24);
            this.serializeButton.TabIndex = 1;
            this.serializeButton.Text = "Serialize >>";
            this.serializeButton.Click += new System.EventHandler(this.serializeButton_Click);
            // 
            // deserializeButton
            // 
            this.deserializeButton.Location = new System.Drawing.Point(432, 9);
            this.deserializeButton.Name = "deserializeButton";
            this.deserializeButton.Size = new System.Drawing.Size(106, 24);
            this.deserializeButton.TabIndex = 2;
            this.deserializeButton.Text = "<< Deserialize";
            this.deserializeButton.Click += new System.EventHandler(this.deserializeButton_Click);
            // 
            // ssValueLabel
            // 
            this.ssValueLabel.Location = new System.Drawing.Point(125, 17);
            this.ssValueLabel.Name = "ssValueLabel";
            this.ssValueLabel.Size = new System.Drawing.Size(105, 17);
            this.ssValueLabel.TabIndex = 6;
            this.ssValueLabel.Text = "Value:";
            // 
            // ssValueTextBox
            // 
            this.ssValueTextBox.Location = new System.Drawing.Point(125, 34);
            this.ssValueTextBox.Name = "ssValueTextBox";
            this.ssValueTextBox.Size = new System.Drawing.Size(105, 21);
            this.ssValueTextBox.TabIndex = 7;
            this.ssValueTextBox.Text = "value";
            // 
            // htGroupBox
            // 
            this.htGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.htGroupBox.Controls.Add(this.intGroupBox);
            this.htGroupBox.Controls.Add(this.alGroupBox);
            this.htGroupBox.Controls.Add(this.ssGroupBox);
            this.htGroupBox.Location = new System.Drawing.Point(10, 43);
            this.htGroupBox.Name = "htGroupBox";
            this.htGroupBox.Size = new System.Drawing.Size(259, 280);
            this.htGroupBox.TabIndex = 8;
            this.htGroupBox.TabStop = false;
            this.htGroupBox.Text = "Hashtable";
            // 
            // intGroupBox
            // 
            this.intGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.intGroupBox.Controls.Add(this.intValueLabel);
            this.intGroupBox.Controls.Add(this.intKeyLabel);
            this.intGroupBox.Controls.Add(this.intValueTextBox);
            this.intGroupBox.Controls.Add(this.intKeyTextBox);
            this.intGroupBox.Location = new System.Drawing.Point(10, 202);
            this.intGroupBox.Name = "intGroupBox";
            this.intGroupBox.Size = new System.Drawing.Size(240, 69);
            this.intGroupBox.TabIndex = 25;
            this.intGroupBox.TabStop = false;
            this.intGroupBox.Text = "Integer";
            // 
            // intValueLabel
            // 
            this.intValueLabel.Location = new System.Drawing.Point(125, 17);
            this.intValueLabel.Name = "intValueLabel";
            this.intValueLabel.Size = new System.Drawing.Size(105, 17);
            this.intValueLabel.TabIndex = 19;
            this.intValueLabel.Text = "Value:";
            // 
            // intKeyLabel
            // 
            this.intKeyLabel.Location = new System.Drawing.Point(10, 17);
            this.intKeyLabel.Name = "intKeyLabel";
            this.intKeyLabel.Size = new System.Drawing.Size(105, 17);
            this.intKeyLabel.TabIndex = 22;
            this.intKeyLabel.Text = "Key:";
            // 
            // intValueTextBox
            // 
            this.intValueTextBox.Location = new System.Drawing.Point(125, 34);
            this.intValueTextBox.Name = "intValueTextBox";
            this.intValueTextBox.Size = new System.Drawing.Size(105, 21);
            this.intValueTextBox.TabIndex = 20;
            this.intValueTextBox.Text = "1";
            // 
            // intKeyTextBox
            // 
            this.intKeyTextBox.Location = new System.Drawing.Point(10, 34);
            this.intKeyTextBox.Name = "intKeyTextBox";
            this.intKeyTextBox.Size = new System.Drawing.Size(105, 21);
            this.intKeyTextBox.TabIndex = 21;
            this.intKeyTextBox.Text = "intkey";
            // 
            // alGroupBox
            // 
            this.alGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.alGroupBox.Controls.Add(this.alValuesLabel);
            this.alGroupBox.Controls.Add(this.alValue1TextBox);
            this.alGroupBox.Controls.Add(this.alKeyTextBox);
            this.alGroupBox.Controls.Add(this.alValue2TextBox);
            this.alGroupBox.Controls.Add(this.alKeyLabel);
            this.alGroupBox.Controls.Add(this.alValue3TextBox);
            this.alGroupBox.Location = new System.Drawing.Point(10, 95);
            this.alGroupBox.Name = "alGroupBox";
            this.alGroupBox.Size = new System.Drawing.Size(240, 98);
            this.alGroupBox.TabIndex = 24;
            this.alGroupBox.TabStop = false;
            this.alGroupBox.Text = "ArrayList";
            // 
            // alValuesLabel
            // 
            this.alValuesLabel.Location = new System.Drawing.Point(125, 17);
            this.alValuesLabel.Name = "alValuesLabel";
            this.alValuesLabel.Size = new System.Drawing.Size(105, 17);
            this.alValuesLabel.TabIndex = 14;
            this.alValuesLabel.Text = "Values:";
            // 
            // alValue1TextBox
            // 
            this.alValue1TextBox.Location = new System.Drawing.Point(125, 34);
            this.alValue1TextBox.Name = "alValue1TextBox";
            this.alValue1TextBox.Size = new System.Drawing.Size(105, 21);
            this.alValue1TextBox.TabIndex = 15;
            this.alValue1TextBox.Text = "value1";
            // 
            // alKeyTextBox
            // 
            this.alKeyTextBox.Location = new System.Drawing.Point(10, 34);
            this.alKeyTextBox.Name = "alKeyTextBox";
            this.alKeyTextBox.Size = new System.Drawing.Size(105, 21);
            this.alKeyTextBox.TabIndex = 12;
            this.alKeyTextBox.Text = "arraylistkey";
            // 
            // alValue2TextBox
            // 
            this.alValue2TextBox.Location = new System.Drawing.Point(125, 60);
            this.alValue2TextBox.Name = "alValue2TextBox";
            this.alValue2TextBox.Size = new System.Drawing.Size(105, 21);
            this.alValue2TextBox.TabIndex = 16;
            this.alValue2TextBox.Text = "value2";
            // 
            // alKeyLabel
            // 
            this.alKeyLabel.Location = new System.Drawing.Point(10, 17);
            this.alKeyLabel.Name = "alKeyLabel";
            this.alKeyLabel.Size = new System.Drawing.Size(105, 17);
            this.alKeyLabel.TabIndex = 13;
            this.alKeyLabel.Text = "Key:";
            // 
            // alValue3TextBox
            // 
            this.alValue3TextBox.Location = new System.Drawing.Point(125, 86);
            this.alValue3TextBox.Name = "alValue3TextBox";
            this.alValue3TextBox.Size = new System.Drawing.Size(105, 21);
            this.alValue3TextBox.TabIndex = 17;
            this.alValue3TextBox.Text = "value3";
            // 
            // ssGroupBox
            // 
            this.ssGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ssGroupBox.Controls.Add(this.ssValueTextBox);
            this.ssGroupBox.Controls.Add(this.ssValueLabel);
            this.ssGroupBox.Controls.Add(this.ssKeyLabel);
            this.ssGroupBox.Controls.Add(this.ssKeyTextBox);
            this.ssGroupBox.Location = new System.Drawing.Point(10, 17);
            this.ssGroupBox.Name = "ssGroupBox";
            this.ssGroupBox.Size = new System.Drawing.Size(240, 69);
            this.ssGroupBox.TabIndex = 23;
            this.ssGroupBox.TabStop = false;
            this.ssGroupBox.Text = "Single String";
            // 
            // ssKeyLabel
            // 
            this.ssKeyLabel.Location = new System.Drawing.Point(10, 17);
            this.ssKeyLabel.Name = "ssKeyLabel";
            this.ssKeyLabel.Size = new System.Drawing.Size(96, 17);
            this.ssKeyLabel.TabIndex = 11;
            this.ssKeyLabel.Text = "Key:";
            // 
            // ssKeyTextBox
            // 
            this.ssKeyTextBox.Location = new System.Drawing.Point(10, 34);
            this.ssKeyTextBox.Name = "ssKeyTextBox";
            this.ssKeyTextBox.Size = new System.Drawing.Size(105, 21);
            this.ssKeyTextBox.TabIndex = 10;
            this.ssKeyTextBox.Text = "stringkey";
            // 
            // openButton1
            // 
            this.openButton1.Location = new System.Drawing.Point(211, 9);
            this.openButton1.Name = "openButton1";
            this.openButton1.Size = new System.Drawing.Size(90, 24);
            this.openButton1.TabIndex = 9;
            this.openButton1.Text = "Open";
            this.openButton1.Click += new System.EventHandler(this.openButton1_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(338, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(550, 337);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.openButton1);
            this.Controls.Add(this.htGroupBox);
            this.Controls.Add(this.deserializeButton);
            this.Controls.Add(this.serializeButton);
            this.Controls.Add(this.serializeTextBox);
            this.Name = "Form1";
            this.Text = "Sharp Serialization Library Tester";
            this.htGroupBox.ResumeLayout(false);
            this.intGroupBox.ResumeLayout(false);
            this.intGroupBox.PerformLayout();
            this.alGroupBox.ResumeLayout(false);
            this.alGroupBox.PerformLayout();
            this.ssGroupBox.ResumeLayout(false);
            this.ssGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new Form1());
        }

        private void serializeButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                Hashtable ht = new Hashtable();
                ht[this.ssKeyTextBox.Text] = this.ssValueTextBox.Text;//single string
                ArrayList al = new ArrayList(3);
                al.Add(this.alValue1TextBox.Text);
                al.Add(this.alValue2TextBox.Text);
                al.Add(this.alValue3TextBox.Text);
                ht[this.alKeyTextBox.Text] = al;//ArrayList
                int i = Int32.Parse(this.intValueTextBox.Text);
                ht[this.intKeyTextBox.Text] = i;//int

                string stSerializedText = this.serializer.Serialize(ht); //Serialize the Hashtable
                this.serializeTextBox.Text = stSerializedText;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error Serializing: " + ex.Message, "Serialization Error");
            }
        }

        private void deserializeButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                Hashtable ht = (Hashtable)this.serializer.Deserialize(this.serializeTextBox.Text);//Deserialize the Hashtable
                this.ssValueTextBox.Text = (string)ht[this.ssKeyTextBox.Text];//single string
                ArrayList al = (ArrayList)ht[this.alKeyTextBox.Text];//ArrayList
                this.alValue1TextBox.Text = (string)al[0];
                this.alValue2TextBox.Text = (string)al[1];
                this.alValue3TextBox.Text = (string)al[2];
                this.intValueTextBox.Text = ht[this.intKeyTextBox.Text].ToString();//int
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error Deserializing: " + ex.Message, "Deserialization Error");
            }
        }

        private void openButton1_Click(object sender, System.EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(this.openFileDialog1.FileName/*, System.Text.Encoding.Unicode*/);
                string stSerial = sr.ReadToEnd();
                Hashtable htFile = (Hashtable)this.serializer.Deserialize(stSerial);
                sr.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] abc = System.Text.Encoding.GetEncoding("gb2312").GetBytes(this.serializeTextBox.Text);
                object obj = PHPSerializer.UnSerialize(abc, System.Text.Encoding.GetEncoding("gb2312"));
                //object obj = serializer.Deserialize(this.serializeTextBox.Text);
                Hashtable al = (Hashtable)obj;
                foreach (object o in al)
                {
                    if (o.GetType() == typeof(Hashtable))
                    {
                        Hashtable ht = (Hashtable)o;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(o.GetType() + ":" + o.ToString());
                    }
                }
                for (int i = 0; i < al.Count; i++)
                {
                    Hashtable ht = (Hashtable)al[i];
                    //do something
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error Deserializing: " + ex.Message, "Deserialization Error");
            }
        }
    }
}
