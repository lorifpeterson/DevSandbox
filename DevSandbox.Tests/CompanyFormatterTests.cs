using Microsoft.VisualStudio.TestTools.UnitTesting;
using DevSandbox.Models;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System;
using FluentAssertions;
using DevSandbox.Tests.SampleData;

namespace DevSandbox.Tests
{
    [TestClass]
    public class CompanyFormatterTests
    {
        [TestMethod]
        public void Can_Serialize_SimpleObject()
        {
            var expected = "[SIMPLEOBJECT]\r\n\tName:Simple\r\n\tDescription:Test\r\n";

            var simple = new SimpleObject { Name = "Simple", Description = "Test" };

            var memStream = new MemoryStream();
            var serializer = new CompanyFormatter();
            serializer.Serialize(memStream, simple);

            var result = Encoding.ASCII.GetString(memStream.ToArray());

            Console.Write(result);
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void Can_Serialize_DeSerialize_Company()
        {
            var company = SampleCompany.Create();

            var memStream = new MemoryStream();
            var serializer = new CompanyFormatter();
            serializer.Serialize(memStream, company);

            var obj = serializer.Deserialize(new MemoryStream(memStream.ToArray()));
            Assert.IsInstanceOfType(obj, typeof(Company));
            var companyResult = (Company)obj;

            companyResult.Should().BeEquivalentTo(company);
        }

        [TestMethod]
        public void Can_SerializeToFile_DeSerializeFromFile()
        {
            var fileName = @"C:\Testcompany.cfg";
            if (File.Exists(fileName)) File.Delete(fileName);

            var company = SampleCompany.Create();

            // Create Config File
            var fileStream = new FileStream(fileName, FileMode.Create);
            var serializer = new CompanyFormatter();
            serializer.Serialize(fileStream, company);

            File.Exists(fileName).Should().BeTrue();

            // Read Config File
            var obj = serializer.Deserialize(new FileStream(fileName, FileMode.Open));
            Assert.IsInstanceOfType(obj, typeof(Company));
            var companyResult = (Company)obj;

            companyResult.Should().BeEquivalentTo(company);
        }
    }
}
