param(
	[string]$SolutionDir,
	[string]$SolutionName,
	[string]$ProjectDir,
	[string]$ProjectName,
	[string]$OutDir,
	[string]$TargetName
)

& "${SolutionDir}${OutDir}gwconfig.exe" merge $SolutionDir$SolutionName.sln $ProjectName
