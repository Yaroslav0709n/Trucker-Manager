using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trucker
{
    public partial class Data : Form
    {
        public Data()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            
        }
        readonly DataBase data = new DataBase();
        int selectRow;
        public string Txt
        {
            get { return userName.Text; }
            set { userName.Text = value; }
        }
        /// <summary>
        /// Method for create colums
        /// </summary>
        private void CreateColumns()
        {
            dataGridView1.Columns.Add("id", "Id");
            dataGridView1.Columns.Add("driver", "Name Driver");
            dataGridView1.Columns.Add("car", "Model of Car");
            dataGridView1.Columns.Add("cargo", "Type of Cargo");
            dataGridView1.Columns.Add("weight", "Weight, kg");
            dataGridView1.Columns.Add("price", "Price, $");
            dataGridView1.Columns.Add("startPoint", "Start City");
            dataGridView1.Columns.Add("endPoint", "Last City");
            dataGridView1.Columns.Add("Is New", string.Empty);
        }
        private void ReadSingleRaw(DataGridView dataGrid, IDataRecord dataRecord)
        {
            dataGrid.Rows.Add(dataRecord.GetInt32(0), dataRecord.GetString(1), dataRecord.GetString(2), dataRecord.GetString(3), dataRecord.GetInt32(4), dataRecord.GetInt32(5), dataRecord.GetString(6), dataRecord.GetString(7), RowState.ModifiedNew);
        }
        /// <summary>
        /// Method to refresh the table
        /// </summary>
        private void RefreshDataGrid(DataGridView dataGrid)
        {
            dataGrid.Rows.Clear();
            Thread.Sleep(200);
            string queryString = $"SELECT * FROM {Txt}";

            SqlCommand sqlCommand = new SqlCommand(queryString, data.GetConn());
            data.OpenConn();

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
                ReadSingleRaw(dataGrid, sqlDataReader);

            sqlDataReader.Close();

            data.CloseConn();
        }
        private void Data_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectRow = e.RowIndex;
            if(e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectRow];

                IDBox.Text = row.Cells[0].Value.ToString();
                DriverBox.Text = row.Cells[1].Value.ToString();
                CarBox.Text = row.Cells[2].Value.ToString();
                CargoBox.Text = row.Cells[3].Value.ToString();
                WeightBox.Text = row.Cells[4].Value.ToString();
                PriceBox.Text = row.Cells[5].Value.ToString();
                StartBox.Text = row.Cells[6].Value.ToString();
                EndBox.Text = row.Cells[7].Value.ToString();
            }
        }
        /// <summary>
        /// Event for creating a new entry
        /// </summary>
        private void NewBtn_Click_1(object sender, EventArgs e)
        {
            NewData newData = new NewData();
            newData.Txt = userName.Text;
            newData.Show();
        }
        /// <summary>
        /// Table search method
        /// </summary>
        
        private void Search(DataGridView dataGridView)
        {
            dataGridView.Rows.Clear();
            string stringSelect = $"SELECT * FROM {Txt} where concat (id, driver, car, cargo, weightcar, price) like '%" + searchBox.Text + "%'";
            SqlCommand sqlCommand = new SqlCommand(stringSelect, data.GetConn());
            data.OpenConn();

            SqlDataReader dataReader = sqlCommand.ExecuteReader();
            while(dataReader.Read())
                ReadSingleRaw(dataGridView, dataReader);

            dataReader.Close();
        }
        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }
        /// <summary>
        /// Method for deleting from a table
        /// </summary>
        
        private void Delete()
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            dataGridView1.Rows[index].Visible = false;
            if (dataGridView1.Rows[index].Cells[0].Value.ToString() == String.Empty)
            {
                dataGridView1.Rows[index].Cells[8].Value = RowState.Deleted;
                return;
            }
            dataGridView1.Rows[index].Cells[8].Value = RowState.Deleted;
            data.OpenConn();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var rowState = (RowState)dataGridView1.Rows[i].Cells[8].Value;
                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var id = Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value);
                    var deleteQuery = $"DELETE FROM {Txt} where id = {id}";
                    SqlCommand sqlCommand = new SqlCommand(deleteQuery, data.GetConn());
                    sqlCommand.ExecuteNonQuery();
                }
            }
            data.CloseConn();
        }
        private void deleteBtn_Click_1(object sender, EventArgs e)
        {
            Delete();
            Clear();
        }
        private void Refresh_Click_1(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
            Clear();
        }
        /// <summary>
        /// Method for changing entry in table
        /// </summary>
        
        private void Change()
        {
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
            var id = IDBox.Text;
            var driver = DriverBox.Text;
            var car = CarBox.Text;
            var cargo = CargoBox.Text;
            int weight;
            int price;
            var start = StartBox.Text;
            var end = EndBox.Text;
            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != String.Empty)
            {
                if(int.TryParse(WeightBox.Text,out weight) && int.TryParse(PriceBox.Text,out price))
                {
                    dataGridView1.Rows[selectedRowIndex].SetValues(id,driver, car, cargo, weight, price, start, end);
                    dataGridView1.Rows[selectedRowIndex].Cells[8].Value = RowState.Modified;
                }
                else
                    MessageBox.Show("Please fill in the lines correctly");
            }
            data.OpenConn();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var rowState = (RowState)dataGridView1.Rows[i].Cells[8].Value;
                if (rowState == RowState.Modified)
                {
                    var id1 = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    var driver1 = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    var car1 = dataGridView1.Rows[i].Cells[2].Value.ToString();
                    var cargo1 = dataGridView1.Rows[i].Cells[3].Value.ToString();
                    var weight1 = dataGridView1.Rows[i].Cells[4].Value.ToString();
                    var price1 = dataGridView1.Rows[i].Cells[5].Value.ToString();
                    var start1 = dataGridView1.Rows[i].Cells[6].Value.ToString();
                    var end1 = dataGridView1.Rows[i].Cells[7].Value.ToString();
                    var updateQuery = $"UPDATE {Txt} SET driver='{driver1}', car='{car1}', cargo='{cargo1}', weightcar='{weight1}', price='{price1}', start_Point='{start1}', end_Point='{end1}' where id='{id1}'";
                    SqlCommand cmd = new SqlCommand(updateQuery, data.GetConn());
                    cmd.ExecuteNonQuery();
                }
            }
            data.CloseConn();
        }
        private void changeBtn_Click_1(object sender, EventArgs e)
        {
            Change();
            Clear();
        }
        /// <summary>
        /// Method for clearing
        /// </summary>
        private void Clear()
        {
            IDBox.Text = "";
            DriverBox.Text = "";
            CarBox.Text = "";
            CargoBox.Text = "";
            WeightBox.Text = "";
            PriceBox.Text = "";
            StartBox.Text = "";
            EndBox.Text = "";
        }
    }
    enum RowState
    {
        Existed,
        Nex,
        Modified,
        ModifiedNew,
        Deleted
    }
}
