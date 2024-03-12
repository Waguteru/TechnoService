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

namespace TechnoService
{
    public partial class FormAuten : Form
    {
        DataBase dataBase = new DataBase();

        private bool closed = false;
        private NpgsqlConnection conn;
         
        public FormAuten()
        {
            InitializeComponent();
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            if (closed)
            {
                return;
            }
            else if (CheckLogin(username, password))
            {
                ShowUserRoleForm(username);
            }
        }

        string connectingString = "Server = localhost; port = 5432; Database = TechnoService; User Id = postgres; Password = 123";

        private bool CheckLogin(string username, string password)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectingString))
            {
                string query = "SELECT COUNT(*) FROM roleuser_tbl WHERE login_user = @username AND password_user = @password";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("username", username);
                    command.Parameters.AddWithValue("password", password);

                    connection.Open();
                    int count  = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private void ShowUserRoleForm(string username)
        {
            string role = GetUserRole(username);

            if(role == "Менеджер")
            {
                FormApplication formApplication = new FormApplication(username,role);
                formApplication.Show();
            }
            else if(role == "Исполнитель")
            {
                FormWorkingWithApplications formWorkingWithApplications = new FormWorkingWithApplications(username,role);
                formWorkingWithApplications.Show();
            }
            else
            {
                MessageBox.Show("ошибка: неизвестная роль пользователя","ошибка",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

            this.Hide();
        }

        private string GetUserRole(string username)
        {
            string role = "";
            using(NpgsqlConnection connection = new NpgsqlConnection(connectingString))
            {
                string query = "SELECT name_role FROM roleuser_tbl WHERE login_user = @username";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            role = result.ToString();
                        }
                        else
                        {
                            MessageBox.Show("не удалось получить роль пользователя", "ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"ошибка при получении роли пользователя: {ex.Message}", "ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return role;
        }
    }
}
