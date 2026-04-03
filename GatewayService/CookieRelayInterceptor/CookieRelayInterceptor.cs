using HotChocolate.AspNetCore;
using HotChocolate.Execution;

namespace GatewayService.CookieRelayInterceptor
{
    public class CookieRelayInterceptor : DefaultHttpRequestInterceptor
    {
        public override ValueTask OnCreateAsync(HttpContext context,
        IRequestExecutor executor, IQueryRequestBuilder requestBuilder,
        CancellationToken cancellationToken)
    {
        // This ensures the Gateway passes the cookies it receives from 
        // the client down to the sub-services
        return base.OnCreateAsync(context, executor, requestBuilder, cancellationToken);
    }
    }
}