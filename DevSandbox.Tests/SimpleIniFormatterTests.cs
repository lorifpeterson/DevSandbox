using Microsoft.VisualStudio.TestTools.UnitTesting;
using DevSandbox.Models;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace DevSandbox.Tests
{
    [TestClass]
    public class SimpleIniFormatterTests
    {
        [TestMethod]
        public void CanSerializeObject()
        {
            var detail = new SampleIni
            {
                name = "Abc Sample Company",
                email = "info@abc.com",

            };

            var ms = new MemoryStream();
            var serializer = new SimpleIniFormatter();
            serializer.Serialize(ms, detail);

            var text = Encoding.ASCII.GetString(ms.ToArray());
        }

        [TestMethod]
        public void CanDeSerializeObject()
        {
            var detail = new SampleIni
            {
                name = "Abc Sample Company",
                email = "info@abc.com",

            };

            var ms = new MemoryStream();
            var serializer = new SimpleIniFormatter();
            serializer.Serialize(ms, detail);

            var text = Encoding.ASCII.GetString(ms.ToArray());

            var obj = serializer.Deserialize(new MemoryStream(ms.ToArray()));
        }

        [TestMethod]
        public void CanSerializeToFile()
        {
            var detail = new SampleIni
            {
                name = "Abc Sample Company",
                email = "info@abc.com",
            };

            var ms = new FileStream(@"C:\TestIni.txt", FileMode.Create);
            var serializer = new SimpleIniFormatter();
            serializer.Serialize(ms, detail);

        }
    }
}
