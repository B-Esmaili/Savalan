using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Savalan.Web.Mvc
{
    public class CRUDController<TModel> : CRUDController<TModel, TModel>
    {

    }
    public class CRUDController<TModel, TResult> : Controller
    {
        [HttpGet]        
        public async Task<IActionResult> Read()
        {
           return await Task.FromResult(new EmptyResult());
        } 
    }
}