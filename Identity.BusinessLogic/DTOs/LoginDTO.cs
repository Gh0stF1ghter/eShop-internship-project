namespace Identity.BusinessLogic.DTOs
{
    public record LoginDTO(string Email, string Password, bool RememberMe);
}
