using Face.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceStudioClient.Service
{
    class PhotoImageService : BaseService
    {
        public event Action<PhotoImage> OnUploadCompleted;

        public void Upload(byte[] data, string fileName, Action<Exception> failFunc)
        {
            var url = "http://localhost:8888/api/File/Upload";
            var para = new FileData
            {
                Data = data,
                FileName = fileName,
            };
            web.AsyncJsonPost(url, para,
                (response, cookies, exp) =>
                {
                    if (null != response)
                    {
                        var ret = JsonSerializer<PhotoImage>.Deserialize(response);
                        if (ret != null)
                        {
                            if (null != OnUploadCompleted)
                            {
                                OnUploadCompleted(ret);
                            }
                        }
                        else
                        {
                            failFunc(new Exception("Failed in Upload File!"));
                        }
                    }
                    else if (null != exp)
                    {
                        failFunc(exp);
                    }
                });
        }


        public async Task<byte[]> Download(String url)
        {
            Byte[] data = await web.GetBytes(url);
            return data;
        }

        public static string GetImageFileURL(Guid id)
        {
            return string.Format("http://localhost:8888/PhotoImage/index/{0}", id);
        }
    }
}
