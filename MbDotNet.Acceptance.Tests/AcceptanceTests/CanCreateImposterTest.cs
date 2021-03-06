using System.Net;
using FluentAssertions;
using MbDotNet.Exceptions;

namespace MbDotNet.Acceptance.Tests.AcceptanceTests
{
    internal class CanCreateImposterTest
    {
        private readonly MountebankClient _client;
        const int ImposterPort = 6000;

        public CanCreateImposterTest(MountebankClient client)
        {
            _client = client;
            
        }

        public void Run()
        {
            DeleteAllImposters();
            CreateImposter();
            VerifyImposterHasBeenCreated();
            DeleteAllImposters();
        }

        private void DeleteAllImposters()
        {
            _client.DeleteAllImposters();
        }

        private void VerifyImposterHasBeenCreated()
        {
            var imposter = _client.GetImposter(ImposterPort);
            imposter.Should().NotBeNull();
        }

        private void CreateImposter()
        {
            var imposter = _client.CreateHttpImposter(ImposterPort);
            _client.Submit(imposter);
        }
    }
}