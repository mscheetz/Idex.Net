using Idex.Net.Core;
using Idex.Net.Data;
using Idex.Net.Data.Interface;
using Idex.Net.Entities;
using System;
using Xunit;

namespace Idex.Net.Tests
{
    public class SecurityTests : IDisposable
    {
        public void Dispose()
        {
        }

        [Fact]
        public void SecureUnSecureTest()
        {
            var unsecureString = "this is an unsecure string";

            var secureString = Security.ToSecureString(unsecureString);

            var unsecuredString = Security.ToUnsecureString(secureString);

            Assert.Equal(unsecuredString, unsecureString);
        }
    }
}
