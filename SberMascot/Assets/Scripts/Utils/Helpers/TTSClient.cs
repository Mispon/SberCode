//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
//
// Microsoft Cognitive Services (formerly Project Oxford): https://www.microsoft.com/cognitive-services
//
// Microsoft Cognitive Services (formerly Project Oxford) GitHub:
// https://github.com/Microsoft/Cognitive-Speech-TTS
//
// Copyright (c) Microsoft Corporation
// All rights reserved.
//
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

// IMPORTANT: THIS CODE ONLY WORKS WITH THE .NET 4.6 SCRIPTING RUNTIME

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

// Note that Unity 2017.x doesn't recognize the namespace System.Net.Http by default.
// This is why we added mcs.rsp with "-r:System.Net.Http.dll" in it in the Assets folder.

namespace Utils.Helpers
{
    /// <summary>
    /// This class demonstrates how to get a valid O-auth token
    /// </summary>
    public class Authentication
    {
        private string _accessUri;
        private string _apiKey;
        private string _accessToken;
        private Timer _accessTokenRenewer;

        private HttpClient client;

        //Access token expires every 10 minutes. Renew it every 9 minutes only.
        private const int RefreshTokenDuration = 9;

        public Authentication()
        {
            client = new HttpClient();
        }

        /// <summary>
        /// The Authenticate method needs to be called separately since it runs asynchronously
        /// and cannot be in the constructor, nor should it block the main Unity thread.
        /// </summary>
        /// <param name="issueTokenUri"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public async Task<string> Authenticate(string issueTokenUri, string apiKey)
        { 
            _accessUri = issueTokenUri;
            _apiKey = apiKey;

            _accessToken = await HttpClientPost(issueTokenUri, this._apiKey);

            // Renew the token every specfied minutes
            _accessTokenRenewer = new Timer(new TimerCallback(OnTokenExpiredCallback),
                                           this,
                                           TimeSpan.FromMinutes(RefreshTokenDuration),
                                           TimeSpan.FromMilliseconds(-1));

            return _accessToken;
        }

        public string GetAccessToken()
        {
            return _accessToken;
        }

        private async void RenewAccessToken()
        {
            string newAccessToken = await HttpClientPost(_accessUri, this._apiKey);
            //swap the new token with old one
            //Note: the swap is thread unsafe
            _accessToken = newAccessToken;
            Debug.Log(string.Format("Renewed token for user: {0} is: {1}",
                              _apiKey,
                              _accessToken));
        }

        private void OnTokenExpiredCallback(object stateInfo)
        {
            try
            {
                RenewAccessToken();
            }
            catch (Exception ex)
            {
                Debug.Log(string.Format("Failed renewing access token. Details: {0}", ex.Message));
            }
            finally
            {
                try
                {
                    _accessTokenRenewer.Change(TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
                }
                catch (Exception ex)
                {
                    Debug.Log(string.Format("Failed to reschedule the timer to renew access token. Details: {0}", ex.Message));
                }
            }
        }

        /// <summary>
        /// Asynchronously calls the authentication service via HTTP POST to obtain 
        /// </summary>
        /// <param name="accessUri"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        private async Task<string> HttpClientPost(string accessUri, string apiKey)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, accessUri);
            request.Headers.Add("Ocp-Apim-Subscription-Key", apiKey);
            request.Content = new StringContent("");

            HttpResponseMessage httpMsg = await client.SendAsync(request);
            Debug.Log($"Authentication Response status code: [{httpMsg.StatusCode}]");

            return await httpMsg.Content.ReadAsStringAsync();
        }
    }

    /// <summary>
    /// Generic event args
    /// </summary>
    /// <typeparam name="T">Any type T</typeparam>
    public class GenericEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericEventArgs{T}" /> class.
        /// </summary>
        /// <param name="eventData">The event data.</param>
        public GenericEventArgs(T eventData)
        {
            EventData = eventData;
        }

        /// <summary>
        /// Gets the event data.
        /// </summary>
        public T EventData { get; private set; }
    }
}