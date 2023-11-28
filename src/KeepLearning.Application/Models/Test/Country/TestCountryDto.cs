﻿using KeepLearning.Domain.Models.Continent;
using KeepLearning.Domain.Models.Exam;
using static KeepLearning.Domain.Models.Enums.GuessType;

namespace KeepLearning.Domain.Models.Test.Country
{
    public class ExamCountryDto : ExamDto
    {
        public required Category Category { get; set; }
        public IEnumerable<ContinentDto> Continents { get; set; } = new List<ContinentDto>();
    }
}
