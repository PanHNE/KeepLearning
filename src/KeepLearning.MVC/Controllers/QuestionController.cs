﻿using KeepLearning.Domain.Commands.CreateTestCountry;
using KeepLearning.Domain.Models.Test.Country;
using KeepLearning.Domain.Queries.CheckAnswer;
using KeepLearning.Domain.Queries.CheckTest;
using KeepLearning.Domain.Queries.GetAllContinents;
using KeepLearning.Domain.Queries.GetRandomQuestion;
using KeepLearning.Domain.Queries.TestToDownload;
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
        public async Task<IActionResult> Create()
        {
            var continents = await _mediator.Send(new GetAllContinentsQuery());
            var questionDataViewModel = new QuestionDataViewModel(continents);

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
        public async Task<IActionResult> CreateTest()
        {
            var continents = await _mediator.Send(new GetAllContinentsQuery());
            var questionDataViewModel = new QuestionDataViewModel(continents);

            return View(questionDataViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTest(CreateTestCountryCommand command)
        {
            var test = await _mediator.Send(command);

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

        [HttpPost]
        public async Task<IActionResult> CheckTest([FromForm] CheckTestQuery query)
        {
            var result = await _mediator.Send(query);

            return Ok(result);
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

        [HttpPost]
        public async Task<IActionResult> Download([FromForm] TestToDownloadQuery query)
        {
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
