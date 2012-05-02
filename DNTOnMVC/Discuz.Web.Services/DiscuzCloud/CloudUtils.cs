using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Discuz.Web.Services.DiscuzCloud
{
    public class CloudUtils
    {
        public static string GetCloudResponse<T>(T obj)
        {
            GetResponse<T> response = new GetResponse<T>();
            response.Result = obj;
            return JavaScriptConvert.SerializeObject(response);
        }
    }
}
