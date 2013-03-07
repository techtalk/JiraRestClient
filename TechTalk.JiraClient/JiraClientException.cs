using System;
using System.Runtime.Serialization;

namespace JiraConsoleApp.Jira
{
    [Serializable]
    public class JiraClientException : Exception
    {
        public JiraClientException() { }
        public JiraClientException(string message) : base(message) { }
        public JiraClientException(string message, Exception inner) : base(message, inner) { }
        protected JiraClientException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
