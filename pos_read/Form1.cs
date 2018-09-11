using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Collections;
using System.Diagnostics;

namespace pos_read
{
    public partial class Form1 : Form
    {
        public static Form1 fm;
        public Form1()
        {

            InitializeComponent();
            fm = this;
        }

        String str = "";
       // int j = 1;
        string trns_no = "";
        string reg_id = "";
        string time = "";
        string hot_key = "";


        ArrayList hot_arr = new ArrayList();
        ArrayList trim_arr = new ArrayList();
        

        delegate void SetRichTextValue(object sender, SerialDataReceivedEventArgs e);
        SerialPort sp = null;
        private void button1_Click(object sender, EventArgs e)
        {
            hot_arr.Add("monst");
            hot_arr.Add("monst spec");
            hot_arr.Add("monst spec coffee");

            sp = new SerialPort();
            sp.PortName = textBox1.Text;
            sp.BaudRate = 9600;
            sp.StopBits = StopBits.One;
            sp.Parity = Parity.None;
            sp.DataBits = 8;
            sp.Open();

            
                sp.DataReceived += new SerialDataReceivedEventHandler(pos_datarecived);
            
            
        }

        private void pos_datarecived(object sender, SerialDataReceivedEventArgs e)
        {

                //ArrayList arr = new ArrayList();
                int iBytesToRead = fm.sp.BytesToRead;
                byte[] comBuffer = new byte[iBytesToRead];
                int i;
                // byte[] buffer = new byte[readByte];

                    
                    fm.sp.Read(comBuffer, 0, iBytesToRead);


               Thread.Sleep(50);
                foreach (byte ch in comBuffer)
                {
                    

                        if (fm.richTextBox1.InvokeRequired)
                        {

                            SetRichTextValue st = new SetRichTextValue(pos_datarecived);
                            fm.Invoke(st, new object[] { sender, e });

                        }
                        else
                        {
                           
                           // fm.richTextBox1.Text += (char)ch;
                            str += (char)ch;
                            
                        }

                       
                }





                if (str != "") {

                    String[] sp_arr = str.Trim().Split();
                    //string an_s = "";

                    foreach (string s2 in sp_arr)
                    {
                        if (s2 != " " && s2 != null && s2!="") {

                            trim_arr.Add(s2);
                            
                        }
                    }
                    //string[] sp_arr2 = an_s.Split();
                    for (i = 0; i < trim_arr.Count; i++)
                    {
                            //richTextBox1.Text += "[" + i + "]:" + sp_arr[i] + "\n";
                            //Debug.Write("[" + i + "]:" + trim_arr[i].ToString() + "\n");
                            if (sp_arr[i].ToString() == "!")
                            {
                                time = trim_arr[i].ToString() + " "+trim_arr[i+1].ToString();
                                
                                //break;
                            }

                            if (trim_arr[i].ToString() == "TRAN#")
                            {
                                trns_no = trim_arr[i+1].ToString();
                                break;
                                
                            }

                        

                    }

                   foreach (string s in hot_arr) {



                        if (str.ToLower().Contains(s)) {

                            hot_key = s ;
                        }
                    }

                   if (trns_no != "") {
                       reg_id = trns_no.Substring(0, 3);
                   }

                   Console.Write(time);
                    Console.Write(trns_no + "\n");
                    Console.Write(hot_key + "\n");
                    Console.Write(reg_id + "\n");
                    fm.richTextBox1.Text += time+" "+trns_no+" "+reg_id+" "+hot_key+"\n";
                    str = "";
                    Thread.Sleep(100);
                string query = "insert into posData values('" + trns_no + "','" + reg_id + "','" + hot_key + "','" + time + "');";
                DB.ExecuteNonQuery(query);
                
                    trim_arr.Clear();
                }
                
                
                
                    
            
                
        }
    }
}
