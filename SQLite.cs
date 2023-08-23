using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckPrice
{
    internal class SQLite
    {
        static SQLiteConnection connect;
        public SQLite()
        {
            if (!File.Exists("SQLite.db"))
                    File.WriteAllBytes("SQLite.db", new byte[0]);
            connect = new SQLiteConnection("Data Source=SQLite.db");
        }
        public DataTable SQLiteGet(string sqlQuery)
        {
            try
            {
                connect.Open();
                DataTable dt = new DataTable();
                var adapter = new SQLiteDataAdapter(sqlQuery, connect);
                adapter.Fill(dt);

                connect.Close();
                connect.Dispose();

                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public void SQLiteInsert(List<CatalogList> list, string group)
        {
            try
            {
                connect.Open();
                var commandTable = new SQLiteCommand($@"CREATE TABLE IF NOT EXISTS {group} (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, company TEXT, price REAL, discount REAL);");
                commandTable.ExecuteNonQuery();

                using (var transaction = connect.BeginTransaction())
                {
                    var command = connect.CreateCommand();
                    command.CommandText =
                    @"INSERT INTO data (name, company, price, discount)
                        VALUES ($name, $company, $price, $discount)";

                    var nameParameter = command.CreateParameter();
                    nameParameter.ParameterName = "$name";
                    command.Parameters.Add(nameParameter);
                    var companyParameter = command.CreateParameter();
                    companyParameter.ParameterName = "$company";
                    command.Parameters.Add(companyParameter);
                    var priceParameter = command.CreateParameter();
                    priceParameter.ParameterName = "$price";
                    command.Parameters.Add(companyParameter);
                    var discountParameter = command.CreateParameter();
                    companyParameter.ParameterName = "$discount";
                    command.Parameters.Add(companyParameter);

                    for (int i = 0; i < list.Count; i++)
                    {
                        nameParameter.Value = list[i].Name;
                        companyParameter.Value = list[i].Company;
                        priceParameter.Value = list[i].Price;
                        discountParameter.Value = list[i].DiscontPrice;
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
