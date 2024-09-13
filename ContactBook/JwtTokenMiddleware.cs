﻿namespace ContactBook
{
    public class JwtTokenMiddleware
    {
        private readonly RequestDelegate _next;
        public JwtTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var jwtToken = context.Request.Cookies["jwtToken"];
            if (!string.IsNullOrWhiteSpace(jwtToken))
            {
                context.Request.Headers["Authorization"] = "Bearer " + jwtToken;
            }
            await _next(context);
            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                context.Response.Redirect("/Auth/Login");
            }
        }

    }
}

