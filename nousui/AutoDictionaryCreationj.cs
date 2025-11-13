using System;
using System.Collections.Generic;
using System.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connectionString = "Server=サーバー名;Database=DB名;User Id=ユーザー名;Password=パスワード;";
        string query = "SELECT id, name, pass FROM Users"; // 例として Users テーブル

        List<Dictionary<string, object>> records;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                records = new List<Dictionary<string, object>>();

                while (reader.Read())
                {
                    var row = new Dictionary<string, object>();
                    row["id"] = reader["id"];
                    row["name"] = reader["name"];
                    row["pass"] = reader["pass"];
                    records.Add(row);
                }
            }
        }

        // 確認出力
        foreach (var row in records)
        {
            Console.WriteLine($"id={row["id"]}, name={row["name"]}, pass={row["pass"]}");
        }
    }
}