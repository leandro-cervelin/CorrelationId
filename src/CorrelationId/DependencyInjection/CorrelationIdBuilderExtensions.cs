﻿using CorrelationId.Abstractions;
using CorrelationId.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CorrelationId.DependencyInjection;

/// <summary>
///     Provides basic extension methods for configuring the correlation ID provider in an
///     <see cref="ICorrelationIdBuilder" />.
/// </summary>
public static class CorrelationIdBuilderExtensions
{
    private const string MultipleProviderExceptionMessage =
        "A provider has already been registered. Only a single provider may be registered.";

    /// <summary>
    ///     Clear the existing <see cref="ICorrelationIdProvider" /> if one has been registered.
    /// </summary>
    /// <param name="builder">The <see cref="ICorrelationIdBuilder" />.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="builder" /> parameter is null.</exception>
    public static ICorrelationIdBuilder ClearProvider(this ICorrelationIdBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.RemoveAll<ICorrelationIdProvider>();

        return builder;
    }

    /// <summary>
    ///     Registers the <see cref="GuidCorrelationIdProvider" /> for use when generating correlation IDs.
    /// </summary>
    /// <param name="builder">The <see cref="ICorrelationIdBuilder" />.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="builder" /> parameter is null.</exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if a <see cref="ICorrelationIdProvider" /> has already been
    ///     registered.
    /// </exception>
    public static ICorrelationIdBuilder WithGuidProvider(this ICorrelationIdBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        if (builder.Services.Any(x => x.ServiceType == typeof(ICorrelationIdProvider)))
            throw new InvalidOperationException(MultipleProviderExceptionMessage);

        builder.Services.TryAddSingleton<ICorrelationIdProvider, GuidCorrelationIdProvider>();

        return builder;
    }

    /// <summary>
    ///     Registers the <see cref="TraceIdCorrelationIdProvider" /> for use when generating correlation IDs.
    /// </summary>
    /// <param name="builder">The <see cref="ICorrelationIdBuilder" />.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="builder" /> parameter is null.</exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if a <see cref="ICorrelationIdProvider" /> has already been
    ///     registered.
    /// </exception>
    public static ICorrelationIdBuilder WithTraceIdentifierProvider(this ICorrelationIdBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        if (builder.Services.Any(x => x.ServiceType == typeof(ICorrelationIdProvider)))
            throw new InvalidOperationException("A provider has already been added.");

        builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.TryAddSingleton<ICorrelationIdProvider, TraceIdCorrelationIdProvider>();

        return builder;
    }

    /// <summary>
    ///     Registers an existing instance of a custom <see cref="ICorrelationIdProvider" />.
    /// </summary>
    /// <param name="builder">The <see cref="ICorrelationIdBuilder" />.</param>
    /// <param name="provider">The <see cref="ICorrelationIdProvider" /> instance to register.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="builder" /> parameter is null.</exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if a <see cref="ICorrelationIdProvider" /> has already been
    ///     registered.
    /// </exception>
    public static ICorrelationIdBuilder WithCustomProvider(this ICorrelationIdBuilder builder,
        ICorrelationIdProvider provider)
    {
        ArgumentNullException.ThrowIfNull(builder);

        ArgumentNullException.ThrowIfNull(provider);

        if (builder.Services.Any(x => x.ServiceType == typeof(ICorrelationIdProvider)))
            throw new InvalidOperationException("A provider has already been added.");

        builder.Services.TryAddSingleton(provider);

        return builder;
    }

    /// <summary>
    ///     Registers a custom <see cref="ICorrelationIdProvider" />.
    /// </summary>
    /// <typeparam name="T">The <see cref="ICorrelationIdProvider" /> implementation type.</typeparam>
    /// <param name="builder">The <see cref="ICorrelationIdBuilder" />.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="builder" /> parameter is null.</exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if a <see cref="ICorrelationIdProvider" /> has already been
    ///     registered.
    /// </exception>
    public static ICorrelationIdBuilder WithCustomProvider<T>(this ICorrelationIdBuilder builder)
        where T : class, ICorrelationIdProvider
    {
        ArgumentNullException.ThrowIfNull(builder);

        if (builder.Services.Any(x => x.ServiceType == typeof(ICorrelationIdProvider)))
            throw new InvalidOperationException("A provider has already been added.");

        builder.Services.TryAddSingleton<ICorrelationIdProvider, T>();

        return builder;
    }
}