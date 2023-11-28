﻿using KeepLearning.Infrastructure.Seeders;
using Microsoft.Extensions.DependencyInjection;

namespace KeepLearning.Infrastructure.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static async void SeedData(this IServiceProvider service)
        {
            var scope = service.CreateScope();

            var continentSeeder = scope.ServiceProvider.GetRequiredService<ContinentSeeder>();
            await continentSeeder.Seed();

            var countrySeeder = scope.ServiceProvider.GetRequiredService<CountrySeeder>();
            await countrySeeder.Seed();
        }
    }
}
