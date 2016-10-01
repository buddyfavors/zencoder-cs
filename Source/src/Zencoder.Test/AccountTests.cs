//-----------------------------------------------------------------------
// <copyright file="AccountTests.cs" company="Tasty Codes">
//     Copyright (c) 2010 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------

namespace Zencoder.Test
{
    using System;
    using System.Threading;
    using Xunit;

    /// <summary>
    /// Account tests.
    /// </summary>
    public class AccountTests : TestBase
    {
        /// <summary>
        /// Account details request tests.
        /// </summary>
        [Fact]
        public async void AccountAccountDetailsRequest()
        {
            AccountDetailsResponse response = await Zencoder.AccountDetailsAsync();
            Assert.True(response.Success);

            AutoResetEvent[] handles = new AutoResetEvent[] { new AutoResetEvent(false) };

            Zencoder.AccountDetails(r =>
            {
                Assert.True(r.Success);
                handles[0].Set();
            });

            WaitHandle.WaitAll(handles);
        }

        /// <summary>
        /// Account details response tests.
        /// </summary>
        [Fact]
        public void AccountAccountDetailsResponseFromJson()
        {
            AccountDetailsResponse response = AccountDetailsResponse.FromJson(@"{""account_state"":""active"",""plan"":""Growth"",""minutes_used"":12549,""minutes_included"":25000,""billing_state"":""active"",""integration_mode"":true}");
            Assert.Equal("active", response.AccountState);
            Assert.Equal(true, response.IntegrationMode);
            Assert.Equal(12549, response.MinutesUsed);
        }

        /// <summary>
        /// Account integration mode request tests.
        /// </summary>
        [Fact]
        public async void AccountAccountIntegrationModeRequest()
        {
            AccountIntegrationModeResponse response = await Zencoder.AccountIntegrationModeAsync(true);
            Assert.True(response.Success);

            AutoResetEvent[] handles = new AutoResetEvent[] { new AutoResetEvent(false) };

            Zencoder.AccountIntegrationMode(
                true,
                r =>
                {
                    Assert.True(r.Success);
                    handles[0].Set();
                });

            WaitHandle.WaitAll(handles);
        }

        /// <summary>
        /// Create account request tests.
        /// </summary>
        [Fact]
        public async void AccountCreateAccountRequest()
        {
            CreateAccountResponse response = await Zencoder.CreateAccountAsync(Guid.NewGuid().ToString() + "@tastycodes.com", Guid.NewGuid().ToString(), null, true, false);
            Assert.True(response.Success);

            AutoResetEvent[] handles = new AutoResetEvent[] { new AutoResetEvent(false) };

            Zencoder.CreateAccountAsync(
                Guid.NewGuid().ToString() + "@tastycodes.com",
                Guid.NewGuid().ToString(),
                null,
                true,
                false,
                r =>
                {
                    Assert.True(r.Success);
                    handles[0].Set();
                });

            WaitHandle.WaitAll(handles);
        }

        /// <summary>
        /// Create account request to JSON tests.
        /// </summary>
        [Fact]
        public void AccountCreateAccountRequestToJson()
        {
            CreateAccountRequest request = new CreateAccountRequest(Zencoder.BaseUrl)
            {
                AffiliateCode = "asdf1234",
                Email = "test@tastycodes.com",
                Newsletter = true,
                Password = "1234",
                TermsOfService = true
            };

            Assert.Equal(
                @"{""affiliate_code"":""asdf1234"",""email"":""test@tastycodes.com"",""newsletter"":""1"",""password"":""1234"",""terms_of_service"":""1""}", 
                request.ToJson());
        }

        /// <summary>
        /// Create account response from JSON tests.
        /// </summary>
        [Fact]
        public void AccountCreateAccountResponseFromJson()
        {
            CreateAccountResponse response = CreateAccountResponse.FromJson(@"{""api_key"":""a123afdaf23fa231245fadcbbb"",""password"":""1234""}");
            Assert.Equal("a123afdaf23fa231245fadcbbb", response.ApiKey);
            Assert.Equal("1234", response.Password);
        }
    }
}
