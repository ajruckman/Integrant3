Get-ChildItem -Recurse -Filter *.css     | Remove-Item
Get-ChildItem -Recurse -Filter *.css.map | Remove-Item

foreach ($stylesheet in $( Get-ChildItem -Recurse . -Filter *.scss ))
{
    Write-Output "Building stylesheet: $( $stylesheet.FullName )"
    & sassc -m "$( $stylesheet.FullName )" "$( $stylesheet.DirectoryName )\$( $stylesheet.BaseName ).css"
    if (-not$?)
    {
        exit 2
    }
}
