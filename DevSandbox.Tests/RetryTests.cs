using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace DevSandbox.Tests
{
    [TestClass]
    public class RetryTests
    {
        [TestMethod]
        public void Execute_Succeeds()
        {
            var result = Retry.Execute(() => TestMethod(), 3, new TimeSpan(0, 0, 2));
            Assert.IsTrue(result);
        }

        private bool TestMethod()
        {
            return true;
        }
    }
}
