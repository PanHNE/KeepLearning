﻿using AutoMapper;
using KeepLearning.Domain.Interfaces;
using MediatR;
using KeepLearning.Domain.Exceptions;
using KeepLearning.Application.Common.Models.Continent;
using KeepLearning.Application.Common.Models.Country;
using KeepLearning.Application.Common.Models.Exam;
using KeepLearning.Application.Common.Models.Question;
using KeepLearning.Application.Common.Models.Exam.Country;

namespace KeepLearning.Domain.Commands.CreateExamCountry
{
    public class CreateExamCountryCommandHandler : IRequestHandler<CreateExamCountryCommand, ExamCountryDto>
    {
        private readonly IContinentRepository _continentRepository;
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;

        public CreateExamCountryCommandHandler(IContinentRepository continentRepository, ICountryService countryService, IMapper mapper)
        {
            _continentRepository = continentRepository;
            _countryService = countryService;
            _mapper = mapper;
        }

        public async Task<ExamCountryDto> Handle(CreateExamCountryCommand request, CancellationToken cancellationToken)
        {
            var continents = await _continentRepository.GetByNames(request.Continents);
            if (!continents.Any())
            {
                throw new NotFoundException("Not found any continents");
            }

            var continentIds = continents.Select(c => c.Id);
            var randomCountries = await _countryService.RandomCountries(continentIds, request.NumberOfQuestion);
            if (!randomCountries.Any())
            {
                throw new NotFoundException("Not found any country for these continents");
            }

            var countriesDto = randomCountries.Select(c => _mapper.Map<CountryDto>(c)).ToList();
            var continentsDto = request.Continents.Select(c => _mapper.Map<ContinentDto>(c)).ToList();

            var questionsDto = QuestionDtoBuilder.CreateQuestions(countriesDto, request);
            var examDto = ExamDtoBuilder.CreateExamCountry(request.Name, questionsDto, request.Category, continentsDto);

            return examDto;
        }
    }
}