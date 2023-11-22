﻿using KeepLearning.Domain.Enteties;
using KeepLearning.Domain.Interfaces;
using KeepLearning.Domain.Models.Enums;
using RestaurantAPI.Exceptions;
using static KeepLearning.Domain.Models.Enums.GuessType;

namespace KeepLearning.Infrastructure.Services
{
    internal class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;

        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<Country?> GetCountry(string questionText, GuessType.Category category)
        {
            switch (category)
            {
                case Category.CapitalCity:
                    return await _countryRepository.GetByName(questionText);

                case Category.Country:
                    return await _countryRepository.GetByCapitalCity(questionText);

                default:
                    throw new NotImplementedException();
            }
        }

        public async Task<string> GetCorrectAnswer(string questionText, GuessType.Category category)
        {
            var country = await GetCountry(questionText, category);

            if (country == null)
                throw new NotFoundException("Not found conutry");

            switch (category)
            {
                case Category.CapitalCity:
                    return country.CapitalCity;

                case Category.Country:
                    return country.Name;

                default:
                    throw new NotImplementedException();
            }
        }

        public bool IsCorrectAnswer(Country country, string answerText, GuessType.Category category)
        {
            if (answerText is null)
                return false;

            switch (category)
            {
                case Category.Country:
                    return country.Name.ToLower().Equals(answerText.ToLower());

                case Category.CapitalCity:
                    return country.CapitalCity.ToLower().Equals(answerText.ToLower());

                default: return false;
            }
        }
    }
}
