using MySql.Data.MySqlClient;
using System.Data;
using static Test.ADO.Database;

namespace Test.ADO
{
    public class Database
    {
        public class Dataprovider
        {
            private string constr = @"Data Source=.\sqlexpress;Initial Catalog=SendMail;Integrated Security=True";
            private static Dataprovider instance; 

            public static Dataprovider Instance
            {
                get
                {
                    if (instance == null)
                        instance = new Dataprovider();
                    return instance;
                }
                private set
                {
                    instance = value;
                }
            }

            public static object ConfigurationManager { get; private set; }

            private Dataprovider() { }
            public DataTable ExecuteQuery(string query, object[] parameter = null)
            {
                DataTable tb;
                using (MySqlConnection cnn = new MySqlConnection(constr))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, cnn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cnn.Open();
                        if (parameter != null)
                        {
                            string[] listPara = query.Split(' ');
                            int j = 0;
                            foreach (string item in listPara)
                            {
                                if (item.Contains('@'))
                                {
                                    cmd.Parameters.AddWithValue(item, parameter[j]);
                                    j++;
                                }
                            }
                        }
                        using (MySqlDataAdapter adap = new MySqlDataAdapter(cmd))
                        {
                            tb = new DataTable();
                            adap.Fill(tb);
                        }
                        cnn.Close();
                    }
                }
                return tb;
            }
            public int ExecuteNonQuery(string query, object[] parameter = null) 
            {
                int i;
                using (MySqlConnection cnn = new MySqlConnection(constr))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, cnn))
                    {
                        cnn.Open();
                        if (parameter != null)
                        {
                            string[] listPara = query.Split(' ');
                            int j = 0;
                            foreach (string item in listPara)
                            {
                                if (item.Contains('@'))
                                {
                                    cmd.Parameters.AddWithValue(item, parameter[j]);
                                    j++;
                                }
                            }
                        }
                        i = cmd.ExecuteNonQuery();
                        cnn.Close();
                    }
                }
                return i;
            }
        }
    }
}
