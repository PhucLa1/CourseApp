using AutoMapper;
using Data.Entities;
using Dtos.Models.ExerciseModels;
using Dtos.Models.TestCaseModels;
using Dtos.Results.TestCaseResults;

namespace Data.MappingProfile
{
    public class TestCaseMapper : Profile
    {
        public TestCaseMapper()
        {
            CreateMap<TestCase, TestCaseDto>()
                .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
            CreateMap<TestCaseAddDto, TestCase>()
                .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
            CreateMap<UserExerciseAddDto, UserExercise>()
                .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));

        }
    }
}
