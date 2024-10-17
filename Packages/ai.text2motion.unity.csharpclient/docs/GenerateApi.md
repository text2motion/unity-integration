# Text2MotionClientAPI.Api.GenerateApi

All URIs are relative to *https://api.text2motion.ai*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GenerateApiGeneratePost**](GenerateApi.md#generateapigeneratepost) | **POST** /api/generate | Generate |

<a id="generateapigeneratepost"></a>
# **GenerateApiGeneratePost**
> GenerateResponseBody GenerateApiGeneratePost (GenerateRequestBody generateRequestBody)

Generate

Request text to animation generation. - -- Parameters:     request_body (GenerateRequestBody): The request body.  Returns:     GenerateResponseBody: The generated animation response.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Text2MotionClientAPI.Api;
using Text2MotionClientAPI.Client;
using Text2MotionClientAPI.Model;

namespace Example
{
    public class GenerateApiGeneratePostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.text2motion.ai";
            // Configure API key authorization: APIKeyHeader
            config.AddApiKey("x-apikey", "YOUR_API_KEY");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("x-apikey", "Bearer");

            var apiInstance = new GenerateApi(config);
            var generateRequestBody = new GenerateRequestBody(); // GenerateRequestBody | 

            try
            {
                // Generate
                GenerateResponseBody result = apiInstance.GenerateApiGeneratePost(generateRequestBody);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling GenerateApi.GenerateApiGeneratePost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GenerateApiGeneratePostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Generate
    ApiResponse<GenerateResponseBody> response = apiInstance.GenerateApiGeneratePostWithHttpInfo(generateRequestBody);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling GenerateApi.GenerateApiGeneratePostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **generateRequestBody** | [**GenerateRequestBody**](GenerateRequestBody.md) |  |  |

### Return type

[**GenerateResponseBody**](GenerateResponseBody.md)

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

