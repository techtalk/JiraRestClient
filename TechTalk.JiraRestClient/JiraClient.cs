﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using RestSharp;
using RestSharp.Deserializers;

namespace TechTalk.JiraRestClient
{
    public class JiraClient : IJiraClient
    {
        private readonly string username;
        private readonly string password;
        private readonly RestClient client;
        private readonly JsonDeserializer deserializer;
        public JiraClient(string baseUrl, string username, string password)
        {
            this.username = username;
            this.password = password;
            deserializer = new JsonDeserializer();
            client = new RestClient { BaseUrl = baseUrl + (baseUrl.EndsWith("/") ? "" : "/") + "rest/api/2/" };
        }

        private RestRequest CreateRequest(Method method, String path)
        {
            var request = new RestRequest { Method = method, Resource = path, RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format("{0}:{1}", username, password))));
            return request;
        }

        private void AssertStatus(IRestResponse response, HttpStatusCode status)
        {
            if (response.ErrorException != null)
                throw new JiraClientException("Transport level error: " + response.ErrorMessage, response.ErrorException);
            if (response.StatusCode != status)
                throw new JiraClientException("JIRA returned wrong status: " + response.StatusDescription, response.Content);
        }


        public IEnumerable<Issue> GetIssues(String projectKey)
        {
            return GetIssues(projectKey, null);
        }

        public IEnumerable<Issue> GetIssues(String projectKey, String issueType)
        {
            try
            {
                var result = new List<Issue>(4);
                while (true)
                {
                    var jql = String.Format("project={0}", WebUtility.HtmlEncode(projectKey));
                    if (!String.IsNullOrEmpty(issueType))
                        jql += String.Format(" AND issueType={0}", WebUtility.HtmlEncode(issueType));
                    var path = String.Format("search?jql={0}&startAt={1}&maxResults={2}", jql, result.Count, result.Capacity);
                    var request = CreateRequest(Method.GET, path);

                    var response = client.Execute(request);
                    AssertStatus(response, HttpStatusCode.OK);

                    var data = deserializer.Deserialize<IssueContainer>(response);
                    result.AddRange(data.issues ?? Enumerable.Empty<Issue>());

                    if (result.Count < data.total) continue;
                    else /* all issues received */ break;
                }
                return result;
            }
            catch (Exception ex)
            {
                Trace.TraceError("GetIssues(projectKey) error: {0}", ex);
                throw new JiraClientException("Could not load issues", ex);
            }
        }

        public Issue LoadIssue(IssueRef issueRef)
        {
            if (String.IsNullOrEmpty(issueRef.id))
                return LoadIssue(issueRef.key);
            else /* we have an id */
                return LoadIssue(issueRef.id);
        }

        public Issue LoadIssue(String issueRef)
        {
            try
            {
                var path = String.Format("issue/{0}", issueRef);
                var request = CreateRequest(Method.GET, path);

                var response = client.Execute(request);
                AssertStatus(response, HttpStatusCode.OK);

                var issue = deserializer.Deserialize<Issue>(response);
                issue.fields.comments = GetComments(issue).ToList();
                issue.ExpandLinks(issue);
                return issue;
            }
            catch (Exception ex)
            {
                Trace.TraceError("GetIssue(issueRef) error: {0}", ex);
                throw new JiraClientException("Could not load issue", ex);
            }
        }

        public Issue CreateIssue(String projectKey, String issueType, String summary)
        {
            return CreateIssue(projectKey, issueType, summary, null, null);
        }

        /// <summary>
        /// Creates an issue in JIRA, allowing for immediate entry of estimate and any supplementary fields
        /// </summary>
        /// <param name="projectKey">Key for the project where the issue should be created</param>
        /// <param name="issueType">Type of issue (e.g. bug, feature, ...)</param>
        /// <param name="summary">Title for the issue</param>
        /// <param name="originalEstimate">The estimate to be added to the issue. If not present, nothing will be filled in</param>
        /// <param name="extraFields">Optional extra fields e.g. new { customfield_10601 = "Test string" }</param>
        /// <returns></returns>
        public Issue CreateIssue(string projectKey, string issueType, string summary, string originalEstimate, object extraFields)
        {
            try
            {
                var request = CreateRequest(Method.POST, "issue");
                request.AddHeader("ContentType", "application/json");
                object requestBody = new
                    {
                        fields = new
                            {
                                project = new {key = projectKey},
                                issuetype = new {name = issueType},
                                summary = summary,
                                timetracking = new
                                    { 
                                        originalEstimate, 
                                        remainingEstimate = originalEstimate
                                    }
                                
                            }
                    };

                if (extraFields != null)
                {
                    var rb = requestBody.ToDictionary();
                    IDictionary<string, object> fields = rb["fields"].ToDictionary();
                    foreach (var extraField in extraFields.ToDictionary())
                    {
                           fields.Add(extraField.Key, extraField.Value);
                    }
                    rb["fields"] = fields;
                    requestBody = rb;
                }

                request.AddBody(requestBody);

                var response = this.client.Execute(request);
                this.AssertStatus(response, HttpStatusCode.Created);

                var issue = deserializer.Deserialize<Issue>(response);
                return this.LoadIssue(issue.key);
            }
            catch (Exception ex)
            {
                Trace.TraceError("CreateIssue(projectKey, typeCode) error: {0}", ex);
                throw new JiraClientException("Could not create issue", ex);
            }
        }

        public Issue UpdateIssue(Issue issue)
        {
            try
            {
                var path = String.Format("issue/{0}", issue.id);
                var request = CreateRequest(Method.PUT, path);
                request.AddHeader("ContentType", "application/json");

                var update = new Dictionary<string, object>();
                if (issue.fields.summary != null)
                    update.Add("summary", new[] { new { set = issue.fields.summary } });
                if (issue.fields.description != null)
                    update.Add("description", new[] { new { set = issue.fields.description } });
                if (issue.fields.labels != null)
                    update.Add("labels", new[] { new { set = issue.fields.labels } });
                if (issue.fields.timetracking != null)
                    update.Add("timetracking", new[] { new { set = new { originalEstimate = issue.fields.timetracking.originalEstimate } } });

                request.AddBody(new { update = update });

                var response = client.Execute(request);
                AssertStatus(response, HttpStatusCode.NoContent);

                return LoadIssue(issue);
            }
            catch (Exception ex)
            {
                Trace.TraceError("UpdateIssue(issue) error: {0}", ex);
                throw new JiraClientException("Could not update issue", ex);
            }
        }

        public void DeleteIssue(IssueRef issue)
        {
            try
            {
                var path = String.Format("issue/{0}?deleteSubtasks=true", issue.id);
                var request = CreateRequest(Method.DELETE, path);

                var response = client.Execute(request);
                AssertStatus(response, HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                Trace.TraceError("DeleteIssue(issue) error: {0}", ex);
                throw new JiraClientException("Could not delete issue", ex);
            }
        }


        public IEnumerable<Transition> GetTransitions(IssueRef issue)
        {
            try
            {
                var path = String.Format("issue/{0}/transitions?expand=transitions.fields", issue.id);
                var request = CreateRequest(Method.GET, path);

                var response = client.Execute(request);
                AssertStatus(response, HttpStatusCode.OK);

                var data = deserializer.Deserialize<TransitionsContainer>(response);
                return data.transitions;
            }
            catch (Exception ex)
            {
                Trace.TraceError("UpdateIssue(issue) error: {0}", ex);
                throw new JiraClientException("Could not update issue", ex);
            }
        }

        public Issue TransitionIssue(IssueRef issue, Transition transition)
        {
            try
            {
                var path = String.Format("issue/{0}/transitions", issue.id);
                var request = CreateRequest(Method.POST, path);
                request.AddHeader("ContentType", "application/json");

                var update = new Dictionary<string, object>();
                update.Add("transition", new { id = transition.id });
                if (transition.fields != null)
                    update.Add("fields", transition.fields);

                request.AddBody(update);

                var response = client.Execute(request);
                AssertStatus(response, HttpStatusCode.NoContent);

                return LoadIssue(issue);
            }
            catch (Exception ex)
            {
                Trace.TraceError("UpdateIssue(issue) error: {0}", ex);
                throw new JiraClientException("Could not update issue", ex);
            }
        }


        public IEnumerable<Comment> GetComments(IssueRef issue)
        {
            try
            {
                var result = new List<Comment>(4);
                while (true)
                {
                    var path = String.Format("issue/{0}/comment", issue.id);
                    var request = CreateRequest(Method.GET, path);

                    var response = client.Execute(request);
                    AssertStatus(response, HttpStatusCode.OK);

                    var data = deserializer.Deserialize<CommentsContainer>(response);
                    result.AddRange(data.comments ?? Enumerable.Empty<Comment>());

                    if (result.Count < data.total) continue;
                    else /* all issues received */ break;
                }
                return result;
            }
            catch (Exception ex)
            {
                Trace.TraceError("GetComments(issue) error: {0}", ex);
                throw new JiraClientException("Could not load comments", ex);
            }
        }

        public Comment CreateComment(IssueRef issue, String comment)
        {
            try
            {
                var path = String.Format("issue/{0}/comment", issue.id);
                var request = CreateRequest(Method.POST, path);
                request.AddHeader("ContentType", "application/json");
                request.AddBody(new Comment { body = comment });

                var response = client.Execute(request);
                AssertStatus(response, HttpStatusCode.Created);

                return deserializer.Deserialize<Comment>(response);
            }
            catch (Exception ex)
            {
                Trace.TraceError("CreateComment(issue, comment) error: {0}", ex);
                throw new JiraClientException("Could not create comment", ex);
            }
        }

        public void DeleteComment(IssueRef issue, Comment comment)
        {
            try
            {
                var path = String.Format("issue/{0}/comment/{1}", issue.id, comment.id);
                var request = CreateRequest(Method.DELETE, path);

                var response = client.Execute(request);
                AssertStatus(response, HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                Trace.TraceError("DeleteComment(issue, comment) error: {0}", ex);
                throw new JiraClientException("Could not delete comment", ex);
            }
        }


        public IEnumerable<Attachment> GetAttachments(IssueRef issue)
        {
            return LoadIssue(issue).fields.attachment;
        }

        public Attachment CreateAttachment(IssueRef issue, Stream fileStream, String fileName)
        {
            try
            {
                var path = String.Format("issue/{0}/attachments", issue.id);
                var request = CreateRequest(Method.POST, path);
                request.AddHeader("X-Atlassian-Token", "nocheck");
                request.AddHeader("ContentType", "multipart/form-data");
                request.AddFile("file", stream => fileStream.CopyTo(stream), fileName);

                var response = client.Execute(request);
                AssertStatus(response, HttpStatusCode.OK);

                return deserializer.Deserialize<List<Attachment>>(response).Single();
            }
            catch (Exception ex)
            {
                Trace.TraceError("CreateAttachment(issue, fileStream, fileName) error: {0}", ex);
                throw new JiraClientException("Could not create attachment", ex);
            }
        }

        public void DeleteAttachment(Attachment attachment)
        {
            try
            {
                var path = String.Format("attachment/{0}", attachment.id);
                var request = CreateRequest(Method.DELETE, path);

                var response = client.Execute(request);
                AssertStatus(response, HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                Trace.TraceError("DeleteAttachment(attachment) error: {0}", ex);
                throw new JiraClientException("Could not delete attachment", ex);
            }
        }


        public IEnumerable<IssueLink> GetIssueLinks(IssueRef issue)
        {
            return LoadIssue(issue).fields.issuelinks;
        }

        public IssueLink LoadIssueLink(IssueRef parent, IssueRef child, String relationship)
        {
            try
            {
                var issue = LoadIssue(parent);
                var links = issue.fields.issuelinks
                    .Where(l => l.type.name == relationship)
                    .Where(l => l.inwardIssue.id == parent.id)
                    .Where(l => l.outwardIssue.id == child.id)
                    .ToArray();

                if (links.Length > 1)
                    throw new JiraClientException("Ambiguous issue link");
                return links.SingleOrDefault();
            }
            catch (Exception ex)
            {
                Trace.TraceError("LoadIssueLink(parent, child, relationship) error: {0}", ex);
                throw new JiraClientException("Could not load issue link", ex);
            }
        }

        public IssueLink CreateIssueLink(IssueRef parent, IssueRef child, String relationship)
        {
            try
            {
                var request = CreateRequest(Method.POST, "issueLink");
                request.AddHeader("ContentType", "application/json");
                request.AddBody(new
                {
                    type = new { name = relationship },
                    inwardIssue = new { id = parent.id },
                    outwardIssue = new { id = child.id }
                });

                var response = client.Execute(request);
                AssertStatus(response, HttpStatusCode.Created);

                return LoadIssueLink(parent, child, relationship);
            }
            catch (Exception ex)
            {
                Trace.TraceError("CreateIssueLink(parent, child, relationship) error: {0}", ex);
                throw new JiraClientException("Could not link issues", ex);
            }
        }

        public void CreateIssueRemoteLink(Issue issue, string url, string title = "")
        {
            try
            {
                var path = string.Format("issue/{0}/remotelink", issue.id);
                var request = CreateRequest(Method.POST, path);
                request.AddHeader("ContentType", "application/json");
                request.AddBody(new
                    {
                        @object = new
                            {
                                url = url, title = !string.IsNullOrEmpty(title) ? title : url
                            }
                    });

                var response = client.Execute(request);
                AssertStatus(response, HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                Trace.TraceError("CreateIssueRemoteLink(issue, url, title) error: {0}", ex);
                throw new JiraClientException("Could not create external link for issue", ex);
            }
        }

        public void DeleteIssueLink(IssueLink link)
        {
            try
            {
                var path = String.Format("issueLink/{0}", link.id);
                var request = CreateRequest(Method.DELETE, path);

                var response = client.Execute(request);
                AssertStatus(response, HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                Trace.TraceError("DeleteIssueLink(link) error: {0}", ex);
                throw new JiraClientException("Could not delete issue link", ex);
            }
        }
    }
}
