namespace Identity.BusinessLogic.DTOs
{
    public record RegisterDTO(string Username, string Email, string Password, string ConfirmPassword);
}
