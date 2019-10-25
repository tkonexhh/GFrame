using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace GFrame
{
    public enum ErrorCode
    {
        /// <summary>
        ///服务器会返回500或者其它状态码，表示服务器异常，此时客户端需要稍后再发起相关接口请求。 //此处不用
        /// </summary>
        ERROR_OTHER = -1,

        /// <summary>
        ///正常
        /// </summary>
        ERROR_NOERROR = 200,

        /// <summary>
        /// 接口-非正常形式提交
        /// </summary>
        ERROR_COMMIT = 405,

        /// <summary>
        ///客户端请求无效
        /// </summary>
        ERROR_REQUEST = 600,

        /// <summary>
        /// 可能是密钥过期，
        /// </summary>
        ERROR_KEY_OVERDUE = 601,//或加密后的数据错误  调用服务器接口重新获取密钥来进行消息加密后发起请求

        /// <summary>
        ///解密后数据不标准
        /// </summary>
        ERROR_UNASE_UNNORMAL = 602,//的JSON格式字符串

        /// <summary>
        ///服务器的逻辑是对于比服务器时间早60秒的请求直接返回http错误码603，表示这个是过期的异常请求消息
        /// </summary>
        ERROR_UNNORMAL_REQUEST = 603,

        /// <summary>
        ///服务器异常  
        /// </summary>
        ERROR_SERVER_UNUSUAL = 500,//  服务器会返回500或者其它状态码，表示，此时客户端需要稍后再发起相关接口请求。
    }

}