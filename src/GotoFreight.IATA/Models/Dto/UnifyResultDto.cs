using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJZY.Expand.ABP.Core.Common.Dto
{
    public class UnifyResultDto
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }

        public UnifyResultDto()
        {

        }

        public UnifyResultDto(string code, string msg, object data)
        {
            Code = code;
            Msg = msg;
            Data = data;
        }

        public UnifyResultDto(bool isSuccess, string msg = "")
        {
            Code = isSuccess ? StatusCodes.Status200OK.ToString() : StatusCodes.Status400BadRequest.ToString();
            Msg = msg;
        }
        public UnifyResultDto(bool isSuccess, object data, string msg = "")
        {
            Code = isSuccess ? StatusCodes.Status200OK.ToString() : StatusCodes.Status400BadRequest.ToString();
            Data = data;
            Msg = msg;
        }
    }
}
