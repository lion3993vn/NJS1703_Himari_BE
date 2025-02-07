using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SWD392_Himari.Repository.Middleware
{
    public class DelayMiddleware
    {
        private readonly RequestDelegate _next;

        public DelayMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("deplay", out var delayValue) && int.TryParse(delayValue, out int delay))
            {
                await Task.Delay(delay);
            }

            await _next(context);
        }
    }
}
