using System;
using System.Collections.Generic;
using System.IO;

namespace TechTalk.JiraRestClient
{
    public interface IJiraClient<TIssueFields> where TIssueFields : IssueFields, new()
    {
        /// <summary>Returns all issues for the given project</summary>
        IEnumerable<Issue<TIssueFields>> GetIssues(String projectKey);
        /// <summary>Returns all issues of the specified type for the given project</summary>
        IEnumerable<Issue<TIssueFields>> GetIssues(String projectKey, String issueType);
        /// <summary>Enumerates through all issues for the given project</summary>
        IEnumerable<Issue<TIssueFields>> EnumerateIssues(String projectKey);
        /// <summary>Enumerates through all issues of the specified type for the given project</summary>
        IEnumerable<Issue<TIssueFields>> EnumerateIssues(String projectKey, String issueType);

        /// <summary>Returns the issue identified by the given ref</summary>
        Issue<TIssueFields> LoadIssue(String issueRef);
        /// <summary>Returns the issue identified by the given ref</summary>
        Issue<TIssueFields> LoadIssue(IssueRef issueRef);
        /// <summary>Creates an issue of the specified type for the given project</summary>
        Issue<TIssueFields> CreateIssue(String projectKey, String issueType, String summary);
        /// <summary>Creates an issue of the specified type for the given project</summary>
        Issue<TIssueFields> CreateIssue(String projectKey, String issueType, TIssueFields issueFields);
        /// <summary>Updates the given issue on the remote system</summary>
        Issue<TIssueFields> UpdateIssue(Issue<TIssueFields> issue);
        /// <summary>Deletes the given issue from the remote system</summary>
        void DeleteIssue(IssueRef issue);

        /// <summary>Returns all transitions avilable to the given issue</summary>
        IEnumerable<Transition> GetTransitions(IssueRef issue);
        /// <summary>Changes the state of the given issue as described by the transition</summary>
        Issue<TIssueFields> TransitionIssue(IssueRef issue, Transition transition);

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
        ServerInfo GetServerInfo();

        /// <summary>Returns changelog for issue</summary>
        IEnumerable<Change> GetChangeLog(IssueRef issue);

        /// <summary>Returns issues matching JQL query</summary>
        IEnumerable<Issue<TIssueFields>> SearchIssues(String jql);

        /// <summary>Returns details for a saved filter</summary>
        Filter GetFilter(int id);
    }
}
