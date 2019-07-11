// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------

namespace Provider.Controllers
{
    using System;
    using System.IO;
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Provider.Models;

    /// <summary>
    /// The resource controller.
    /// </summary>
    public class ProviderController : Controller
    {
        /// <summary>
        /// Method that gets called if subscribed for ResourceCreationValidate trigger.
        /// </summary>
        /// <param name="subscriptionId">The subscription Id.</param>
        /// <param name="resourceGroup">The resource group.</param>
        /// <param name="providerNamespace">The provider namespace.</param>
        /// <param name="resourceType">The resource type.</param>
        /// <param name="resourceTypeName">The resource type name.</param>
        [HttpPost]
        [ActionName("OnResourceCreationValidate")]
        public IActionResult OnResourceCreationValidate(string subscriptionId, string resourceGroup, string providerNamespace, string resourceType, string resourceTypeName)
        {
            /*** Implement this method if you want to opt for "ResourceCreationValidate" extension for your resource type. ***/

            // This is a sample implementation
            // Logging: this log will be stored to "DefaultTable" in your geneva namespace.
            var logMessage = @"{""Tablename"" : ""DefaultTable"", ""logMessage"": ""OnResourceCreationValidate method called at " + DateTime.Now + @"""}";
            Console.WriteLine(logMessage);

            /*** Uncomment below code to deserialize the request body ***/

            // StreamReader reader = new StreamReader(this.Request.Body);
            // var input = reader.ReadToEnd();
            // Resource resource = JsonConvert.DeserializeObject<Resource>(input);

            // Validate the request. Example validation.
            if (subscriptionId == null || resourceGroup == null || providerNamespace == null || resourceType == null || resourceTypeName == null)
            {
                ErrorResponse errorRespone = new ErrorResponse
                {
                    Error = new Error
                    {
                        Code = "SampleErrorCode",
                        Message = "SampleErrorMessage - Please don't create this resource. This is dangerous.",
                    },
                    Status = "Failed",
                };

                // Required response format in case of validation failure.
                return CreateResponse(
                    statusCode: HttpStatusCode.OK,
                    value: errorRespone);
            }

            // Required response format in case validation pass with empty body.
            return new OkObjectResult(string.Empty);
        }

        /// <summary>
        /// Method that gets called if subscribed for ResourceCreationBegin trigger.
        /// </summary>
        /// <param name="subscriptionId">The subscription Id.</param>
        /// <param name="resourceGroup">The resource group.</param>
        /// <param name="providerNamespace">The provider namespace.</param>
        /// <param name="resourceType">The resource type.</param>
        /// <param name="resourceTypeName">The resource type name.</param>
        [HttpPut]
        [ActionName("OnResourceCreationBegin")]
        public IActionResult OnResourceCreationBegin(string subscriptionId, string resourceGroup, string providerNamespace, string resourceType, string resourceTypeName)
        {
            /*** Implement this method if you want to opt for "ResourceCreationBegin" extension for your resource type. ***/

            // This is a sample implementation
            // Logging: this log will be stored to "DefaultTable" in your geneva namespace.
            var logMessage = @"{""Tablename"" : ""DefaultTable"", ""logMessage"": ""OnResourceCreationBegin called at " + DateTime.Now + @"""}";
            Console.WriteLine(logMessage);

            // Deserializing the request body
            StreamReader reader = new StreamReader(this.Request.Body);
            var input = reader.ReadToEnd();
            Resource resource = JsonConvert.DeserializeObject<Resource>(input);

            // Provision your resource here, if required
            // Create/update your private data (internal metadata). This will never be returned to end user.
            JObject internalMetadata = (JObject)resource.Properties["internalMetadata"];

            if (internalMetadata == null)
            {
                internalMetadata = new JObject();
                internalMetadata["description"] = "This is your private data.";
                internalMetadata["createdTime"] = DateTime.Now;
                internalMetadata["lastUpdatedTime"] = DateTime.Now;
            }
            else
            {
                internalMetadata["lastUpdatedTime"] = DateTime.Now;
            }

            resource.Properties["internalMetadata"] = internalMetadata;

            // Required response format.
            return CreateResponse(
                statusCode: HttpStatusCode.OK,
                value: resource);
        }

        /// <summary>
        /// Method that gets called if subscribed for ResourceCreationComplete trigger.
        /// </summary>
        /// <param name="subscriptionId">The subscription Id.</param>
        /// <param name="resourceGroup">The resource group.</param>
        /// <param name="providerNamespace">The provider namespace.</param>
        /// <param name="resourceType">The resource type.</param>
        /// <param name="resourceTypeName">The resource type name.</param>
        [HttpPost]
        [ActionName("OnResourceCreationCompleted")]
        public IActionResult OnResourceCreationCompleted(string subscriptionId, string resourceGroup, string providerNamespace, string resourceType, string resourceTypeName)
        {
            /*** Implement this method if you want to opt for "OnResourceCreationCompleted" extension for your resource type. ***/

            // This is a sample implementation
            // Logging: this log will be stored to "DefaultTable" in your geneva namespace.
            var logMessage = @"{""Tablename"" : ""DefaultTable"", ""logMessage"": ""OnResourceCreationCompleted called at " + DateTime.Now + @"""}";
            Console.WriteLine(logMessage);

            // Do post creation processing here ex: start billing

            // Required response format with empty body.
            return new OkObjectResult(string.Empty);
        }

        /// <summary>
        /// Method that gets called if subscribed for ResourceDeletionValidate trigger.
        /// </summary>
        /// <param name="subscriptionId">The subscription Id.</param>
        /// <param name="resourceGroup">The resource group.</param>
        /// <param name="providerNamespace">The provider namespace.</param>
        /// <param name="resourceType">The resource type.</param>
        /// <param name="resourceTypeName">The resource type name.</param>
        [HttpPost]
        [ActionName("OnResourceDeletionValidate")]
        public IActionResult OnResourceDeletionValidate(string subscriptionId, string resourceGroup, string providerNamespace, string resourceType, string resourceTypeName)
        {
            /*** Implement this method if you want to opt for "OnResourceDeletionValidate" extension for your resource type. ***/

            // This is a sample implementation
            // Logging: this log will be stored to "DefaultTable" in your geneva namespace.
            var logMessage = @"{""Tablename"" : ""DefaultTable"", ""logMessage"": ""OnResourceDeletionValidate called at " + DateTime.Now + @"""}";
            Console.WriteLine(logMessage);

            // Do pre deletion validation here
            // This is a sample implementation
            if (subscriptionId == null || resourceGroup == null || providerNamespace == null || resourceType == null || resourceTypeName == null)
            {
                ErrorResponse errorRespone = new ErrorResponse
                {
                    Error = new Error
                    {
                        Code = "SampleErrorCode",
                        Message = "SampleErrorMessage - Please don't delete this resource. This is important.",
                    },
                    Status = "Failed",
                };

                // Required response format in case of validation failure.
                return CreateResponse(
                    statusCode: HttpStatusCode.OK,
                    value: errorRespone);
            }

            // Required response format in case validation pass with empty body.
            return new OkObjectResult(string.Empty);
        }

        /// <summary>
        /// Creates http responses.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="value">The value.</param>
        private static JsonResult CreateResponse(HttpStatusCode statusCode, object value = null)
        {
            var response = new JsonResult(value)
            {
                StatusCode = (int)statusCode,
            };

            return response;
        }
    }
}