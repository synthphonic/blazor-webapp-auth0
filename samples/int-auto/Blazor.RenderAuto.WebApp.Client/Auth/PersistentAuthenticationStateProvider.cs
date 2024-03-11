using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Blazor.RenderAuto.WebApp.Client.Auth;

public class PersistentAuthenticationStateProvider(PersistentComponentState persistentState) : AuthenticationStateProvider
{
    private static readonly Task<AuthenticationState> _unauthenticatedTask =
        Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (!persistentState.TryTakeFromJson<UserInfo>(nameof(UserInfo), out var userInfo) || userInfo is null)
        {
            return _unauthenticatedTask;
        }

        // Claim[] claims = [
        //     new Claim(ClaimTypes.NameIdentifier, userInfo.UserId!),
        //     new Claim(ClaimTypes.Name, userInfo.Name ?? string.Empty),
        //     new Claim("picture", userInfo.Picture ?? string.Empty),
        //     new Claim("sid", userInfo.Sid ?? string.Empty),
        //     new Claim(ClaimTypes.NameIdentifier, userInfo.NameIdentifier ?? string.Empty),
        //     new Claim(ClaimTypes.Email, userInfo.Email ?? string.Empty)];
        var claims = new List<Claim>();
        foreach (var item in userInfo.ClaimsDictionary)
        {            
            claims.Add(new Claim(item.Key, item.Value));
        }
        claims.Add(new Claim(ClaimTypes.Name, userInfo.Name!)); // used for primary claims identity

        return Task.FromResult(
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims,
                authenticationType: nameof(PersistentAuthenticationStateProvider)))));
    }
}