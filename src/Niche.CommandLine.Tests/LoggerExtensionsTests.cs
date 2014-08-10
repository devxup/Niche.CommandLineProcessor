﻿using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine.Tests
{
    [TestFixture]
    public class LoggerExtensionsTests
    {
        [Test]
        public void Tablefy_givenSimpleString_returnsString()
        {
            var original = new List<string> { "sample" };
            var result = LoggerExtensions.Tablefy(original);
            Assert.That(result, Is.EquivalentTo(original));
        }

        [Test]
        public void Tablefy_givenMultipleSimpleStrings_returnsStrings()
        {
            var original = new List<string> { "alpha", "beta", "gamma" };
            var result = LoggerExtensions.Tablefy(original);
            Assert.That(result, Is.EquivalentTo(original));
        }

        [Test]
        public void Tablefy_givenTabbedString_returnsConvertedString()
        {
            var original = new List<string> { "alpha\tbeta\tgamma" };
            var result = LoggerExtensions.Tablefy(original);
            Assert.That(result.Single(), Is.EqualTo("alpha   beta   gamma"));
        }

        [Test]
        public void Tablefy_givenTwoTabbedStrings_returnsFormattedText()
        {
            var original = new List<string> { "alpha\tbeta\tgamma", "one\ttwo\tthree" };
            var result = LoggerExtensions.Tablefy(original);
            var expected = new List<string>{"alpha   beta   gamma", "one     two    three"};
            Assert.That(result, Is.EquivalentTo(expected));
        }
        
        [Test]
        public void Failure_givenSimpleException_logsOneMessage()
        {
            var logger = Substitute.For<ILogger>();
            var ex = new Exception("This is an exception.");

            logger.Failure(ex);

            logger.Received(1).Failure(Arg.Any<string>());
        }

        [Test]
        public void Failure_givenNestedException_logsExpectedMessageCount()
        {
            var logger = Substitute.For<ILogger>();
            var inner = new InvalidOperationException();
            var ex = new Exception("This is an exception.", inner);

            logger.Failure(ex);

            logger.Received(2).Failure(Arg.Any<string>());
        }

        [Test]
        public void Failure_givenSimpleExceptionWithData_logsExpectedMessageCount()
        {
            var logger = Substitute.For<ILogger>();
            var ex = new Exception("This is an exception.");
            ex.Data["name"] = "George";
            ex.Data["repetitions"] = 45;
            
            logger.Failure(ex);
            
            logger.Received(3).Failure(Arg.Any<string>());
        }
    }
}
