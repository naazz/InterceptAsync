using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Interceptors.EntityFrameworkCore
{
    public static class InterceptorsDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<InterceptorsDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<InterceptorsDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
