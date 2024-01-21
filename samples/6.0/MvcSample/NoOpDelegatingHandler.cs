using System.Net;

namespace MvcSample;

public class NoOpDelegatingHandler(ILogger<NoOpDelegatingHandler> logger) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request.Headers.TryGetValues("X-Correlation-Id", out var headerEnumerable))
            logger.LogInformation("Request has the following correlation ID header {CorrelationId}.",
                headerEnumerable.FirstOrDefault());
        else
            logger.LogInformation("Request does not have a correlation ID header.");

        var response = new HttpResponseMessage(HttpStatusCode.OK);

        return Task.FromResult(response);
    }
}