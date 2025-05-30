﻿using Newtonsoft.Json;
using ServiceGraph.Common.Models;
using System.Xml.Linq;

namespace ServiceGraph.Common
{
    public class Project : BaseObject
    {

        public string ProjectName { get; set; }
        public List<string> Owners { get; set; }
        public List<string> Readers { get; set; }
        public List<ServiceNode> nodes { get; set; }
        public List<Edge> edges { get; set; }
        public Guid ServiceTreeId { get; set; }
        public bool IsPublic { get; set; } = false;

        public override Guid GetPid()
        {
            return Id;
        }

        public void UpsertServiceNode(ServiceNode updatedNode)
        {
            if (updatedNode == null)
                return;

            int index = nodes.FindIndex(n => n.Id == updatedNode.Id);

            if (index != -1)
            {
                nodes[index] = updatedNode;
            }
            else
            {
                nodes.Add(updatedNode);
            }
        }

        public void UpsertEdge(Edge updatedEdge)
        {
            if (updatedEdge == null)
                return;

            int index = edges.FindIndex(n => n.Id == updatedEdge.Id);

            if (index != -1)
            {
                edges[index] = updatedEdge;
            }
            else
            {
                edges.Add(updatedEdge);
            }
        }
        public void RemoveServiceNode(ServiceNode node)
        {
            if (node == null)
                return;

            // Remove all edges where this node is a source or target
            edges.RemoveAll(e => e.Source == node.Id.ToString() || e.Destination == node.Id.ToString());

            // Remove the node from the list of nodes
            nodes.RemoveAll(n => n.Id == node.Id);
        }
        public void RemoveServiceNodeFromGroup(ServiceNode node)
        {
            if (node == null)
                return;

            var nodeIndex = nodes.FindIndex(n => n.Id == node.Id);
            nodes[nodeIndex].ParentId = nodes[nodeIndex].Id;
        }
        public void RemoveEdge(Edge edge)
        {
            if (edge == null)
                return;

            edges.RemoveAll(e => e.Id == edge.Id);
        }

       
    }
  
}