using HotChocolate.AspNetCore;
using HotChocolate.Execution;

namespace GatewayService.CookieRelayInterceptor
{
    // DefaultHttpRequestInterceptor is part of the GraphQL request pipeline.
    // It runs before a GraphQL request is executed and lets you modify the request and
    // inject headers and cookies
    public class CookieRelayInterceptor : DefaultHttpRequestInterceptor
    {
        // called everytime a GraphQL request is created
        public override ValueTask OnCreateAsync(HttpContext context,
        IRequestExecutor executor, IQueryRequestBuilder requestBuilder,
        CancellationToken cancellationToken)
        {
            // This ensures the Gateway passes the cookies it receives from 
            // the client down to the sub-services

            // get cookies from the incoming browser request
            var cookieHeader = context.Request.Headers["Cookie"].ToString();
            if (!string.IsNullOrEmpty(cookieHeader))
            {
                // Attach cookies to the GraphQL request context
                requestBuilder.SetGlobalState("Cookie", cookieHeader);
            }

            return base.OnCreateAsync(context, executor, requestBuilder, cancellationToken);
        }
    }
}