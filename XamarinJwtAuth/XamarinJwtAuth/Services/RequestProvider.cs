using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using IdentityModel.Client;
using XamarinJwtAuth.Exceptions;

namespace XamarinJwtAuth.Services
{
    public class RequestProvider : IRequestProvider
    {
        HttpClient _client;
        //readonly JsonSerializerSettings _serializerSettings;

        public RequestProvider()
        {
            _client = CreateHttpClient(string.Empty);

            //_serializerSettings = new JsonSerializerSettings
            //{
            //    ContractResolver = new CamelCasePropertyNamesContractResolver(),
            //    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            //    NullValueHandling = NullValueHandling.Ignore
            //};
            //_serializerSettings.Converters.Add(new StringEnumConverter());
        }

        public async Task<string> GetAsync(string uri, string token = "")
        {
            _client.SetBearerToken(token);
            HttpResponseMessage response = await _client.GetAsync(uri);

            await HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();
            return serialized;
        }

        public async Task<TResult> PostAsync<TResult>(string uri, string data, string clientId, string clientSecret)
        {
            //https://www.davidbritch.com/2017/06/ Is this for IdentityServer only or can this be done
            //  with OpenIdDict?
            if (!string.IsNullOrWhiteSpace(clientId) && !string.IsNullOrWhiteSpace(clientSecret))
            {
                AddBasicAuthenticationHeader(clientId, clientSecret);
            }

            var content = new StringContent(data);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            HttpResponseMessage response = await _client.PostAsync(uri, content);

            await HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();

            //Removed this was NewtonSoft
            //TResult result = await Task.Run(() =>
            //    JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

            //We really don't need this because System.Text.Json has an Async version.
            TResult result = await Task.Run(() =>
                    JsonSerializer.Deserialize<TResult>(serialized));

            //ToDo: Implement DeserializeAsync
            //We can use the DeserializeAsync method here but we will need to read from a stream.



            return result;
        }

        //Shouldn't we use an HttpFactory, I think we can make Xamarin support it using
        //Hosbuilder https://montemagno.com/add-asp-net-cores-dependency-injection-into-xamarin-apps-with-hostbuilder/
        //but Maui will be here soon so making this a factory at this point seems like
        //more effort than necessary.
        HttpClient CreateHttpClient(string token = "")
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return _client;
        }

        void AddBasicAuthenticationHeader(string clientId, string clientSecret)
        {
            if (_client == null)
                return;

            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
                return;

            _client.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(clientId, clientSecret);
        }

        async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Forbidden ||
                    response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new ServiceAuthenticationException(content);
                }

                throw new HttpRequestExceptionEx(response.StatusCode, content);
            }
        }


    }
}
