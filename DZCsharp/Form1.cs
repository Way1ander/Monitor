﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
//Commit

namespace DZCsharp
{
    public partial class Form1 : Form
    {
        Int32 j = 0;
        StringBuilder sb;
        StreamWriter sw;
        Int32 pId1;
        Int32 pID;
        int ms, min, sec, sum, sum0 = 0, minS, min1 = 0;
        public Form1()
        {
            InitializeComponent();
        }
        Process[] processes;
        private void Form1_Load(object sender, EventArgs e)
        {
            timer2.Start();
            timer1.Start();
            sw = new StreamWriter(@"F:\DZCsharp\DZCsharp\X.txt");
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern UInt32 GetWindowThreadProcessId(Int32 hWnd, out Int32 lpdwProcessId);
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        public static extern Int32 GetForegroundWindow();


        private void timer2_Tick(object sender, EventArgs e)
        {
            /* DateTime one = DateTime.Now;
             DateTime two = one + Convert.ToDatetime
             TimeSpan result = one+100;
             MessageBox.Show(result.Seconds.ToString());*/

            Int32 k = GetForegroundWindow(); //Дескриптор окна в форграунде
            if (j == k)
            {
                ms += 1;
                if (ms == 10) { ms = 0; sec += 1; }
                if (sec == 60) { sec = 0; min += 1; }
                label1.Text = "Current process in foreground time " + min + ":" + sec + ":" + ms;
                sum = min * 60000 + sec * 1000 + ms;
                if (sum > sum0)
                {
                    sum0 = sum;

                    GetWindowThreadProcessId(k, out pId1);
                    label5.Text = "The longest time in foreground. Process: " +
                        Convert.ToString(Process.GetProcessById(pId1)) + " Time: "
                        + min + ":" + sec + ":" + ms;
                }
            }
            else
            {
                ms = 0; sec = 0; min = 0;
                sb = new StringBuilder();

                //   sb.Append(string.Format("{0}\t\n", label2.Text));
                sb.Append(string.Format("{0}\t\n", label3.Text));
                sb.Append(string.Format("{0}\t\n", label1.Text));
                sw.WriteLine(sb.ToString());

            }
            j = k;

            GetWindowThreadProcessId(k, out pID); // ID процесса, который создал окно в foreground
            String lv7 = Convert.ToString(pID);
            String lv8 = Convert.ToString(Process.GetProcessById(pID)); //Название процесса в форграунде 
            //    label2.Text = "The foreground window handle: " + Convert.ToString(k);
            label3.Text = "ID and ProcessName in foreground: " + lv7 + " " + lv8;


        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            listView1.Items.Clear();



            processes = Process.GetProcesses();
            foreach (Process instance in processes)
            {
                try
                {
                    String lv1 = instance.ProcessName;
                    String lv2 = Convert.ToString(instance.Id);
                    String lv3 = Convert.ToString(instance.StartTime);
                    String lv4 = Convert.ToString(instance.TotalProcessorTime);
                    if (pID == Convert.ToInt32(instance.Id))
                    {
                        minS = min + min1;

                        String lv5 = Convert.ToString(min + ":" + sec + ":" + ms);
                        ListViewItem item = new ListViewItem(new string[] { lv1, lv2, lv3, lv4, lv5 });
                        listView1.Items.Add(item);
                    }
                    else
                    {

                        ListViewItem item = new ListViewItem(new string[] { lv1, lv2, lv3, lv4 });
                        listView1.Items.Add(item);
                        min1 = minS;
                    }

                }
                catch (Exception ex)
                { }
            }
            label4.Text = "Number of processes : " + processes.Length;
            //Сохранение
            if (listView1.Items.Count > 0)
            {


                foreach (ListViewItem lvi in listView1.Items)
                {
                    sb = new StringBuilder();

                    foreach (ListViewItem.ListViewSubItem listViewSubItem in lvi.SubItems)
                    {
                        sb.Append(string.Format("{0}\t", listViewSubItem.Text));
                    }
                    sw.WriteLine(sb.ToString());
                }
                sw.WriteLine();
            }
            timer1.Interval = 4000;
        }
    }

}
