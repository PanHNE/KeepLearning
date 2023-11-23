﻿using KeepLearning.Domain.Models.Enums;

namespace KeepLearning.Domain.Interfaces
{
    public interface ICountryService
    {
        public Task<string> GetCorrectAnswer(string questionText, GuessType.Category category);
        public bool IsCorrectAnswer(Domain.Enteties.Country country, string answerText, GuessType.Category category);
    }
}
