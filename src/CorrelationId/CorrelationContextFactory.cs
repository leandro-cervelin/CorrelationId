using CorrelationId.Abstractions;

namespace CorrelationId;

/// <inheritdoc />
/// <summary>
///     Initialises a new instance of the <see cref="CorrelationContextFactory" /> class.
/// </summary>
/// <param name="correlationContextAccessor">
///     The <see cref="ICorrelationContextAccessor" /> through which the
///     <see cref="CorrelationContext" /> will be set.
/// </param>
public class CorrelationContextFactory(ICorrelationContextAccessor correlationContextAccessor) : ICorrelationContextFactory
{

    /// <summary>
    ///     Initialises a new instance of <see cref="CorrelationContextFactory" />.
    /// </summary>
    public CorrelationContextFactory()
        : this(null)
    {
    }

    /// <inheritdoc />
    public CorrelationContext Create(string correlationId, string header)
    {
        var correlationContext = new CorrelationContext(correlationId, header);

        if (correlationContextAccessor != null) correlationContextAccessor.CorrelationContext = correlationContext;

        return correlationContext;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (correlationContextAccessor != null) correlationContextAccessor.CorrelationContext = null;
    }
}