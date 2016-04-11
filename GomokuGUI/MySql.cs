using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Collections;

namespace GomokuGUI
{
    class MySql
    {
        readonly static string server = "***********";
        readonly static string id = "*********";
        readonly static string pwd = "********";
        readonly static string database = "********";

        MySqlConnection conn;

        public MySql()
        {
            string connStr = String.Format("server={0};user id={1}; password={2}; database={3}; pooling=false", server, id, pwd, database);
            conn = new MySqlConnection(connStr);
        }

        public bool checkuser(string username,string password)
        {
            conn.Open();
            string sql = "select * from userinfo where user=@username";
            MySqlCommand cmd1 = new MySqlCommand(sql, conn);
            cmd1.Parameters.AddWithValue("@username", username);
            MySqlDataReader reader = cmd1.ExecuteReader();
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    if (reader.GetString(1).Equals(password))
                    {
                        conn.Close();
                        return true;
                    }
                }
            }

            conn.Close();
            return false;
        }

        public void insert(int level, string first, string second, string winner, int duration, int round, string pos)
        {
            //DateTime dt = DateTime.Parse("1970-01-01 00:00:00");
            //TimeSpan ts = DateTime.Now - dt;
            //int time = Convert.ToInt32(ts.TotalSeconds);

            conn.Open();
            string sql = "insert into history values(null, now(), @level, @first, @second, @winner, @duration, @round, @pos)";
            MySqlCommand cmd1 = new MySqlCommand(sql, conn);

            //cmd1.Parameters.AddWithValue("@time", time);
            cmd1.Parameters.AddWithValue("@level", level);
            cmd1.Parameters.AddWithValue("@first", first);
            cmd1.Parameters.AddWithValue("@second", second);
            cmd1.Parameters.AddWithValue("@winner", winner);
            cmd1.Parameters.AddWithValue("@duration", duration);
            cmd1.Parameters.AddWithValue("@round", round);
            cmd1.Parameters.AddWithValue("@pos", pos);
            cmd1.ExecuteNonQuery();
            conn.Close();       
        }

        public ArrayList select()
        {
            ArrayList history = new ArrayList();
            conn.Open();
            string sql = "select * from history";
            MySqlCommand cmd1 = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd1.ExecuteReader();
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    string[] record = new string[8];
                    for(int i=0;i<8;i++)
                        record[i] = reader.GetString(i+1);
                    record[5] += 's';
                    history.Add(record);
                }
            }

            conn.Close();

            return history;
 
        }

    }
}
