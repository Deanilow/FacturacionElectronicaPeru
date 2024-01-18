namespace InvoicePeru.Application.Authentication.Commands.Register;
public record RegisterCommand(
        string Email,
        string Password,
        string Role) : IRequest<AuthenticationResult>;