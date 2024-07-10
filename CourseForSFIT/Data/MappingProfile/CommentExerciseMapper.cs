using AutoMapper;
using Data.Entities;
using Dtos.Models;
using Dtos.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.MappingProfile
{
    public class CommentExerciseMapper : Profile
    {
        public CommentExerciseMapper() 
        {
            CreateMap<CommentExerciseAddDto, ExerciseComment>()
                .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
        }
    }
}
