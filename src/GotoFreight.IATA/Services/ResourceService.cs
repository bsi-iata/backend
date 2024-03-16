using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
using Aliyun.OSS.Common;
using Aliyun.OSS;
using System.Xml.Linq;
using GotoFreight.IATA.Dto;
using Microsoft.AspNetCore.Hosting;

namespace GotoFreight.IATA.Services
{
    public class ResourceService
    {
        #region 构造函数
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private const string _dirName = "resource";
        public ResourceService(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }
        #endregion


        public UploadResultDto UploadToOSS(Stream stream, string fileName)
        {
            var _endpoint = _configuration["OSS:EndPoint"]!;
            var _accessKeyId = _configuration["OSS:AccessKeyId"]!;
            var _accessKeySecret = _configuration["OSS:AccessKeySecret"]!;
            var _bucketName = _configuration["OSS:BucketName"]!;


            var client = new OssClient(_endpoint, _accessKeyId, _accessKeySecret);

            var ossKey = $"{DateTime.Now.ToString("MMddHHmmssfff")}_{fileName}";

            using (var pubObj = client.PutObject(_bucketName, ossKey, stream))
            {
                if (pubObj.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    Uri url = client.GeneratePresignedUri(_bucketName, ossKey);
                    return new UploadResultDto(true, url.ToString(), ossKey);
                }
                else
                {
                    return new UploadResultDto(false);
                }
            }
        }


        public Stream GetExampleFile()
        {
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "GoodsExample.xlsx");

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var fileStream = new MemoryStream(fileBytes);
            return fileStream;
        }


        public async Task<string> SaveFileToLocalAsync(IFormFile file)
        {
            var path = GetPath(file.FileName, out string realName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"{_configuration["Server"]}/{_dirName}/{realName}";
        }


        public async Task<string> SaveFileToLocalAsync(byte[] bytes, string fileName)
        {
            var path = GetPath(fileName, out string realName);

            await File.WriteAllBytesAsync(path, bytes);

            return $"{_configuration["Server"]}/{_dirName}/{realName}";
        }


        private string GetPath(string fileName,out string realName)
        {
            fileName = $"{DateTime.Now.ToString("MMddHHmmssfff")}_{fileName}";
            realName = fileName;
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, _dirName, fileName);

            // 创建目录
            var directory = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(Path.Combine(_webHostEnvironment.WebRootPath, _dirName)))
                Directory.CreateDirectory(Path.Combine(_webHostEnvironment.WebRootPath, _dirName));

            return fullPath;
        }
    }
}
