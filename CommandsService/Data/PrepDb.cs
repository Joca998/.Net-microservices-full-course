using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandsService.Data
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var servicesScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = servicesScope.ServiceProvider.GetService<IPlatformDataClient>();

                var platforms = grpcClient.ReturnAllPlatforms();

                SeedData(servicesScope.ServiceProvider.GetService<ICommandRepo>(), platforms);
            }
        }

        private static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("Seeding new platforms...");

            foreach (var plat in platforms)
            {
                if (!repo.ExternalPlatformExist(plat.ExternalId))
                {
                    repo.CreatePlatform(plat);
                }
                repo.SaveChanges();
            }
        }
    }
}
