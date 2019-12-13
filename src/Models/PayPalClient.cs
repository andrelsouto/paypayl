using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using BraintreeHttp;
using PayPalCheckoutSdk.Core;

namespace PayPalApi.Models
{
    public class PayPalClient
    {
        
        public static PayPalEnvironment environment() {
            return new SandboxEnvironment(
                "AQWC9nOP9NfbRLcATVsCydBURJfl7dGAR43f59FAl1SxLdRRgpBXFzGFPnnUQV9Mgk9vNDngP5O7FHlw",
                "EMD11LGww6AKt-Qi-VHwS1INmIGb-fO_pxuo6gGQ6g0mTs2_vRlCUGud5T1kIgRNLVG6S8Q6lbx6hauj");
        }

        public static HttpClient client() {
            return new PayPalHttpClient(environment());
        }

        public static HttpClient client(string refreshToken) {

            return new PayPalHttpClient(environment(), refreshToken);
        }

        public static String ObjectToJSONString(Object serializableObject)
        {
            MemoryStream memoryStream = new MemoryStream();
            var writer = JsonReaderWriterFactory.CreateJsonWriter(
                memoryStream, Encoding.UTF8, true, true, " ");
            DataContractJsonSerializer ser = 
                new DataContractJsonSerializer(serializableObject.GetType(),
                    new DataContractJsonSerializerSettings{ UseSimpleDictionaryFormat = true });
            ser.WriteObject(writer, serializableObject);
            memoryStream.Position = 0;
            StreamReader sr = new StreamReader(memoryStream);
            return sr.ReadToEnd();
        }
    }
}