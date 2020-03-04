using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using DevSandbox.Models;

namespace DevSandbox.Tests
{
    [TestClass]
    public class AllowedIPTests
    {
        [TestMethod]
        public void IpIsAllowed()
        {
            int mask;
            Int32.TryParse("32", out mask);

            var allowedIps = new List<CIDR> { new CIDR { Address = BitConverter.ToInt32(IPAddress.Parse("10.4.40.10").GetAddressBytes(), 0), Mask = IPAddress.HostToNetworkOrder(-1 << (32-mask)) } };

            var ipAddress = "10.4.40.10";

            IPAddress address;
            IPAddress.TryParse(ipAddress, out address);
            var intAddress = BitConverter.ToInt32(address.GetAddressBytes(), 0);
            var result = allowedIps.Any(allowedIp => allowedIp.IsInRange(intAddress));
        }


    }

}
