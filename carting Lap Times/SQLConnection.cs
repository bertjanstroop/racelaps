using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace carting_Lap_Times
{
    class SQLConnection
    {
        public MySqlConnection cs;
        public MySqlCommand cmd;
        public MySqlDataAdapter da;
        SettingsReader sr = new SettingsReader();
        public SQLConnection()
        {
            cs = new MySqlConnection(sr.ConnectionString());
            cmd = new MySqlCommand();
        }

        public string TestConnection()
        {
            string result = "";
            if (cs.Ping())
            {
                result = "Verbinding gelukt \nServer is online";
            }
            else
            {
                result = "Verbinding mislukt \nServer is ofline of niet benaderbaar";
            }
            return result;
        }


    }
}
