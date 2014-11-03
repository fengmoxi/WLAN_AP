/** 
* 程序设计、编写者：风漠兮
* 作者博客：http://www.fengmx.com
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;

namespace MyWifi
{
    public partial class MainForm : Form
    {
       private void run()
        {
            for (int i = 2; i > 0; i++)
            {
                string cmdtext = "netsh wlan show hostednetwork";
                Process MyProcess = new Process();
                //设定程序名 
                MyProcess.StartInfo.FileName = "cmd.exe";
                //关闭Shell的使用 
                MyProcess.StartInfo.UseShellExecute = false;
                //重定向标准输入 
                MyProcess.StartInfo.RedirectStandardInput = true;
                //重定向标准输出 
                MyProcess.StartInfo.RedirectStandardOutput = true;
                //重定向错误输出 
                MyProcess.StartInfo.RedirectStandardError = true;
                //设置不显示窗口 
                MyProcess.StartInfo.CreateNoWindow = true;
                //执行VER命令 
                MyProcess.Start();
                MyProcess.StandardInput.WriteLine(cmdtext);
                MyProcess.StandardInput.WriteLine("exit");
                //从输出流获取命令执行结果， 
                //string exepath = Application.StartupPath; 
                //把返回的DOS信息读出来 
                String StrInfo = MyProcess.StandardOutput.ReadToEnd();
                StrInfo = StrInfo.Replace(" ", "");
                toolStripStatusLabel1.Text = "模式：" + StrInfo.Substring(StrInfo.LastIndexOf("模式:") + 3, StrInfo.LastIndexOf("SSID名称:") - StrInfo.LastIndexOf("模式:") - 2);
                toolStripStatusLabel2.Text = "状态：" + StrInfo.Substring(StrInfo.LastIndexOf("状态:") + 3, 3);
                toolStripStatusLabel3.Text = "SSID：" + StrInfo.Substring(StrInfo.LastIndexOf("SSID名称:“") + 8, StrInfo.LastIndexOf("”") - StrInfo.LastIndexOf("SSID名称:“") - 8);
                if (toolStripStatusLabel2.Text != "状态：已启动")
                {
                    toolStripStatusLabel4.Text = "已连接客户端数：0";
                }
                else
                {
                    toolStripStatusLabel4.Text = "已连接客户端数：" + StrInfo.Substring(StrInfo.LastIndexOf("客户端数:") + 5, 3);
                }
                Thread.Sleep(100);
            }
        }
        public MainForm()
        {
            InitializeComponent();  
        }

        public string executeCmd(string Command)
        {
            Process process = new Process
            {
                StartInfo = { FileName = " cmd.exe ", UseShellExecute = false, RedirectStandardInput = true, RedirectStandardOutput = true, CreateNoWindow = true }
            };
            process.Start();
            process.StandardInput.WriteLine(Command);
            process.StandardInput.WriteLine("exit");
            process.WaitForExit();
            string str = process.StandardOutput.ReadToEnd();
            process.Close();
            return str;
        }
        private void btnCreate_Click(object sender, EventArgs e)
        {
            if ((textName.Text == "") || (textPsw.Text == ""))
            {
                ListBoxLogs.AddCtrlValue(this, sysLogs, DateTime.Now.ToString("HH:mm:ss") + "---" + "用户名和密码均不能为空！");
            }
            else if (textPsw.Text.Length < 8)
            {
                ListBoxLogs.AddCtrlValue(this, sysLogs, DateTime.Now.ToString("HH:mm:ss") + "---" + "密码不能少于8位！");
            }
            else
            {
                string command = "netsh wlan set hostednetwork mode=allow ssid=" + textName.Text + " key=" + textPsw.Text;
                string str2 = executeCmd(command);
                if (((str2.IndexOf("承载网络模式已设置为允许") > -1) && (str2.IndexOf("已成功更改承载网络的 SSID。") > -1)) && (str2.IndexOf("已成功更改托管网络的用户密钥密码。") > -1))
                {
                    ListBoxLogs.AddCtrlValue(this, sysLogs, DateTime.Now.ToString("HH:mm:ss") + "---" + "新建共享网络成功！");
                }
                else
                {
                    ListBoxLogs.AddCtrlValue(this, sysLogs, DateTime.Now.ToString("HH:mm:ss") + "---" + "搭建失败，请重试！");
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string command = "netsh wlan set hostednetwork mode=disallow";
            if (executeCmd(command).IndexOf("承载网络模式已设置为禁止") > -1)
            {
                ListBoxLogs.AddCtrlValue(this, sysLogs, DateTime.Now.ToString("HH:mm:ss") + "---" + "禁止共享网络成功！");
            }
            else
            {
                ListBoxLogs.AddCtrlValue(this, sysLogs, DateTime.Now.ToString("HH:mm:ss") + "---" + "操作失败，请重试！");
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (executeCmd("netsh wlan start hostednetwork").IndexOf("已启动承载网络") > -1)
            {
                ListBoxLogs.AddCtrlValue(this, sysLogs, DateTime.Now.ToString("HH:mm:ss") + "---" + "已启动承载网络！");
            }
            else
            {
                ListBoxLogs.AddCtrlValue(this, sysLogs, DateTime.Now.ToString("HH:mm:ss") + "---" + "承载失败，请尝试新建网络共享！");
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (executeCmd("netsh wlan stop hostednetwork").IndexOf("已停止承载网络") > -1)
            {
                ListBoxLogs.AddCtrlValue(this, sysLogs, DateTime.Now.ToString("HH:mm:ss") + "---" + "已停止承载网络！");
            }
            else
            {
                ListBoxLogs.AddCtrlValue(this, sysLogs, DateTime.Now.ToString("HH:mm:ss") + "---" + "停止承载失败！");
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ListBoxLogs.AddCtrlValue(this, sysLogs, DateTime.Now.ToString("HH:mm:ss")+"---"+"欢迎使用本系统");
            thread1 = new Thread(new ThreadStart(run));
            thread1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            run();
        }
        private Thread thread1;

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thread1.IsAlive)
            {
                thread1.Abort();
            }
        }

        private void textName_TextChanged(object sender, EventArgs e)
        {
            string pat = @"[\u4e00-\u9fa5]";
            Regex rg = new Regex(pat);
            Match mh = rg.Match(textName.Text);
            if (!mh.Success)
                return;
            MessageBox.Show("暂不支持含中文字符的热点名称！","温馨提示");
            textName.Undo();
        }

        private void textPsw_TextChanged(object sender, EventArgs e)
        {
            string pat = @"[\u4e00-\u9fa5]";
            Regex rg = new Regex(pat);
            Match mh = rg.Match(textPsw.Text);
            if (!mh.Success)
                return;
            MessageBox.Show("热点密码不支持中文！","警告");
            textPsw.Undo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream("开启无线AP.bat", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write("@netsh wlan set hostednetwork mode=allow ssid=" + textName.Text + " key=" + textPsw.Text + "\r\n");
            sw.Write("@netsh wlan start hostednetwork");
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
            fs = new FileStream("禁用并关闭无线AP.bat", FileMode.Create);
            sw = new StreamWriter(fs);
            //开始写入
            sw.Write("@netsh wlan set hostednetwork mode=disallow\r\n");
            sw.Write("@netsh wlan stop hostednetwork");
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }    
    }
}
