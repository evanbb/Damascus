using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Damascus.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Damascus.Http
{
    public static class IApplicationBuilderExtensions
    {
        public static void UseExceptionStatusMap(
            this IApplicationBuilder builder
        )
        {
            builder.UseExceptionStatusMap(new Dictionary<Type, int>
            {
                { typeof (InvalidOperationException), 400 }
            });
        }

        public static void UseExceptionStatusMap(
            this IApplicationBuilder builder,
            IDictionary<Type, int> map
        )
        {
            //builder.UseExceptionHandler("/error");

            //builder.UseExceptionHandler(new ExceptionHandlerOptions
            //{
            //    ExceptionHandler = async ctxt =>
            //    {
            //        var exception = ctxt.Features.Get<IExceptionHandlerPathFeature>()?.Error;

            //        if (exception.IsNull())
            //        {
            //            return;
            //        }

            //        var exceptionType = exception.GetType();

            //        if (!map.TryGetValue(exceptionType, out var newStatusCode))
            //        {
            //            return;
            //        }

            //        ctxt.Response.Clear();
            //        ctxt.Response.StatusCode = newStatusCode;
            //        await ctxt.Response.WriteAsync(exception.Message);
            //        await ctxt.Response.WriteAsync(exception.StackTrace);
            //    }
            //});
        }
    }

    public class ExceptionMappingFilter : IExceptionFilter
    {
        //private readonly IDictionary<Type, int> _map;

        private readonly IDictionary<Type, int> _map = new Dictionary<Type, int>
        {
            { typeof (InvalidOperationException), 400 }
        };

        //public ExceptionMappingFilter(IDictionary<Type, int> map)
        //{
        //    _map = map;
        //}

        public void OnException(ExceptionContext context)
        {
            if (!_map.TryGetValue(context.Exception.GetType(), out var newStatus))
            {
                return;
            }

            context.HttpContext.Response.StatusCode = newStatus;
            context.ExceptionHandled = true;
        }
    }

    //[ApiController]
    //public class ErrorController : ControllerBase
    //{
    //    [Route("/error")]
    //    public IActionResult Error() => Problem();
    //}
}
