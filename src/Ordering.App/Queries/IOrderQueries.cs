using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.App.Queries
{
    public interface IOrderQueries
    {
        /// <summary>
        /// Gets all user's orders.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>The list of user's orders.</returns>
        Task<IEnumerable<OrderViewModel>> GetOrderFromUserAsync(int userId);

        /// <summary>
        /// Gets if the user has exceed the amount of pending money.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="max">The max amount of pending bills.</param>
        /// <returns>Returns true if the sum of all pending orders amount is more that the max, false otherwise.</returns>
        Task<bool> HasExceedMaxPendingAmount(int userId, decimal max);

        /// <summary>
        /// Gets if the user has exceed the pending amount of a specific product.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="productId">The product ID.</param>
        /// <param name="max">The max amount of pending bills.</param>
        /// <returns>Returns true if the count of a specific product in pending orders is more that the max, false otherwise.</returns>
        Task<bool> HasExeedMaxPendingProducts(int userId, int productId, int max);
    }
}
