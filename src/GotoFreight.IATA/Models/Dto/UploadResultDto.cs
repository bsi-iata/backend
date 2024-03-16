using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotoFreight.IATA.Dto
{
    public class UploadResultDto
    {
        /// <summary>
        /// 标识本次操作是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 临时URL
        /// </summary>
        public string TempUrl { get; set; }

        /// <summary>
        /// 文件唯一标识Key
        /// </summary>
        public string OSSKey { get; set; }

        /// <summary>
        /// 文件原始名称
        /// </summary>
        public string FileName { get; set; }

        public UploadResultDto(bool isSuccess, string tempUrl, string oSSKey)
        {
            IsSuccess = isSuccess;
            TempUrl = tempUrl;
            OSSKey = oSSKey;
        }

        public UploadResultDto(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
    }
}
