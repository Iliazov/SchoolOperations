using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.TeamsFx;
using Microsoft.TeamsFx.Configuration;
using SchoolOperations.Bll.DTOs;
using SchoolOperations.Bll.Interfaces;

namespace SchoolOperations.DAL.Services;

public class UserService : IUserService
{
    private static readonly string[] GraphScopes = ["User.ReadBasic.All"];
    private readonly TeamsUserCredential _credential;
    private readonly AuthenticationOptions _authenticationOptions;

    public UserService(TeamsUserCredential credential, IOptions<AuthenticationOptions> authenticationOptions)
    {
        _credential = credential;
        _authenticationOptions = authenticationOptions.Value;
    }
    public async Task<List<AppUserDTO>> GetAssignableUsersAsync()
    {
        var ssoToken = await _credential.GetTokenAsync(
            new TokenRequestContext([]),
            CancellationToken.None);

        var tenantId = new Uri(_authenticationOptions.OAuthAuthority)
            .AbsolutePath
            .Trim('/');

        var oboCredential = new OnBehalfOfCredential(
            tenantId,
            _authenticationOptions.ClientId,
            _authenticationOptions.ClientSecret,
            ssoToken.Token);

        var graphClient = new GraphServiceClient(
            oboCredential,
            GraphScopes);

        var response = await graphClient.Users.GetAsync(request =>
        {
            request.QueryParameters.Select =
            [
                "id",
                "displayName",
                "mail",
                "userPrincipalName"
            ];
            request.QueryParameters.Top = 50;
        });

        return response?.Value?
            .Where(user => !string.IsNullOrWhiteSpace(user.Id))
            .Select(user => new AppUserDTO
            {
                Id = user.Id!,
                DisplayName = user.DisplayName ?? string.Empty,
                Email = user.Mail ?? user.UserPrincipalName ?? string.Empty
            }).OrderBy(user => user.DisplayName).ToList() ?? [];
    }

    public async Task<AppUserDTO> GetCurrentUserAsync()
    {
        var user = await _credential.GetUserInfoAsync();
        if (string.IsNullOrWhiteSpace(user.ObjectId))
        {
            throw new InvalidOperationException("Unable to determine current Teams-user");
        }
        return new AppUserDTO
        {
            Id = user.ObjectId,
            DisplayName = user.DisplayName ?? string.Empty,
            Email = user.PreferredUserName ?? string.Empty
        };
    }
}
