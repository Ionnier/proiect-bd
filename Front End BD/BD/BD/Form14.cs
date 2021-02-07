using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BD
{
    public partial class Form14 : Form
    {
        public Form14()
        {
            InitializeComponent();
           
        }
        public class Join
        {
            public int id_utilizator { get; set; }
            public string nume_utilizator { get; set; }
            public string prenume_utilizator { get; set; }
            public string email { get; set; }
            public string parola { get; set; }
            public string data_nastere { get; set; }
            public int id_comanda { get; set; }
            public string id_judet { get; set; }
            public string nume_judet { get; set; }
            public string oras { get; set; }
            public string strada { get; set; }
            public string numar { get; set; }
            public string cod_postal { get; set; }
            public string scara { get; set; }
            public string bloc { get; set; }
            public string apartament { get; set; }
            public string numar_telefon { get; set; }
            public int id_produs { get; set; }
            public string nume_produs { get; set; }
            public int cantitate_comandata { get; set; }
            public double pret { get; set; }
            public int stoc { get; set; }
            public string descriere { get; set; }
            public string categorie { get; set; }
            public int id_producator{ get; set; }
            public string nume_producator{ get; set; }
            public string website{ get; set; }

            
            
        }

        private async void Form14_Load(object sender, EventArgs e)
        {
            await Populate_DGV();
        }


        static readonly HttpClient client = new HttpClient();

        public string Replace_spaces(string oldstring){
            
            return oldstring.Replace(" ", "%20");
        }

        public async Task Populate_DGV()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                string url = "http://localhost:3000/api/join";
                
                if (textBox1.Text.Length > 0)
                {
                    url += ("/" + textBox1.Text.ToString());
                }
                url += "?";
                int counter = 0;
                if (textBox2.Text.Length > 0)
                {
                    if (counter == 1) url += "&";
                    counter = 1;
                    url += ("nume_utilizator=" + Replace_spaces(textBox2.Text.ToString()));
                }
                if (textBox3.Text.Length > 0)
                {
                    if (counter == 1) url += "&";
                    counter = 1;
                    url += ("prenume_utilizator=" + Replace_spaces(textBox3.Text.ToString()));
                }
                if (textBox4.Text.Length > 0)
                {
                    if (counter == 1) url += "&";
                    counter = 1;
                    url += ("email=" + Replace_spaces(textBox4.Text.ToString()));
                }
                if (textBox5.Text.Length > 0)
                {
                    if (counter == 1) url += "&";
                    counter = 1;
                    url += ("data_nastere=" + Replace_spaces(textBox5.Text.ToString()));
                }
                if (textBox6.Text.Length > 0)
                {
                    if (counter == 1) url += "&";
                    counter = 1;
                    url += ("id_comanda=" + Replace_spaces(textBox6.Text.ToString()));
                }
                if (textBox7.Text.Length > 0)
                {
                    if (counter == 1) url += "&";
                    counter = 1;
                    url += ("id_judet=" + Replace_spaces(textBox7.Text.ToString()));
                }
                if (textBox8.Text.Length > 0)
                {
                    if (counter == 1) url += "&";
                    counter = 1;
                    url += ("nume_judet=" + Replace_spaces(textBox8.Text.ToString()));
                }
                if (textBox9.Text.Length > 0)
                {
                    if (counter == 1) url += "&";
                    counter = 1;
                    url += ("oras=" + Replace_spaces(textBox9.Text.ToString()));
                }
                if (textBox10.Text.Length > 0)
                {
                    if (counter == 1) url += "&";
                    counter = 1;
                    url += ("strada=" + Replace_spaces(textBox10.Text.ToString()));
                }
                if (textBox11.Text.Length > 0)
                {
                    if (counter == 1) url += "&";
                    counter = 1;
                    url += ("numar=" + Replace_spaces(textBox11.Text.ToString()));
                }
                if (textBox12.Text.Length > 0)
                {
                    if (counter == 1) url += "&";
                    counter = 1;
                    url += ("cod_postal=" + Replace_spaces(textBox12.Text.ToString()));
                }
                if (textBox13.Text.Length > 0)
                {
                    if (counter == 1) url += "&";
                    counter = 1;
                    url += ("scara=" + Replace_spaces(textBox13.Text.ToString()));
                }
                if (textBox14.Text.Length > 0)
                {
                    if (counter == 1) url += "&";
                    counter = 1;
                    url += ("bloc=" + Replace_spaces(textBox14.Text.ToString()));
                }
                if (textBox15.Text.Length > 0)
                {
                    if (counter == 1) url += "&";
                    counter = 1;
                    url += ("apartament=" + Replace_spaces(textBox15.Text.ToString()));
                }
                if (textBox16.Text.Length > 0)
                {
                    if (counter == 1) url += "&";
                    counter = 1;
                    url += ("numar_telefon=" + Replace_spaces(textBox16.Text.ToString()));
                }
                if (textBox17.Text.Length > 0)
                {
                    if (counter == 1) url += "&";
                    counter = 1;
                    url += ("id_produs=" + Replace_spaces(textBox17.Text.ToString()));
                }
                if (textBox18.Text.Length > 0)
                {
                    if (counter == 1) url += "&";
                    counter = 1;
                    url += ("nume_produs=" + Replace_spaces(textBox18.Text.ToString()));
                }
                if (textBox19.Text.Length > 0)
                {
                    if (counter == 1) url += "&";
                    counter = 1;
                    url += ("cantitate_comanda=" + Replace_spaces(textBox19.Text.ToString()));
                }
                if (textBox20.Text.Length > 0)
                {
                    if (counter == 1) url += "&";
                    counter = 1;
                    url += ("pret=" + Replace_spaces(textBox20.Text.ToString()));
                }
                if (textBox21.Text.Length > 0)
                {
                    if (counter == 1) url += "&";
                    counter = 1;
                    url += ("categorie=" + Replace_spaces(textBox21.Text.ToString()));
                }
                if (textBox22.Text.Length > 0)
                {
                    if (counter == 1) url += "&";
                    counter = 1;
                    url += ("id_producator=" + Replace_spaces(textBox22.Text.ToString()));
                }
                if (textBox23.Text.Length > 0)
                {
                    if (counter == 1) url += "&";
                    counter = 1;
                    url += ("nume_producator=" + Replace_spaces(textBox23.Text.ToString()));
                }
                if (textBox24.Text.Length > 0)
                {
                    if (counter == 1) url += "&";
                    counter = 1;
                    url += ("website=" + Replace_spaces(textBox24.Text.ToString()));
                }

                Console.WriteLine(url);
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    var result = JsonConvert.DeserializeObject<List<Join>>(responseBody, settings);
                    DataTable dt = new DataTable();
                    dt = ToDataTable(result);
                    dataGridView1.DataSource = dt;
                } else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(responseBody);
                }
            }
            catch (HttpRequestException e)
            {
                
                string exce = "\nException Caught!";
                exce+=("Message:", e.Message);
                MessageBox.Show(exce);

            }
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

        

        private async void button1_Click(object sender, EventArgs e)
        {
            await Populate_DGV();
        }
    }
}
