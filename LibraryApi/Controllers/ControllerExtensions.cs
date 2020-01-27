using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public static class ControllerExtensions
    {
        public static ActionResult<T> Maybe<T>(this Controller controller, T entity)
        {
            if(entity == null)
            {
                return new NotFoundResult();
            } else
            {
                return new OkObjectResult(entity);
            }
        }
    }
}
