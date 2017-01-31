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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;
namespace carting_Lap_Times
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SQLConnection sql = new SQLConnection();
        DataTable LapTimes = new DataTable();
        DataTable RaceTracks = new DataTable();
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            LoadraceTrackData();
            FillcmbRaceTrack();
        }

        private void FillcmbRaceTrack()
        {
            foreach (DataRow row in RaceTracks.Rows)
            {
                cmbRaceTrack.Items.Add(row[0].ToString());
            }
            //cmbRaceTrack.SelectedIndex = 0;
        }

        private void LoadraceTrackData()
        {
            string Query = "Select racetrack from racetrack";
            sql.cmd.Parameters.Clear();
            sql.da = new MySqlDataAdapter(Query, sql.cs);
            sql.cs.Open();
            sql.da.Fill(RaceTracks);
            sql.cs.Close();
        }

        private void btnRaceTracks_Click(object sender, RoutedEventArgs e)
        {
            ManageRaceTracks mrt = new ManageRaceTracks();
            mrt.ShowDialog();
            LoadData();
            LoadraceTrackData();
            FillcmbRaceTrack();
        }

        private void btnNewTrackTimes_Click(object sender, RoutedEventArgs e)
        {
            NewTrackTimes ntt = new NewTrackTimes();
            ntt.ShowDialog();
            LoadData();

        }

        private void LoadData()
        {
            try
            {
                RaceGrid.ItemsSource = null;
                LapTimes.Clear();
                string Query = "Select * from laptimes";
                sql.da = new MySqlDataAdapter(Query, sql.cs);
                sql.cs.Open();
                sql.da.Fill(LapTimes);
                sql.cs.Close();
                RaceGrid.ItemsSource = LapTimes.AsDataView();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadData(MySqlDataAdapter da)
        {
            try
            {
                RaceGrid.ItemsSource = null;
                LapTimes.Clear();
                sql.cs.Open();
                da.Fill(LapTimes);
                sql.cs.Close();
                RaceGrid.ItemsSource = LapTimes.AsDataView();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RaceGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string ID = ((DataRowView)RaceGrid.SelectedItem).Row[0].ToString();
                RaceStats rs = new RaceStats(ID);
                rs.ShowDialog();
                LoadData();
            }
            catch (NullReferenceException)
            {
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Search s = new Search();
            s.ShowDialog();
            ExecuteSearch(s.ReturnSearchParameters());
        }

        /// <summary>
        /// param 0 is racetrack, param 1 is race date
        /// </summary>
        /// <param name="SearchParameters"></param>
        private void ExecuteSearch(object[] SearchParameters)
        {

            if (SearchParameters[0] == null && SearchParameters[1] == null)
            {
                //do nothing, just call loaddata
                LoadData();
            }
            if (SearchParameters[0] != null && SearchParameters[1] == null)
            {
                //search for racetracks
                string Query = "select * from laptimes where racetrack =@racetrack";
                MySqlDataAdapter da = new MySqlDataAdapter(Query,sql.cs);
                da.SelectCommand.Parameters.AddWithValue("@racetrack",SearchParameters[0]);
                LoadData(da);
            }
            if (SearchParameters[0] == null && SearchParameters[1] != null)
            {
                //search for date
                string Query = "select * from laptimes where date =@date";
                MySqlDataAdapter da = new MySqlDataAdapter(Query, sql.cs);
                da.SelectCommand.Parameters.AddWithValue("@date", SearchParameters[1]);
                LoadData(da);
            }
            if (SearchParameters[0] != null && SearchParameters[1] != null)
            {
                //search for both racetrack and date
                string Query = "select * from laptimes where racetrack =@racetrack and date =@date";
                MySqlDataAdapter da = new MySqlDataAdapter(Query, sql.cs);
                da.SelectCommand.Parameters.AddWithValue("@racetrack", SearchParameters[0]);
                da.SelectCommand.Parameters.AddWithValue("@Date", SearchParameters[1]);
                LoadData(da);
            }
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void btnSearch_Click_1(object sender, RoutedEventArgs e)
        {
            ExecuteSearch(ReturnSearchParameters());
        }

        public object[] ReturnSearchParameters()
        {
            object[] parameters = new object[2];
            //chec racetrack
            if (cmbRaceTrack.SelectedItem != null)
            {
                parameters[0] = cmbRaceTrack.SelectedItem.ToString();
            }
            else
            {
                parameters[0] = null;
            }
            //check date
            if (RaceDate.SelectedDate != null)
            {
                parameters[1] = RaceDate.SelectedDate.Value.ToShortDateString();
            }
            else
            {
                parameters[1] = null;
            }
            return parameters;
        }

        private void btnResetSearch_Click(object sender, RoutedEventArgs e)
        {
            cmbRaceTrack.SelectedIndex = 0;
            RaceDate.SelectedDate = null;
            LoadData();
        }
    }
}
