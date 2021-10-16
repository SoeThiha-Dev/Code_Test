using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.Model
{
    public class Response
    {
        public string Message { get; set; }
        public APIStatus Status { get; set; }
        public object Data { get; set; }
    }

    public enum APIStatus
    {
        Successfull = 0,
        Error = 1,
        SystemError = 2,
    }
}
