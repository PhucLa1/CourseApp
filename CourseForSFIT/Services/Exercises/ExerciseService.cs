using AutoMapper;
using Data.Entities;
using Dtos.Results;
using Repositories.Repositories.IRepo;
using Repositories.unitOfWork;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Services.Exercises
{
    public interface IExerciseService
    {
        Task<ApiResponse<PagedResult<ExerciseDto>>> GetExercisesPaginated(int  pageNumber = 1, int pageSize = 10);
    }
    public class ExerciseService : IExerciseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ExerciseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse<PagedResult<ExerciseDto>>> GetExercisesPaginated(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                IEnumerable<Exercise> exercises = await _unitOfWork.ExerciseRepository.GetPaginatedAsync(pageNumber, pageSize);
                int totalCount = await _unitOfWork.ExerciseRepository.GetTotalCountAsync();
                return new ApiResponse<PagedResult<ExerciseDto>> { IsSuccess = true, Metadata = HandlePagination<ExerciseDto>.PageList(pageNumber, totalCount, pageSize, _mapper.Map<List<ExerciseDto>>(exercises)) };
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
