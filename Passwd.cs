/** 
* 程序设计、编写者：风漠兮
* 作者博客：http://www.fengmx.com
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyWifi
{
    public partial class Passwd : Form
    {
        Point mouse_offset;
        public Passwd()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "rightpassword")//正确密码
            {
                MessageBox.Show("身份已确定");
                this.Close();
            }
            else
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("特喵的，你丫不输密码是做森么?_?");

                }
                else
                {
                    MessageBox.Show("逗比，密码错了，你造嘛！");
                }
            }
        }

        private void Passwd_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_offset = new Point(-e.X, -e.Y);
        }

        private void Passwd_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouse_offset.X, mouse_offset.Y);
                Location = mousePos;
            }
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouse_offset.X, mouse_offset.Y);
                Location = mousePos;
            }
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_offset = new Point(-e.X, -e.Y);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox1.Text == "rightpassword")//正确密码
                {
                    MessageBox.Show("身份已确定");
                    this.Close();
                }
                else
                {
                    if (textBox1.Text == "")
                    {
                        MessageBox.Show("特喵的，你丫不输密码是做森么?_?");

                    }
                    else
                    {
                        MessageBox.Show("逗比，密码错了，你造嘛！");
                    }
                }
            }
        }        
    }
}
