﻿using AutoMapper;
using KeepLearning.Application.Common.Models.Continent;
using KeepLearning.Application.Common.Models.Country;
using KeepLearning.Domain.Interfaces;
using Moq;
using static KeepLearning.Domain.Models.Enums.GuessType;

namespace KeepLearning.Application.Question.Queries.GetRandomQuestion.UnitTests
{
    public class GetRandomQuestionQueryHandlerTests
    {
        public static IEnumerable<object[]> GetRandomQuestionData()
        {

            var list = new List<GetRandomQuestionQuery>(){
                new GetRandomQuestionQuery() {
                    Category = Category.Country,
                    Continent = "Europe"
                },
                new GetRandomQuestionQuery() {
                    Category = Category.CapitalCity,
                    Continent= "Europe"
                },
            };

            return list.Select(q => new object[] { q });

        }

        [Theory]
        [MemberData(nameof(GetRandomQuestionData))]
        public async void Handle_GetRandomQuestion_WhenGiveGuessTypeAndContinent(GetRandomQuestionQuery getRandomQuestionQuery)
        {
            // arrange
            var continent = new Domain.Enteties.Continent()
            {
                Id = Guid.NewGuid(),
                Name = getRandomQuestionQuery.Continent
            };

            var continentDto = new ContinentDto(getRandomQuestionQuery.Continent);

            var country = new Domain.Enteties.Country()
            {
                Id = Guid.NewGuid(),
                Name = "Poland",
                Abbreviation = "POL",
                CapitalCity = "Warsaw",
                ContinentId = continent.Id,
                Continent = continent
            };

            var countryDto = new CountryDto()
            {
                Name = "Poland",
                Abbreviation = "POL",
                CapitalCity = "Warsaw",
                ContinentDto = new ContinentDto(continentDto.Name)
            };

            var continentRepositoryMock = new Mock<IContinentRepository>();
            continentRepositoryMock.Setup(country => country.GetByName(continent.Name)).ReturnsAsync(continent);

            var countryServiceMock = new Mock<ICountryService>();
            countryServiceMock.Setup(country => country.GetRandom(continent.Id)).ReturnsAsync(country);

            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<CountryDto>(country)).Returns(countryDto);

            var handler = new GetRandomQuestionQueryHandler(continentRepositoryMock.Object, countryServiceMock.Object, mapper.Object);

            var expectedAnswerText = GetAnswerText(countryDto, getRandomQuestionQuery.Category);
            var expectedQuestionText = GetQuestionText(countryDto, getRandomQuestionQuery.Category);

            // act
            var result = await handler.Handle(getRandomQuestionQuery, CancellationToken.None);

            // assert
            result.AnswerText.Should().Be(expectedAnswerText);
            result.QuestionText.Should().Be(expectedQuestionText);
        }

        private string GetQuestionText(CountryDto countryDto, Category categry)
        {
            switch (categry)
            {
                case Category.Country: return countryDto.CapitalCity;
                case Category.CapitalCity: return countryDto.Name;
                default: throw new NotImplementedException();
            }
        }

        private string GetAnswerText(CountryDto countryDto, Category categry)
        {
            switch (categry)
            {
                case Category.Country: return countryDto.Name;
                case Category.CapitalCity: return countryDto.CapitalCity;
                default: throw new NotImplementedException();
            }
        }
    }
}