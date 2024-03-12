using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TechnoService
{
    public partial class FormWorkingWithApplications : Form
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

        public FormWorkingWithApplications(string username, string role)
        {
            InitializeComponent();

            label5.Text = username;
            label7.Text = role;
        }

        public void CreateColumns()
        {
            dataGridViewApp.Columns.Add("id", "id заявки"); //0
            dataGridViewApp.Columns.Add("number_appli", "номер заявки"); //1
            dataGridViewApp.Columns.Add("date_add", "дата добавления"); //2
            dataGridViewApp.Columns.Add("equipment", "оборудование");       //3
            dataGridViewApp.Columns.Add("type_of_fault", "тип поломки");     //4
            dataGridViewApp.Columns.Add("description_fault", "описание поломки"); //5
            dataGridViewApp.Columns.Add("client", "фио клиента"); //6
            dataGridViewApp.Columns.Add("status", "статус заявки"); //7
            dataGridViewApp.Columns.Add("executor", "исполняющий"); //8
            dataGridViewApp.Columns.Add("comment_colunm", "комментарий"); //9
            dataGridViewApp.Columns.Add("email", "email"); //10
            dataGridViewApp.Columns.Add("accessories", "комплектующие"); //11
            dataGridViewApp.Columns.Add("isNew", String.Empty); //12
            dataGridViewApp.Columns["isNew"].Visible = false;
        }


        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetInt64(1), record.GetDateTime(2), record.GetString(3), record.GetString(4), record.GetString(5), record.GetString(6), record.GetString(7), record.GetString(8), record.GetString(9), record.GetString(10), record.GetString(11), RowState.ModifidedNew);
        }

        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string queryString = $"select id,number_appli,date_add,equipment,type_of_fault,description_fault,client,status,executor,comment_colunm,email,accessories from applications";
       
            NpgsqlCommand command = new NpgsqlCommand(queryString,dataBase.GetConnection());

            dataBase.OpenConnection();

            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();
        }



        private void FormWorkingWithApplications_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridViewApp);
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataBase.OpenConnection();

            var status = comboBox1.SelectedItem.ToString();
          //  var executortb = comboBox2.SelectedItem.ToString();
            var id = Convert.ToInt32(textBox4.Text);
            var description_fault = textBox1.Text;
            var accessories = textBox3.Text;
            var comment_colunm = textBox2.Text;

            string query = $"UPDATE applications SET status = '{status}',comment_colunm = '{comment_colunm}', accessories = '{accessories}', description_fault = '{description_fault}' WHERE id = " + id;
            NpgsqlCommand command = new NpgsqlCommand(@query, dataBase.GetConnection());
            command.ExecuteNonQuery();
            dataBase.CloseConnection();

            MessageBox.Show("Данные успешно изменены");

            RefreshDataGrid(dataGridViewApp);
        }

        private void dataGridViewApp_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewApp.Rows[selectedRow];

                textBox1.Text = row.Cells[5].Value.ToString();
                textBox3.Text = row.Cells[11].Value.ToString();
                comboBox1.Text = row.Cells[7].Value.ToString();
                textBox2.Text = row.Cells[9].Value.ToString();
                textBox4.Text = row.Cells[0].Value.ToString();
               
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void вернутьсяКАвторизацииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAuten formAuten = new FormAuten();
            formAuten.Show();
            this.Close();
        }
    }
}
