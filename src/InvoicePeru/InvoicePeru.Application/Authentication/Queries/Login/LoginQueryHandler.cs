namespace InvoicePeru.Application.Authentication.Queries.Login;
public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthenticationResult>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _PasswordService;

    public LoginQueryHandler(IUserRepository userRepository, IPasswordService passwordService, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _PasswordService = passwordService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthenticationResult> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        IList<FluentValidation.Results.ValidationFailure> errorMessages = new List<FluentValidation.Results.ValidationFailure>();

        var user = await _userRepository.GetUserByEmail(query.Email);

        if (user is null) errorMessages.Add(new FluentValidation.Results.ValidationFailure("UserName", $"No Accounts Registered with {query.Email}."));

        var isBase64String = this._PasswordService.IsBase64String(query.Password);

        if (!isBase64String) errorMessages.Add(new FluentValidation.Results.ValidationFailure("Password", $"Password is not base64 encoded."));

        if (user != null)
        {
            var isPasswordValid = this._PasswordService.VerifyPasswordHash(this._PasswordService.Base64Decode(query.Password), Convert.FromBase64String(user.PasswordHash), Convert.FromBase64String(user.PasswordSalt));

            if (!isPasswordValid) errorMessages.Add(new FluentValidation.Results.ValidationFailure("Password", $"Password is Incorrect."));
        }

        if (errorMessages.Count > 0) throw new FluentValidation.ValidationException(errorMessages);

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(
            user,
            token);
    }
}
