﻿using System;

using Microsoft.Extensions.Logging;

using Sokan.Yastah.Common;

namespace Microsoft.Extensions.Hosting
{
    internal static class HostingLogMessages
    {
        public enum EventType
        {
            BehaviorsStarting       = CommonLogEventType.Hosting + 0x0001,
            BehaviorsStarted        = CommonLogEventType.Hosting + 0x0002,
            BehaviorsStopping       = CommonLogEventType.Hosting + 0x0003,
            BehaviorsStopped        = CommonLogEventType.Hosting + 0x0004,
            StartupActionExecuting  = CommonLogEventType.Hosting + 0x0005,
            StartupActionExecuted   = CommonLogEventType.Hosting + 0x0006,
            BehaviorStarting        = CommonLogEventType.Hosting + 0x0007,
            BehaviorStarted         = CommonLogEventType.Hosting + 0x0008,
            BehaviorStopping        = CommonLogEventType.Hosting + 0x0009,
            BehaviorStopped         = CommonLogEventType.Hosting + 0x000A
        }

        public static IDisposable BeginBehaviorScope(
                ILogger logger,
                IBehavior behavior)
            => _beginBehaviorScope.Invoke(
                logger,
                behavior);
        private static readonly Func<ILogger, IBehavior, IDisposable> _beginBehaviorScope
            = LoggerMessage.DefineScope<IBehavior>(
                "Behavior: {Behavior}");

        public static IDisposable BeginStartupActionScope(
                ILogger logger,
                IScopedStartupAction startupAction)
            => _beginStartupActionScope.Invoke(
                logger,
                startupAction);
        private static readonly Func<ILogger, IScopedStartupAction, IDisposable> _beginStartupActionScope
            = LoggerMessage.DefineScope<IScopedStartupAction>(
                "StartupAction: {StartupAction}");

        public static void BehaviorsStarted(
                ILogger logger)
            => _behaviorsStarted.Invoke(
                logger);
        private static readonly Action<ILogger> _behaviorsStarted
            = LoggerMessage.Define(
                    LogLevel.Information,
                    new EventId(3002, nameof(BehaviorsStarted)),
                    $"All {nameof(IBehavior)}'s started")
                .WithoutException();

        public static void BehaviorsStarting(
                ILogger logger)
            => _behaviorsStarting.Invoke(
                logger);
        private static readonly Action<ILogger> _behaviorsStarting
            = LoggerMessage.Define(
                    LogLevel.Information,
                    new EventId(3001, nameof(BehaviorsStarting)),
                    $"Starting {nameof(IBehavior)}'s")
                .WithoutException();
                                    
        public static void BehaviorsStopped(
                ILogger logger)
            => _behaviorsStoped.Invoke(
                logger);
        private static readonly Action<ILogger> _behaviorsStoped
            = LoggerMessage.Define(
                    LogLevel.Information,
                    new EventId(3004, nameof(BehaviorsStopped)),
                    $"All {nameof(IBehavior)}'s stoped")
                .WithoutException();

        public static void BehaviorsStopping(
                ILogger logger)
            => _behaviorsStoping.Invoke(
                logger);
        private static readonly Action<ILogger> _behaviorsStoping
            = LoggerMessage.Define(
                    LogLevel.Information,
                    new EventId(3003, nameof(BehaviorsStopping)),
                    $"Stoping {nameof(IBehavior)}'s")
                .WithoutException();

        public static void BehaviorStarted(
                ILogger logger)
            => _behaviorStarted.Invoke(
                logger);
        private static readonly Action<ILogger> _behaviorStarted
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    new EventId(4002, nameof(BehaviorStarted)),
                    $"{nameof(IBehavior)} started")
                .WithoutException();

        public static void BehaviorStarting(
                ILogger logger)
            => _behaviorStarting.Invoke(
                logger);
        private static readonly Action<ILogger> _behaviorStarting
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    new EventId(4001, nameof(BehaviorsStarting)),
                    $"Starting {nameof(IBehavior)}")
                .WithoutException();

        public static void BehaviorStopped(
                ILogger logger)
            => _behaviorStoped.Invoke(
                logger);
        private static readonly Action<ILogger> _behaviorStoped
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    new EventId(4004, nameof(BehaviorStopped)),
                    $"{nameof(IBehavior)} stoped")
                .WithoutException();

        public static void BehaviorStopping(
                ILogger logger)
            => _behaviorStoping.Invoke(
                logger);
        private static readonly Action<ILogger> _behaviorStoping
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    new EventId(4003, nameof(BehaviorsStopping)),
                    $"Stoping {nameof(IBehavior)}")
                .WithoutException();

        public static void StartupActionExecuted(
                ILogger logger)
            => _startupActionExecuted.Invoke(
                logger);
        private static readonly Action<ILogger> _startupActionExecuted
            = LoggerMessage.Define(
                    LogLevel.Information,
                    new EventId(3006, nameof(StartupActionExecuted)),
                    $"{nameof(ScopedStartupActionBase)} executed")
                .WithoutException();

        public static void StartupActionExecuting(
                ILogger logger)
            => _startupActionExecuting.Invoke(
                logger);
        private static readonly Action<ILogger> _startupActionExecuting
            = LoggerMessage.Define(
                    LogLevel.Information,
                    new EventId(3005, nameof(StartupActionExecuting)),
                    $"Executing {nameof(ScopedStartupActionBase)}")
                .WithoutException();
    }
}
