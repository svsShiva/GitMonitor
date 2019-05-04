using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GitMonitor.UWP.Utilities
{
    class APIUtility
    {
        //token for authentication.
        private string _token;

        public APIUtility(string token = null)
        {
            _token = token;
        }

        private HttpClient CreateHttpClient(string route)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(route);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //TODO implement security
            //client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", Encryption.Encrypt(_token));

            return client;
        }

        internal async Task<T> Get<T>(string route)
        {
            try
            {
                HttpClient client = CreateHttpClient(route);

                using (client)
                {
                    HttpResponseMessage response = await client.GetAsync(route);

                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                    }

                    // Returning error message
                    var message = JsonConvert.DeserializeObject<Dictionary<string, string>>(await response.Content.ReadAsStringAsync());
                    throw new Exception(message["Message"]);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal async Task<U> Post<T, U>(T data, string route)
        {
            try
            {
                StringContent content = null;

                if (data != null)
                {
                    string jsonData = JsonConvert.SerializeObject(data);
                    content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                }

                HttpClient client = CreateHttpClient(route);

                // using string builder since _URL is static
                string postingURL = route;

                using (client)
                {
                    HttpResponseMessage response = await client.PostAsync(postingURL, data != null ? content : null);

                    if (response.IsSuccessStatusCode)
                    {
                        var res = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<U>(res);
                    }

                    // Returning error message
                    var message = JsonConvert.DeserializeObject<Dictionary<string, string>>(await response.Content.ReadAsStringAsync());
                    throw new Exception(message["Message"]);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal async void Delete(string route)
        {
            try
            {
                HttpClient client = CreateHttpClient(route);

                using (client)
                {
                    HttpResponseMessage response = await client.DeleteAsync(route);

                    if (response.IsSuccessStatusCode)
                    {
                        return;
                    }

                    // Returning error message
                    var message = JsonConvert.DeserializeObject<Dictionary<string, string>>(await response.Content.ReadAsStringAsync());
                    throw new Exception(message["Message"]);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
