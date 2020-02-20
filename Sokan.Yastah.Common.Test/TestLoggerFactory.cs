﻿using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Sokan.Yastah.Common.Test
{
    public sealed class TestLoggerFactory
        : IDisposable
    {
        public TestLoggerFactory()
        {
            _serviceProvider = new ServiceCollection()
                .AddLogging(loggingBuilder => loggingBuilder
                    .AddConsole(options => options
                        .IncludeScopes = true)
                    .SetMinimumLevel(LogLevel.Trace))
                .BuildServiceProvider();
        }

        public ILogger<T> CreateLogger<T>()
            => _serviceProvider.GetRequiredService<ILogger<T>>();

        public void Dispose()
            => _serviceProvider.Dispose();

        private readonly ServiceProvider _serviceProvider;
    }
}
