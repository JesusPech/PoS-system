using Microsoft.VisualStudio.TestTools.UnitTesting;
using ApplicationCore.Interfaces;
using Moq;
using ApplicationCore;
using System.IO;
using System;
using System.Collections.Generic;

namespace PoS_TestProject
{
    [TestClass]
    public class POS_Test_Configuration 
    {
        [TestMethod]
        public void SetInitialConfiguration_Test()
        {
            const string _APPSETTINGSFILENAME = "appsettings.json";
            string _AppSettingsFullPath = Path.Combine(Environment.CurrentDirectory, _APPSETTINGSFILENAME);
            var config = new Configuration();
            config.SetInitialConfiguration();
            Assert.IsTrue(File.Exists(_AppSettingsFullPath));
        }

        [TestMethod]
        public void SetRegioUS_Test()
        {           
            var config = new Configuration();
            config.SetRegion("US",false);
            Assert.AreEqual("US", config.GetRegion());
        }

        [TestMethod]
        public void SetRegioMX_Test()
        {
            var config = new Configuration();
            config.SetRegion("MX", false);
            Assert.AreEqual("MX", config.GetRegion());
        }

        [TestMethod]
        public void SetRegioDefault_Test()
        {
            var config = new Configuration();
            config.SetRegion("AR", true);
            Assert.AreEqual("US", config.GetRegion());
        }

        [TestMethod]
        public void SetDenominationsUS_Test()
        {
            List<decimal> _USDenominations = new List<decimal>() { 0.01M, 0.05M, 0.10M, 0.25M, 0.50M, 1.00M, 2.00M, 5.00M, 10.00M, 20.00M, 50.00M, 100.00M };          
            var config = new Configuration();
            config.SetRegion("US", false);
            config.SetDenominations("US");
            CollectionAssert.AreEqual(_USDenominations, config.GetDenominations());
        }

        [TestMethod]
        public void SetDenominationsMX_Test()
        {
            List<decimal> _MXDenominations = new List<decimal>() { 0.05M, 0.10M, 0.20M, 0.50M, 1.00M, 2.00M, 5.00M, 10.00M, 20.00M, 50.00M, 100.00M };
            var config = new Configuration();
            config.SetRegion("MX", false);
            config.SetDenominations("MX");
            CollectionAssert.AreEqual(_MXDenominations, config.GetDenominations());
        }

        [TestMethod]
        public void SetDenominationsDefault_Test()
        {
            List<decimal> _USDenominations = new List<decimal>() { 0.01M, 0.05M, 0.10M, 0.25M, 0.50M, 1.00M, 2.00M, 5.00M, 10.00M, 20.00M, 50.00M, 100.00M };
            var config = new Configuration();
            config.SetRegion("AR", true);
            config.SetDenominations("AR");
            CollectionAssert.AreEqual(_USDenominations, config.GetDenominations());
        }


        [TestMethod]
        public void GetRegionUS_Test()
        {           
            var config = new Configuration();
            config.SetRegion("US", false);          
            Assert.AreEqual("US", config.GetRegion());
        }


        [TestMethod]
        public void GetRegionDefault_Test()
        {
            var config = new Configuration();
            config.SetRegion("AR", true);
            Assert.AreEqual("US", config.GetRegion());
        }

    }   
   
}
