namespace ServiceGraph.Common
{ 
public class Edge:BaseObject
    {
        public Edge() : base()
        {
            Metadata = new List<Metadata>();
        }

        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public bool IsBidirectional { get; set; } = false;
       
        public EdgeStyle Style { get; set; } = new EdgeStyle();
        public override Guid GetPid()
        {
            return Id;
        }
    }

 }