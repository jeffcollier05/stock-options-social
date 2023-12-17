using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TradeHarborApi.Data
{
    /// <summary>
    /// Represents the database context for the TradeHarbor API, extending IdentityDbContext for user management.
    /// </summary>
    public class ApiDbContext : IdentityDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiDbContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {
        }
    }
}
