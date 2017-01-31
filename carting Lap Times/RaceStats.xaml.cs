using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Controls.DataVisualization.Charting;
namespace carting_Lap_Times
{
    /// <summary>
    /// Interaction logic for RaceStats.xaml
    /// </summary>
    public partial class RaceStats : Window
    {
        SQLConnection sql = new SQLConnection();
        private string ID;
        private DataTable LapTimes = new DataTable();
        private Dictionary<string, double> GraphData = new Dictionary<string, double>();
        public RaceStats(string ID)
        {
            InitializeComponent();
            this.ID = ID;
            LoadData();
            FillLapTimeLabels();
            INITGraphDataDict();
            GenerateGraphData();
            DrawGraph();
        }

        private void LoadData()
        {
            LapTimes.Clear();
            string Query = "select lap1,lap2,lap3,lap4,lap5,lap6,lap7,lap8,lap9,lap10,lap11,lap12,lap13,lap14,lap15,lap16,lap17,lap18,lap19,lap20,lap21,lap22 from laptimes where id='" + ID + "'" ;
            sql.da = new MySqlDataAdapter(Query, sql.cs);
            sql.cs.Open();
            sql.da.Fill(LapTimes);
            sql.cs.Close();
        }

        private void FillLapTimeLabels()
        {
            List<double> Laps = new List<double>();
            foreach (DataRow row in LapTimes.Rows)
            {
                foreach (DataColumn column in LapTimes.Columns)
                {
                    Laps.Add(double.Parse( row[column].ToString()));
                }
            }
            //remove 0.0 values
            var itemToRemove = Laps.Where(r => r == 0.0).ToList();
            foreach (var item in itemToRemove)
            {
                Laps.Remove(item);
            }

            lblFastestLapTime.Content = Laps.Min();
            lblSlowestLapTime.Content = Laps.Max();
            lblAverageLapTime.Content = Math.Round( Laps.Average(),2);
        }

        private void INITGraphDataDict()
        {
            GraphData.Clear();
            for (int i = 1; i <= 22; i++)
            {
                GraphData.Add(("Lap" + i),0); 
            }
        }

        private void GenerateGraphData()
        {
            int _Count = 1;
            foreach (DataRow row in LapTimes.Rows)
            {
                foreach (DataColumn column in LapTimes.Columns)
                {
                    GraphData[("Lap" + _Count)] = double.Parse(row[column].ToString());
                    _Count++;
                }
            }
        }

        private void DrawGraph()
        {
            ((ColumnSeries)WPFChart.Series[0]).ItemsSource = null;
            ((ColumnSeries)WPFChart.Series[0]).ItemsSource = GraphData;
        }

        private void DeleteRecord()
        {
            string Query = "delete from laptimes where id=@id";
            sql.cmd.Connection = sql.cs;
            sql.cmd.CommandText = Query;
            sql.cmd.Parameters.AddWithValue("@id",ID);
            sql.cs.Open();
            sql.cmd.ExecuteNonQuery();
            sql.cs.Close();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure that you want to remove this record? \nThis window will close if you Click yes!","Confirm",MessageBoxButton.YesNo)== MessageBoxResult.Yes)
            {
                DeleteRecord();
                this.Close();
            }
            else
            {
                MessageBox.Show("Canceled");
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            EditTrackTimes edt = new EditTrackTimes(ID);
            edt.ShowDialog();
            LoadData();
            FillLapTimeLabels();
            INITGraphDataDict();
            GenerateGraphData();
            DrawGraph();
        }
    }
}
