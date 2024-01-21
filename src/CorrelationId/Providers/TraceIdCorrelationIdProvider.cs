using CorrelationId.Abstractions;
using Microsoft.AspNetCore.Http;

namespace CorrelationId.Providers;

/// <summary>
///     Sets the correlation ID to match the TraceIdentifier set on the <see cref="HttpContext" />.
/// </summary>
/// <remarks>
/// </remarks>
/// <param name="httpContextAccessor"></param>
public class TraceIdCorrelationIdProvider(IHttpContextAccessor httpContextAccessor) : ICorrelationIdProvider
{

    /// <inheritdoc />
    public string GenerateCorrelationId()
    {
        return httpContextAccessor.HttpContext?.TraceIdentifier;
    }
}