using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using internet_shop.Models;

namespace internet_shop.Controllers
{
    public class ValueController : ApiController
    {
        static Mymodel p = new Mymodel();


        public HttpResponseMessage Post(Parameters q)
        {
            p.Post(q);
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        public HttpResponseMessage GetManager(int id)
        {
            if (id == -1) return Request.CreateResponse(HttpStatusCode.OK, p.Get_m());
            if (id == -2) return Request.CreateResponse(HttpStatusCode.OK, p.Get_num());
            return Request.CreateResponse(HttpStatusCode.OK, p.Get_order(id));
        }

        public HttpResponseMessage GetGoods()
        {
            return Request.CreateResponse(HttpStatusCode.OK, p.Get());
        }
    }
}
