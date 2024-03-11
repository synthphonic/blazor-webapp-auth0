using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;

namespace Blazor.RenderAuto.WebApp;

public class CustomRemoteUserAccount : AccountClaimsPrincipalFactory<RemoteUserAccount>
{
    public CustomRemoteUserAccount(IAccessTokenProviderAccessor accessor)
        : base(accessor)
    {
        //Console.WriteLine(nameof(CustomUserFactory));
    }

    public async override ValueTask<ClaimsPrincipal> CreateUserAsync(
        RemoteUserAccount account,
        RemoteAuthenticationUserOptions options)
    {
        //Console.WriteLine($"{nameof(CustomUserFactory)}:{nameof(CreateUserAsync)}");

        var user = await base.CreateUserAsync(account, options);

        if (account == null)
            return user;

        //Console.WriteLine($"=== account is null: {account! == null}");
        //Console.WriteLine($"=== account.AdditionalProperties is null: {account!.AdditionalProperties == null}");

        if (account.AdditionalProperties!.TryGetValue(/*BlacksmithClaimTypes.Roles*/ "roles", out var rolesClaim))
        {
            //Console.WriteLine($"=== Blacksmith Role Claim : {rolesClaim.ToString()}");
            var rolesArray = JsonSerializer.Deserialize<string[]>(rolesClaim.ToString()!);

            var identity = (ClaimsIdentity)user.Identity!;

            foreach (var roleName in rolesArray!)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, roleName));
            }
        }

        if (account?.AdditionalProperties != null &&
            account.AdditionalProperties.TryGetValue("YOUR_CUSTOM_CLAIM_NAME", out var customClaimValue))
        {
            //Console.WriteLine($"==== ADD MORE PROPERTIES");

            var identity = (ClaimsIdentity)user.Identity!;
            identity.AddClaim(new Claim("YOUR_CUSTOM_CLAIM_NAME", customClaimValue.ToString()!));
        }

        //Console.WriteLine($"==== user != null [{user != null}]");
        //Console.WriteLine($"==== user!.Identity!.IsAuthenticated : [{user!.Identity!.IsAuthenticated}]");

        return user!;
    }
}