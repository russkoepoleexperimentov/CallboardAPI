using System.Security.Claims;

namespace Web
{
    public static class HttpContextExtension
    {
        public static Guid? GetUserId(this HttpContext context)
        {
            var str = context.User?.Claims?.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Name, StringComparison.OrdinalIgnoreCase))?.Value;
            return str == null ? null : new Guid(str);
        }
    }

}
