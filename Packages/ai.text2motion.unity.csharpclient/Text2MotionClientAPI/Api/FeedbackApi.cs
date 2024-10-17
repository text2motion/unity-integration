/*
 * Text2Motion API
 *
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: 0.1.0
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mime;
using Text2MotionClientAPI.Client;
using Text2MotionClientAPI.Model;

namespace Text2MotionClientAPI.Api
{

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IFeedbackApiSync : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// Feedback
        /// </summary>
        /// <remarks>
        /// Provide feedback to a generate animation request. - -- Parameters:     request_body (GenerateRequestBody): The request body.
        /// </remarks>
        /// <exception cref="Text2MotionClientAPI.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="feedbackRequestBody"></param>
        /// <returns>Object</returns>
        Object FeedbackApiFeedbackPost(FeedbackRequestBody feedbackRequestBody);

        /// <summary>
        /// Feedback
        /// </summary>
        /// <remarks>
        /// Provide feedback to a generate animation request. - -- Parameters:     request_body (GenerateRequestBody): The request body.
        /// </remarks>
        /// <exception cref="Text2MotionClientAPI.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="feedbackRequestBody"></param>
        /// <returns>ApiResponse of Object</returns>
        ApiResponse<Object> FeedbackApiFeedbackPostWithHttpInfo(FeedbackRequestBody feedbackRequestBody);
        #endregion Synchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IFeedbackApiAsync : IApiAccessor
    {
        #region Asynchronous Operations
        /// <summary>
        /// Feedback
        /// </summary>
        /// <remarks>
        /// Provide feedback to a generate animation request. - -- Parameters:     request_body (GenerateRequestBody): The request body.
        /// </remarks>
        /// <exception cref="Text2MotionClientAPI.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="feedbackRequestBody"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        System.Threading.Tasks.Task<Object> FeedbackApiFeedbackPostAsync(FeedbackRequestBody feedbackRequestBody, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Feedback
        /// </summary>
        /// <remarks>
        /// Provide feedback to a generate animation request. - -- Parameters:     request_body (GenerateRequestBody): The request body.
        /// </remarks>
        /// <exception cref="Text2MotionClientAPI.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="feedbackRequestBody"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> FeedbackApiFeedbackPostWithHttpInfoAsync(FeedbackRequestBody feedbackRequestBody, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IFeedbackApi : IFeedbackApiSync, IFeedbackApiAsync
    {

    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public partial class FeedbackApi : IDisposable, IFeedbackApi
    {
        private Text2MotionClientAPI.Client.ExceptionFactory _exceptionFactory = (name, response) => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackApi"/> class.
        /// **IMPORTANT** This will also create an instance of HttpClient, which is less than ideal.
        /// It's better to reuse the <see href="https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net">HttpClient and HttpClientHandler</see>.
        /// </summary>
        /// <returns></returns>
        public FeedbackApi() : this((string)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackApi"/> class.
        /// **IMPORTANT** This will also create an instance of HttpClient, which is less than ideal.
        /// It's better to reuse the <see href="https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net">HttpClient and HttpClientHandler</see>.
        /// </summary>
        /// <param name="basePath">The target service's base path in URL format.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public FeedbackApi(string basePath)
        {
            this.Configuration = Text2MotionClientAPI.Client.Configuration.MergeConfigurations(
                Text2MotionClientAPI.Client.GlobalConfiguration.Instance,
                new Text2MotionClientAPI.Client.Configuration { BasePath = basePath }
            );
            this.ApiClient = new Text2MotionClientAPI.Client.ApiClient(this.Configuration.BasePath);
            this.Client =  this.ApiClient;
            this.AsynchronousClient = this.ApiClient;
            this.ExceptionFactory = Text2MotionClientAPI.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackApi"/> class using Configuration object.
        /// **IMPORTANT** This will also create an instance of HttpClient, which is less than ideal.
        /// It's better to reuse the <see href="https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net">HttpClient and HttpClientHandler</see>.
        /// </summary>
        /// <param name="configuration">An instance of Configuration.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public FeedbackApi(Text2MotionClientAPI.Client.Configuration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            this.Configuration = Text2MotionClientAPI.Client.Configuration.MergeConfigurations(
                Text2MotionClientAPI.Client.GlobalConfiguration.Instance,
                configuration
            );
            this.ApiClient = new Text2MotionClientAPI.Client.ApiClient(this.Configuration.BasePath);
            this.Client = this.ApiClient;
            this.AsynchronousClient = this.ApiClient;
            ExceptionFactory = Text2MotionClientAPI.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackApi"/> class
        /// using a Configuration object and client instance.
        /// </summary>
        /// <param name="client">The client interface for synchronous API access.</param>
        /// <param name="asyncClient">The client interface for asynchronous API access.</param>
        /// <param name="configuration">The configuration object.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FeedbackApi(Text2MotionClientAPI.Client.ISynchronousClient client, Text2MotionClientAPI.Client.IAsynchronousClient asyncClient, Text2MotionClientAPI.Client.IReadableConfiguration configuration)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (asyncClient == null) throw new ArgumentNullException("asyncClient");
            if (configuration == null) throw new ArgumentNullException("configuration");

            this.Client = client;
            this.AsynchronousClient = asyncClient;
            this.Configuration = configuration;
            this.ExceptionFactory = Text2MotionClientAPI.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Disposes resources if they were created by us
        /// </summary>
        public void Dispose()
        {
            this.ApiClient?.Dispose();
        }

        /// <summary>
        /// Holds the ApiClient if created
        /// </summary>
        public Text2MotionClientAPI.Client.ApiClient ApiClient { get; set; } = null;

        /// <summary>
        /// The client for accessing this underlying API asynchronously.
        /// </summary>
        public Text2MotionClientAPI.Client.IAsynchronousClient AsynchronousClient { get; set; }

        /// <summary>
        /// The client for accessing this underlying API synchronously.
        /// </summary>
        public Text2MotionClientAPI.Client.ISynchronousClient Client { get; set; }

        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        public string GetBasePath()
        {
            return this.Configuration.BasePath;
        }

        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        /// <value>An instance of the Configuration</value>
        public Text2MotionClientAPI.Client.IReadableConfiguration Configuration { get; set; }

        /// <summary>
        /// Provides a factory method hook for the creation of exceptions.
        /// </summary>
        public Text2MotionClientAPI.Client.ExceptionFactory ExceptionFactory
        {
            get
            {
                if (_exceptionFactory != null && _exceptionFactory.GetInvocationList().Length > 1)
                {
                    throw new InvalidOperationException("Multicast delegate for ExceptionFactory is unsupported.");
                }
                return _exceptionFactory;
            }
            set { _exceptionFactory = value; }
        }

        /// <summary>
        /// Feedback Provide feedback to a generate animation request. - -- Parameters:     request_body (GenerateRequestBody): The request body.
        /// </summary>
        /// <exception cref="Text2MotionClientAPI.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="feedbackRequestBody"></param>
        /// <returns>Object</returns>
        public Object FeedbackApiFeedbackPost(FeedbackRequestBody feedbackRequestBody)
        {
            Text2MotionClientAPI.Client.ApiResponse<Object> localVarResponse = FeedbackApiFeedbackPostWithHttpInfo(feedbackRequestBody);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Feedback Provide feedback to a generate animation request. - -- Parameters:     request_body (GenerateRequestBody): The request body.
        /// </summary>
        /// <exception cref="Text2MotionClientAPI.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="feedbackRequestBody"></param>
        /// <returns>ApiResponse of Object</returns>
        public Text2MotionClientAPI.Client.ApiResponse<Object> FeedbackApiFeedbackPostWithHttpInfo(FeedbackRequestBody feedbackRequestBody)
        {
            // verify the required parameter 'feedbackRequestBody' is set
            if (feedbackRequestBody == null)
                throw new Text2MotionClientAPI.Client.ApiException(400, "Missing required parameter 'feedbackRequestBody' when calling FeedbackApi->FeedbackApiFeedbackPost");

            Text2MotionClientAPI.Client.RequestOptions localVarRequestOptions = new Text2MotionClientAPI.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = Text2MotionClientAPI.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Text2MotionClientAPI.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = feedbackRequestBody;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("x-apikey")))
            {
                localVarRequestOptions.HeaderParameters.Add("x-apikey", this.Configuration.GetApiKeyWithPrefix("x-apikey"));
            }

            // make the HTTP request
            var localVarResponse = this.Client.Post<Object>("/api/feedback", localVarRequestOptions, this.Configuration);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("FeedbackApiFeedbackPost", localVarResponse);
                if (_exception != null) throw _exception;
            }

            return localVarResponse;
        }

        /// <summary>
        /// Feedback Provide feedback to a generate animation request. - -- Parameters:     request_body (GenerateRequestBody): The request body.
        /// </summary>
        /// <exception cref="Text2MotionClientAPI.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="feedbackRequestBody"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of Object</returns>
        public async System.Threading.Tasks.Task<Object> FeedbackApiFeedbackPostAsync(FeedbackRequestBody feedbackRequestBody, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var task = FeedbackApiFeedbackPostWithHttpInfoAsync(feedbackRequestBody, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
            Text2MotionClientAPI.Client.ApiResponse<Object> localVarResponse = await task.ConfigureAwait(false);
#else
            Text2MotionClientAPI.Client.ApiResponse<Object> localVarResponse = await task;
#endif
            return localVarResponse.Data;
        }

        /// <summary>
        /// Feedback Provide feedback to a generate animation request. - -- Parameters:     request_body (GenerateRequestBody): The request body.
        /// </summary>
        /// <exception cref="Text2MotionClientAPI.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="feedbackRequestBody"></param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (Object)</returns>
        public async System.Threading.Tasks.Task<Text2MotionClientAPI.Client.ApiResponse<Object>> FeedbackApiFeedbackPostWithHttpInfoAsync(FeedbackRequestBody feedbackRequestBody, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'feedbackRequestBody' is set
            if (feedbackRequestBody == null)
                throw new Text2MotionClientAPI.Client.ApiException(400, "Missing required parameter 'feedbackRequestBody' when calling FeedbackApi->FeedbackApiFeedbackPost");


            Text2MotionClientAPI.Client.RequestOptions localVarRequestOptions = new Text2MotionClientAPI.Client.RequestOptions();

            string[] _contentTypes = new string[] {
                "application/json"
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };


            var localVarContentType = Text2MotionClientAPI.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = Text2MotionClientAPI.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = feedbackRequestBody;

            // authentication (APIKeyHeader) required
            if (!string.IsNullOrEmpty(this.Configuration.GetApiKeyWithPrefix("x-apikey")))
            {
                localVarRequestOptions.HeaderParameters.Add("x-apikey", this.Configuration.GetApiKeyWithPrefix("x-apikey"));
            }

            // make the HTTP request

            var task = this.AsynchronousClient.PostAsync<Object>("/api/feedback", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
            var localVarResponse = await task.ConfigureAwait(false);
#else
            var localVarResponse = await task;
#endif

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("FeedbackApiFeedbackPost", localVarResponse);
                if (_exception != null) throw _exception;
            }

            return localVarResponse;
        }

    }
}
