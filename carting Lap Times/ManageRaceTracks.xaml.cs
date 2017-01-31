using MySql.Data.MySqlClient;
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
using System.Data;
namespace carting_Lap_Times
{
    /// <summary>
    /// Interaction logic for ManageRaceTracks.xaml
    /// </summary>
    public partial class ManageRaceTracks : Window
    {
        SQLConnection sql = new SQLConnection();
        DataTable RaceTracks = new DataTable();
        public ManageRaceTracks()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                TrackGrid.ItemsSource = null;
                RaceTracks.Clear();
                string Query = "Select racetrack from racetrack";
                sql.cmd.Parameters.Clear();
                sql.da = new MySqlDataAdapter(Query, sql.cs);
                sql.cs.Open();
                sql.da.Fill(RaceTracks);
                sql.cs.Close();
                TrackGrid.ItemsSource = RaceTracks.AsDataView();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveTrack()
        {
            string Query = "insert into racetrack (racetrack) values  (@racetrack)";
            sql.cmd.Parameters.Clear();
            sql.cmd.Connection = sql.cs;
            sql.cmd.CommandText = Query;
            sql.cmd.Parameters.AddWithValue("@racetrack",txtRaceTrack.Text);
            sql.cs.Open();
            sql.cmd.ExecuteNonQuery();
            sql.cs.Close();
        }

        private void RemoveTrack(string SelectedTrack)
        {
            string Query = "delete from racetrack where racetrack=@racetrack";
            sql.cmd.Parameters.Clear();
            sql.cmd.Connection = sql.cs;
            sql.cmd.CommandText = Query;
            sql.cmd.Parameters.AddWithValue("@racetrack", SelectedTrack);
            sql.cs.Open();
            sql.cmd.ExecuteNonQuery();
            sql.cs.Close();
        }

        private void btnAddNewTrack_Click(object sender, RoutedEventArgs e)
        {
            if (txtRaceTrack.Text.Length > 0)
            {
                SaveTrack();
                txtRaceTrack.Clear();
                LoadData();
            }
            else
            {
                MessageBox.Show("Please Fill in the RaceTrack");
            }
        }

        private void btnRemoveSelectedTrack_Click(object sender, RoutedEventArgs e)
        {
            if (TrackGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Are you sure your want to remove this track?","Confirm",MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    RemoveTrack(((DataRowView)TrackGrid.SelectedItem).Row[0].ToString());
                    LoadData(); 
                }
            }
            else
            {
                MessageBox.Show("select a Track first");
            }
        }
    }
}
