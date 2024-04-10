﻿//-----------------------------------------------------------------------
// <copyright file="HttpProxy.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: https://cslanet.com
// </copyright>
// <summary>Implements a data portal proxy to relay data portal</summary>
//-----------------------------------------------------------------------
using Csla.Configuration;
using Csla.DataPortalClient;
using Csla.Properties;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Csla.Channels.Http
{
  /// <summary>
  /// Implements a data portal proxy to relay data portal
  /// calls to a remote application server by using http.
  /// </summary>
  public class HttpProxy : DataPortalProxy
  {
    /// <summary>
    /// Creates an instance of the type, initializing
    /// it to use the supplied HttpClient object and options.
    /// </summary>
    /// <param name="applicationContext"></param>
    /// <param name="httpClient">HttpClient instance</param>
    /// <param name="options">Options for HttpProxy</param>
    /// <param name="dataPortalOptions">Data portal options</param>
    public HttpProxy(ApplicationContext applicationContext, HttpClient httpClient, HttpProxyOptions options, DataPortalOptions dataPortalOptions)
      : base(applicationContext)
    {
      _httpClient = httpClient;
      Options = options;
      DataPortalUrl = options.DataPortalUrl;
      VersionRoutingTag = dataPortalOptions.VersionRoutingTag;
    }

    /// <summary>
    /// Current options for the proxy.
    /// </summary>
    protected HttpProxyOptions Options { get; set; }
    private HttpClient _httpClient;
    private string VersionRoutingTag { get; set; }

    /// <summary>
    /// Gets an HttpClientHandler for use
    /// in initializing the HttpClient instance.
    /// </summary>
    protected virtual HttpClientHandler GetHttpClientHandler()
    {
      return new HttpClientHandler();
    }

    /// <summary>
    /// Gets an HttpClient object for use in
    /// asynchronous communication with the server.
    /// </summary>
    protected virtual HttpClient GetHttpClient()
    {
      if (_httpClient == null)
      {
        var handler = GetHttpClientHandler();
        if (Options.UseTextSerialization)
        {
          handler = new HttpClientHandler();
        }
        else
        {
          handler = new HttpClientHandler()
          {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
          };
        }
        _httpClient = new HttpClient(handler);
        if (Timeout > 0)
        {
          _httpClient.Timeout = TimeSpan.FromMilliseconds(this.Timeout);
        }
      }

      return _httpClient;
    }

    /// <summary>
    /// Gets an WebClient object for use in
    /// synchronous communication with the server.
    /// </summary>
    protected virtual WebClient GetWebClient()
    {
      return new DefaultWebClient(this.Timeout);
    }

    /// <summary>
    /// Select client to make request based on isSync parameter and return response from server
    /// </summary>
    /// <param name="serialized">Serialized request</param>
    /// <param name="operation">DataPortal operation</param>
    /// <param name="routingToken">Routing Tag for server</param>
    /// <param name="isSync">True if the client-side proxy should synchronously invoke the server.</param>
    /// <returns>Serialized response from server</returns>
    protected override async Task<byte[]> CallDataPortalServer(byte[] serialized, string operation, string routingToken, bool isSync)
    {
      return isSync
        ? CallViaWebClient(serialized, operation, routingToken)
        : await CallViaHttpClient(serialized, operation, routingToken);
    }

    /// <summary>
    /// Override to set headers or other properties of the
    /// HttpRequestMessage before it is sent to the server
    /// (asynchronous only).
    /// </summary>
    /// <param name="request">HttpRequestMessage instance</param>
    protected virtual void SetHttpRequestHeaders(HttpRequestMessage request)
    { }

    /// <summary>
    /// Override to set headers or other properties of the
    /// WebClient before it is sent to the server
    /// (synchronous only).
    /// </summary>
    /// <param name="client">WebClient instance</param>
    protected virtual void SetWebClientHeaders(WebClient client)
    { }

    private async Task<byte[]> CallViaHttpClient(byte[] serialized, string operation, string routingToken)
    {
      HttpClient client = GetHttpClient();
      HttpRequestMessage httpRequest;
      httpRequest = new HttpRequestMessage(
        HttpMethod.Post,
        $"{DataPortalUrl}?operation={CreateOperationTag(operation, VersionRoutingTag, routingToken)}");
      SetHttpRequestHeaders(httpRequest);
#if NET8_0_OR_GREATER
      if (Options.UseTextSerialization)
        httpRequest.Content = new StringContent(
          System.Convert.ToBase64String(serialized), 
          mediaType: new MediaTypeHeaderValue("application/base64,text/plain"));
      else
        httpRequest.Content = new ByteArrayContent(serialized);
#else
      if (Options.UseTextSerialization)
        httpRequest.Content = new StringContent(System.Convert.ToBase64String(serialized));
      else
        httpRequest.Content = new ByteArrayContent(serialized);
#endif
      var httpResponse = await client.SendAsync(httpRequest);
      await VerifyResponseSuccess(httpResponse);
      if (Options.UseTextSerialization)
        serialized = Convert.FromBase64String(await httpResponse.Content.ReadAsStringAsync());
      else
        serialized = await httpResponse.Content.ReadAsByteArrayAsync();
      return serialized;
    }

    private byte[] CallViaWebClient(byte[] serialized, string operation, string routingToken)
    {
      if (!WebCallCapabilities.AreSyncWebClientMethodsSupported())
      {
        throw new NotSupportedException(Resources.SyncDataAccessNotSupportedException);
      }
      WebClient client = GetWebClient();
      var url = $"{DataPortalUrl}?operation={CreateOperationTag(operation, VersionRoutingTag, routingToken)}";
      client.Headers["Content-Type"] = Options.UseTextSerialization ? "application/base64,text/plain" : "application/octet-stream";
      SetWebClientHeaders(client);
      try
      {
        if (Options.UseTextSerialization)
        {
          var result = client.UploadString(url, System.Convert.ToBase64String(serialized));
          serialized = System.Convert.FromBase64String(result);
        }
        else
        {
          var result = client.UploadData(url, serialized);
          serialized = result;
        }
        return serialized;
      }
      catch (WebException ex)
      {
        string message;
        if (ex.Response != null)
        {
          using (var reader = new System.IO.StreamReader(ex.Response.GetResponseStream()))
            message = reader.ReadToEnd();
        }
        else
        {
          message = ex.Message;
        }
        throw new DataPortalException(message, ex);
      }
    }

    private static async Task VerifyResponseSuccess(HttpResponseMessage httpResponse)
    {
      if (!httpResponse.IsSuccessStatusCode)
      {
        var message = new StringBuilder();
        message.Append((int)httpResponse.StatusCode);
        message.Append(": ");
        message.Append(httpResponse.ReasonPhrase);
        var content = await httpResponse.Content.ReadAsStringAsync();
        if (!string.IsNullOrWhiteSpace(content))
        {
          message.AppendLine();
          message.Append(content);
        }
        throw new HttpRequestException(message.ToString());
      }
    }

    private string CreateOperationTag(string operation, string versionToken, string routingToken)
    {
      if (!string.IsNullOrWhiteSpace(versionToken) || !string.IsNullOrWhiteSpace(routingToken))
        return $"{operation}/{routingToken}-{versionToken}";
      return operation;
    }

#pragma warning disable SYSLIB0014
    private class DefaultWebClient : WebClient
    {
      private int Timeout { get; set; }

      public DefaultWebClient(int timeout) => Timeout = timeout;

      protected override WebRequest GetWebRequest(Uri address)
      {
        var req = base.GetWebRequest(address) as HttpWebRequest;
        if (Timeout > 0)
          req.Timeout = Timeout;
        return req;
      }
    }
#pragma warning restore SYSLIB0014
  }
}