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

            return Task.Factory.StartNew(() => Task<WebResponse>.Factory.FromAsync(
                webRequest.BeginGetResponse,
                webRequest.EndGetResponse,
                null).Result);
        }

        public static Task<Stream> Stream(this WebRequest webRequest)
        {
            return Task.Factory.StartNew(() => Task<Stream>.Factory.FromAsync(
                webRequest.BeginGetRequestStream,
                webRequest.EndGetRequestStream,
                null).Result);
        }
    }
}
