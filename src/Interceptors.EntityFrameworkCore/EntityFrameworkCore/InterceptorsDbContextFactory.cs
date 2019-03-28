using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Interceptors.Configuration;
using Interceptors.Web;

namespace Interceptors.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class InterceptorsDbContextFactory : IDesignTimeDbContextFactory<InterceptorsDbContext>
    {
        public InterceptorsDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<InterceptorsDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            InterceptorsDbContextConfigurer.Configure(builder, configuration.GetConnectionString(InterceptorsConsts.ConnectionStringName));

            return new InterceptorsDbContext(builder.Options);
        }
    }
}
