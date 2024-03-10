using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Auth0.AspNetCore.Authentication.Custom;

public static class OidcAuthenticationRegistrationExtension
{
    public static void AddOidcPKCEAuthentication<TRemoteUser>(this IServiceCollection services, Action<AuthOptions> options) 
        where TRemoteUser : AccountClaimsPrincipalFactory<RemoteUserAccount>
    {
        Console.WriteLine("INSIDE AddOidcPKCEAuthentication");

        services.AddOidcAuthentication(oidcOptions =>
        {
            // Configure your authentication provider options here.
            // For more information, see https://aka.ms/blazor-standalone-auth
            //builder.Configuration.Bind("Local", options.ProviderOptions);

            // builder.Configuration.Bind("Auth0", options.ProviderOptions);
            Console.WriteLine("INSIDE AddOidcPKCEAuthentication 2");
            AuthOptions configureOptions = new();
            options(configureOptions);
            
            oidcOptions.ProviderOptions.AdditionalProviderParameters.Add("audience", configureOptions.Audience!);
            oidcOptions.ProviderOptions.PostLogoutRedirectUri = configureOptions.PostLogoutRedirectUri;

            // Use Authorization Code Flow With PKCE if it uses ResponseType = code
            oidcOptions.ProviderOptions.ResponseType = configureOptions.ResponseType;

            // Request additional scopes
            foreach (var scope in configureOptions.DefaultScopes!.Split(' '))
            {
                // oidcOptions.ProviderOptions.DefaultScopes.Add("email");
                oidcOptions.ProviderOptions.DefaultScopes.Add(scope);
            }            
        }).AddAccountClaimsPrincipalFactory<TRemoteUser>();
    }
}

public class AuthOptions
{
    public string? Audience { get; set; }
    public string? PostLogoutRedirectUri { get; set; } = "/logout";
    public string? CallbackUri { get; set; } = "/callback";
    public string? DefaultScopes { get; set; }
    /// <summary>
    /// Defaulted to 'code'
    /// </summary>
    /// <value></value>
    public string? ResponseType { get; set; } = "code";
}