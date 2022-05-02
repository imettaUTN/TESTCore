using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MovieService.WebAPI
{
    public class SetLanguageMiddleware
    {
        private readonly RequestDelegate _next;

        public SetLanguageMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next.Invoke(context);
        }
    }
}
