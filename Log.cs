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
    public partial class Log : Form
    {
        DataBase data = new DataBase();
        public Log()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            passBox.PasswordChar = '*';
            loginBox.MaxLength = 50;
            passBox.MaxLength = 50;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var login = loginBox.Text;
            var pass = passBox.Text;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();
            string queryString = $"SELECT id_user, login_user, pass FROM registers where login_user = '{login}' AND pass = '{pass}'";

            SqlCommand sqlCommand = new SqlCommand(queryString, data.GetConn());

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(dataTable);

            if(dataTable.Rows.Count == 1)
            {
                MessageBox.Show("You log in complete");
                Data data = new Data();
                data.Txt = login;
                this.Hide();
                data.ShowDialog();
                this.Show();
            }
            else
                MessageBox.Show("You log in is not complete");
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Sign sign = new Sign();
            sign.Show();
            this.Hide();
        }

        private void demoVariant_Click(object sender, EventArgs e)
        {

        }
    }
}
