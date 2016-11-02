param(
	[string]$SolutionDir,
	[string]$SolutionName,
	[string]$ProjectDir,
	[string]$ProjectName,
	[string]$OutDir,
	[string]$TargetName
)

$ErrorActionPreference = "Stop"

& "${SolutionDir}${OutDir}gwconfig.exe" merge $SolutionDir$SolutionName.sln $ProjectName

cd "${ProjectDir}..\..\build\Jhu.SkyQuery.Parser.Generator\${OutDir}"
.\sqpgen.exe generate -o ..\..\..\..\dll\Jhu.SkyQuery.Parser\Parser\SkyQueryParser.cs

exit $LASTEXITCODE