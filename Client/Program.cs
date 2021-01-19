using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using RabbitMQ.Client;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:44330");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
            }
            
            // request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "platformApiClient",
                ClientSecret = "SuperSecretPassword",
                Scope = "quasar.read:*/*"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            var factory = new ConnectionFactory {Password = tokenResponse.AccessToken};
// "guest"/"guest" by default, limited to localhost connections

            var conn = factory.CreateConnection();
            
            
            Console.WriteLine(tokenResponse.Json);
            Console.Read();
        }
    }
}