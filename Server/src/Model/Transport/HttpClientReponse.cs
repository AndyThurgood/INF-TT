using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Transport
{
    public class HttpClientResponse<T>
    {
        public bool IsSuccessful { get; set; }
        public int HttpStatusCode { get; set; }
        public T ResponseContent { get; set; }
        public string ErrorMessage { get; set; }
    }
}
