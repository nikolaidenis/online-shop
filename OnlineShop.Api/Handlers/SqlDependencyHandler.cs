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

        public static void Register()
        {
            Register(ConnectionString);
        }
        public static void Register(string connectionString)
        {
            ConnectionString = connectionString;
            string commandText = @"SELECT [Id], [Cost] FROM [dbo].[Products]";

            SqlDependency.Start(ConnectionString);
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.Notification = null;
                    var sqlDependency = new SqlDependency(command);
                    sqlDependency.OnChange += sqlDependency_OnChange;
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                    }
                }
            }
        }

        private static void sqlDependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            CustomHub.UpdateProductCosts();
            Register();
        }
    }
}