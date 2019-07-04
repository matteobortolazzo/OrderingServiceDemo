using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.App.Queries
{
    public class OrderQueries : IOrderQueries
    {
        private string _connectionString = string.Empty;

        public OrderQueries(string constr)
        {
            _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
        }

        public async Task<IEnumerable<OrderViewModel>> GetOrderFromUserAsync(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var orders = await connection.QueryAsync<OrderViewModel>(
                   @"SELECT 
	                    o.[IdentityGuid] as [Id]
	                    ,o.[ProductId]
	                    ,(o.[Quantity] * o.[UnitPrice]) as [Total]
	                    ,o.[OrderDate] as [Date]
	                    ,s.[Name] as [Status]
	                    ,o.[DeliveryAddress_Street] as [Street]
	                    ,o.[DeliveryAddress_City] as [City]
	                    ,o.[DeliveryAddress_Zipcode] as [Zipcode]
	                    ,o.[DeliveryAddress_Country] as [Country]
                    FROM [Orders] o INNER JOIN [OrderStatus] s
                    ON o.OrderStatusId = s.Id
                    WHERE o.[UserId] = @userId", new { userId });
                return orders;
            }
        }

        public async Task<bool> HasExceedMaxPendingAmount(int userId, decimal max)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var totalPendingAmount = await connection.QuerySingleOrDefaultAsync<decimal?>(
                  @"SELECT SUM(o.[Quantity] * o.[UnitPrice]) as Total
                    FROM[Orders] o
                    WHERE o.[UserId] = @userId AND o.[OrderStatusId] = @statusId
                    GROUP BY o.[UserId], o.[Quantity], o.[UnitPrice]",
                  new { userId, statusId = OrderStatus.Submitted.Id });
                return totalPendingAmount.HasValue && totalPendingAmount > max;
            }
        }

        public async Task<bool> HasExeedMaxPendingProducts(int userId, int productId, int max)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var totalPendingProducts = await connection.QuerySingleOrDefaultAsync<decimal?>(
                  @"SELECT SUM(o.Quantity) as Total
                    FROM [Orders] o
                    WHERE o.[UserId] = @userId AND o.[ProductId] = @productId AND o.[OrderStatusId] = @statusId", 
                  new { userId, productId, statusId = OrderStatus.Submitted.Id });
                return totalPendingProducts.HasValue && totalPendingProducts > max;
            }
        }
    }
}
