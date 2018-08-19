using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Lykke.Service.LykkeDevelopers.Extentions
{
    public static class HttpContextExtensions
    {
        public static string GetUserEmail(this HttpContext ctx) =>
            ctx.User.Claims.FirstOrDefault(u => u.Type.Equals(ClaimTypes.Sid))?.Value;

        public static string GetUserName(this HttpContext ctx) =>
            ctx.User.Claims.FirstOrDefault(u => u.Type.Equals(ClaimTypes.Name))?.Value;

        public static bool IsAdmin(this HttpContext ctx) => bool.TryParse(ctx.User.Claims.FirstOrDefault(u => u.Type.Equals("IsAdmin"))?.Value, out bool res) && res;
    }
}
