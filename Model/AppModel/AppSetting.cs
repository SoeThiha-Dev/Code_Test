using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.Model
{
    public class AppSetting
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        public string ConnectionString { get; set; }
    }

    public class Token
    {
        public string Mobile { get; set; }
        public int Role { get; set; }
    }
}
