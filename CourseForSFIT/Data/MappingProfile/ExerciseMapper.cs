using AutoMapper;
using Data.Entities;
using Dtos.Models;
using Dtos.Results;
using Dtos.Results.TagExerciseResults;


namespace Data.MappingProfile
{
    public class ExerciseMapper : Profile
    {
        public ExerciseMapper()
        {
            CreateMap<TagExercise, TagExerciseDto>()
               .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
            CreateMap<TagExercise, TagExerciseAdminDto>()
                .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
            CreateMap<Exercise, ExerciseDto>()
                .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
            CreateMap<TagExerciseAddDto, TagExercise>()
                .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
            CreateMap<Exercise, ExerciseAdminDto>()
                .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
            

        }
    }
}
