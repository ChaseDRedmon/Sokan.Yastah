﻿using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.Hosting
{
    public interface IScopedStartupAction
        : IBehavior
    {
        Task WhenDone { get; }
    }

    public abstract class ScopedStartupActionBase
        : IScopedStartupAction
    {
        #region Construction

        protected ScopedStartupActionBase(
            ILogger logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        #endregion Construction

        #region Public Properties

        public Task WhenDone
            => _whenDone.Task;

        #endregion Public Properties

        #region IBehavior

        public async Task StartAsync(
            CancellationToken cancellationToken)
        {
            using var logScope = HostingLogMessages.BeginStartupActionScope(_logger, this);
            HostingLogMessages.StartupActionExecuting(_logger);

            using var serviceScope = _serviceScopeFactory.CreateScope();
            DependencyInjectionLogMessages.ServiceScopeCreated(_logger, serviceScope);

            await OnStartingAsync(
                serviceScope.ServiceProvider,
                cancellationToken);

            HostingLogMessages.StartupActionExecuted(_logger);
            _whenDone.SetResult(null);
        }

        public Task StopAsync(
                CancellationToken cancellationToken)
            => Task.CompletedTask;

        #endregion IBehavior

        #region Protected Members

        internal protected abstract Task OnStartingAsync(
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken);

        #endregion Protected Members

        #region State

        internal protected readonly ILogger _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly TaskCompletionSource<object?> _whenDone
            = new TaskCompletionSource<object?>();

        #endregion State
    }
}
