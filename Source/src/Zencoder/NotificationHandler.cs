//-----------------------------------------------------------------------
// <copyright file="NotificationHandler.cs" company="Tasty Codes">
//     Copyright (c) 2010 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------

namespace Zencoder
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    /// <summary>
    /// Implements <see cref="IHttpHandler"/> for receiving Zencoder notifications.
    /// </summary>
    public class NotificationHandler// : IHttpHandler, IRequiresSessionState
    {
        private static readonly object locker = new object();
        private static IList<INotificationReceiver> receivers;

        /// <summary>
        /// Gets a list of current notification receivers.
        /// This list is intialized on first access from the configuration.
        /// </summary>
        public static IList<INotificationReceiver> Receivers
        {
            get
            {
                lock (locker)
                {
                    if (receivers == null)
                    {
                        receivers = new List<INotificationReceiver>();

                        foreach (var notification in ZencoderSettings.Section.Notifications)
                        {
                            receivers.Add(CreateReceiver(notification));
                        }
                    }

                    return receivers;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IHttpHandler"/> is reusable.
        /// </summary>
        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// Creates a <see cref="INotificationReceiver"/> from the given type name.
        /// </summary>
        /// <param name="typeName">The name of the type to create the receiver from.</param>
        /// <returns>The created receiver.</returns>
        public static INotificationReceiver CreateReceiver(string typeName)
        {
            return (INotificationReceiver)Activator.CreateInstance(Type.GetType(typeName));
        }

        /// <summary>
        /// Processes the request for the given context.
        /// </summary>
        /// <param name="context">The request context to process.</param>
        public static void ProcessRequest(HttpContext context)
        {
            if ("POST".Equals(context.Request.Method, StringComparison.OrdinalIgnoreCase) &&
                "application/json".Equals(context.Request.ContentType, StringComparison.OrdinalIgnoreCase) &&
                context.Request.Body != null &&
                context.Request.Body.Length > 0)
            {
                HttpPostNotification notification;

                using (StreamReader sr = new StreamReader(context.Request.Body))
                {
                    using (JsonReader jr = new JsonTextReader(sr))
                    {
                        notification = new JsonSerializer().Deserialize<HttpPostNotification>(jr);
                    }
                }

                // We don't want changes to this collection while we're notifying.
                lock (Receivers)
                {
                    foreach (INotificationReceiver receiver in Receivers)
                    {
                        // Send the notifications out of band.
                        receiver.OnReceive(notification);
                    }
                }
            }
        }
    }
}
