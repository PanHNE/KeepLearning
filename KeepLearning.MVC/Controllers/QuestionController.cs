﻿using KeepLearning.Application.Models.Enums;
using KeepLearning.Application.Models.Question;
using KeepLearning.Application.Models.TestCountry;
using KeepLearning.Application.Queries.CheckAnswer;
using KeepLearning.Application.Queries.GetQuestionsQuery;
using KeepLearning.Application.Queries.GetRandomQuestion;
using KeepLearning.MVC.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantAPI.Exceptions;

namespace KeepLearning.MVC.Controllers
{
    public class QuestionController : Controller
    {
        const string STDTestCountry = "SerializedTestCountry";

        private readonly IMediator _mediator;

        public QuestionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var questionDataViewModel = CreateQuestionDataViewModel();

            return View(questionDataViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Random(GetRandomQuestionQuery getRandomQuestionQuery)
        {

            var question = await _mediator.Send(getRandomQuestionQuery);

            var modelView = new GuessRandomQuestionModelView(getRandomQuestionQuery, question);

            return View(modelView);
        }

        [HttpGet]
        public async Task<IActionResult> RandomAnotherQuestion(GetRandomQuestionQuery getRandomQuestionQuery)
        {

            var question = await _mediator.Send(getRandomQuestionQuery);

            return Ok(question);
        }

        [HttpGet]
        public async Task<IActionResult> CheckAnswer(CheckAnswerQuery checkAnswerQuery)
        {
            var result = await _mediator.Send(checkAnswerQuery);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> CheckTest(CheckTestQuery checkAnswerQuery)
        {
            var result = await _mediator.Send(checkAnswerQuery);

            return Ok(result);
        }

        public IActionResult CreateTest()
        {
            var questionDataViewModel = CreateQuestionDataViewModel();

            return View(questionDataViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTest(GetQuestionsQuery query)
        {
            var test = await _mediator.Send(query);

            var serializedTest = JsonConvert.SerializeObject(test);
            TempData[STDTestCountry] = serializedTest;

            return RedirectToAction(nameof(Test));
        }

        public IActionResult Test()
        {
            var serializedTest = CheckTempData(STDTestCountry);

            var testCountryDto = JsonConvert.DeserializeObject<TestCountryDto>(serializedTest);

            TempData[STDTestCountry] = serializedTest;

            return View(testCountryDto);
        }

        private string CheckTempData(string name)
        {
            var tempData = TempData[name];
            if (tempData is null)
            {
                throw new NotFoundException("TempData not found");
            }

            var serializedString = tempData.ToString();
            if (serializedString is null)
            {
                throw new Exception("TempData can not map to string");
            }

            return serializedString;
        }

        private QuestionDataViewModel CreateQuestionDataViewModel()
        {
            var continents = Continent.GetAllLikeStrings();
            var guessTypes = GuessType.GetAllLikeStrings();

            return new QuestionDataViewModel(continents, guessTypes);
        }
    }
}
