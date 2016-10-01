//-----------------------------------------------------------------------
// <copyright file="NotificationTests.cs" company="Tasty Codes">
//     Copyright (c) 2010 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------

namespace Zencoder.Test
{
    using System.IO;
    using System.Text;
    using System.Threading;
    using Newtonsoft.Json;
    using Xunit;
    using Moq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Notification tests.
    /// </summary>
    public class NotificationTests
    {
        private const string NotificationJson = @"{""job"":{""state"":""processing"",""id"":1234},""output"":{""label"":""web"",""url"":""http://example.com/file.mp4"",""state"":""processing"",""id"":12345}}";
        private static AutoResetEvent receiverHandle = new AutoResetEvent(false);

        /// <summary>
        /// Initializes the class for testing.
        /// </summary>
        static NotificationTests()
        {
            NotificationHandler.Receivers.Add(new TestNotificationReceiver());
        }

        /// <summary>
        /// Notification handler process request tests.
        /// </summary>
        [Fact]
        public void NotificationHandlerProcessRequest()
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(NotificationJson)))
            {
                var mockContext = new Mock<HttpContext>()
                {
                    DefaultValue = DefaultValue.Mock
                };

                var mockRequest = new Mock<HttpRequest>()
                {
                    DefaultValue = DefaultValue.Mock
                };

                mockRequest.Setup(r => r.ContentType).Returns("application/json");
                mockRequest.Setup(r => r.Method).Returns("POST");
                mockRequest.Setup(r => r.Body).Returns(stream);

                mockContext.Setup(c => c.Request).Returns(mockRequest.Object);

                NotificationHandler.ProcessRequest(mockContext.Object);

                WaitHandle.WaitAll(new WaitHandle[] { receiverHandle });
            }
        }

        /// <summary>
        /// HTTP POST notification from JSON tests.
        /// </summary>
        [Fact]
        public void NotificationHttpPostNotificationFromJson()
        {
            HttpPostNotification notification = JsonConvert.DeserializeObject<HttpPostNotification>(NotificationJson);
            Assert.Equal(JobState.Processing, notification.Job.State);
            Assert.Equal("http://example.com/file.mp4", notification.Output.Url);
        }

        #region TestNotificationReceiver Class

        /// <summary>
        /// Test <see cref="INotificationReceiver"/> implementation.
        /// </summary>
        private class TestNotificationReceiver : INotificationReceiver
        {
            /// <summary>
            /// Called when a notification is received.
            /// </summary>
            /// <param name="notification">The notification that was received.</param>
            public void OnReceive(HttpPostNotification notification)
            {
                Assert.NotNull(notification);
                Assert.Equal(1234, notification.Job.Id);
                Assert.Equal("web", notification.Output.Label);
                receiverHandle.Set();
            }
        }

        #endregion
    }
}
