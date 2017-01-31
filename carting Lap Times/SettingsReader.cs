using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
namespace carting_Lap_Times
{
    class SettingsReader
    {
        private XmlReader reader = XmlReader.Create("DataBase_Settings.xml");
        public SettingsReader()
        {

        }

        public string ConnectionString()
        {
            string Server = ""; 
            string Database = ""; 
            string Uid = "";
            string Pwd = "";
            string ConnectionString = "";

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Server")
                {
                    Server = reader.GetAttribute(0);
                    Database = reader.GetAttribute(1);
                    Uid = reader.GetAttribute(2);
                    Pwd = reader.GetAttribute(3);
                }
            }
            ConnectionString = "Server=" + Server + ";Database=" + Database + ";Uid=" + Uid + ";Pwd=" + Pwd + ";Convert Zero Datetime=True;";
            return ConnectionString;
        }
    }
}
