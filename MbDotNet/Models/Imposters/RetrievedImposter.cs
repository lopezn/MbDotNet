﻿using Newtonsoft.Json;

namespace MbDotNet.Models.Imposters
{
    public class RetrievedImposter
    {

        /// <summary>
        /// The port the imposter is set up to accept requests on.
        /// </summary>
        [JsonProperty("port")]
        public int Port { get; private set; }

        /// <summary>
        /// The protocol the imposter is set up to accept requests through.
        /// </summary>
        [JsonProperty("protocol")]
        public string Protocol { get; private set; }

        /// <summary>
        /// Optional name for the imposter, used in the logs.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("numberOfRequests")]
        public int NumberOfRequests { get; private set; }

        [JsonProperty("requests")]
        public Request[] Requests { get; private set; }

    }
}