using MediatR;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Auth.Common;
using Trackly.Domain.Entities;

namespace Trackly.Application.Features.Auth.Commands.RefreshAccessToken;

public sealed class RefreshAccessTokenCommandHandler : IRequestHandler<RefreshAccessTokenCommand, AuthResult>
{
    private const string InvalidTokenMessage = "Invalid refresh token.";

    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;

    public RefreshAccessTokenCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IJwtTokenService jwtTokenService)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResult> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var tokenHash = _jwtTokenService.HashRefreshToken(request.RefreshToken);
        var existingToken = await _refreshTokenRepository.GetByHashAsync(tokenHash, cancellationToken)
            ?? throw new UnauthorizedException(InvalidTokenMessage);

        if (!existingToken.IsActive)
        {
            throw new UnauthorizedException(InvalidTokenMessage);
        }

        var user = await _userRepository.GetByIdAsync(existingToken.UserId, cancellationToken)
            ?? throw new UnauthorizedException(InvalidTokenMessage);

        var (rawNewToken, newTokenHash) = _jwtTokenService.GenerateRefreshToken();
        var newRefreshToken = RefreshToken.Create(user.Id, newTokenHash, DateTime.UtcNow.AddDays(7));
        await _refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);

        existingToken.RotateTo(newRefreshToken.Id);

        var accessToken = _jwtTokenService.GenerateAccessToken(user.Id, user.TenantId, user.Email, user.Role.ToString());

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResult(user.Id, user.TenantId, accessToken, rawNewToken);
    }
}
