namespace MedicaBE.Auth
{
    public class CookieAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly List<string> _excludedPaths;

        public CookieAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
            _excludedPaths = new List<string> { "/api/User/UserLogin" , "/api/User/GetUserById" };
        }

        public async Task Invoke(HttpContext context)
        {
            var requestPath = context.Request.Path;

            if (!_excludedPaths.Contains(requestPath))
            {
                if (context.Request.Cookies.TryGetValue("userInfo", out var userInfoJson))
                {
                    if (IsValidCookie(userInfoJson))
                    {
                        await _next.Invoke(context);
                        return;
                    }
                }

                context.Response.StatusCode = 401;
                return;
            }

            await _next.Invoke(context);
        }

        private bool IsValidCookie(string cookieValue)
        {
            return !string.IsNullOrEmpty(cookieValue);
        }
    }




}
