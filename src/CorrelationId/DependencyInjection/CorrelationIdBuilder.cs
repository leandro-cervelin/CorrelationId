using Microsoft.Extensions.DependencyInjection;

namespace CorrelationId.DependencyInjection;

/// <inheritdoc />
internal class CorrelationIdBuilder(IServiceCollection services) : ICorrelationIdBuilder
{

    /// <inheritdoc />
    public IServiceCollection Services { get; } = services;
}