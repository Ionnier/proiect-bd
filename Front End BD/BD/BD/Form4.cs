﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BD
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            populateDGV();
        }
        public class Utilizator
        {
            public int id { get; set; }
            public string prenume { get; set; }
            public string nume { get; set; }
            public string email { get; set; }
            public string parola { get; set; }
            public string data_nastere { get; set; }
        }

        public static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
        public void populateDGV()
        {
            try
            {
                string html = string.Empty;
                string url = @"http://localhost:3000/api/utilizatori";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                var result = JsonConvert.DeserializeObject<List<Utilizator>>(html,settings);
                DataTable dt = new DataTable();
                dt = ToDataTable(result);

                dataGridView1.DataSource = dt;
            }
            catch { MessageBox.Show("eroare"); }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                textBox1.Text = row.Cells[0].Value.ToString();
                textBox2.Text = row.Cells[1].Value.ToString();
                textBox3.Text = row.Cells[2].Value.ToString();
                textBox4.Text = row.Cells[3].Value.ToString();
                textBox5.Text = row.Cells[4].Value.ToString();
                textBox6.Text = row.Cells[5].Value.ToString();
                //string value2 = row.Cells[1].Value.ToString();
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:3000/api/utilizatori");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"prenume_utilizator\":\"" + textBox2.Text.ToString() + "\"," +
                                  "\"nume_utilizator\":\"" + textBox3.Text.ToString() + "\"," +
                                  "\"email\":\"" + textBox4.Text.ToString() + "\"," +
                                  "\"parola\":\"" + textBox5.Text.ToString() + "\"," +
                                  "\"data_nastere\":\"" + textBox6.Text.ToString() + "\"}";
                    streamWriter.Write(json);

                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                button1.PerformClick();
                populateDGV();
            }
            catch (WebException ex)
            {
                string message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                MessageBox.Show(message);
            }


        }

        private void button5_Click(object sender, EventArgs e)
        {
            populateDGV();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:3000/api/utilizatori/"+textBox1.Text.ToString());
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "PUT";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"prenume_utilizator\":\"" + textBox2.Text.ToString() + "\"," +
                                  "\"nume_utilizator\":\"" + textBox3.Text.ToString() + "\"," +
                                  "\"email\":\"" + textBox4.Text.ToString() + "\"," +
                                  "\"parola\":\"" + textBox5.Text.ToString() + "\"," +
                                  "\"data_nastere\":\"" + textBox6.Text.ToString() + "\"}";
                    streamWriter.Write(json);

                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                button1.PerformClick();
                populateDGV();
            }
            catch (WebException ex)
            {
                string message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                MessageBox.Show(message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:3000/api/utilizatori/" + textBox1.Text.ToString());
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "DELETE";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
  
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                button1.PerformClick();
                populateDGV();
            }
            catch (WebException ex)
            {
                string message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                MessageBox.Show(message);
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }
    }

}


