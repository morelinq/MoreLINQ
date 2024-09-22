[CmdletBinding(PositionalBinding=$false)]
param ()

Set-StrictMode -Version 2.0
$ErrorActionPreference = 'Stop'

function MarkShipped([string]$dir)
{
    $shippedFilePath = Join-Path $dir 'PublicAPI.Shipped.txt'
    [array]$shipped = Get-Content $shippedFilePath

    $unshippedFilePath = Join-Path $dir 'PublicAPI.Unshipped.txt'
    [array]$unshipped = Get-Content $unshippedFilePath | ? { $_ -and $_ -notmatch '^#' }

    $removed = @()
    $removedPrefix = '*REMOVED*';
    Write-Verbose "Processing $dir"

    foreach ($item in $unshipped)
    {
        if ($item.StartsWith($removedPrefix))
        {
            $item = $item.Substring($removedPrefix.Length)
            $removed += $item
        }
        else
        {
            $shipped += $item
        }
    }

    $shipped |
        Sort-Object |
        ? { -not $removed.Contains($_) } |
        Out-File $shippedFilePath -Encoding Ascii

    '#nullable enable' | Out-File $unshippedFilePath -Encoding Ascii
}

foreach ($file in Get-ChildItem -re -in 'PublicApi.Shipped.txt')
{
    $dir = Split-Path -parent $file
    MarkShipped $dir
}
