namespace Dtos.Models.AuthModels
{
    public record VerifyVerificationCodeRequest
    {
        public required string Email { get; set; }
        public required string Code { get; set; }
    }
}
