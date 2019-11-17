﻿using System;
using System.Collections.Generic;

using NUnit.Framework;
using Moq;
using Shouldly;

using Sokan.Yastah.Common.OperationModel;

namespace Sokan.Yastah.Common.Test.OperationModel
{
    [TestFixture]
    public class OperationResultTests
    {
        #region Test Cases

        public static readonly IReadOnlyList<string?> Value_TestCaseData
            = new[]
            {
                null,
                "Value"
            };

        public static readonly IReadOnlyList<TestCaseData> OperationResult_TestCaseData
            = new[]
            {
                /*                  error           */
                new TestCaseData(   null            ).SetName("{m}(Success)"),
                new TestCaseData(   MakeFakeError() ).SetName("{m}(Failure)")
            };

        public static readonly IReadOnlyList<TestCaseData> OperationResultsAreNotEqual_TestCaseData
            = new[]
            {
                /*                  xError,             yError          */
                new TestCaseData(   MakeFakeError(),    MakeFakeError() ).SetName("{m}(Both Failure, Different Error)"),
                new TestCaseData(   null,               MakeFakeError() ).SetName("{m}(Success, Failure)"),
                new TestCaseData(   MakeFakeError(),    null            ).SetName("{m}(Failure, Success)"),
            };

        private static IOperationError MakeFakeError()
            => new Mock<IOperationError>().Object;

        #endregion Test Cases

        #region Default Tests

        [Test]
        public void Default_Always_IsSuccess()
        {
            var result = default(OperationResult);

            result.IsSuccess.ShouldBeTrue();
            result.IsFailure.ShouldBeFalse();
        }

        [Test]
        public void Default_Always_ErrorThrowsException()
        {
            Should.Throw<InvalidOperationException>(() =>
            {
                _ = default(OperationResult).Error;
            });
        }

        #endregion Default Tests

        #region Success Tests

        [Test]
        public void Success_Always_IsSuccess()
        {
            var result = OperationResult.Success;

            result.IsSuccess.ShouldBeTrue();
            result.IsFailure.ShouldBeFalse();
        }

        [Test]
        public void Success_Always_ErrorThrowsException()
        {
            Should.Throw<InvalidOperationException>(() =>
            {
                _ = OperationResult.Success.Error;
            });
        }

        #endregion Success Tests

        #region FromError() Tests

        [Test]
        public void FromError_Otherwise_IsFailure()
        {
            var error = new Mock<IOperationError>().Object;

            var result = OperationResult.FromError(error);

            result.IsSuccess.ShouldBeFalse();
            result.IsFailure.ShouldBeTrue();
        }

        [Test]
        public void FromError_Otherwise_ErrorIsGiven()
        {
            var error = new Mock<IOperationError>().Object;

            var result = OperationResult.FromError(error);

            result.Error.ShouldBeSameAs(error);
        }

        #endregion FromError() Tests

        #region FromError<T>() Tests

        [Test]
        public void FromError_Generic_Otherwise_IsFailure()
        {
            var error = new Mock<IOperationError>().Object;

            var result = OperationResult.FromError<string>(error);

            result.IsSuccess.ShouldBeFalse();
            result.IsFailure.ShouldBeTrue();
        }

        [Test]
        public void FromError_Generic_Otherwise_ErrorIsGiven()
        {
            var error = new Mock<IOperationError>().Object;

            var result = OperationResult.FromError<string>(error);

            result.Error.ShouldBeSameAs(error);
        }

        [Test]
        public void FromError_Generic_Otherwise_ValueThrowsException()
        {
            var error = new Mock<IOperationError>().Object;

            var result = OperationResult.FromError<string>(error);

            Should.Throw<InvalidOperationException>(() =>
            {
                _ = result.Value;
            });
        }

        #endregion FromError<T>() Tests

        #region FromValue<T>() Tests

        [TestCaseSource(nameof(Value_TestCaseData))]
        public void FromValue_Always_IsSuccess(
            string? value)
        {
            var result = OperationResult.FromValue(value);

            result.IsSuccess.ShouldBeTrue();
            result.IsFailure.ShouldBeFalse();
        }

        [TestCaseSource(nameof(Value_TestCaseData))]
        public void FromValue_Always_ErrorThrowsException(
            string? value)
        {
            var result = OperationResult.FromValue(value);

            Should.Throw<InvalidOperationException>(() =>
            {
                _ = result.Error;
            });
        }

        [TestCaseSource(nameof(Value_TestCaseData))]
        public void FromValue_Always_ValueIsGiven(
            string? value)
        {
            var result = OperationResult.FromValue(value);

            result.Value.ShouldBe(value);
        }

        #endregion FromValue<T>() Tests

        #region Equals() Tests

        [TestCaseSource(nameof(OperationResult_TestCaseData))]
        public void Equals_NonGeneric_ObjIsNull_ReturnsFalse(
            IOperationError? error)
        {
            var uut = (error is null)
                ? OperationResult.Success
                : OperationResult.FromError(error);

            uut.Equals(null).ShouldBeFalse();
        }

        [TestCaseSource(nameof(OperationResult_TestCaseData))]
        public void Equals_NonGeneric_ObjIsNotOperationResult_ReturnsFalse(
            IOperationError? error)
        {
            var uut = (error is null)
                ? OperationResult.Success
                : OperationResult.FromError(error);

            uut.Equals("obj").ShouldBeFalse();
        }

        [TestCaseSource(nameof(OperationResult_TestCaseData))]
        public void Equals_NonGeneric_ObjIsEqual_ReturnsTrue(
            IOperationError? error)
        {
            var (uut, obj) = (error is null)
                ? (OperationResult.Success,             OperationResult.Success as object)
                : (OperationResult.FromError(error),    OperationResult.FromError(error) as object);

            uut.Equals(obj).ShouldBeTrue();
        }

        [TestCaseSource(nameof(OperationResultsAreNotEqual_TestCaseData))]
        public void Equals_NonGeneric_ObjIsNotEqual_ReturnsFalse(
            IOperationError? xError,
            IOperationError? yError)
        {
            var uut = (xError is null)
                ? OperationResult.Success
                : OperationResult.FromError(xError);

            var obj = (yError is null)
                    ? OperationResult.Success
                    : OperationResult.FromError(yError)
                as object;

            uut.Equals(obj).ShouldBeFalse();
        }

        #endregion Equals() Tests

        #region Equals<T>() Tests

        [TestCaseSource(nameof(OperationResult_TestCaseData))]
        public void Equals_Generic_OtherIsEqual_ReturnsTrue(
            IOperationError? error)
        {
            var (uut, other) = (error is null)
                ? (OperationResult.Success,             OperationResult.Success)
                : (OperationResult.FromError(error),    OperationResult.FromError(error));

            uut.Equals(other).ShouldBeTrue();
        }

        [TestCaseSource(nameof(OperationResultsAreNotEqual_TestCaseData))]
        public void Equals_Generic_OtherIsNotEqual_ReturnsFalse(
            IOperationError? xError,
            IOperationError? yError)
        {
            var uut = (xError is null)
                ? OperationResult.Success
                : OperationResult.FromError(xError);

            var other = (yError is null)
                ? OperationResult.Success
                : OperationResult.FromError(yError);

            uut.Equals(other).ShouldBeFalse();
        }

        #endregion Equals<T>() Tests

        #region GetHashCode() Tests

        [TestCaseSource(nameof(OperationResult_TestCaseData))]
        public void GetHashCode_UnitsAreEqual_HashCodesAreEqual(
            IOperationError? error)
        {
            var (uut, other) = (error is null)
                ? (OperationResult.Success, OperationResult.Success)
                : (OperationResult.FromError(error), OperationResult.FromError(error));

            uut.GetHashCode().ShouldBe(other.GetHashCode());
        }

        [TestCaseSource(nameof(OperationResultsAreNotEqual_TestCaseData))]
        public void GetHashCode_UnitsAreNotEqual_HashCodesAreNotEqual(
            IOperationError? xError,
            IOperationError? yError)
        {
            var uut = (xError is null)
                ? OperationResult.Success
                : OperationResult.FromError(xError);

            var other = (yError is null)
                ? OperationResult.Success
                : OperationResult.FromError(yError);

            uut.GetHashCode().ShouldNotBe(other.GetHashCode());
        }

        #endregion GetHashCode() Tests

        #region == Tests

        [TestCaseSource(nameof(OperationResult_TestCaseData))]
        public void Operator_Equals_XAndYAreEqual_ReturnsTrue(
            IOperationError? error)
        {
            var (x, y) = (error is null)
                ? (OperationResult.Success, OperationResult.Success)
                : (OperationResult.FromError(error), OperationResult.FromError(error));

            (x == y).ShouldBeTrue();
        }

        [TestCaseSource(nameof(OperationResultsAreNotEqual_TestCaseData))]
        public void Operator_Equals_XAndYAreNotEqual_ReturnsFalse(
            IOperationError? xError,
            IOperationError? yError)
        {
            var x = (xError is null)
                ? OperationResult.Success
                : OperationResult.FromError(xError);

            var y = (yError is null)
                ? OperationResult.Success
                : OperationResult.FromError(yError);

            (x == y).ShouldBeFalse();
        }

        #endregion == Tests

        #region != Tests

        [TestCaseSource(nameof(OperationResult_TestCaseData))]
        public void Operator_NotEquals_XAndYAreEqual_ReturnsFalse(
            IOperationError? error)
        {
            var (x, y) = (error is null)
                ? (OperationResult.Success, OperationResult.Success)
                : (OperationResult.FromError(error), OperationResult.FromError(error));

            (x != y).ShouldBeFalse();
        }

        [TestCaseSource(nameof(OperationResultsAreNotEqual_TestCaseData))]
        public void Operator_NotEquals_XAndYAreNotEqual_ReturnsTrue(
            IOperationError? xError,
            IOperationError? yError)
        {
            var x = (xError is null)
                ? OperationResult.Success
                : OperationResult.FromError(xError);

            var y = (yError is null)
                ? OperationResult.Success
                : OperationResult.FromError(yError);

            (x != y).ShouldBeTrue();
        }

        #endregion != Tests
    }
}
