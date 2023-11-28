﻿using KeepLearning.Domain.Interfaces;
using KeepLearning.Domain.Models.Result;
using KeepLearning.Domain.Models.Result.Exam;
using MediatR;

namespace KeepLearning.Domain.Queries.CheckExam
{
    public class CheckExamQueryHandler : IRequestHandler<CheckExamQuery, ExamResultDto>
    {
        private readonly ICountryService _countryService;

        public CheckExamQueryHandler(ICountryService countryService)
        {
            _countryService = countryService;
        }

        public async Task<ExamResultDto> Handle(CheckExamQuery request, CancellationToken cancellationToken)
        {
            var answers = new List<AnswerResultDto>();
            var numberOfCorrectAnswers = 0;
            var numberOfIncorrectAnswers = 0;


            foreach (var answer in request.Answers)
            {
                var correctAnswer = await _countryService.GetCorrectAnswer(answer.QuestionText, request.Category);

                if (answer.AnswerText is not null && answer.AnswerText?.ToLower() == correctAnswer.ToLower())
                {
                    numberOfCorrectAnswers++;
                }
                else
                {
                    numberOfIncorrectAnswers++;
                }
                answers.Add(new AnswerResultDto(answer.NumberOfQuestion, answer.AnswerText, correctAnswer));
            }

            var examResultDto = new ExamResultDto(answers, numberOfCorrectAnswers, numberOfIncorrectAnswers);

            return examResultDto;
        }
    }
}