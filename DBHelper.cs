using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace University_Information_System
{
    internal class DB
    {
    }

    public class DBHelper
    {
        public static string connStr = "server=localhost;user=admin_user;password=admin123;database=univ_information_sys;";
        public static MySqlConnection conn = new MySqlConnection(connStr);
    }
}
