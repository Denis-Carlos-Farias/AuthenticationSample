using AuthenticationSample.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace AuthenticationSample;

public class AuthenticationService
{
    public async Task Login(HttpContext ctx, User user)
    {
        var claims = new List<Claim>
         {
             new Claim(ClaimTypes.Name, user.UserName),
             new Claim(ClaimTypes.Role, user.Position)
         };

        var claimIdentity =
            new ClaimsPrincipal(
                new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                )
            );

        var authProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(2),
            IssuedUtc = DateTimeOffset.UtcNow
        };

        await ctx.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimIdentity, authProperties);

    }

    public async Task Logout(HttpContext ctx) => 
        await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
}
