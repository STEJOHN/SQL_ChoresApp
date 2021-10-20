using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SqlServerSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Connect to SQL Server and demo Create, Read, Update and Delete operations.");

                // Build connection string
                SqlConnectionStringBuilder sbSQL = new SqlConnectionStringBuilder();
                sbSQL.DataSource = "WINDOWS10-PC\\SQLSERVER2019";   // update me
                sbSQL.UserID = "Sa";              // update me
                sbSQL.Password = "Temp123!";      // update me
                sbSQL.InitialCatalog = "master";

                // Connect to SQL
                Console.Write("Connecting to SQL Server ... ");
                using (SqlConnection connection = new SqlConnection(sbSQL.ConnectionString))
                {
                    connection.Open();
                    Console.WriteLine("Done.");

                    // Create a Chores database 
                    Console.Write("Dropping and creating database 'ChoresDB' ... ");
                    String sql = "DROP DATABASE IF EXISTS [ChoresDB]; CREATE DATABASE [ChoresDB]";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Done.");
                    }

                    // Create a Table and insert data
                    Console.Write("Creating a table for Chores with ID,Name, and assignment, press any key to continue...");
                    Console.ReadKey(true);
                    StringBuilder sb = new StringBuilder();
                    sb.Append("USE ChoresDB; ");
                    sb.Append("CREATE TABLE Chores ( ");
                    sb.Append(" ChoreId INT IDENTITY(1,1) NOT NULL PRIMARY KEY, ");
                    sb.Append(" ChoreName NVARCHAR(MAX), ");
                    sb.Append(" ChoreAssignment NVARCHAR(MAX) ");
                    sb.Append("); ");
                    sb.Append("INSERT INTO Chores (ChoreName, ChoreAssignment) VALUES ");
                    sb.Append("(N'Stephen', N'Walk the dog'), ");
                    sb.Append("(N'Noah', N'Vacuum room'), ");
                    sb.Append("(N'Holly', N'Make table for dinner'); ");
                    //sb.Append("(N'Natileah', N'Take out trash'); ");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Done.");
                    }

                    // INSERT 
                    Console.Write("Inserting a new row into table, press any key to continue...");
                    Console.ReadKey(true);
                    sb.Clear();
                    sb.Append("INSERT Chores (ChoreName, ChoreAssignment) ");
                    sb.Append("VALUES (@ChoreName, @ChoreAssignment);");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ChoreName", "Mowing grass");
                        command.Parameters.AddWithValue("@ChoreAssignment", "Sophia");
                        var rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " row(s) inserted");
                    }

                    // UPDATE 
                    var userToUpdate = "Holly";
                    Console.Write("Updating 'ChoreAssignment' for user '" + userToUpdate + "', press any key to continue...");
                    Console.ReadKey(true);
                    sb.Clear();
                    sb.Append("UPDATE Chores SET ChoreAssignment = N'Clean the fish tank' WHERE ChoreAssignment = @ChoreAssignment");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ChoreAssignment", userToUpdate);
                        var choresAffected = command.ExecuteNonQuery();
                        Console.WriteLine(choresAffected + " Chore Assignment updated");
                    }

                    // DELETE 
                    var choresToDelete = "Vacuum room";
                    Console.Write("Deleting chore '" + choresToDelete + "', press any key to continue...");
                    Console.ReadKey(true);
                    sb.Clear();
                    sb.Append("DELETE FROM Chores WHERE ChoreAssignment = @ChoreAssignment;");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ChoreAssignment", choresToDelete);
                        var choresAffected = command.ExecuteNonQuery();
                        Console.WriteLine(choresAffected + " chore deleted");
                    }

                    // READ 
                    Console.WriteLine("Reading data from table, press any key to continue...");
                    Console.ReadKey(true);
                    sql = "SELECT ChoreId, choreName, ChoreAssignment FROM Chores;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("{0} {1} {2}", reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("All done. Press any key to finish...");
            Console.ReadKey(true);
        }
    }
}