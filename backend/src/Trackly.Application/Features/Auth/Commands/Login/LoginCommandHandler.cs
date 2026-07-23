using MediatR;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Auth.Common;
using Trackly.Domain.Entities;

namespace Trackly.Application.Features.Auth.Commands.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResult>
{
    private const string InvalidCredentialsMessage = "Invalid credentials.";

    private readonly ITenantRepository _tenantRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(
        ITenantRepository tenantRepository,
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _tenantRepository = tenantRepository;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantRepository.GetBySlugAsync(request.TenantSlug, cancellationToken)
            ?? throw new UnauthorizedException(InvalidCredentialsMessage);

        var user = await _userRepository.GetByEmailAsync(tenant.Id, request.Email, cancellationToken);
        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedException(InvalidCredentialsMessage);
        }

        user.UpdateLastLogin();

        var accessToken = _jwtTokenService.GenerateAccessToken(user.Id, tenant.Id, user.Email, user.Role.ToString());
        var (rawRefreshToken, refreshTokenHash) = _jwtTokenService.GenerateRefreshToken();
        var refreshToken = RefreshToken.Create(user.Id, refreshTokenHash, DateTime.UtcNow.AddDays(7));
        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResult(user.Id, tenant.Id, accessToken, rawRefreshToken);
    }
}
