namespace TechTalk.JiraRestClient
{
    using System;

    public class FieldAttribute : Attribute
    {
        public string FieldName { get; set; }

        public FieldAttribute(string fieldName)
        {
            this.FieldName = fieldName;
        }
    }
}