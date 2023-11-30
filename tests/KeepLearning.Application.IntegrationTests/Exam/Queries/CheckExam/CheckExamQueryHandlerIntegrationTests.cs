﻿using KeepLearning.Application.Common.Models.Answer;
using KeepLearning.Application.Helper.Seeders.IntegrationTests;
using KeepLearning.Domain.Exceptions;
using KeepLearning.Domain.Interfaces;
using KeepLearning.Infrastructure.Persistence;
using KeepLearning.Infrastructure.Repositories;
using KeepLearning.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using static KeepLearning.Domain.Models.Enums.GuessType;

namespace KeepLearning.Application.Exam.Queries.CheckExam.IntegrationTests
{
    public class CheckExamQueryHandlerIntegrationTests
    {
        private readonly KeepLearningDbContext _dbContext;
        private readonly ICountryRepository _countryRepositoryTest;
        private readonly ICountryService _countryService;

        public CheckExamQueryHandlerIntegrationTests()
        {
            var builder = new DbContextOptionsBuilder<KeepLearningDbContext>();
            builder.UseInMemoryDatabase("TestKeepLearningDb-CheckExamQueryHandlerIntegrationTests");

            _dbContext = new KeepLearningDbContext(builder.Options);

            var continentSeederTest = new ContinentSeederTest(_dbContext);
            continentSeederTest.Seed();

            var countrySeederTest = new CountrySeederTest(_dbContext);
            countrySeederTest.Seed();

            _countryRepositoryTest = new CountryRepository(_dbContext);

            _countryService = new CountryService(_countryRepositoryTest);
        }

        public record CheckExamQueryWithResult(CheckExamQuery checkExamQuery, int correctAnswer, int incorrectAnswers) { }

        public static IEnumerable<object[]> GetCorrectData()
        {
            var list = new List<CheckExamQueryWithResult>()
            {
                new CheckExamQueryWithResult(
                    new CheckExamQuery()
                    {
                        Category = Category.CapitalCity,
                        Answers = new List<AnswerDto>()
                        {
                            new AnswerDto(1, "Germany", "Berlin"),
                            new AnswerDto(1, "Bulgaria", "Sofia"),
                            new AnswerDto(1, "Egypt", "Cairo"),
                            new AnswerDto(1, "Poland", "Warsaw"),
                            new AnswerDto(1, "Ghana", ""),
                        }
                    },
                    4,
                    1
                ),
                new CheckExamQueryWithResult(
                    new CheckExamQuery()
                    {
                        Category = Category.CapitalCity,
                        Answers = new List<AnswerDto>()
                        {
                            new AnswerDto(1, "Germany", ""),
                            new AnswerDto(1, "Bulgaria", "Soia"),
                            new AnswerDto(1, "Egypt", ""),
                            new AnswerDto(1, "Poland", ""),
                            new AnswerDto(1, "Ghana", ""),
                        }
                    },
                    0,
                    5
                ),
                new CheckExamQueryWithResult(
                    new CheckExamQuery()
                    {
                        Category = Category.Country,
                        Answers = new List<AnswerDto>()
                        {
                            new AnswerDto(1, "Berlin", "Germany"),
                            new AnswerDto(1, "Sofia", "Bulgaria"),
                            new AnswerDto(1, "Cairo", "Egypt"),
                            new AnswerDto(1, "Warsaw", "Poland"),
                            new AnswerDto(1, "Accra", "Ghana"),
                            new AnswerDto(1, "Nairobi", "Kenya"),
                            new AnswerDto(1, "Luxembourg City", "Luxembourg"),
                            new AnswerDto(1, "Ulaanbaatar", "Mongolia"),
                            new AnswerDto(1, "Ngerulmud", "Palau"),
                            new AnswerDto(1, "Basseterre", "Saint Kitts & Nevis"),
                        }
                    },
                    10,
                    0
                ),
            };

            return list.Select(el => new object[] { el });
        }

        [Theory()]
        [MemberData(nameof(GetCorrectData))]
        public async void Handle_CheckExamWithSomeCorrectAndIncorrectAnswer_ReturnExamResultWithExpectedData(CheckExamQueryWithResult checkExamQueryWithResult)
        {
            // arrange
            var checkExamQueryHandler = new CheckExamQueryHandler(_countryService);
            var correctAnswers = checkExamQueryWithResult.correctAnswer;
            var incorrectAnswers = checkExamQueryWithResult.incorrectAnswers;

            // act
            var result = await checkExamQueryHandler.Handle(checkExamQueryWithResult.checkExamQuery, CancellationToken.None);

            // assert
            result.NumberOfGoodAnswers.Should().Be(correctAnswers);
            result.NumberOfBadAnswers.Should().Be(incorrectAnswers);
        }

        public static IEnumerable<object[]> GetIncorrectData()
        {
            var list = new List<CheckExamQueryWithResult>()
            {
                new CheckExamQueryWithResult
                (
                    new CheckExamQuery()
                    {
                        Category = Category.CapitalCity,
                        Answers = new List<AnswerDto>()
                        {
                            new AnswerDto(1, "Gerrmany", "Berlin"),
                            new AnswerDto(1, "Bulgaria", "Sofia"),
                            new AnswerDto(1, "", "Cairo"),
                            new AnswerDto(1, "Poland", "Warsaw"),
                            new AnswerDto(1, "Ghana", ""),
                        }
                    },
                    4,
                    1
                ),
                new CheckExamQueryWithResult
                (
                    new CheckExamQuery()
                    {
                        Category = Category.CapitalCity,
                        Answers = new List<AnswerDto>()
                    },
                    0,
                    0
                )
            };

            return list.Select(el => new object[] { el });
        }

        [Theory()]
        [MemberData(nameof(GetIncorrectData))]
        public void Handle_CheckExamWithIncorrectQuestion_ReturnNotFoundException(CheckExamQueryWithResult checkExamQueryWithResult)
            {
                // arrange
                var checkExamQueryHandler = new CheckExamQueryHandler(_countryService);
                var correctAnswers = checkExamQueryWithResult.correctAnswer;
                var incorrectAnswers = checkExamQueryWithResult.incorrectAnswers;

                // act
                var action = () => checkExamQueryHandler.Handle(checkExamQueryWithResult.checkExamQuery, CancellationToken.None);

            // assert
            action.Invoking(a => a.Invoke())
                .Should().ThrowAsync<NotFoundException>();
            }
        }
    }