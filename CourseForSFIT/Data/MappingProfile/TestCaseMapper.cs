using AutoMapper;
using Data.Entities;
using Dtos.Models;
using Dtos.Results.TestCaseResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            CreateMap<TestCaseUpdateDto, TestCase>()
                .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
            CreateMap<UserExerciseAddDto, UserExercise>()
                .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
        }
    }
}
