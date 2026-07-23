using MediatR;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Auth.Common;
using Trackly.Domain.Entities;
using Trackly.Domain.Enums;

namespace Trackly.Application.Features.Auth.Commands.Register;

public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResult>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public RegisterCommandHandler(
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

    public async Task<AuthResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingTenant = await _tenantRepository.GetBySlugAsync(request.TenantSlug, cancellationToken);
        if (existingTenant is not null)
        {
            throw new ConflictException($"Slug \"{request.TenantSlug}\" is already taken.");
        }

        var tenant = Tenant.Create(request.TenantName, request.TenantSlug);
        await _tenantRepository.AddAsync(tenant, cancellationToken);

        var user = User.Create(tenant.Id, request.Email, request.FirstName, request.LastName);
        user.SetPasswordHash(_passwordHasher.Hash(request.Password));
        user.SetRole(UserRole.Owner);
        await _userRepository.AddAsync(user, cancellationToken);

        var accessToken = _jwtTokenService.GenerateAccessToken(user.Id, tenant.Id, user.Email, user.Role.ToString());
        var (rawRefreshToken, refreshTokenHash) = _jwtTokenService.GenerateRefreshToken();
        var refreshToken = RefreshToken.Create(user.Id, refreshTokenHash, DateTime.UtcNow.AddDays(7));
        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResult(user.Id, tenant.Id, accessToken, rawRefreshToken);
    }
}
