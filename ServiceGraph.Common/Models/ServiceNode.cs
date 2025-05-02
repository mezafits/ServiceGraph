using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace ServiceGraph.Common
{
    public class ServiceNode : BaseObject
    {
        public string NodeType { get; set; } = "node";
        public Guid ParentId { get; set; } = Guid.Empty;
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public string IconId { get; set; }
        public float Xpos { get; set; }
        public float Ypos { get; set; }

        public List<Metadata> Metadata { get; set; } = new List<Metadata>();

        public override Guid GetPid()
        {
            return Id;
        }
    }

}