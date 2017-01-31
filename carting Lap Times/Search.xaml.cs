using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace carting_Lap_Times
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        SQLConnection sql = new SQLConnection();
        DataTable RaceTracks = new DataTable();
        string _RaceTrack;
        string _RaceDate;
        public Search()
        {
            InitializeComponent();
            LoadData();
            FillcmbRaceTrack();
        }

        private void FillcmbRaceTrack()
        {
            foreach (DataRow row in RaceTracks.Rows)
            {
                cmbRaceTrack.Items.Add(row[0].ToString());
            }
            cmbRaceTrack.SelectedIndex = 0;
        }

        private void LoadData()
        {
            string Query = "Select racetrack from racetrack";
            sql.cmd.Parameters.Clear();
            sql.da = new MySqlDataAdapter(Query, sql.cs);
            sql.cs.Open();
            sql.da.Fill(RaceTracks);
            sql.cs.Close();
        }

        public object[] ReturnSearchParameters()
        {
            object[] parameters = new object[2];
            if (chkRacetrackEnable.IsChecked == true)
            {
                parameters[0] = _RaceTrack;
            }
            else
            {
                parameters[0] = null;
            }
            if (chkDateEnable.IsChecked == true)
            {
                parameters[1] = _RaceDate;
            }
            else
            {
                parameters[1] = null;
            }
            return parameters;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            _RaceTrack = cmbRaceTrack.SelectedItem.ToString();
            if (chkDateEnable.IsChecked == true)
            {
                _RaceDate = calRace.SelectedDate.Value.ToShortDateString(); 
            }
            else
            {
                _RaceDate = string.Empty;
            }
            Close();
        }
    }
}
