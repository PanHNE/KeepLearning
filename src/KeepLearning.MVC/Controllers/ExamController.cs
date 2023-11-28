using KeepLearning.Domain.Commands.CreateExamCountry;
using KeepLearning.Domain.Exceptions;
using KeepLearning.Domain.Models.Exam.Country;
using KeepLearning.Domain.Queries.CheckExam;
using KeepLearning.Domain.Queries.DownloadExam;
using KeepLearning.Domain.Queries.GetAllContinents;
using KeepLearning.MVC.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KeepLearning.MVC.Controllers
{
    public class ExamController : Controller
    {
        const string SerializedExamCountry = "SerializedExamCountry";
        private readonly IMediator _mediator;

        public ExamController(IMediator mediator)
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

        [HttpPost]
        public async Task<IActionResult> Create(CreateExamCountryCommand command)
        {
            var test = await _mediator.Send(command);

            var serializedTest = JsonConvert.SerializeObject(test);
            TempData[SerializedExamCountry] = serializedTest;

            return RedirectToAction(nameof(Exam));
        }

        public IActionResult Exam()
        {
            var serializedExam = CheckTempData(SerializedExamCountry);

            var examCountryDto = JsonConvert.DeserializeObject<ExamCountryDto>(serializedExam);

            TempData[SerializedExamCountry] = serializedExam;

            return View(examCountryDto);
        }

        [HttpPost]
        public async Task<IActionResult> Check([FromForm] CheckExamQuery query)
        {
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Download([FromForm] DownloadExamQuery query)
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
    }
}