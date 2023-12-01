﻿using KeepLearning.Domain.Enteties;
using KeepLearning.Domain.Interfaces;
using KeepLearning.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KeepLearning.Infrastructure.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly KeepLearningDbContext _dbContext;

        public CountryRepository(KeepLearningDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Country?> GetByCapitalCity(string capitalCity)
            => await _dbContext.Countries.FirstAsync(country => country.CapitalCity == capitalCity);

        public async Task<Country?> GetByName(string name)
            => await _dbContext.Countries.FirstAsync(country => country.Name == name);

        public async Task<IEnumerable<Country>> GetByContinents(IEnumerable<Guid> continentIds)
            => await _dbContext.Countries
                    .Where(country => continentIds.Contains(country.ContinentId))
                    .ToListAsync();

        public async Task<IEnumerable<Country>> GetAll()
            => await _dbContext.Countries.ToListAsync();

        public async Task<int> GetNumberOfCountries(IEnumerable<Guid> continentIds)
            => await _dbContext.Countries.Where(country => continentIds.Contains(country.ContinentId)).CountAsync();
    }
}
