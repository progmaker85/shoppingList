using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using ShoppingList.API.Services;

namespace ShoppingList.API.Repositories
{
    public class MssqlDatabaseRepository : IDatabaseRepository
    {
        private readonly string _connectionString;
        public MssqlDatabaseRepository(IConfiguration configuration)
        {
            try
            {
                _connectionString = configuration.GetConnectionString("ShoppingListDb");
            }
            catch (Exception exception)
            {
                exception.ToLogError();
            }
        }

        public void CheckOffShoppingItem(int shoppingItemId)
        {
            try
            {
                using (var dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Execute(MssqlQueriesResource.CheckOffShoppingItem, new
                    {
                        id = shoppingItemId
                    });
                }
            }
            catch (Exception exception)
            {
                exception.ToLogError();
            }
        }
        public void AddShoppingItem(ShoppingItem shoppingItem)
        {
            try
            {
                using (var dbConnection = new SqlConnection(_connectionString))
                {
                    var result = dbConnection.Execute(MssqlQueriesResource.InsertShoppingItem, shoppingItem);
                    if (result == 0) throw new Exception("Inserting ShoppingItem has failed.");
                }
            }
            catch (Exception exception)
            {
                exception.ToLogError();
            }
        }
        public IList<ShoppingItem> GetBoughtItems()
        {
            var result = new List<ShoppingItem>();
            try
            {
                using (var dbConnection = new SqlConnection(_connectionString))
                {
                    result.AddRange(dbConnection.Query<ShoppingItem>(MssqlQueriesResource.SelectBoughtItems, new
                    {
                        checkOffDate = DateTime.Now.Date.AddDays(-1)
                    }).ToList());
                }
            }
            catch (Exception exception)
            {
                exception.ToLogError();
            }

            return result;
        }
        public IList<ShoppingItem> GetShoppingItems()
        {
            var result = new List<ShoppingItem>();
            try
            {
                using (var dbConnection = new SqlConnection(_connectionString))
                {
                    result.AddRange(dbConnection.Query<ShoppingItem>(MssqlQueriesResource.SelectUnboughtItems).ToList());
                }
            }
            catch (Exception exception)
            {
                exception.ToLogError();
            }

            return result;
        }
    }
}
