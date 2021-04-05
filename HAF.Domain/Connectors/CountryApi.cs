
using HAF.Domain.Entities;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAF.Domain.Connectors
{
    public static class CountryApi
    {
        private static readonly RestClient _client = new RestClient(ConfigurationManager.AppSettings["CountryUrl"]);

        public static IEnumerable<Country> GetCountries()
        {
            var request = CreateRequest("/rest/v2");
            var result = _client.Execute<List<Country>>(request);
            if ((int)result.StatusCode != 200) return null;
            return result.Data;
        }

        public static IEnumerable<Country> GetCountriesByName(string country)
        {
            var request = CreateRequest($"/rest/v2/name/{country}? fullText=true");
            var result =  _client.Execute<List<Country>>(request);
            if ((int)result.StatusCode != 200) return null;
            return result.Data;
        }


        private static RestRequest CreateRequest(string resource)
        {
            var request = new RestRequest(resource);
            return request;
        }
    }
}
