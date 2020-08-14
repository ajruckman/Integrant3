function Clean-DotNETProject
{
    Get-ChildItem . -include bin, obj -Recurse | foreach ($_) {
        Write-Output $_.fullname
        Remove-Item $_.fullname -Force -Recurse
    }
}

Clean-DotNETProject
dotnet restore

###

cd ./Element/wwwroot/css;  ./_build.ps1; cd ../../..
cd ./Rudiment/wwwroot/css; ./_build.ps1; cd ../../..

###

dotnet pack -c Debug -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg

if (!(Test-Path ./_published/))
{
    md ./_published/
}

Get-ChildItem -Directory | foreach {
    if (Test-Path "$( $_.FullName )/bin")
    {
        Get-ChildItem "$( $_.FullName )/bin/" -Depth 1  | ? { ($_.FullName -like "*.nupkg") -or ($_.FullName -like "*.snupkg") } | foreach {
            Write-Output "Copying package: $( $_.Name )"
            Copy-Item $_.FullName ./_published/
        }
    }
}

Remove-Item -Force -Recurse -ErrorAction Ignore $HOME/.nuget/packages/integrant.*
