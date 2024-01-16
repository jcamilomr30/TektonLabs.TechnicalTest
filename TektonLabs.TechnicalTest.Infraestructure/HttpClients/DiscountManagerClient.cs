using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TektonLabs.TechnicalTest.Infraestructure.HttpClients
{
    public class DiscountManagerClient : ApiClient, IDiscountManagerClient
    {
        public DiscountManagerClient(HttpClient httpClient, IConfiguration configuration) : base(httpClient)
        {
            ServiceName = "Discount Manager Application";
        }

    }
}
