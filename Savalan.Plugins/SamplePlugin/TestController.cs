

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace SamplePlugin
{
    [Route("test/{action}")]
    public class TestController : Controller
    {
        [EnableCors("Default")]
        public string Test(){
            return "Helllo From Plugin"; 
        }
    }
}