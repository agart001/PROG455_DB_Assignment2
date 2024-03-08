using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PROG455_DB_Assignment2.Models
{
    /// <summary>
    /// Represents an API for interacting with a web service.
    /// </summary>
    public class API
    {
        #region Client/URL

        /// <summary>
        /// API base url
        /// </summary>
        private string? url;

        /// <summary>
        /// Sets the base URL for the API.
        /// </summary>
        public void SetUrl(string url) => this.url = url;

        /// <summary>
        /// API http-client
        /// </summary>
        private static readonly HttpClient client = new HttpClient();

        #endregion

        #region Constructors

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

        #endregion

        #region POST

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
        /// Gets the result of the last POST request
        /// </summary>
        public string? POSTResult { get; internal set; }

        #endregion

        #region GET

        /// <summary>
        /// Asynchronously makes a GET request to the API.
        /// </summary>
        public async Task AsyncGET(KeyValuePair<string, string> parameters)
        {
            var method = $"{parameters.Key}={parameters.Value}";
            var request = url + method;
            var response = await client.GetAsync(request);
            GETResult = await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Gets the result of the last GET request.
        /// </summary>
        public string? GETResult { get; internal set; }

        #endregion

        #region JSON

        public string NSJsonSerialize(object obj) => JsonConvert.SerializeObject(obj , Formatting.Indented);

        public T? NSJsonDeserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json);

        #endregion
    }


    public struct APIQuery
    {
        public string Table { get; internal set; }
        public string Query { get; internal set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
