namespace ServiceGraph.Common
{
    using System.Text.Json;

    public partial class ProjectUtilities
    {
        public static Project GenerateDefaultProject(string user)
        {
            var project = new Project
            {
                Id = Guid.NewGuid(),
                ProjectName = "Default Project",
                Owners = new List<string> { user },
            };
            var nodes = new List<ServiceNode>();
            var edges = new List<Edge>();
            var Id = Guid.NewGuid();
            var node = new ServiceNode()
            {
                Id = Id,
                ProjectId = project.Id,
                ParentId = Id,
                Name = $"New Node",
                IconId = "00046",
                Xpos = 0,
                Ypos = 0,
                NodeType = "node"
            };

            nodes.Add(node);
            project.edges = edges;
            project.nodes = nodes;

            return project;
        }

    }

}