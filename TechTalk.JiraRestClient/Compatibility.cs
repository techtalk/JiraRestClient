using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TechTalk.JiraRestClient
{
    public interface IJiraClient
    {
        /// <summary>Returns all issues for the given project</summary>
        IEnumerable<Issue> GetIssues(String projectKey);
        /// <summary>Returns all issues of the specified type for the given project</summary>
        IEnumerable<Issue> GetIssues(String projectKey, String issueType);
        /// <summary>Returns the issue identified by the given ref</summary>
        Issue LoadIssue(String issueRef);
        /// <summary>Returns the issue identified by the given ref</summary>
        Issue LoadIssue(IssueRef issueRef);
        /// <summary>Creates an issue of the specified type for the given project</summary>
        Issue CreateIssue(String projectKey, String issueType, String summary);
        /// <summary>Creates an issue of the specified type for the given project</summary>
        Issue CreateIssue(String projectKey, String issueType, IssueFields issueFields);
        /// <summary>Updates the given issue on the remote system</summary>
        Issue UpdateIssue(Issue issue);
        /// <summary>Deletes the given issue from the remote system</summary>
        void DeleteIssue(IssueRef issue);

        /// <summary>Returns all transitions avilable to the given issue</summary>
        IEnumerable<Transition> GetTransitions(IssueRef issue);
        /// <summary>Changes the state of the given issue as described by the transition</summary>
        Issue TransitionIssue(IssueRef issue, Transition transition);

        /// <summary>Returns all watchers for the given issue</summary>
        IEnumerable<JiraUser> GetWatchers(IssueRef issue);

        /// <summary>Returns all comments for the given issue</summary>
        IEnumerable<Comment> GetComments(IssueRef issue);
        /// <summary>Adds a comment to the given issue</summary>
        Comment CreateComment(IssueRef issue, String comment);
        /// <summary>Deletes the given comment</summary>
        void DeleteComment(IssueRef issue, Comment comment);

        /// <summary>Return all attachments for the given issue</summary>
        IEnumerable<Attachment> GetAttachments(IssueRef issue);
        /// <summary>Creates an attachment to the given issue</summary>
        Attachment CreateAttachment(IssueRef issue, Stream stream, String fileName);
        /// <summary>Deletes the given attachment</summary>
        void DeleteAttachment(Attachment attachment);

        /// <summary>Returns all links for the given issue</summary>
        IEnumerable<IssueLink> GetIssueLinks(IssueRef issue);
        /// <summary>Returns the link between two issues of the given relation</summary>
        IssueLink LoadIssueLink(IssueRef parent, IssueRef child, String relationship);
        /// <summary>Creates a link between two issues with the given relation</summary>
        IssueLink CreateIssueLink(IssueRef parent, IssueRef child, String relationship);
        /// <summary>Removes the given link of two issues</summary>
        void DeleteIssueLink(IssueLink link);

        /// <summary>Returns all remote links (attached urls) for the given issue</summary>
        IEnumerable<RemoteLink> GetRemoteLinks(IssueRef issue);
        /// <summary>Creates a remote link (attached url) for the given issue</summary>
        RemoteLink CreateRemoteLink(IssueRef issue, RemoteLink remoteLink);
        /// <summary>Updates the given remote link (attached url) of the specified issue</summary>
        RemoteLink UpdateRemoteLink(IssueRef issue, RemoteLink remoteLink);
        /// <summary>Removes the given remote link (attached url) of the specified issue</summary>
        void DeleteRemoteLink(IssueRef issue, RemoteLink remoteLink);

        /// <summary>Returns all issue types</summary>
        IEnumerable<IssueType> GetIssueTypes();

        /// <summary>Returns information about the JIRA server</summary>
        RemoteServerInfo GetServerInfo();
    }

    public class JiraClient : IJiraClient
    {
        private readonly IJiraClient<IssueFields> client;
        public JiraClient(string baseUrl, string username, string password)
        {
            client = new JiraClient<IssueFields>(baseUrl, username, password);
        }

        public IEnumerable<Issue> GetIssues(String projectKey)
        {
            return client.GetIssues(projectKey).Select(Issue.From);
        }

        public IEnumerable<Issue> GetIssues(String projectKey, String issueType)
        {
            return client.GetIssues(projectKey, issueType).Select(Issue.From);
        }

        public Issue LoadIssue(String issueRef)
        {
            return Issue.From(client.LoadIssue(issueRef));
        }

        public Issue LoadIssue(IssueRef issueRef)
        {
            return Issue.From(client.LoadIssue(issueRef));
        }

        public Issue CreateIssue(String projectKey, String issueType, String summary)
        {
            return Issue.From(client.CreateIssue(projectKey, issueType, summary));
        }

        public Issue CreateIssue(String projectKey, String issueType, IssueFields issueFields)
        {
            return Issue.From(client.CreateIssue(projectKey, issueType, issueFields));
        }

        public Issue UpdateIssue(Issue issue)
        {
            return Issue.From(client.UpdateIssue(issue));
        }

        public void DeleteIssue(IssueRef issue)
        {
            client.DeleteIssue(issue);
        }

        public IEnumerable<Transition> GetTransitions(IssueRef issue)
        {
            return client.GetTransitions(issue);
        }

        public Issue TransitionIssue(IssueRef issue, Transition transition)
        {
            return Issue.From(client.TransitionIssue(issue, transition));
        }

        public IEnumerable<JiraUser> GetWatchers(IssueRef issue)
        {
            return client.GetWatchers(issue);
        }

        public IEnumerable<Comment> GetComments(IssueRef issue)
        {
            return client.GetComments(issue);
        }

        public Comment CreateComment(IssueRef issue, string comment)
        {
            return client.CreateComment(issue, comment);
        }

        public void DeleteComment(IssueRef issue, Comment comment)
        {
            client.DeleteComment(issue, comment);
        }

        public IEnumerable<Attachment> GetAttachments(IssueRef issue)
        {
            return client.GetAttachments(issue);
        }

        public Attachment CreateAttachment(IssueRef issue, Stream stream, string fileName)
        {
            return client.CreateAttachment(issue, stream, fileName);
        }

        public void DeleteAttachment(Attachment attachment)
        {
            client.DeleteAttachment(attachment);
        }

        public IEnumerable<IssueLink> GetIssueLinks(IssueRef issue)
        {
            return client.GetIssueLinks(issue);
        }

        public IssueLink LoadIssueLink(IssueRef parent, IssueRef child, string relationship)
        {
            return client.LoadIssueLink(parent, child, relationship);
        }

        public IssueLink CreateIssueLink(IssueRef parent, IssueRef child, string relationship)
        {
            return client.CreateIssueLink(parent, child, relationship);
        }

        public void DeleteIssueLink(IssueLink link)
        {
            client.DeleteIssueLink(link);
        }

        public IEnumerable<RemoteLink> GetRemoteLinks(IssueRef issue)
        {
            return client.GetRemoteLinks(issue);
        }

        public RemoteLink CreateRemoteLink(IssueRef issue, RemoteLink remoteLink)
        {
            return client.CreateRemoteLink(issue, remoteLink);
        }

        public RemoteLink UpdateRemoteLink(IssueRef issue, RemoteLink remoteLink)
        {
            return client.UpdateRemoteLink(issue, remoteLink);
        }

        public void DeleteRemoteLink(IssueRef issue, RemoteLink remoteLink)
        {
            client.DeleteRemoteLink(issue, remoteLink);
        }

        public IEnumerable<IssueType> GetIssueTypes()
        {
            return client.GetIssueTypes();
        }

        public RemoteServerInfo GetServerInfo()
        {
            return client.GetServerInfo();
        }
    }

    public class Issue : Issue<IssueFields>
    {
        internal static Issue From(Issue<IssueFields> other)
        {
            if (other == null)
                return null;

            return new Issue
            {
                expand = other.expand,
                id = other.id,
                key = other.key,
                self = other.self,
                fields = other.fields,
            };
        }
    }
}
