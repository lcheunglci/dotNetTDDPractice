﻿using System;
using Xunit;

namespace DeskBooker.Core.Validation
{
    public class DateWithoutTimeAttributeTests
    {
        [InlineData(true, 0, 0, 0)]
        [InlineData(false, 1, 0, 0)]
        [InlineData(false, 0, 1, 0)]
        [InlineData(false, 0, 0, 1)]
        [Theory]
        public void ShouldReturnDateMustBeInTheFuture(bool expectedIsValid, int hour, int minute, int second)
        {
            var dateTime = new DateTime(2020, 1, 28, hour, minute, second);

            var attribute = new DateWithoutTimeAttribute();

            var isValid = attribute.IsValid(dateTime);

            Assert.Equal(expectedIsValid, isValid);
        }

        [Fact]
        public void ShouldHaveExpectedErrorMessage()
        {
            var attribute = new DateWithoutTimeAttribute();

            Assert.Equal("Date must not contain time", attribute.ErrorMessage);
        }
    }
}
