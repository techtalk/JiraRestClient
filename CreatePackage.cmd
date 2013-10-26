@echo off

setlocal

cd /d "%~dp0"

set solutionDir=%cd%
set config=Release
set platform=Any CPU


echo Building solution 'TechTalk.JiraRestClient.sln' (%config%^|%platform%)
%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe /nologo "/p:SolutionDir=%solutionDir%"\ "/p:Configuration=%config%" "/p:Platform=%platform%" /verbosity:minimal "TechTalk.JiraRestClient.sln"

if ERRORLEVEL 1 goto :EOF


.nuget\nuget pack

