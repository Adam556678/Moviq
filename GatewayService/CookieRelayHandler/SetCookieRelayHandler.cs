
namespace GatewayService.CookieRelayHandler
{
    // Its job is to listen to the traffic between the Gateway and the Sub-services.
    // DelegatingHandler in .NET is essentially a middleware for HTTP requests.
    // It can : Inspect the request, modify it, and pass it to the next handler
    public class SetCookieRelayHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SetCookieRelayHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // SendASync: Forwards the request to the next handler
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // check if the downstream service returned cookies
            var response = await base.SendAsync(request, cancellationToken);
            if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
            {
                var context = _httpContextAccessor.HttpContext;
                if (context != null)
                {
                    foreach (var cookie in cookies)
                    {
                        // Forward the cookie header to the Browser's response
                        context.Response.Headers.Append("Set-Cookie", cookie);
                    }
                }
            }

            return response;
        }
    }
}