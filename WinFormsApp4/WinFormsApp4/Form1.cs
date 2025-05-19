using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.SQLite;
using System.Runtime.InteropServices;
using TextBox = System.Windows.Forms.TextBox;

namespace WinFormsApp4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Console.WriteLine("Progrma run");
        }
        string connectionString = "Data Source=C:\\Users\\Asus\\source\\repos\\WinFormsApp2\\data.db;Version=3;";
        string personelSifresi = "12345qwerty";
        private void InsertIntoDB(string str)
        {

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(str, connection))
                {
                    command.ExecuteNonQuery();

                }
            }
        }


        private List<string> GetDataFromDB(string str, int columnNum)
        {
            var liste = new List<string>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(str, connection))
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        liste.Add(reader.GetValue(columnNum).ToString());
                    }
                }

            }
            return liste;
        }

        //-------------------------------1111-------------------------------------

        private void button2_Click_1(object sender, EventArgs e)
        {
            Console.WriteLine("Button2 clicked");
            splitContainer8.Panel1.Controls.Add(flowLayoutPanel1);
            panel1.Visible = true;
            panel1.Dock = DockStyle.Fill;

            panel2.Visible = false;
            panel5.Visible = false;
            panel2.Dock = DockStyle.None;
            panel5.Dock = DockStyle.None;
        }

        private void CreatelistItem(string kimlik)
        {
            var controls = flowLayoutPanel3.Controls;
            string ism = kimlik + "_plan";
            foreach (Control c in controls)
            {
                if (c.Name == ism)
                {
                    Console.WriteLine("c.Name");
                    var contr = flowLayoutPanel3.Controls.Find(ism, true);
                    flowLayoutPanel3.Controls.Remove(contr[0]); //
                }
                ;
            }
            var yapilacaklar = GetDataFromDB($"SELECT yapilacak FROM \"{kimlik}_plan\"", 0);
            TextBox textBox = new TextBox();
            textBox.Height = (int)textBox.Font.Height * (yapilacaklar.Count() + 1) + 5;
            textBox.Width = flowLayoutPanel3!.Height;
            textBox.Text = kimlik;
            textBox.Multiline = true;
            textBox.Margin = new Padding(0);
            textBox.ReadOnly = true;

            for (int i = 0; i < yapilacaklar.Count(); i++)
            {

                textBox.Text += Environment.NewLine + $"-{yapilacaklar[i]}";
            }

            FlowLayoutPanel flowPanel = new FlowLayoutPanel();
            flowPanel.Padding = new Padding(0, 0, 3, 0);
            flowPanel.Width = textBox.Width;
            flowPanel.Height = textBox.Height + 3;
            flowPanel.Controls.Add(textBox);
            flowPanel.BackColor = Color.Black;
            flowPanel.Margin = new Padding(0);
            flowLayoutPanel3!.Controls.Add(flowPanel);
            flowPanel.Name = kimlik + "_plan";
            flowLayoutPanel3!.Controls.SetChildIndex(flowPanel, 0);
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            button2.BackColor = Color.White;
            button2.ForeColor = Color.FromArgb(51, 17, 245);
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackColor = Color.White;
            button1.ForeColor = Color.FromArgb(51, 17, 245);
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            button3.BackColor = Color.White;
            button3.ForeColor = Color.FromArgb(51, 17, 245);
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.Transparent;
            button1.ForeColor = Color.White;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackColor = Color.Transparent;
            button2.ForeColor = Color.White;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button3.BackColor = Color.Transparent;
            button3.ForeColor = Color.White;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox2 == null)
                Console.WriteLine("listBox2 is null!");

            if (listBox1 == null)
                Console.WriteLine("listBox1 is null!");


            int kimlik = Convert.ToInt32(textBox2.Text);
            string text = textBox2.Text;
            bool bulundu = false;
            List<string> uyeler = GetDataFromDB("Select * FROM uyeler", 1);
            foreach (string s in uyeler)
            {
                if (s == text)
                {
                    bulundu = true;
                }
            }
            if (bulundu == false)
            {
                textBox2.Text = "Kiþi üye deðil";
            }
            else if (!listBox2.Items.Contains(kimlik) && !listBox1.Items.Contains(kimlik))
            {
                if (kimlik < 1000)
                {
                    listBox2.Items.Add(kimlik);
                }
                else
                {
                    listBox1.Items.Add(kimlik);
                }
                string str = $"INSERT INTO iceridekiler (uye_kimligi) VALUES ({textBox2.Text});";
                InsertIntoDB(str);
                str = $"CREATE TABLE IF NOT EXISTS \"{kimlik}_plan\"  (ID INTEGER PRIMARY KEY AUTOINCREMENT,yapilacak CHAR(50));";
                InsertIntoDB(str);
            }
            textBox2.Text = "Üye Kimliðini Yazýn";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            InsertIntoDB($"DELETE FROM iceridekiler WHERE uye_kimligi = {textBox3.Text}");
            ListBox boxToBeChecked = Convert.ToInt32(textBox3.Text) < 1000 ? listBox2 : listBox1;
            foreach (var item in boxToBeChecked.Items)
            {
                if (item.ToString() == textBox3.Text)
                {
                    boxToBeChecked.Items.Remove(item);
                    break;
                }
            }
            bool bulundu = false;
            foreach (Control item in flowLayoutPanel3.Controls)
            {
                if (item.Name == $"{textBox3.Text}_plan")
                {
                    flowLayoutPanel3.Controls.Remove(item);
                    Console.WriteLine("bulundu");
                    bulundu = true;
                    break;
                }
            }
            if (bulundu == false)
            {
                Console.WriteLine("Bulunamadý");
                foreach (Control item in flowLayoutPanel3.Controls)
                {
                    Console.WriteLine(item.Name);
                }
                Console.WriteLine(",,," + "textbox3 " + $"{textBox3.Text}_plan" + ",,,");
            }
            textBox3.Text = "Üye kimliðini yazýn";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string kimlik = textBox4.Text;
            string plan = textBox5.Text;
            InsertIntoDB($"INSERT INTO \"{kimlik}_plan\" (yapilacak) VALUES(\"{textBox5.Text}\");");
            textBox5.Text = "Program";
            textBox4.Text = "Seçilen üye";
            Control[] result = flowLayoutPanel3.Controls.Find($"\"{kimlik}_plan\"", true);
            if (result.Length == 0) CreatelistItem(kimlik); //
            else
            {
                var textBox = result[0].Controls[0];
                textBox.Height += (int)result[0].Controls[0].Font.Size;
                textBox.Text += Environment.NewLine + $"{textBox5.Text}";
            }
            listBox2!.ClearSelected();
            listBox1!.ClearSelected();
            textBox4.Text = "Seçilen Üye";
        }


        private void listBox2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1) return;
            else
            {
                CreatelistItem(listBox2.SelectedItem.ToString());
            }
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            else
            {
                CreatelistItem(listBox1.SelectedItem.ToString());
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                Console.WriteLine("ListBox1 SElectedItem null");
                return;
            }
            ;
            textBox4.Text = listBox1.SelectedItem.ToString();

        }

        private void listBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
            {
                Console.WriteLine("ListBox2 SElectedItem null");
                return;
            }
            else
                textBox4.Text = listBox2.SelectedItem.ToString();
        }

        private void button5_MouseCaptureChanged(object sender, EventArgs e)
        {

        }


        private void textBox2_Enter(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            textBox4.Text = "";
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            textBox3.Text = "";
        }
        //-------------------------------1111-------------------------------------

        //-------------------------------2222-------------------------------------
        private void button1_Click_1(object sender, EventArgs e)
        {
            splitContainer4.Panel1.Controls.Add(flowLayoutPanel1);
            panel2.Visible = true;
            panel2.Dock = DockStyle.Fill;

            panel1.Visible = false;
            panel5.Visible = false;
            panel1.Dock = DockStyle.None;
            panel5.Dock = DockStyle.None;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            InsertIntoDB($"INSERT INTO uyeler (uye_kimligi, yas, cinsiyet, boy, kilo, uyelik_ucreti) VALUES ({textBox11.Text}, {textBox6.Text}, {textBox7.Text}, {textBox8.Text}, {textBox9.Text}, {textBox10.Text}");
            textBox7.Text = "Yaþý giriniz";
            textBox8.Text = "Cinsiyeti giriniz";
            textBox9.Text = "Boyu giriniz";
            textBox10.Text = "Üyelik ücreti";
            textBox11.Text = "Üye kimliðini giriniz";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            InsertIntoDB($"DELETE FROM uyeler WHERE uye_kimligi = {textBox12.Text}");
            textBox12.Text = "Üye kimliði";
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            InsertIntoDB($"DELETE FROM uyeler WHERE uye_kimligi = {textBox12.Text}");
            textBox12.Text = "Üye kimliði";
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            InsertIntoDB($"INSERT INTO uyeler (yas, cinsiyet, boy, kilo, uyelik_ucreti, uye_kimligi, isim) VALUES ({textBox6.Text}, \"{textBox7.Text}\", {textBox8.Text}, {textBox9.Text}, {textBox10.Text}, {textBox11.Text}, \"{textBox18.Text}\");");
            textBox6.Text = "Yaþ giriniz";
            textBox7.Text = "Cinsiyet giriniz";
            textBox8.Text = "Boy giriniz";
            textBox9.Text = "Kilo giriniz";
            textBox10.Text = "Üyelik ücreti";
            textBox11.Text = "Üye kimliði";
            textBox18.Text = "Üye adý soyadý";
        }
        private void textBox6_Enter(object sender, EventArgs e)
        {
            textBox6.Text = "";
        }

        private void textBox7_Enter(object sender, EventArgs e)
        {
            textBox7.Text = "";
        }

        private void textBox8_Enter(object sender, EventArgs e)
        {
            textBox8.Text = "";
        }

        private void textBox9_Enter(object sender, EventArgs e)
        {
            textBox9.Text = "";
        }

        private void textBox10_Enter(object sender, EventArgs e)
        {
            textBox10.Text = "";
        }

        private void textBox11_Enter(object sender, EventArgs e)
        {
            textBox11.Text = "";
        }

        private void textBox18_Enter(object sender, EventArgs e)
        {
            textBox18.Text = "";
        }

        private void textBox12_Enter(object sender, EventArgs e)
        {
            textBox12.Text = "";
        }
        //-------------------------------2222-------------------------------------

        //-------------------------------3333-------------------------------------

        private void button3_Click(object sender, EventArgs e)
        {
            splitContainer6.Panel1.Controls.Add(flowLayoutPanel1);
            flowLayoutPanel1.Dock = DockStyle.Left;
            panel5.Visible = true;
            panel5.Dock = DockStyle.Fill;

            panel1.Visible = false;
            panel2.Visible = false;
            panel1.Dock = DockStyle.None;
            panel2.Dock = DockStyle.None;
        }
        private void button9_Click(object sender, EventArgs e)
        {
            flowLayoutPanel4.Controls.Clear();
            List<string> uye_kimlikleri = GetDataFromDB("SELECT * FROM uyeler", 1);
            List<string> uye_isimleri = GetDataFromDB("SELECT isim FROM uyeler", 0);
            var textBox = new TextBox();
            textBox.Multiline = true;
            textBox.Height = (int)textBox.Font.Height * (uye_kimlikleri.Count() + 1) + 5;
            textBox.Width = flowLayoutPanel3!.Height;
            textBox.Text += "Üye isimleri: üye kimlikleri" + Environment.NewLine;
            for (int i = 0; i < uye_kimlikleri.Count(); i++)
            {
                textBox.Text += uye_isimleri[i] + ": " + uye_kimlikleri[i] + Environment.NewLine;
            }
            flowLayoutPanel4.Controls.Add(textBox);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            flowLayoutPanel4.Controls.Clear();
            var bilgi = new string[7];
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand($"SELECT * FROM uyeler WHERE uye_kimligi = {textBox17.Text};", connection))
                using (var reader = command.ExecuteReader())
                {
                    Console.WriteLine("textbox17 text: " + textBox17.Text);
                    reader.Read();
                    bilgi[1] = reader.GetValue(2).ToString();
                    bilgi[2] = reader.GetValue(3).ToString();
                    bilgi[3] = reader.GetValue(4).ToString();
                    bilgi[4] = reader.GetValue(5).ToString();
                    bilgi[5] = reader.GetValue(6).ToString();
                    bilgi[6] = reader.GetValue(7).ToString();
                }

            }
            var textBox = new TextBox();
            textBox.Multiline = true;
            textBox.Height = (int)textBox.Font.Height * (8) + 5;
            textBox.Width = flowLayoutPanel3!.Height;
            textBox.Text =
                $"Üye kimliði: {bilgi[1]}" + Environment.NewLine +
                $"Yaþ: {bilgi[2]}" + Environment.NewLine +
                $"Cinsiyet: {bilgi[3]}" + Environment.NewLine +
                $"Boy: {bilgi[4]}" + Environment.NewLine +
                $"Kilo: {bilgi[5]}" + Environment.NewLine +
                $"Ýsim: {bilgi[6]}";
            flowLayoutPanel4.Controls.Add(textBox);
        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            InsertIntoDB($"UPDATE uyeler SET yas = {textBox13.Text},cinsiyet = {textBox14.Text},boy = {textBox15.Text},kilo = {textBox16.Text} WHERE uye_kimligi = {textBox17.Text}");
            textBox13.Text = "Yaþ";
            textBox13.Text = "Cinsiyet";
            textBox13.Text = "Boy";
            textBox13.Text = "Kilo";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        //-------------------------------3333-------------------------------------
    }
}
