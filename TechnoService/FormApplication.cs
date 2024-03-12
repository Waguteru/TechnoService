using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TechnoService
{
    public partial class FormApplication : Form
    {

        DataBase dataBase = new DataBase();
        

        enum RowState
        {
            Existed,
            New,
            Modfied,
            ModifidedNew,
            Deleted,
            Exited
        }

        int selectedRow;

        public FormApplication(string username, string role)
        {
            InitializeComponent();

            comboBox2.Items.Add("Ханин Сергей Алексеевич");
            comboBox2.Items.Add("Соколов Дмитрий Сергеевич");
            
            label5.Text = username;
            label7.Text = role;
        }



        public void CreateColumns()
        {
            dataGridView1.Columns.Add("id", "id заявки"); //0
            dataGridView1.Columns.Add("number_app", "номер заявки"); //1
            dataGridView1.Columns.Add("date_add", "дата добавления"); //2
            dataGridView1.Columns.Add("equipment", "оборудование");       //3
            dataGridView1.Columns.Add("type_of_fault", "тип поломки");     //4
            dataGridView1.Columns.Add("description_fault", "описание поломки"); //5
            dataGridView1.Columns.Add("client", "фио клиента"); //6
            dataGridView1.Columns.Add("status", "статус заявки"); //7
            dataGridView1.Columns.Add("email", "email"); //8
            dataGridView1.Columns.Add("executor", "исполняющий"); //9
            dataGridView1.Columns.Add("comment_colunm", "комментарий"); //10
            dataGridView1.Columns.Add("accessories", "комплектующие"); //11
            dataGridView1.Columns.Add("isNew", String.Empty); //12
            dataGridView1.Columns["isNew"].Visible = false;
        }

        private void RefreshDataGrid(DataGridView dataGridView)
        {
            dataGridView.Rows.Clear();
            string queryString = $"select id,number_appli,date_add,equipment,type_of_fault,description_fault,client,status,email,executor,comment_colunm,accessories from applications";

            NpgsqlCommand command = new NpgsqlCommand(queryString, dataBase.GetConnection());

            dataBase.OpenConnection();

            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dataGridView, reader);
            }
            reader.Close();
        }

        private void ReadSingleRow(DataGridView gridView, IDataRecord record)
        {
            gridView.Rows.Add(record.GetInt32(0), record.GetInt64(1), record.GetDateTime(2), record.GetString(3), record.GetString(4), record.GetString(5), record.GetString(6), record.GetString(7), record.GetString(8), record.GetString(9), record.GetString(10), record.GetString(11), RowState.ModifidedNew);
        }

        private void FormApplication_Load(object sender, EventArgs e)
        {
            
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataBase.OpenConnection();

          


            var status = comboBox1.SelectedItem.ToString();
            var executortb = comboBox2.SelectedItem.ToString();
            var id = Convert.ToInt32(textBox2.Text);
            var description_fault = textBox1.Text;

            string query = $"UPDATE applications SET status = '{status}', executor = '{executortb}', description_fault = '{description_fault}' WHERE id = " + id;
            NpgsqlCommand command = new NpgsqlCommand(@query, dataBase.GetConnection());
            command.ExecuteNonQuery();
            dataBase.CloseConnection();

            MessageBox.Show("Данные успешно изменены");

            RefreshDataGrid(dataGridView1);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];

                textBox1.Text = row.Cells[5].Value.ToString();
                textBox2.Text = row.Cells[0].Value.ToString();
                comboBox1.Text = row.Cells[7].Value.ToString();
                comboBox2.Text = row.Cells[9].Value.ToString();
                textBox3.Text = row.Cells[8].Value.ToString();
                textBox4.Text = row.Cells[1].Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string smtpServer = "smtp.mail.ru";
            int smtpPort = 587;

            string smtpUsername = "waguteru@mail.ru";
            string smtpPassword = "kG6K9KvM5PtpENRLi1Vp";

            using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;

                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(smtpUsername);
                    mailMessage.To.Add(textBox3.Text);
                    mailMessage.Subject = "ООО Техносервис";
                    mailMessage.Body = "Это автоматическое сообщение. На него не нужно отвечать.\nЕсли вы хотите узнать статус вашей заявки,то можете указать его в приложении.\nВаш номер заявки: " + textBox4.Text;

                    try
                    {
                        smtpClient.Send(mailMessage);
                        MessageBox.Show("Сообщение отправлено");

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Сообщение не отправлено {ex.Message}");
                    }
                }
            }
        }

        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void вернуьтсяКАвторизацииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAuten formAuten = new FormAuten();
            formAuten.Show();
            this.Close();
        }
    }
}
