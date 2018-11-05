/*
* Copyright (c) 2018 Algolia
* http://www.algolia.com/
* Based on the first version developed by Christopher Maneu under the same license:
*  https://github.com/cmaneu/algoliasearch-client-csharp
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using Algolia.Search.Http;
using Algolia.Search.Models.Analytics;
using Algolia.Search.Models.Enums;
using Algolia.Search.Transport;
using Algolia.Search.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Clients
{
    public class AnalyticsClient : IAnalyticsClient
    {
        private readonly IRequesterWrapper _requesterWrapper;

        /// <summary>
        /// Initialize a client with default settings
        /// </summary>
        public AnalyticsClient() : this(new AlgoliaConfig(), new AlgoliaHttpRequester())
        {
        }

        /// <summary>
        /// Create a new search client for the given appID
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="apiKey"></param>
        public AnalyticsClient(string applicationId, string apiKey) : this(new AlgoliaConfig { ApiKey = apiKey, AppId = applicationId }, new AlgoliaHttpRequester())
        {
        }

        /// <summary>
        /// Initialize a client with custom config
        /// </summary>
        /// <param name="config"></param>
        public AnalyticsClient(AlgoliaConfig config) : this(config, new AlgoliaHttpRequester())
        {
        }

        /// <summary>
        /// Initialize the client with custom config and custom Requester
        /// </summary>
        /// <param name="config"></param>
        /// <param name="httpRequester"></param>
        public AnalyticsClient(AlgoliaConfig config, IHttpRequester httpRequester)
        {
            if (httpRequester == null)
            {
                throw new ArgumentNullException(nameof(httpRequester), "An httpRequester is required");
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config), "A config is required");
            }

            if (string.IsNullOrWhiteSpace(config.AppId))
            {
                throw new ArgumentNullException(nameof(config.AppId), "Application ID is required");
            }

            if (string.IsNullOrWhiteSpace(config.ApiKey))
            {
                throw new ArgumentNullException(nameof(config.ApiKey), "An API key is required");
            }

            _requesterWrapper = new RequesterWrapper(config, httpRequester);
        }

        /// <summary>
        /// Get an A/B test information and results.
        /// </summary>
        /// <param name="abTestId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public ABTestResponse GetABTest(int abTestId, RequestOption requestOptions = null) =>
            AsyncHelper.RunSync(() => GetABTestAsync(abTestId, requestOptions));

        /// <summary>
        /// Get an A/B test information and results.
        /// </summary>
        /// <param name="abTestId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ABTestResponse> GetABTestAsync(int abTestId, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<ABTestResponse>(HttpMethod.Get,
                $"/2/abtests/{abTestId}", CallType.Read, requestOptions, ct).ConfigureAwait(false);
        }
        /// <summary>
        /// Fetch all existing AB Tests for App that are available for the current API Key. Returns an array of metadata and metrics.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public ABTestsReponse GetABTests(int offset = 0, int limit = 10, RequestOption requestOptions = null) =>
            AsyncHelper.RunSync(() => GetABTestsAsync(offset, limit, requestOptions));

        /// <summary>
        /// Fetch all existing AB Tests for App that are available for the current API Key. Returns an array of metadata and metrics.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ABTestsReponse> GetABTestsAsync(int offset = 0, int limit = 10, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<ABTestsReponse>(HttpMethod.Get,
                $"/2/abtests?offset=${offset}&limit=${limit}", CallType.Read, requestOptions, ct).ConfigureAwait(false);
        }
    }
}