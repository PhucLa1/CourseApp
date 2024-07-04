namespace Dtos.Results
{
    public class ApiResponse<T>
    {
        public T? Metadata { get; set; }
        public List<string>? Message { get; set; }
        public bool IsSuccess { get; set; } = false;
    }
}
