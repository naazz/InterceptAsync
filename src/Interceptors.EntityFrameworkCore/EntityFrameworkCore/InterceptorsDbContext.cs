using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Interceptors.Authorization.Roles;
using Interceptors.Authorization.Users;
using Interceptors.MultiTenancy;
using Interceptors.Tests;

namespace Interceptors.EntityFrameworkCore
{
    public class InterceptorsDbContext : AbpZeroDbContext<Tenant, Role, User, InterceptorsDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Test> Tests { get; set; }

        public DbSet<TestPreTreatment> TestPreTreatments { get; set; }

        public InterceptorsDbContext(DbContextOptions<InterceptorsDbContext> options)
            : base(options)
        {
        }
    }
}
