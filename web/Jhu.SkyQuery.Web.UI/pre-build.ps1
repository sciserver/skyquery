rm $ProjectDir$OutDir\Microsoft.SqlServer.*.dll
rm $ProjectDir$OutDir\Microsoft.SqlServer.*.xml

$ErrorActionPreference='Stop'

. ${SolutionDir}modules/graywulf/web/web-build.ps1

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
Add-Script "Bootstrap-Lightbox.Nuget.5.1.1" "Bootstrap-Lightbox"
Add-Script "CodeMirror.Nuget.5.19.0" "CodeMirror"
Add-Script "JQuery.Nuget.2.2.4" "jQuery"
Add-Script "JQuery-Validation.Nuget.1.15.1" "jQuery-Validation"
Add-Script "JQuery-Validation-Unobtrusive.Nuget.5.2.3" "jQuery-Validation-Unobtrusive"
Add-Script "SyntaxHighlighter.Nuget.3.0.83.01" "SyntaxHighlighter"

