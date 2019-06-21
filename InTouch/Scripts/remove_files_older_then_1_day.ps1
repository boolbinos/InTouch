param([String]$folderPath)

echo $folderPath

$limit = (Get-Date).AddDays(-1)

if([IO.Directory]::Exists($folderPath))
{
    echo $limit

   Get-ChildItem $folderPath -Recurse | ? {
      -not $_.PSIsContainer -and $_.CreationTime -lt $limit
    } | Remove-Item
}
else
{
    echo 'folder doesn`t exist';
}