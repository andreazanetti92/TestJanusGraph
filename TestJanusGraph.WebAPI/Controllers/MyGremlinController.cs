using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Driver.Remote;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Structure;
using Gremlin.Net.Structure.IO.GraphBinary;
using JanusGraph.Net;
using JanusGraph.Net.IO.GraphBinary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestJanusGraph.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MyGremlinController : ControllerBase
    {
        private GremlinClient gremlinClient;
        private readonly GraphTraversalSource g;

        public MyGremlinController(GremlinClient gremlinClient, GraphTraversalSource g)
        {
            this.gremlinClient = gremlinClient;
            this.g = g;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var city = g.V().Has("airport", "code", "AUS").Values<string>("city").Next();

                //var pathFromAUSToSYD = g.V().Has("airport", "code", "AUS").Repeat(__.Out()).Until(__.Has("code", "SYD")).Limit<int>(10).Path().By("code").Current;

                if (city != null) return Ok(city);

                return BadRequest();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
