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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BD
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            populateDGV();
            Thread.Sleep(500);
        }

        private void label1_Click(object sender, EventArgs e)
        {

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
            string html = string.Empty;
            Thread.Sleep(50);
            string url = @"http://localhost:3000/api/producatori";

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
            var result = JsonConvert.DeserializeObject<List<Producator>>(html,settings);
            DataTable dt = new DataTable();
            dt = ToDataTable(result);
            Thread.Sleep(50);
            dataGridView1.DataSource = dt;

        }

        public class Producator
        {
            public int id { get; set; }
            public string nume { get; set; }
            public string website { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
      
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                textBox1.Text = row.Cells[0].Value.ToString();
                textBox2.Text = row.Cells[1].Value.ToString();
                textBox3.Text = row.Cells[2].Value.ToString();
                //string value2 = row.Cells[1].Value.ToString();

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            populateDGV();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:3000/api/producatori");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"nume_producator\":\"" + textBox2.Text.ToString() + "\"," +                               
                                  "\"website\":\"" + textBox3.Text.ToString() + "\"}";
                    streamWriter.Write(json);

                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Thread.Sleep(500);
                button1.PerformClick();
                populateDGV();
            }
            catch (WebException ex)
            {
                string message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                MessageBox.Show(message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:3000/api/producatori/" + textBox1.Text.ToString());
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "PUT";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"nume_producator\":\"" + textBox2.Text.ToString() + "\"," +
                                  "\"website\":\"" + textBox3.Text.ToString() + "\"}";
                    streamWriter.Write(json);

                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Thread.Sleep(500);
                button1.PerformClick();
               
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
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:3000/api/producatori/" + textBox1.Text.ToString());
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "DELETE";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Thread.Sleep(500);
                button1.PerformClick();
                populateDGV();
            }
            catch (WebException ex)
            {
                string message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                MessageBox.Show(message);
            }
        }
    }
}
