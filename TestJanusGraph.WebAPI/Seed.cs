using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Structure;

namespace TestJanusGraph.WebAPI
{
    public static class Seed
    {
        public static void LoadGraph(GraphTraversalSource g)
        {
            //g.Io<Graph>("C:\\Users\\ext-azanetti\\Desktop\\janusgraph\\mydata\\air-routes.graphml");

            g.Io<Graph>("/opt/janusgraph/mydata/air-routes.graphml").With(IO.reader, IO.graphml);
            
        }
    }
}
