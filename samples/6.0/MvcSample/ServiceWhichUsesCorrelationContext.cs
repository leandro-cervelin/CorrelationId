using CorrelationId.Abstractions;

namespace MvcSample;

public class ServiceWhichUsesCorrelationContext(ICorrelationContextAccessor correlationContextAccessor)
{
    public string DoStuff()
    {
        var correlationId = correlationContextAccessor.CorrelationContext.CorrelationId;

        return $"Formatted correlation ID:{correlationId}";
    }
}