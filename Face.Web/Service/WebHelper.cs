using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Face.Web.Service
{
    public class WebHelper
    {
        /// <summary>
        /// 使用新的HTTP封装:.Net4.5以后版本才有
        /// </summary>
        //protected static HttpClientHandler handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
        protected static CookieContainer cookieContainer = new CookieContainer();
        protected static HttpClient web = null;

        public static CookieContainer Cookie
        {
            get
            {
                return cookieContainer;
            }
        }

        static WebHelper()
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                CookieContainer = cookieContainer,
                AllowAutoRedirect = true,
                UseCookies = true,
            };
            web = new HttpClient(httpClientHandler);
            //web.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("ascii"));
            //web.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<String> Get(String url)
        {
            var response = await web.GetAsync(url);
            response.EnsureSuccessStatusCode();
            if (null != response.Content)
            {
                var result = await response.Content.ReadAsStringAsync();
                var cookie = cookieContainer.GetCookies(new Uri(url));
                return result;
            }

            return null;
        }

        public async Task<byte[]> GetBytes(String url)
        {
            var response = await web.GetAsync(url);
            response.EnsureSuccessStatusCode();
            if (null != response.Content)
            {
                var result = await response.Content.ReadAsByteArrayAsync();
                var cookie = cookieContainer.GetCookies(new Uri(url));
                return result;
            }

            return null;
        }

        public void Get(String url, Action<string,Exception> completed)
        {
            web.GetAsync(url).ContinueWith((postTask) => {
                HttpResponseMessage response = postTask.Result;
                try
                {
                    response.EnsureSuccessStatusCode();
                    response.Content.ReadAsStringAsync().ContinueWith((readTask) => {
                        completed(readTask.Result, null);
                    });
                }
                catch (Exception exp)
                {
                    System.Diagnostics.Debug.WriteLine(exp);
                    completed(null, exp);
                }
            });
        }

        public void AsyncGet(String url, Action<string, Exception> completed)
        {
            throw new NotImplementedException();
        }

        public void Post(KeyValuePair<String, String>[] para, String url, Action<string, Exception> completed)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var v in para)
            {
                sb.Append(v.Key);
                sb.Append("=");
                sb.Append(v.Value);
                sb.Append("&");
            }
            HttpContent content = new StringContent(sb.ToString());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            web.PostAsync(url, content).ContinueWith((postTask) => {
                HttpResponseMessage response = postTask.Result;
                try
                {
                    response.EnsureSuccessStatusCode();
                    response.Content.ReadAsStringAsync().ContinueWith((readTask) => {
                        completed(readTask.Result, null);
                    });
                }
                catch (Exception exp)
                {
                    System.Diagnostics.Debug.WriteLine(exp);
                    completed(null, exp);
                }
            });
        }

        public async Task<string> Post(KeyValuePair<String, String>[] para, String url)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var v in para)
            {
                sb.Append(v.Key);
                sb.Append("=");
                sb.Append(v.Value);
                sb.Append("&");
            }
            HttpContent content = new StringContent(sb.ToString());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var response = await web.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            if(response.Content != null)
            {
                var str = await response.Content.ReadAsStringAsync();
                return str;
            }

            return null;
        }

        public void Post(String para, String url, Action<string, Exception> completed)
        {
            throw new NotImplementedException();
        }

        public void JsonPost(String para, String url, Action<string, Exception> completed)
        {
            throw new NotImplementedException();
        }

        public async void JsonPost<T>(String url, T para, Action<string, CookieCollection, Exception> completed)
        {
            var response = await web.PostAsJsonAsync(url, para);
            response.EnsureSuccessStatusCode();
            var cookie = cookieContainer.GetCookies(new Uri(url));
            var str = await response.Content.ReadAsStringAsync();
            if(null != completed)
            {
                completed(str, cookie, null);
            }
            else
            {
                System.Diagnostics.Debug.Assert(false);
            }
            //var task = web.PostAsJsonAsync(url, para).ContinueWith(
            //    (postTask) => {
            //        try
            //        {
            //            HttpResponseMessage response = postTask.Result;
            //            response.EnsureSuccessStatusCode();
            //            response.Content.ReadAsStringAsync().ContinueWith((readTask) => {
            //                var cookie = cookieContainer.GetCookies(new Uri(url));
            //                completed(readTask.Result, cookie, null);
            //                readTask.Wait();
            //                System.Diagnostics.Debug.WriteLine("Waiting GetCookies ....");
            //            });
            //            postTask.Wait();
            //            System.Diagnostics.Debug.WriteLine("Waiting ReadAsStringAsync ......");
            //        }
            //        catch (Exception exp)
            //        {
            //            System.Diagnostics.Debug.WriteLine(exp);
            //            completed(null, null, exp);
            //        }
            //    });

            //task.Wait();
            //System.Diagnostics.Debug.WriteLine("Waiting PostAsJsonAsync........");
        }

        public async Task<string> JsonPost<T>(String url, T para)
        {
            var response = await web.PostAsJsonAsync(url, para);
            response.EnsureSuccessStatusCode();
            if (null != response.Content)
            {
                var result = await response.Content.ReadAsStringAsync();
                var cookie = cookieContainer.GetCookies(new Uri(url));
                return result;
            }

            return null;
        }

        //public void Post(KeyValuePair<String, String>[] para, String url)
        //{
        //    Post(KeyValueToString(para), url);
        //}

        public void Post(String para, String url)
        {
            throw new NotImplementedException();
        }

        public void AsyncPost(KeyValuePair<String, String>[] para, String url, Action<string, Exception> completed)
        {
            throw new NotImplementedException();
        }

        public void AsyncPost<T>(String url, T para, Action<string, CookieContainer, Exception> completed)
        {
            web.PostAsJsonAsync(url, para).ContinueWith(
                (postTask)=> {
                    HttpResponseMessage response = postTask.Result;
                    try
                    {
                        response.EnsureSuccessStatusCode();
                        response.Content.ReadAsStringAsync().ContinueWith((readTask)=> {
                            completed(readTask.Result, cookieContainer, null);
                        });
                    }
                    catch(Exception exp)
                    {
                        System.Diagnostics.Debug.WriteLine(exp);
                        completed(null, null, exp);
                    }
                });
        }

        public void AsyncJsonPost<T>(String url, T para, Action<string, CookieCollection, Exception> completed)
        {
            var task = web.PostAsJsonAsync(url, para).ContinueWith(
                (postTask) => {
                    try
                    {
                        HttpResponseMessage response = postTask.Result;
                        response.EnsureSuccessStatusCode();
                        response.Content.ReadAsStringAsync().ContinueWith((readTask) => {
                            var cookie = cookieContainer.GetCookies(new Uri(url));
                            completed(readTask.Result, cookie, null);
                        });
                    }
                    catch (Exception exp)
                    {
                        System.Diagnostics.Debug.WriteLine(exp);
                        completed(null, null, exp);
                    }
                });
        }

        /// <summary>
        /// 我们的程序要求使用UTF-8编码, 服务器端也应该使用UTF-8编码
        /// </summary>
        /// <param name="para"></param>
        /// <param name="url"></param>
        /// <param name="completed"></param>
        public void AsyncJsonPost(String para, String url, Action<string, Exception> completed)
        {
            throw new NotImplementedException();
        }

        private void ResponseCallback(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        private void GetResponse(object state)
        {
            throw new NotImplementedException();
        }

        public String KeyValueToString(KeyValuePair<String, String>[] para)
        {
            if (null == para)
            {
                throw new ArgumentNullException();
            }

            int count = para.Length;
            if (0 == count)
            {
                return String.Empty;
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < count; ++i)
            {
                sb.Append(para[i].Key);
                sb.Append("=");
                sb.Append(para[i].Value);
                sb.Append("&");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }
}
