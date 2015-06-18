using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Pelykh.Common.Net
{
    public static class WebRequestExtensions
    {
        public static Task<WebResponse> GetResponseAsync(this WebRequest webRequest)
        {
            webRequest.ThrowIfNull("this");

            var task = Task.Factory.StartNew(() => Task<WebResponse>.Factory.FromAsync(
                    webRequest.BeginGetResponse,
                    webRequest.EndGetResponse,
                    null));
            try
            {
                task.Wait();
            }
            catch (AggregateException aggregateException)
            {
                throw aggregateException.Flatten();
            }

            return task.Result;
        }

        public static Task<Stream> GetRequestStreamAsync(this WebRequest webRequest)
        {
            webRequest.ThrowIfNull("this");

            var task = Task.Factory.StartNew(() => Task<Stream>.Factory.FromAsync(
                    webRequest.BeginGetRequestStream,
                    webRequest.EndGetRequestStream,
                    null));

            try
            {
                task.Wait();
            }
            catch (AggregateException aggregateException)
            {
                throw aggregateException.InnerException;
            }

            return task.Result;
        }
    }
}
