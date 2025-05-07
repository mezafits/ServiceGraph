using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceGraph.Common
{
    public abstract class BaseObject
    {

        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "pid")]
        public Guid Pid { get { return GetPid(); } }

        public List<Metadata> Metadata { get; set; } = new List<Metadata>();

        public abstract Guid GetPid();
    }

}