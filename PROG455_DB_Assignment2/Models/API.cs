using Newtonsoft.Json;

namespace PROG455_DB_Assignment2.Models
{
    /// <summary>
    /// Represents an API for interacting with a web service.
    /// </summary>
    public class API
    {
        private string? url;
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Sets the base URL for the API.
        /// </summary>
        public void SetUrl(string url) => this.url = url;

        /// <summary>
        /// Gets the result of the last GET request.
        /// </summary>
        public string? GETResult { get; internal set; }

        /// <summary>
        /// Gets the result of the last POST request
        /// </summary>
        public string? POSTResult { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="API"/> class.
        /// </summary>
        public API()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="API"/> class with a specified URL.
        /// </summary>
        public API(string url)
        {
            this.url = url;
        }

        /// <summary>
        /// Asynchronously makes a POST request to the API.
        /// </summary>
        public async Task AsyncPOST(IDictionary<string, string> values)
        {
            var request = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(url, request);
            var asString = await response.Content.ReadAsStringAsync();

            if(asString != null)
            {
                POSTResult = asString;
            }
        }

        /// <summary>
        /// Asynchronously makes a GET request to the API.
        /// </summary>
        public async Task AsyncGET(string method)
        {
            var request = url + method;
            var response = await client.GetAsync(request);
            GETResult = await response.Content.ReadAsStringAsync();
        }

        public string NSJsonSerialize(object obj) => JsonConvert.SerializeObject(obj , Formatting.Indented);

        public T? NSJsonDeserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json);
    }
}
