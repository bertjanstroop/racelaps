﻿using System;
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
using MySql.Data.MySqlClient;
namespace carting_Lap_Times
{
    /// <summary>
    /// Interaction logic for NewTrackTimes.xaml
    /// </summary>
    public partial class NewTrackTimes : Window
    {
        SQLConnection sql = new SQLConnection();
        DataTable RaceTracks = new DataTable();
        public NewTrackTimes()
        {
            InitializeComponent();
            LoadData();
            FillcmbRaceTrack();
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

        private void FillcmbRaceTrack()
        {
            foreach (DataRow row in RaceTracks.Rows)
            {
                cmbRaceTrack.Items.Add( row[0].ToString());
            }
            cmbRaceTrack.SelectedIndex = 0;
        }

        private void SaveTrackTimes()
        {
            string Query = "insert into laptimes (racetrack,date,time,lap1,lap2,lap3,lap4,lap5,lap6,lap7,lap8,lap9,lap10,lap11,lap12,lap13,lap14,lap15,lap16,lap17,lap18,lap19,lap20,lap21,lap22,note)"
                + " VALUES (@raceTrack,@date,@time,@lap1,@lap2,@lap3,@lap4,@lap5,@lap6,@lap7,@lap8,@lap9,@lap10,@lap11,@lap12,@lap13,@lap14,@lap15,@lap16,@lap17,@lap18,@lap19,@lap20,@lap21,@lap22,@note)";
            sql.cmd.Connection = sql.cs;
            sql.cmd.CommandText = Query;
            sql.cmd.Parameters.AddWithValue("@racetrack",cmbRaceTrack.SelectedItem.ToString());
            sql.cmd.Parameters.AddWithValue("@date",RaceDate.SelectedDate.Value.ToShortDateString());
            sql.cmd.Parameters.AddWithValue("@time",txtRaceTime.Text);
            sql.cmd.Parameters.AddWithValue("@lap1",txtLap1.Text);
            sql.cmd.Parameters.AddWithValue("@lap2", txtLap2.Text);
            sql.cmd.Parameters.AddWithValue("@lap3", txtLap3.Text);
            sql.cmd.Parameters.AddWithValue("@lap4", txtLap4.Text);
            sql.cmd.Parameters.AddWithValue("@lap5", txtLap5.Text);
            sql.cmd.Parameters.AddWithValue("@lap6", txtLap6.Text);
            sql.cmd.Parameters.AddWithValue("@lap7", txtLap7.Text);
            sql.cmd.Parameters.AddWithValue("@lap8", txtLap8.Text);
            sql.cmd.Parameters.AddWithValue("@lap9", txtLap9.Text);
            sql.cmd.Parameters.AddWithValue("@lap10", txtLap10.Text);
            sql.cmd.Parameters.AddWithValue("@lap11", txtLap11.Text);
            sql.cmd.Parameters.AddWithValue("@lap12", txtLap12.Text);
            sql.cmd.Parameters.AddWithValue("@lap13", txtLap13.Text);
            sql.cmd.Parameters.AddWithValue("@lap14", txtLap14.Text);
            sql.cmd.Parameters.AddWithValue("@lap15", txtLap15.Text);
            sql.cmd.Parameters.AddWithValue("@lap16", txtLap16.Text);
            sql.cmd.Parameters.AddWithValue("@lap17", txtLap17.Text);
            sql.cmd.Parameters.AddWithValue("@lap18", txtLap18.Text);
            sql.cmd.Parameters.AddWithValue("@lap19", txtLap19.Text);
            sql.cmd.Parameters.AddWithValue("@lap20", txtLap20.Text);
            sql.cmd.Parameters.AddWithValue("@lap21", txtLap21.Text);
            sql.cmd.Parameters.AddWithValue("@lap22", txtLap22.Text);
            sql.cmd.Parameters.AddWithValue("@note", txtNote.Text);
            sql.cs.Open();
            sql.cmd.ExecuteNonQuery();
            sql.cs.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to save these TrackTimes?","Confirm",MessageBoxButton.YesNo)== MessageBoxResult.Yes)
            {
                SaveTrackTimes();
                this.Close(); 
            }
            else
            {
                MessageBox.Show("Nothing is saved");
            }
        }
    }
}
