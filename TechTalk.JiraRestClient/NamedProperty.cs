namespace TechTalk.JiraRestClient
{
    using System.Reflection;

    internal class NamedProperty
    {
        public PropertyInfo Property { get; set; }

        public string FieldName { get; set; }

        public NamedProperty(PropertyInfo property, string fieldName)
        {
            this.Property = property;
            this.FieldName = fieldName;
        }
    }
}