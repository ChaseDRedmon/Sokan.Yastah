﻿using System;

namespace Sokan.Yastah.Common.OperationModel
{
    public struct OperationResult<T>
    {
        public static OperationResult<T> FromError(IOperationError error)
            => new OperationResult<T>(error, default);

        public static OperationResult<T> FromValue(T value)
            => new OperationResult<T>(null, value);

        private OperationResult(IOperationError error, T value)
        {
            _error = error;
            _value = value;
        }

        public bool IsFailure
            => !(_error is null);

        public bool IsSuccess
            => _error is null;

        public T Value
            => (_error is null)
                ? _value
                : throw new InvalidOperationException($"Unable to retrieve {nameof(Value)} from a failed {nameof(OperationResult<T>)}");

        private readonly IOperationError _error;

        private readonly T _value;
    }
}