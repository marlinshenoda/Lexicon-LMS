using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Extensions
{
    public static class StudentExtension
    {
        // Returns true if HttpRequest is ajax
        public static bool IsAjax(this HttpRequest request) 
        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
