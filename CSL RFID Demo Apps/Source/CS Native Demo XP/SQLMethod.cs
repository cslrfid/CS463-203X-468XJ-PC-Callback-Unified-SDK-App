/*
Copyright (c) 2023 Convergence Systems Limited

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace CS203_CALLBACK_API_DEMO
{
    class SQLMethod
    {
        private static string connectionString = string.Empty;
        private static SqlConnection sqlConn;

        public SQLMethod()
        {
            connectionString = string.Format("Data Source={0}\\{1};Initial Catalog={2};User ID={3};Password={4}",
                                            LocalSettings.ServerIP,
                                            LocalSettings.ServerName,
                                            LocalSettings.DBName,
                                            LocalSettings.UserID,
                                            LocalSettings.Password);
        }

        /// <summary>
        /// This method attempts to create the TagInfo SQL table.
        /// </summary>
        public bool Prepare()
        {
            //
            // Make sure connection is ok.
            //
            using (sqlConn = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConn.Open();
                }
                catch (System.Data.SqlClient.SqlException ee)
                {
                    MessageBox.Show(ee.Message.ToString() + "\nPlease check SQL settings");
                    return false;
                }

                using (SqlCommand command = new SqlCommand("CREATE TABLE TagInfo (TagId TEXT, TagTime DATETIME)", sqlConn))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        //MessageBox.Show("Could not create table.");
                    }
                }

                try
                {
                    sqlConn.Close();
                }
                catch (System.Data.SqlClient.SqlException ee)
                {
                    MessageBox.Show(ee.Message.ToString());
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Insert tag ID data into the SQL database table.
        /// </summary>
        /// <param name=epc>The tag ID.</param>
        public void AddData(string epc)
        {
            //
            // Create a DataTable with two columns.
            //
            DataTable table = new DataTable();
            table.Columns.Add("TagId", typeof(string));
            table.Columns.Add("TagTime", typeof(DateTime));

            //
            // Add data to the DataTable.
            //
            table.Rows.Add(epc, DateTime.Now);

            //
            // Create new SqlConnection, SqlDataAdapter, and builder.
            // 
            using (sqlConn = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM TagInfo", sqlConn))
            using (new SqlCommandBuilder(adapter))
            {
                try
                {
                    //
                    // Fill the DataAdapter with the values in the DataTable.
                    //
                    adapter.Fill(table);
                    //
                    // Open the connection to the SQL database.
                    //
                    sqlConn.Open();
                    //
                    // Update the SQL database table with the values.
                    //
                    adapter.Update(table);

                    //
                    // Close the connection to the SQL database.
                    //
                    sqlConn.Close();
                }
                catch (System.Data.SqlClient.SqlException ee)
                {
                    MessageBox.Show(ee.Message.ToString());
                }
            }
        }

        /// <summary>
        /// Get tag ID data from the SQL database table.
        /// </summary>
        public void GetData(ListView listview)
        {
            using (sqlConn = new SqlConnection(connectionString))
            {
                sqlConn.Open();
                using (SqlDataReader reader = new SqlCommand("SELECT * FROM TagInfo", sqlConn).ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            string[] rowitem = new string[2];
                            rowitem[0] = reader.GetString(reader.GetOrdinal("TagId"));
                            rowitem[1] = reader.GetDateTime(reader.GetOrdinal("TagTime")).ToString();
                            ListViewItem listitem = new ListViewItem(rowitem);
                            //table.Rows.Add(epc, time);
                            listview.Items.Add(listitem);
                        }
                        reader.Close();
                    }
                    catch (System.Data.SqlClient.SqlException ee)
                    {
                        MessageBox.Show(ee.Message.ToString());
                    }
                }
                sqlConn.Close();
            }
        }

    }
}
