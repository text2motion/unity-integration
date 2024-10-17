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
    /// Request schema for Generate API.
    /// </summary>
    [DataContract(Name = "GenerateRequestBody")]
    public partial class GenerateRequestBody
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateRequestBody" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected GenerateRequestBody() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateRequestBody" /> class.
        /// </summary>
        /// <param name="prompt">The text prompt to generate animation. (required).</param>
        /// <param name="targetSkeleton">The target skeleton for the animation. (required).</param>
        public GenerateRequestBody(string prompt = default(string), Skeleton targetSkeleton = default(Skeleton))
        {
            // to ensure "prompt" is required (not null)
            if (prompt == null)
            {
                throw new ArgumentNullException("prompt is a required property for GenerateRequestBody and cannot be null");
            }
            this.Prompt = prompt;
            // to ensure "targetSkeleton" is required (not null)
            if (targetSkeleton == null)
            {
                throw new ArgumentNullException("targetSkeleton is a required property for GenerateRequestBody and cannot be null");
            }
            this.TargetSkeleton = targetSkeleton;
        }

        /// <summary>
        /// The text prompt to generate animation.
        /// </summary>
        /// <value>The text prompt to generate animation.</value>
        /// <example>Run forward</example>
        [DataMember(Name = "prompt", IsRequired = true, EmitDefaultValue = true)]
        public string Prompt { get; set; }

        /// <summary>
        /// The target skeleton for the animation.
        /// </summary>
        /// <value>The target skeleton for the animation.</value>
        [DataMember(Name = "target_skeleton", IsRequired = true, EmitDefaultValue = true)]
        public Skeleton TargetSkeleton { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class GenerateRequestBody {\n");
            sb.Append("  Prompt: ").Append(Prompt).Append("\n");
            sb.Append("  TargetSkeleton: ").Append(TargetSkeleton).Append("\n");
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
