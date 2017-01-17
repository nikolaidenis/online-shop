using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using OnlineShop.Api.Hubs;

namespace OnlineShop.Api.Handlers
{
    public class SqlDependencyHandler
    {
        private static string ConnectionString { get; set; }
        public static void Register(string connectionString)
        {
            ConnectionString = connectionString;
            //We have selected the entire table as the command, so SQL Server executes this script and sees if there is a change in the result, raise the event
            string commandText = @"SELECT * FROM [OnlineShop].[dbo].[Products]";

            //Start the SQL Dependency
            SqlDependency.Start(ConnectionString);
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.Notification = null;
                    var sqlDependency = new SqlDependency(command);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    sqlDependency.OnChange += sqlDependency_OnChange;

                    // NOTE: You have to execute the command, or the notification will never fire.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                    }
                }
            }
        }

        private static void sqlDependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            CustomHub.UpdateProductCosts();
        }
    }
}