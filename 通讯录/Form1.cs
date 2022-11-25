using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace 通讯录
{
    public partial class frm : Form
    {
        public frm()
        {
            InitializeComponent();
        }
        string fileNameString = "";
        string[,] myPhones = new string[50, 3];
        int totalCountInteger = 0;
        private void frm_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
        }

        private void RefreshListBox()
        {
            if (totalCountInteger < 1) return;
            listBox1.Items.Clear();
            for (int i = 0; i < totalCountInteger; i++)
                listBox1.Items.Add(myPhones[i, 0]);
            num.Text = totalCountInteger.ToString();
            groupBox1.Enabled = true;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ofd.InitialDirectory = Directory.GetCurrentDirectory();
            ofd.Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*";
            ofd.CheckFileExists = false;
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.Cancel)
                return;

            fileNameString = ofd.FileName;

            this.Text = "通讯录 - " + fileNameString;
            myPhones[0, 0] = "（新增）";
            totalCountInteger = 1;
            RefreshListBox();
            listBox1.SelectedIndex = 0;
            nametxt.Focus();
            //idtxt.Enabled = true;
            sextxt.Enabled = false;
            birthtxt.Enabled = false;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            birthtxt.Enabled = false;
            sextxt.Enabled = false;
            ofd.InitialDirectory = Directory.GetCurrentDirectory();
            ofd.Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*";
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.Cancel)
                return;
            fileNameString = ofd.FileName;
            this.Text = "通讯录 - " + fileNameString;
            String[] onePhone = new string[3];
            string tempString;
            totalCountInteger = 0;
            StreamReader phoneStreamReader = new StreamReader(fileNameString);
            while (phoneStreamReader.Peek() != -1)
            {
                tempString = phoneStreamReader.ReadLine();
                onePhone = tempString.Split(',');
                myPhones[totalCountInteger, 0] = onePhone[0];
                myPhones[totalCountInteger, 1] = onePhone[1];
                myPhones[totalCountInteger, 2] = onePhone[2];
                totalCountInteger ++;
            }
            phoneStreamReader.Close();
            if (totalCountInteger < 1)
            {
                myPhones[0, 0] = "(新增)";
                nametxt.Text = "(新增)";
                totalCountInteger = 1;
            }
            RefreshListBox();
            listBox1.SelectedIndex = 0;

            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
        }

        private void SavePhoneInformation(string fileName)
        {
            string str;
            str = Regex.Replace(idtxt.Text, @"[^0-9]+", "");
            if (str.Length != 18)
            {
                MessageBox.Show("身份证号不是18位，请重新输入！");
                return;
            }
            StreamWriter sw = new StreamWriter(fileName);
            int i;
            for (i = 0; i < totalCountInteger; i++ )
            {
                str = Regex.Replace(myPhones[i, 2], @"[^0-9]+","");
                string s = myPhones[i, 0] + "," + myPhones[i, 1] + "," + str;
                sw.WriteLine(s, true);
            }
            sw.Close();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sfd.InitialDirectory = Directory.GetCurrentDirectory();
            sfd.OverwritePrompt = true;
            ofd.Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                fileNameString = sfd.FileName;
                SavePhoneInformation(fileNameString);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i < totalCountInteger; i++ )
            {
                myPhones[i, 0] = "";
                myPhones[i, 1] = "";
                myPhones[i, 2] = "";
            }
            birthtxt.Text = null;
            nametxt.Text = null;
            phonetxt.Text = null;
            sextxt.Text = null;
            idtxt.Text = null;
            totalCountInteger = 0;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("是否结束程序运行", "操作提示",
                MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (dr == DialogResult.Yes)
                Application.Exit();
            else
                return;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SavePhoneInformation(fileNameString);
        }

        private void SetSexBirthByID()
        {
            string str;
            str = Regex.Replace(idtxt.Text, @"[^0-9]+", "");
            if (str.Length != 18)
                return;
            string s = idtxt.Text;
            string sBirth, sSex="";
            int sSexmath;
            sBirth = s.Substring(7, 4)+"-"+ s.Substring(11, 2) +"-"+s.Substring(13, 2);
            sSexmath = int.Parse(s.Substring(18, 1));
            birthtxt.Text = sBirth;
            if (sSexmath % 2 == 0)
                sSex = "女";
            else
                sSex = "男";
            sextxt.Text = sSex;
            
        }

        private void appendToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (fileNameString == "")
                this.openToolStripMenuItem_Click(null, null);

            myPhones[totalCountInteger, 0] = "(新增)";
            myPhones[totalCountInteger, 1] = "";
            myPhones[totalCountInteger, 2] = "";
            listBox1.SelectedIndex = totalCountInteger - 1;
            totalCountInteger++;
            RefreshListBox();
        }

        private void insertToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (fileNameString == "")
                this.openToolStripMenuItem_Click(null, null);
            int j;
            int i = listBox1.SelectedIndex;
            for (j = totalCountInteger; j > i; j--)
            {
                myPhones[j, 0] = myPhones[j - 1, 0];
                myPhones[j, 1] = myPhones[j - 1, 1];
                myPhones[j, 2] = myPhones[j - 1, 2];
            }
            myPhones[i, 0] = "(新增)";
            myPhones[i, 1] = "";
            myPhones[i, 2] = "";
            totalCountInteger++;
            RefreshListBox();
        }

        private void deleteToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            sextxt.Enabled = false;
            birthtxt.Enabled = false;
            if (fileNameString == "")
                this.openToolStripMenuItem_Click(null, null);
            int i;
            int j = listBox1.SelectedIndex;
            for (i = j; i < totalCountInteger; i++)
            {
                myPhones[i, 0] = myPhones[i + 1, 0];
                myPhones[i, 1] = myPhones[i + 1, 1];
                myPhones[i, 2] = myPhones[i + 1, 2];
            }
            totalCountInteger--;
            RefreshListBox();
        }

        private void aboutToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("刘祖浩版权所有！");
        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            int indexInteger;
            indexInteger = listBox1.SelectedIndex;
            if (indexInteger < 0)
            {
                groupBox2.Enabled = false;
                groupBox3.Enabled = false;
            }
            else
            {
                groupBox2.Enabled = true;
                groupBox3.Enabled = true;
                nametxt.Text = myPhones[indexInteger, 0];
                phonetxt.Text = myPhones[indexInteger, 1];
                idtxt.Text = myPhones[indexInteger, 2];
                SetSexBirthByID();
            }
        }

        private void nametxt_TextChanged_1(object sender, EventArgs e)
        {
            if (totalCountInteger == 0)
                return;
            int i = listBox1.SelectedIndex;
            myPhones[i, 0] = nametxt.Text;
            RefreshListBox();
            listBox1.SelectedIndex = i;
        }

        private void phonetxt_TextChanged(object sender, EventArgs e)
        {
            int i = listBox1.SelectedIndex;
            myPhones[i, 1] = phonetxt.Text;
            listBox1.SelectedIndex = i;
        }

        private void idtxt_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            string str;
            str = Regex.Replace(idtxt.Text, @"[^0-9]+", "");
            if (str.Length == 18)
            {
                int i = listBox1.SelectedIndex;
                myPhones[i, 2] = str;
                SetSexBirthByID();
                listBox1.SelectedIndex = i;
            }
            else
                return;
        }
    }
}



