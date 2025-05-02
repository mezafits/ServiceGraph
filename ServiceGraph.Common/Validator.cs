using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceGraph.Common;

public static class Validator
    {
        public static List<Exception> ValidateServiceNode(ServiceNode node)
        {
            var errors = new List<Exception>();

            if (string.IsNullOrWhiteSpace(node.Name))
            {
                errors.Add(new ArgumentNullException("ServiceNode Name is required."));
            }

            // Add more validation rules as needed

            return errors;
        }

        public static List<Exception> ValidateEdge(Edge edge)
        {
            var errors = new List<Exception>();

            if (string.IsNullOrWhiteSpace(edge.Source))
            {
                errors.Add(new ArgumentNullException("Source", "Source value missing"));
            }

            if (string.IsNullOrWhiteSpace(edge.Destination))
            {
                errors.Add(new ArgumentNullException("Destination", "Destination value missing"));
            }

            if (edge.Source == edge.Destination)
            {
                errors.Add(new InvalidDataException("Source and Destination cannot be equal"));
            }


            return errors;
        }

    public static List<Exception> ValidateProject(Project projectRequest)
    {
        var errors = new List<Exception>();

        if (string.IsNullOrWhiteSpace(projectRequest.ProjectName))
        {
            errors.Add(new ArgumentNullException("Project Name", "Project Name Missing"));
        }

        if (projectRequest.Owners == null)
        {
            errors.Add(new ArgumentNullException("Owners", "No Owners assigned"));
        }
         
        return errors;
    }
}

 
