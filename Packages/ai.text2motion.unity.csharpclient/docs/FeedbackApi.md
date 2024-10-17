# Text2MotionClientAPI.Api.FeedbackApi

All URIs are relative to *https://api.text2motion.ai*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**FeedbackApiFeedbackPost**](FeedbackApi.md#feedbackapifeedbackpost) | **POST** /api/feedback | Feedback |

<a id="feedbackapifeedbackpost"></a>
# **FeedbackApiFeedbackPost**
> Object FeedbackApiFeedbackPost (FeedbackRequestBody feedbackRequestBody)

Feedback

Provide feedback to a generate animation request. - -- Parameters:     request_body (GenerateRequestBody): The request body.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Text2MotionClientAPI.Api;
using Text2MotionClientAPI.Client;
using Text2MotionClientAPI.Model;

namespace Example
{
    public class FeedbackApiFeedbackPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.text2motion.ai";
            // Configure API key authorization: APIKeyHeader
            config.AddApiKey("x-apikey", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("x-apikey", "Bearer");

            var apiInstance = new FeedbackApi(config);
            var feedbackRequestBody = new FeedbackRequestBody(); // FeedbackRequestBody | 

            try
            {
                // Feedback
                Object result = apiInstance.FeedbackApiFeedbackPost(feedbackRequestBody);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling FeedbackApi.FeedbackApiFeedbackPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the FeedbackApiFeedbackPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Feedback
    ApiResponse<Object> response = apiInstance.FeedbackApiFeedbackPostWithHttpInfo(feedbackRequestBody);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling FeedbackApi.FeedbackApiFeedbackPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **feedbackRequestBody** | [**FeedbackRequestBody**](FeedbackRequestBody.md) |  |  |

### Return type

**Object**

### Authorization

[APIKeyHeader](../README.md#APIKeyHeader)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Successful Response |  -  |
| **404** | Not found |  -  |
| **422** | Validation Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

