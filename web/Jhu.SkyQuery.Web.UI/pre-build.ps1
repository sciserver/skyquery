param(
	[string]$SolutionDir,
	[string]$SolutionName,
	[string]$ProjectDir,
	[string]$ProjectName,
	[string]$OutDir,
	[string]$ConfigurationName,
	[string]$TargetName
)

$ErrorActionPreference='Stop'

. ${SolutionDir}graywulf/web/web-build.ps1

& "${SolutionDir}${OutDir}${ConfigurationName}\gwconfig.exe" merge $SolutionDir$SolutionName.sln $ProjectName

Add-Master Basic
Add-Theme Basic

Add-App Api
Add-App Common
Add-App Docs
Add-App Jobs
Add-App MyDB
Add-App Query
Add-App Schema

Add-Script "Bootstrap.Nuget.3.3.6" "Bootstrap"
Add-Script "CodeMirror.Nuget.5.19.0" "CodeMirror"
Add-Script "JQuery.Nuget.2.2.4" "jQuery"
Add-Script "JQuery-Validation.Nuget.1.15.1" "jQuery-Validation"
Add-Script "JQuery-Validation-Unobtrusive.Nuget.5.2.3" "jQuery-Validation-Unobtrusive"
Add-Script "SyntaxHighlighter.Nuget.3.0.83.01" "SyntaxHighlighter"