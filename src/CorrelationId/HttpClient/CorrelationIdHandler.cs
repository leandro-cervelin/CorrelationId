using CorrelationId.Abstractions;

namespace CorrelationId.HttpClient;

/// <summary>
///     A <see cref="DelegatingHandler" /> which adds the correlation ID header from the <see cref="CorrelationContext" />
///     onto outgoing HTTP requests.
/// </summary>
internal sealed class CorrelationIdHandler(ICorrelationContextAccessor correlationContextAccessor) : DelegatingHandler
{

    /// <inheritdoc cref="DelegatingHandler" />
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var correlationId = correlationContextAccessor?.CorrelationContext?.CorrelationId;
        if (!string.IsNullOrEmpty(correlationId)
            && !request.Headers.Contains(correlationContextAccessor.CorrelationContext.Header))
            request.Headers.Add(correlationContextAccessor.CorrelationContext.Header,
                correlationContextAccessor.CorrelationContext.CorrelationId);

        return base.SendAsync(request, cancellationToken);
    }
}