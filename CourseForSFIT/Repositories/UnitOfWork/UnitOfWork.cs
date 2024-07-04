using Data.Data;
using NPOI.SS.Formula.Functions;
using Repositories.Repositories.Base;
using Repositories.Repositories.IRepo;
using Repositories.Repositories.Repo;
using Repositories.unitOfWork;

namespace authen_service.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private CourseForSFITContext _context;
        private IUserRepository? userRepository;
        private ITagExerciseRepository? tagExerciseRepository;
        private IExerciseRepository? exerciseRepository;

        public UnitOfWork(CourseForSFITContext context)
        {
            _context = context;
        }
        public IUserRepository UserRepository
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new UserRepository(_context);
                }

                return userRepository;
            }
        }
        public ITagExerciseRepository TagExerciseRepository 
        {
            get
            {
                if (tagExerciseRepository == null)
                {
                    tagExerciseRepository = new TagExerciseRepository(_context);
                }

                return tagExerciseRepository;
            }
        }
        public IExerciseRepository ExerciseRepository
        {
            get
            {
                if (exerciseRepository == null)
                {
                    exerciseRepository = new ExerciseRepository(_context);
                }

                return exerciseRepository;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
