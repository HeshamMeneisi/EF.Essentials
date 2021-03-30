// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

using System.Collections.Generic;
using Newtonsoft.Json;

namespace GenericCompany.Common.KeyStore.Models
{
    public partial class ProblemDetails
    {
        /// <summary>
        /// Initializes a new instance of the ProblemDetails class.
        /// </summary>
        public ProblemDetails()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ProblemDetails class.
        /// </summary>
        public ProblemDetails(string type = default(string), string titleProperty = default(string), int? status = default(int?), string detail = default(string), string instance = default(string), IDictionary<string, object> extensions = default(IDictionary<string, object>))
        {
            Type = type;
            TitleProperty = titleProperty;
            Status = status;
            Detail = detail;
            Instance = instance;
            Extensions = extensions;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string TitleProperty { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public int? Status { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "detail")]
        public string Detail { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "instance")]
        public string Instance { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "extensions")]
        public IDictionary<string, object> Extensions { get; private set; }

    }
}
