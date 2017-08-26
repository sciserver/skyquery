$ErrorActionPreference = "Stop"

cd "${ProjectDir}..\..\build\Jhu.SkyQuery.Parser.Generator\${OutDir}"
.\sqpgen.exe generate -o ..\..\..\..\dll\Jhu.SkyQuery.Parser\Parser\SkyQueryParser.g.cs

exit $LASTEXITCODE