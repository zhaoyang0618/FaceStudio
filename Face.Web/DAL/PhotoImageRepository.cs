using Face.Contract;
using Face.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Face.Web.DAL
{
    public class PhotoImageRepository : GenericRepository<PhotoImage>
    {
        public PhotoImageRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public PhotoImage Create(HttpPostedFileBase fileToUpload, string username)
        {
            var file = SaveFile(fileToUpload, System.Configuration.ConfigurationManager.AppSettings["PhotoImages"]);
            file.CreateUser = username;
            file.UpdateUser = username;
            Insert(file);
            context.SaveChanges();
            return file;
        }

        public PhotoImage Create(Byte[] data, string filename, string username)
        {
            var file = SaveFile(data, filename, System.Configuration.ConfigurationManager.AppSettings["PhotoImages"]);
            file.CreateUser = username;
            file.UpdateUser = username;
            Insert(file);
            context.SaveChanges();
            return file;
        }

        PhotoImage SaveFile(HttpPostedFileBase fileToUpload, String saveFilePath)
        {
            string fileName = Path.GetFileName(fileToUpload.FileName);      //原始文件名称
            string fileExtension = Path.GetExtension(fileName);         //文件扩展名
                                                                        //使用年月创建文件夹
            DateTime now = DateTime.Now;
            String dirName = String.Format("{0}{1:D2}", now.Year, now.Month);
            String dirPath = Path.Combine(saveFilePath, dirName);
            try
            {
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
            }
            catch
            {
            }

            //
            var file = new PhotoImage() { ID = Guid.NewGuid() };
            string saveName = file.ID.ToString() + fileExtension; //保存文件名称
            String filePath = Path.Combine(dirPath, saveName);
            fileToUpload.SaveAs(filePath);

            file.FileName = fileName;
            file.FilePath = filePath;
            file.MimeType = fileToUpload.ContentType;
            return file;
        }

        PhotoImage SaveFile(Byte[] data, string fileName, String saveFilePath)
        {
            string fileExtension = Path.GetExtension(fileName);         //文件扩展名
                                                                        //使用年月创建文件夹
            DateTime now = DateTime.Now;
            String dirName = String.Format("{0}{1:D2}", now.Year, now.Month);
            String dirPath = Path.Combine(saveFilePath, dirName);
            try
            {
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
            }
            catch
            {
            }

            //
            var file = new PhotoImage() { ID = Guid.NewGuid() };
            string saveName = file.ID.ToString() + fileExtension; //保存文件名称
            String filePath = Path.Combine(dirPath, saveName);
            System.IO.File.WriteAllBytes(filePath, data);
            file.FileName = fileName;
            file.FilePath = filePath;
            file.MimeType = MimeMapping.GetMimeMapping(fileName);
            return file;
        }

        /// <summary>
        /// 只更新部分数据
        /// </summary>
        /// <param name="photo"></param>
        public void UpdateFeature(PhotoImage photo, string user)
        {
            dbSet.Attach(photo);
            DbHelper<PhotoImage>.DbColumnsUpdate(context, photo,
                new String[] { "Feature", "FeatureKey" });
            context.SaveChanges();
        }
    }
}
