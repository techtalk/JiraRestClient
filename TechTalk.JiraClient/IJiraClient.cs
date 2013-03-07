using System;
using System.Collections.Generic;
using System.IO;

namespace JiraConsoleApp.Jira
{
    public interface IJiraClient
    {
        /// <summary>Returns all issues for the given JIRA project</summary>
        IEnumerable<Issue> GetIssues(String projectKey);
        /// <summary>Returns the issue identified by the given ref</summary>
        Issue LoadIssue(String issueRef);
        /// <summary>Returns the issue identified by the given ref</summary>
        Issue LoadIssue(IssueRef issueRef);
        /// <summary>Creates an issue (of the given type) for the given JIRA project</summary>
        Issue CreateIssue(String projectKey, String typeCode, String summary);
        /// <summary>Updates the given JIRA issue on the remote system</summary>
        Issue UpdateIssue(Issue issue);
        /// <summary>Deletes the given JIRA issue from the remote system</summary>
        void DeleteIssue(IssueRef issue);

        /// <summary>Returns all comments for the given JIRA issue</summary>
        IEnumerable<Comment> GetComments(IssueRef issue);
        /// <summary>Adds a comment to the given JIRA issue</summary>
        Comment CreateComment(IssueRef issue, String comment);
        /// <summary>Deletes the given comment</summary>
        void DeleteComment(IssueRef issue, Comment comment);

        /// <summary>Return all attachments for the given JIRA issue</summary>
        IEnumerable<Attachment> GetAttachments(IssueRef issue);
        /// <summary>Creates an attachment to the given JIRA issue</summary>
        Attachment CreateAttachment(IssueRef issue, Stream stream, String fileName);
        /// <summary>Deletes the given attachment</summary>
        void DeleteAttachment(Attachment attachment);

        /// <summary>Returns all links for the given JIRA issue</summary>
        IEnumerable<IssueLink> GetIssueLinks(IssueRef issue);
        /// <summary>Returns the link between two JIRA issues of the given relation</summary>
        IssueLink LoadIssueLink(IssueRef parent, IssueRef child, String relationship);
        /// <summary>Creates a link between two JIRA issues with the given relation</summary>
        IssueLink CreateIssueLink(IssueRef parent, IssueRef child, String relationship);
        /// <summary>Removes the given link of two JIRA issues</summary>
        void DeleteIssueLink(IssueLink link);
    }
}
