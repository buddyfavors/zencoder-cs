//-----------------------------------------------------------------------
// <copyright file="TestBase.cs" company="Tasty Codes">
//     Copyright (c) 2010 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------

namespace Zencoder.Test
{
    /// <summary>
    /// Base class for tests.
    /// </summary>
    public abstract class TestBase
    {
        /// <summary>
        /// Gets the currently configured API key.
        /// </summary>
        public static readonly string ApiKey = ZencoderSettings.Section.ApiKey;

        /// <summary>
        /// Gets the default test <see cref="Zencoder"/> instance.
        /// </summary>
        public static readonly Zencoder Zencoder = new Zencoder();
    }
}
