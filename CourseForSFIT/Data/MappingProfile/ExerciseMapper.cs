using AutoMapper;
using Data.Entities;
using Dtos.Models.ExerciseModels;
using Dtos.Results.ExerciseResults;



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
            CreateMap<ExerciseAddDto, Exercise>()
                .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
            CreateMap<ExerciseHasTagAddDto, ExerciseHasTag>()
                .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
            CreateMap<UserExercise, ContentCodes>()
                 .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
        }
    }
}
