using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace TechnoService
{
    public partial class FormApplicationUser : Form
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

        public FormApplicationUser()
        {
            InitializeComponent();
        }

        public void CreateColumns()
        {
           // dataGridView1.Columns.Add("id", "id заявки"); //0
            dataGridView1.Columns.Add("number_app", "номер заявки"); //0
            dataGridView1.Columns.Add("date_add", "дата добавления"); //1
            dataGridView1.Columns.Add("equipment", "оборудование");       //2
            dataGridView1.Columns.Add("type_of_fault", "тип поломки");     //3
            dataGridView1.Columns.Add("description_fault", "описание поломки"); //4
            dataGridView1.Columns.Add("client", "фио клиента"); //5
            dataGridView1.Columns.Add("status", "статус заявки"); //6
         //   dataGridView1.Columns.Add("executor", "исполняющий"); //8
          //  dataGridView1.Columns.Add("comment_colunm", "комментарий"); //9
            dataGridView1.Columns.Add("email", "email"); //7
         //   dataGridView1.Columns.Add("accessories", "комплектующие"); //11
            dataGridView1.Columns.Add("isNew", String.Empty); //8
            dataGridView1.Columns["isNew"].Visible = false;
        }

        private void RefreshDataGrid(DataGridView dataGridView)
        {
            dataGridView.Rows.Clear();
            string queryString = $"select number_appli,date_add,equipment,type_of_fault,description_fault,client,status,email from applications";

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
            gridView.Rows.Add(record.GetInt64(0), record.GetDateTime(1), record.GetString(2), record.GetString(3), record.GetString(4), record.GetString(5), record.GetString(6), record.GetString(7), RowState.ModifidedNew);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddAplication addAplication = new AddAplication();
            addAplication.Show();
            this.Close();
        }


        private void Search(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string queryString = $"select number_appli,date_add,equipment,type_of_fault,description_fault,client,status,email from applications where concat (client, number_appli) like '%" + textBox1.Text + "%'";

            NpgsqlCommand comm = new NpgsqlCommand(queryString, dataBase.GetConnection());

            dataBase.OpenConnection();

            NpgsqlDataReader read = comm.ExecuteReader();

            while (read.Read())
            {
                ReadSingleRow(dgw, read);
            }

            read.Close();
        }

        private void FormApplicationUser_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }

        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Zen.Barcode.CodeQrBarcodeDraw qrcode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            pictureBox1.Image = qrcode.Draw("https://docs.google.com/forms/d/1Am2gjwtnH-2fB8A3r7J_nfwGYK6vUd-3Q36BAyYnWs4/edit", 70);
        }
    }
}
