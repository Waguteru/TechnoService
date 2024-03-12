using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections;
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
    public partial class AddAplication : Form
    {
        DataBase dataBase = new DataBase();

        private NpgsqlConnection conn;

        public AddAplication()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormApplicationUser user = new FormApplicationUser();
            user.Show();
            this.Hide();
        }

        private string GenerateOrderNumber()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        private void ClearFields()
        {
            fio_tb.Text = "";
            email_tb.Text = "";
            equipment_tb.Text = "";
            type_of_fault_tb.Text = "";
            description_fault_tb.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataBase.OpenConnection();

            var fio = fio_tb.Text;
            var number_app = GenerateOrderNumber();
            var email = email_tb.Text;
            var equipmenttb = equipment_tb.Text;
            var type_of_fault = type_of_fault_tb.Text;
            var description = description_fault_tb.Text;
            var status = "в ожидании";
            var executor = "назначить";
            var comment_colunm = "-";
            var accessories = "-";
            var date_add = new NpgsqlParameter("@date_add", NpgsqlDbType.Date).Value = DateTime.Now.Date;



            string sql = $"INSERT INTO applications (number_appli,date_add, equipment,type_of_fault,description_fault,client,status,executor,comment_colunm,email,accessories) VALUES ('{number_app}','{date_add}','{equipmenttb}','{type_of_fault}','{description}','{fio}','{status}','{executor}','{comment_colunm}','{email}','{accessories}')";

            NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sql, dataBase.GetConnection());

            npgsqlCommand.ExecuteNonQuery();

            MessageBox.Show("Заявка успешно отправлена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ClearFields();

            dataBase.CloseConnection();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
