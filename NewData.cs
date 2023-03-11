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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Trucker
{
    public partial class NewData : Form
    {
        public NewData()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }
        readonly DataBase data = new DataBase();
        private void NewData_Load(object sender, EventArgs e)
        {

        }
        public string Txt
        {
            get { return userNameAddForm.Text; }
            set { userNameAddForm.Text = value; }
        }
        private void butnCreate_Click(object sender, EventArgs e)
        {
            data.OpenConn();
            var driver = DriverBox.Text;
            var car = CarBox.Text;
            var cargo = CargoBox.Text;
            int weight;
            int price;
            var start = startPont.Text;
            var end = endPoint.Text;
            if (int.TryParse(WeightBox.Text, out weight) && int.TryParse(PriceBox.Text, out price))
            {
                var addQuery = $"INSERT INTO {Txt} (driver, car, cargo, weightcar, price, start_Point, end_Point) VALUES ('{driver}', '{car}', '{cargo}', '{weight}', '{price}', '{start}', '{end}')";
                SqlCommand sqlCommand = new SqlCommand(addQuery, data.GetConn());
                sqlCommand.ExecuteNonQuery();
                MessageBox.Show("New entry complete add.Please refresh the page");
            }
            else
                MessageBox.Show("New entry is not complete add");
            data.CloseConn();
        }
    }
}
