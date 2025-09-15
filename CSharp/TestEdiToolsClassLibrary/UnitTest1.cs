using NUnit.Framework;
using EdiTools;

namespace TestEdiToolsClassLibrary
{
    public class Tests
    {
        private EdiFile TestFile835 { get; set; }
        private EdiFile TestFile835Wrapped { get; set; }
        private EdiFile TestFile837 { get; set; }
        private EdiFile TestFile837Wrapped { get; set; }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}