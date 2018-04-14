# Copy plugins
$source = "$SolutionDir\plugins\bin\$PlatformName\$ConfigurationName\*"
$target = "$ProjectDir$OutDir"
cp $source $target