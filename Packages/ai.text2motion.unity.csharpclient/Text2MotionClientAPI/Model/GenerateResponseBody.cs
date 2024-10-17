/*
 * Text2Motion API
 *
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: 0.1.0
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using OpenAPIDateConverter = Text2MotionClientAPI.Client.OpenAPIDateConverter;

namespace Text2MotionClientAPI.Model
{
    /// <summary>
    /// Response schema for Generate API.
    /// </summary>
    [DataContract(Name = "GenerateResponseBody")]
    public partial class GenerateResponseBody
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateResponseBody" /> class.
        /// </summary>
        /// <param name="result">result.</param>
        /// <param name="requestId">requestId.</param>
        public GenerateResponseBody(string result = default(string), string requestId = default(string))
        {
            this.Result = result;
            this.RequestId = requestId;
        }

        /// <summary>
        /// Gets or Sets Result
        /// </summary>
        [DataMember(Name = "result", EmitDefaultValue = true)]
        public string Result { get; set; }

        /// <summary>
        /// Gets or Sets RequestId
        /// </summary>
        [DataMember(Name = "request_id", EmitDefaultValue = true)]
        public string RequestId { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class GenerateResponseBody {\n");
            sb.Append("  Result: ").Append(Result).Append("\n");
            sb.Append("  RequestId: ").Append(RequestId).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

    }

}
