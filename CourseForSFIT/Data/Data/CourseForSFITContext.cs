using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
namespace Data.Data
{
    public class CourseForSFITContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CourseForSFITContext(DbContextOptions<CourseForSFITContext> options, IHttpContextAccessor httpContextAccessor)
    : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditFields()
        {
            var userId = GetCurrentUserId();

            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added))
            {
                if (entry.Entity is IAuditable)
                {
                    var entity = (IAuditable)entry.Entity;
                    entity.UpdatedAt = DateTime.UtcNow;
                    entity.UpdatedBy = userId;

                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedAt = DateTime.UtcNow;
                        entity.CreatedBy = userId;
                    }
                }
            }
        }

        private int GetCurrentUserId()
        {
            // Assuming the user ID is stored in the claims of the current user
            return _httpContextAccessor.HttpContext.Items["UserId"] == null ? 0 : int.Parse(_httpContextAccessor.HttpContext.Items["UserId"] as string);
        }
        public DbSet<Blog> blog { get; set; }
        public DbSet<BlogComment> blog_comment { get; set; }
        public DbSet<Chapter> chapter { get; set; }
        public DbSet<Contest> contest { get; set; }
        public DbSet<ContestExercise> contest_exercise { get; set; }
        public DbSet<Course> course { get; set; }
        public DbSet<CourseComment> course_comment { get; set; }
        public DbSet<CourseType> course_type { get; set; }
        public DbSet<CourseUser> course_user { get; set; }
        public DbSet<Exercise> exercise { get; set; }
        public DbSet<ExerciseComment> exercise_comment { get; set; }
        public DbSet<LessonComment> lesson_comment { get; set; }
        public DbSet<LessonCourse> lesson_course { get; set; }
        public DbSet<TagBlog> tag_blog { get; set; }
        public DbSet<TagExercise> tag_exercise { get; set; }
        public DbSet<TestCase> test_case { get; set; }
        public DbSet<User> user { get; set; }
        public DbSet<UserExercise> user_exercise { get; set; }
        public DbSet<UserJoin> user_join { get; set; }
        public DbSet<UserResult> user_result { get; set; }
    }
}
