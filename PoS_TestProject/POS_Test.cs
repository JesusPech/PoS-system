using Microsoft.VisualStudio.TestTools.UnitTesting;
using ApplicationCore.Interfaces;
using Moq;
using ApplicationCore;
using System.IO;
using System;

namespace PoS_TestProject
{
    [TestClass]
    public class POS_Test 
    {
        [TestMethod]
        public void GetRegion()
        {
            var serviceMock = new Mock<IPOCConfiguration>();
            serviceMock
                .Setup(m => m.GetRegion())//the expected method called with provided Id
                .Returns("US")//If called as expected what result to return
                .Verifiable();//expected service behavior can be verified

            var x = serviceMock.Object.GetRegion();
            Assert.AreEqual("US", x);


        }

        [TestMethod]
        public void SetInitialConfiguration_Test()
        {
            const string _APPSETTINGSFILENAME = "appsettings.json";
            string _AppSettingsFullPath = Path.Combine(Environment.CurrentDirectory, _APPSETTINGSFILENAME);
            var config = new Configuration();
            config.SetInitialConfiguration();
            Assert.IsTrue(File.Exists(_AppSettingsFullPath)); 
        }
    }

    //Samples class and interface to explain example
   
}
