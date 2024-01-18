namespace InvoicePeru.Application.Authentication.Commands.Register;
public class RegisterCommandHandler :
    IRequestHandler<RegisterCommand, AuthenticationResult>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthenticationResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        //validate so that user doesn't exists
        if (await _userRepository.GetUserByEmail(command.Email) is not null)
        {
            throw new DuplicateEmailException();
        }

        var user = new User
        {
            Email = command.Email,
            Password = command.Password,
            Role = command.Role

        };

        _userRepository.Add(user);

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(
            user,
            token);
    }
}

