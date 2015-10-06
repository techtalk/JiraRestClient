using System;

namespace TechTalk.JiraRestClient
{
    public class FieldAttribute : Attribute
    {
        public string FieldName { get; set; }

        public FieldAttribute(string fieldName)
        {
            this.FieldName = fieldName;
        }
    }
}
