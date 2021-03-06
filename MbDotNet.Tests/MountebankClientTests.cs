﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MbDotNet.Enums;
using MbDotNet.Interfaces;
using MbDotNet.Models.Imposters;
using Moq;

namespace MbDotNet.Tests
{
    [TestClass]
    public class MountebankClientTests
    {
        private IClient _client;
        private Mock<IRequestProxy> _mockRequestProxy;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRequestProxy = new Mock<IRequestProxy>();
            _client = new MountebankClient(_mockRequestProxy.Object);
        }

        [TestMethod]
        public void Constructor_InitializesImposterCollection()
        {
            var client = new MountebankClient();
            Assert.IsNotNull(client.Imposters);
        }

        [TestMethod]
        public void CreateHttpImposter_ShouldNotAddNewImposterToCollection()
        {
            _client.CreateHttpImposter(123);
            Assert.AreEqual(0, _client.Imposters.Count);
        }

        [TestMethod]
        public void CreateHttpImposter_WithoutName_SetsNameToNull()
        {
            var imposter = _client.CreateHttpImposter(123);
            
            Assert.IsNotNull(imposter);
            Assert.IsNull(imposter.Name);
        }

        [TestMethod]
        public void CreateHttpImposter_WithName_SetsName()
        {
            const string expectedName = "Service";

            var imposter = _client.CreateHttpImposter(123, expectedName);
            
            Assert.IsNotNull(imposter);
            Assert.AreEqual(expectedName, imposter.Name);
        }

        [TestMethod]
        public void CreateTcpImposter_ShouldNotAddNewImposterToCollection()
        {
            _client.CreateTcpImposter(123);
            Assert.AreEqual(0, _client.Imposters.Count);
        }

        [TestMethod]
        public void CreateTcpImposter_WithoutName_SetsNameToNull()
        {
            var imposter = _client.CreateTcpImposter(123);

            Assert.IsNotNull(imposter);
            Assert.IsNull(imposter.Name);
        }

        [TestMethod]
        public void CreateTcpImposter_WithName_SetsName()
        {
            const string expectedName = "Service";

            var imposter = _client.CreateTcpImposter(123, expectedName);
            
            Assert.IsNotNull(imposter);
            Assert.AreEqual(expectedName, imposter.Name);
        }

        [TestMethod]
        public void CreateTcpImposter_WithoutMode_SetsModeToText()
        {
            const string expectedMode = "text";

            var imposter = _client.CreateTcpImposter(123, null);
            
            Assert.IsNotNull(imposter);
            Assert.AreEqual(expectedMode, imposter.Mode);
        }

        [TestMethod]
        public void CreateTcpImposter_WithMode_SetsMode()
        {
            const string expectedMode = "binary";

            var imposter = _client.CreateTcpImposter(123, null, TcpMode.Binary);

            Assert.IsNotNull(imposter);
            Assert.AreEqual(expectedMode, imposter.Mode);
        }

        [TestMethod]
        public void Submit_CallsSubmitOnAllPendingImposters()
        {
            const int firstPortNumber = 123;
            const int secondPortNumber = 456;

            var imposter1 = new HttpImposter(firstPortNumber, null);
            var imposter2 = new HttpImposter(secondPortNumber, null);
          
            _client.Submit(new [] { imposter1, imposter2});

            _mockRequestProxy.Verify(x => x.CreateImposter(It.Is<Imposter>(imp => imp.Port == firstPortNumber)), Times.Once);
            _mockRequestProxy.Verify(x => x.CreateImposter(It.Is<Imposter>(imp => imp.Port == secondPortNumber)), Times.Once);
        }

        [TestMethod]
        public void DeleteImposter_CallsDelete()
        {
            const int port = 8080;

            _client.Imposters.Add(new HttpImposter(port, null));

            _mockRequestProxy.Setup(x => x.DeleteImposter(port));

            _client.DeleteImposter(port);

            _mockRequestProxy.Verify(x => x.DeleteImposter(port), Times.Once);
        }

        [TestMethod]
        public void DeleteImposter_RemovesImposterFromCollection()
        {
            const int port = 8080;

            _client.Imposters.Add(new HttpImposter(port, null));

            _client.DeleteImposter(port);

            Assert.AreEqual(0, _client.Imposters.Count);
        }

        [TestMethod]
        public void DeleteAllImposters_CallsDeleteAll()
        {
            _mockRequestProxy.Setup(x => x.DeleteAllImposters());

            _client.Imposters.Add(new HttpImposter(123, null));
            _client.Imposters.Add(new HttpImposter(456, null));

            _client.DeleteAllImposters();

            _mockRequestProxy.Verify(x => x.DeleteAllImposters(), Times.Once);
        }

        [TestMethod]
        public void DeleteAllImposters_RemovesAllImpostersFromCollection()
        {
            _mockRequestProxy.Setup(x => x.DeleteAllImposters());

            _client.Imposters.Add(new HttpImposter(123, null));
            _client.Imposters.Add(new HttpImposter(456, null));

            _client.DeleteAllImposters();

            Assert.AreEqual(0, _client.Imposters.Count);
        }

        [TestMethod]
        public void SubmitCollection_ShouldSubmitImpostersUsingProxy()
        {
            const int firstPortNumber = 123;
            const int secondPortNumber = 456;

            var imposter1 = _client.CreateHttpImposter(firstPortNumber);
            var imposter2 = _client.CreateHttpImposter(secondPortNumber);
            
            _client.Submit(new[] { imposter1, imposter2 });

            _mockRequestProxy.Verify(x => x.CreateImposter(It.Is<Imposter>(imp => imp.Port == firstPortNumber)), Times.Once);
            _mockRequestProxy.Verify(x => x.CreateImposter(It.Is<Imposter>(imp => imp.Port == secondPortNumber)), Times.Once);
        }

        [TestMethod]
        public void SubmitCollection_ShouldAddImpostersToCollection()
        {
            const int firstPortNumber = 123;
            const int secondPortNumber = 456;

            var imposter1 = _client.CreateHttpImposter(firstPortNumber);
            var imposter2 = _client.CreateHttpImposter(secondPortNumber);

            _client.Submit(new[] { imposter1, imposter2 });

            Assert.AreEqual(1, _client.Imposters.Count(x => x.Port == firstPortNumber));
            Assert.AreEqual(1, _client.Imposters.Count(x => x.Port == secondPortNumber));
        }
    }
}
