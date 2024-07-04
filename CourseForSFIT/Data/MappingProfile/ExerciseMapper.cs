using AutoMapper;
using Data.Entities;
using Dtos.Results;


namespace Data.MappingProfile
{
    public class ExerciseMapper : Profile
    {
        public ExerciseMapper()
        {
            CreateMap<TagExercise, TagExerciseDto>()
               .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
            CreateMap<Exercise, ExerciseDto>()
                .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
        }
    }
}
