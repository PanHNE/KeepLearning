﻿namespace KeepLearning.Application.Models.Result.Test
{
    public record TestResultDto(IEnumerable<AnswerResultDto> AnswerResults, int numberOfGoodAnswers, int numberOfFailAnswers) { }
}