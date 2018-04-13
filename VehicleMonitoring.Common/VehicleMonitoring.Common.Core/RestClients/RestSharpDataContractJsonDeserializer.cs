using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace VehicleMonitoring.Common.Core.RestClients
{
    public class RestSharpDataContractJsonDeserializer : IDeserializer
    {

        /// Unused for JSON Serialization
        public string DateFormat { get; set; }

        public T Deserialize<T>(IRestResponse response)
        {
            using (var ms = new MemoryStream(response.RawBytes))
            {
                var ser = new DataContractJsonSerializer(typeof(T));
                return (T)ser.ReadObject(ms);
            }
        }

        /// Unused for JSON Serialization
        public string RootElement { get; set; }

        /// Unused for JSON Serialization
        public string Namespace { get; set; }

    }
}
