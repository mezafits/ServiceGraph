namespace ServiceGraph.Common
{
    public enum MetadataType
    {
        Text,
        Link,
        Query,
        Tags,
        Properties
    }

    public class Metadata
    {
        public Guid Id { get; set; }
        public string TagType { get; set; }
        public string TagName { get; set; }
        public string TagValue { get; set; }

    }

}