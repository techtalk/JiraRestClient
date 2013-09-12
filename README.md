 JiraRestClient
================

#### A simple client for Atlassian JIRA service REST API

Available on NuGet as [TechTalk.JiraRestClient](http://nuget.org/packages/TechTalk.JiraRestClient/)

## Capabilities
1. List issues of a project
2. Create, load and delete issues
3. Update some issue fields
    - summary
    - description
    - labels
    - original estimate
4. Query and apply state transitions 
5. List, create and delete comments
6. List, create and delete attachments
7. List, create and delete issue links
8. List, create, update and delete issue remote links

--------------------------------------------------

### Version History

**1.0.0**
initial version

**1.0.1**
removed dependency on Newtonsoft.Json

**1.0.2**
fixed the bug introduced in 1.0.1

**1.0.3**
fields set to null are not updated

**1.0.4**
list issues of a given type within a project

**1.0.5**
exceptions contain more information

**1.0.6**
support for issue state transitions

**1.0.7**
support for issue remote links (attached URLs)
