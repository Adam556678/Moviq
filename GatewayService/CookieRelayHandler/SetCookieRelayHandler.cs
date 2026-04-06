
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
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, // outgoing request from gateway
            CancellationToken cancellationToken
        )
        {
            var context = _httpContextAccessor.HttpContext; // get the incoming request from the browser

            // ----- Browser -> Gateway -> Sub-services -----
            // Grab cookies from the original browser request and attach them to the outgoing HTTP call
            if (context?.Request.Headers.ContainsKey("Cookie") == true)
            {
                request.Headers.Add("Cookie", context.Request.Headers["Cookie"].ToString());
            }

            // ----- Sub-services -> Gateway -> Browser -----
            // check if the downstream service returned cookies
            var response = await base.SendAsync(request, cancellationToken);
            if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
            {
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