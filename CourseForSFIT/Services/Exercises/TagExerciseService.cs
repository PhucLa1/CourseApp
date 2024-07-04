using AutoMapper;
using Data.Entities;
using Dtos.Results;
using Repositories.unitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Exercises
{
    public interface ITagExerciseService
    {
        Task<ApiResponse<IEnumerable<TagExerciseDto>>> GetAllTagExercise();
    }
    public class TagExerciseService : ITagExerciseService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TagExerciseService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse<IEnumerable<TagExerciseDto>>> GetAllTagExercise()
        {
            try
            {
                IEnumerable<TagExercise> tagExercises = await _unitOfWork.TagExerciseRepository.GetAllAsync();
                return new ApiResponse<IEnumerable<TagExerciseDto>> { Metadata = _mapper.Map<List<TagExerciseDto>>(tagExercises), IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
