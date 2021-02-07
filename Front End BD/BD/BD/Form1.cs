using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace BD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:3000/api/login");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"email\":\"" + textBox1.Text.ToString() + "\"," +
                                  "\"parola\":\"" + textBox2.Text.ToString() + "\"}";

                    streamWriter.Write(json);
                }
                try
                {
                    string html = "";
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (Stream stream = httpResponse.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        html = reader.ReadToEnd();
                    }
                    Console.WriteLine("\r\nResponse Status Code is OK and StatusDescription is: {0}",
                                         httpResponse.StatusDescription);
                    textBox1.Text = "";
                    textBox2.Text = "";

                    Form2 otherForm = new Form2(Int32.Parse(html));
                    otherForm.FormClosed += new FormClosedEventHandler(otherForm_FormClosed);
                    this.Hide();
                    otherForm.Show();
                }
                catch
                {
                    
                    MessageBox.Show("Nu a functionat");
                }
            }
            catch { MessageBox.Show("Serverul este offline"); }


            /*
            
            */
        }

        void otherForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 otherForm = new Form3();
            otherForm.FormClosed += new FormClosedEventHandler(otherForm_FormClosed);
            this.Hide();
            otherForm.Show();
        }
    }
}
