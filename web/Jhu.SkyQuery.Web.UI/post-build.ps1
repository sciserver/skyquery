# Copy plugins
$source = "$SolutionDir\plugins\$ConfigurationName\*"
$target = "$ProjectDir$OutDir"
cp $source $target