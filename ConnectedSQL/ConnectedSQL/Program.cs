using ConnectedSQL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedSql
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Block 3
            #region Connected SQL Notes
            //Connected SQL is a part of ADO.NET (Active DataX Objects -> Microsoft's solution to incorporating data
            //into applications).  
            //With this implementation, we typically retrieve items from the database and convert those to 
            //C# objects.  Once that is done, we can display them.

            /*
             *  In order to retrieve data from the database we must define the connection string
             *  (roadmap the database)
             *  ConnectionString 3 basic parts: Data Source (SERVER), Initial Catalog (Db Name)
             *  Integrated Security (true/false/sspi). True indicates Windows (integrated) authentication.
             *  False indicates Sql Server authentication which requires a user name and password combo.
             *  SSPI basically works the same as false, but it rarely used.
             *  
             *  SqlConnection Object.  The connection object requires a connection string as a parameter
             *  to build it.  To acces the data, call SqlConnection.Open() then articulate our query.
             *  
             *  SqlCommand Object.  The command requires a minimum of CommandText (sql query as a string)
             *  and a SqlConnection object, to initialize the request.
             *  If a parameterized query is written, you can use the Paramters property and the 
             *  AddWithValue() to specify both the parameter AND its value.
             *  Available Methods (SqlCommand)
             *  ExecuteReader() - SELECT statements - ReturnType is SqlDataReader
             *  ExecuteNonQuery() - INSERT, UPDATE, DELETE statments - ReturnType is an int.
             *                    - Usually used to show RowsAffected by the statement
             *  ExecuteScalar()   - Aggragate Functions: SUM, AVG, COUNT, etc. - ReturnType Object
             *  
             *  SqlDataReader - This object holds the results of the command object's ExecuteReader().
             *  The reader will need to be looped through (if more than result is desired) or branched
             *  (if or switch) if a single result is required.  To call ExecuteReader() the connection
             *  object's Open() must have been called.  When the reader is done, you should call
             *  SqlDataReader.Close().  You should also close the connection as soon as possible.
             *  (SqlConnection.Close()).
             */
            #endregion

            #region Set up DB Connection

            //Connection string needed by the console app to connect to the db
            string cs = @"Data Source=.\sqlexpress;Initial Catalog=GadgetStore;Integrated Security=true;";

            SqlConnection conn = new SqlConnection(cs);
            #endregion

            #region Example -> Retrieve a single category
            Console.WriteLine("*** Example of retrieving a single Category from the DB to display. **\n----------------------\n");

            //Open the gates, let the data flow!
            conn.Open();

            //The Command object is the text for your SQL query
            //NOTE: You MUST have two args with the command object -> QueryText, Connection object (conn)
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 c.CategoryID, c.CategoryName, c.CategoryDescription FROM Categories c", conn);

            //The Reader object allows us to peruse the data that we retrieve from the db
            SqlDataReader rdr = cmd.ExecuteReader();

            //output string
            string result = "";

            //Make the rdr read the object to us. This essentially puts the record in memory.
            rdr.Read();

            //After reading, grab the specific column values we want to dsiplay and concat those to our output string
            result += $"{rdr["CategoryName"]}\n- {rdr["CategoryDescription"]}\n\n";

            //When finished querying, you MUST close the reader AND the connection. 
            //Failure to do so WILL result in catastrophe on your machine.
            conn.Close();
            rdr.Close();

            Console.WriteLine(result);
            #endregion

            Console.Write("\n\nPress any key to continue...\n");
            Console.ReadKey(true);
            Console.Clear();

            #region Retrieve ALL categories
            Console.WriteLine("\n\n*** Example of retrieving all Categories from the DB to display.***" +
                "\n\n**************************\n");

            conn.Open();

            cmd.CommandText = "SELECT CategoryName, CategoryDescription FROM Categories";

            rdr = cmd.ExecuteReader();

            //empty old string
            result = "";

            while (rdr.Read())
            {
                result += $"{rdr["CategoryName"]}\n- {rdr["CategoryDescription"]}\n\n";
            }

            rdr.Close();
            conn.Close();

            Console.WriteLine(result);

            #endregion

            Console.Write("\n\nPress any key to continue...\n");
            Console.ReadKey(true);
            Console.Clear();

            #region Retrieve all results from vwGadgetsOverview
            Console.WriteLine("\n\n*** Example of displaying a view from the DB.***" +
                "\n\n**************************\n");

            conn.Open();

            cmd.CommandText = "SELECT * FROM vwGadgetsOverview;";

            rdr = cmd.ExecuteReader();

            //empty old string
            result = "";

            while (rdr.Read())
            {
                result += $"{rdr["ProductName"]}\n- {rdr["ProductPrice"]:c}\n\n";
            }

            rdr.Close();
            conn.Close();

            Console.WriteLine(result);


            #endregion

            Console.Write("\n\nPress any key to continue...\n");
            Console.ReadKey(true);
            Console.Clear();

            #region MINI LAB

            Console.WriteLine("Northwind Mini Lab");
            Console.WriteLine("**********************************");

            cs = cs.Replace("GadgetStore", "Northwind");

            conn.ConnectionString = cs;
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT p.ProductName, c.CategoryName " +
                              "FROM Products p JOIN Categories c " +
                              "ON p.CategoryID = c.CategoryID " +
                              "ORDER BY c.CategoryName;";
            rdr = cmd.ExecuteReader();
            result = "";
            while (rdr.Read())
            {
                result += $"\t{rdr["ProductName"]} - {rdr["CategoryName"]}\n";
            }
            rdr.Close();
            conn.Close();
            Console.WriteLine(result);

            #endregion

            Console.Write("\n\nPress any key to continue...\n");
            Console.ReadKey(true);
            Console.Clear();

            #region Mini Lab 2
            Console.WriteLine("Northwind Mini Lab 2");
            Console.WriteLine("**********************************");

            conn.Open();
            cmd.CommandText = "SELECT c.CategoryName, Count(p.ProductId) AS [Nbr of Products] " +
                              "FROM Products p " +
                                "RIGHT JOIN Categories c " +
                                    "ON p.CategoryID = c.CategoryID " +
                              "GROUP BY c.CategoryName";
            rdr = cmd.ExecuteReader();
            result = "";
            while (rdr.Read())
            {
                result += $"\"{rdr["CategoryName"]}\" has {rdr["Nbr of Products"]} products\n";
            }
            conn.Close();
            rdr.Close();
            Console.WriteLine(result);
            #endregion

            Console.Write("\n\nPress any key to continue...\n");
            Console.ReadKey(true);
            Console.Clear();
            #endregion

            #region Block 4

            #region Retrieve Cats with DomainModels

            #endregion
            Console.WriteLine("***  Retrieving all Categories from the DB with Domain Models ***");
            Console.WriteLine("**********************************");

            cs = cs.Replace("Northwind", "GadgetStore");
            conn.ConnectionString = cs;
            conn.Open();

            cmd = new SqlCommand("SELECT CategoryId, CategoryName, CategoryDescription FROM Categories", conn);
            rdr = cmd.ExecuteReader();

            List<CategoryDomainModel> categories = new List<CategoryDomainModel>();

            while (rdr.Read())
            {
                CategoryDomainModel cat = new CategoryDomainModel()
                {
                    CategoryId = (int)rdr["CategoryId"],
                    CategoryName = (string)rdr["CategoryName"],
                    CategoryDescription = rdr["CategoryDescription"] is DBNull ? "N/A" : (string)rdr["CategoryDescription"]
                };
                categories.Add(cat);
            }
            rdr.Close();
            conn.Close();

            foreach (var category in categories)
            {
                Console.WriteLine($"{category.CategoryName}\n- {category.CategoryDescription}\n");
            }

            #endregion

        }
    }
}