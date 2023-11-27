﻿using AutoMapper;
using KeepLearning.Domain.Enteties;
using KeepLearning.Domain.Models.Continent;
using KeepLearning.Domain.Models.Country;
using KeepLearning.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KeepLearning.Infrastructure.Seeders
{
    public class CountrySeeder
    {
        private readonly KeepLearningDbContext _dbContext;

        public CountrySeeder(KeepLearningDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Seed()
        {
            var continents = await _dbContext.Continents.ToListAsync();

            if (await _dbContext.Database.CanConnectAsync())
            {
                if (!_dbContext.Countries.Any())
                {
                    var countries = GetCountriesFromFile();

                    if (countries != null)
                    {
                        countries.ToList().ForEach(countryDto =>
                        {
                            var newCountry = CreateCountry(continents, countryDto);

                            _dbContext.Countries.Add(newCountry);
                            _dbContext.SaveChanges();
                        });
                    }
                }
            }
        }

        private Country CreateCountry(List<Continent> continents, CountryDto countryDto)
        {
            var continent = continents.First(c => c.Name == countryDto.Continent.Name);

            return new Country()
            {
                Name = countryDto.Name,
                Abbreviation = countryDto.Abbreviation,
                CapitalCity = countryDto.CapitalCity,
                ContinentId = continent.Id
            };
        }

        private IEnumerable<CountryDto> GetCountriesFromFile()
        {
            IEnumerable<CountryDto> countries = new List<CountryDto>();

            try
            {
                countries = File.ReadAllLines("../KeepLearning.Infrastructure/Seeders/FilesWithData/WorldCountriesList.csv")
                    .Skip(1)
                    .Select(c => c.Split(','))
                    .Select(c => new CountryDto()
                    {
                        Name = c[0],
                        Abbreviation = c[1],
                        CapitalCity = c[2],
                        Continent = new ContinentDto(c[3])
                    });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return countries;
        }
    }
}
