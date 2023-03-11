using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trucker
{
    public partial class Sign : Form
    {
        public Sign()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        readonly DataBase data = new DataBase();
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            passBox.PasswordChar = '*';
            loginBox.MaxLength = 50;
            passBox.MaxLength = 50;
        }

        private void butnCreate_Click(object sender, EventArgs e)
        {
            var login = loginBox.Text;
            var pass = passBox.Text;
            if(login == "" || pass == "")
            {
                MessageBox.Show("None of the fields can be empty");
            }
            else
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable dataTable = new DataTable();

                string queryString = $"INSERT INTO registers(login_user, pass) VALUES('{login}', '{pass}')";

                SqlCommand sqlCommand = new SqlCommand(queryString, data.GetConn());

                data.OpenConn();
                if (CheckerUser())
                {
                    return;
                }
                else
                {
                    if (sqlCommand.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Account succesfully create");
                        string queryCreate = $"CREATE TABLE {login}(id int PRIMARY KEY IDENTITY(1,1) NOT NULL ,driver varchar(50) NOT NULL,car varchar(50) NOT NULL,cargo varchar(50) NOT NULL,weightcar int NOT NULL,price int NOT NULL,start_Point varchar(50) NOT NULL,end_Point varchar(50) NOT NULL)";
                        SqlCommand sqlCommand2 = new SqlCommand(queryCreate, data.GetConn());
                        data.OpenConn();
                        sqlCommand2.ExecuteNonQuery();
                        data.CloseConn();
                        Log log = new Log();
                        this.Close();
                        log.ShowDialog();
                    }
                    else
                        MessageBox.Show("Account is not create");
                }
                data.CloseConn();
            }
        }
        private bool CheckerUser()
        {
            var login = loginBox.Text;
            var pass = passBox.Text;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();

            string queryString = $"SELECT * FROM registers where login_user = '{login}' AND pass = '{pass}'";
            SqlCommand sqlCommand = new SqlCommand(queryString, data.GetConn());

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(dataTable);
            if (dataTable.Rows.Count > 0)
            {
                MessageBox.Show("User was registered");
                return true;
            }
            else
                return false;
        }
    }
}
