[CmdletBinding()]
param (
    [Parameter(Mandatory=$true, ParameterSetName='TrimTrailingWhitespace')]
    [switch]$TrimTrailingWhitespace,

    [Parameter(Mandatory=$true, ParameterSetName='InsertFinalNewline')]
    [switch]$InsertFinalNewline,

    [Parameter(Mandatory=$true, ParameterSetName='ShowGlob')]
    [switch]$ShowGlob
)

$ErrorActionPreference = 'Stop'

$exts =
    git ls-files --eol |            # get versioned file paths with line endings
    ? { $_ -notmatch '/-text\b' } | # exclude binary files
    % { ($_ -split '\t', 2)[1] } |  # get file path
    Split-Path -Extension |         # get file extension
    ? { $_.Length -gt 1 } |         # exclude those without an extension
    Sort-Object |                   # sort alphabetically
    Select-Object -Unique |         # remove duplicates
    % { $_.Substring(1) }           # remove leading dot

$glob = "**/*.{$($exts -join ',')}"

if ($PSCmdlet.ParameterSetName -eq 'ShowGlob') {
    Write-Output $glob
    return
}

if (-not (Get-Command eclint -ErrorAction SilentlyContinue)) {
    throw 'ECLint is not installed. To install, run: npm install -g eclint'
}

$rule = switch ($PSCmdlet.ParameterSetName) {
    'TrimTrailingWhitespace' { '--trim_trailing_whitespace' }
    'InsertFinalNewline' { '--insert_final_newline' }
}

Write-Verbose "eclint check $rule $glob"

# https://github.com/jednano/eclint
eclint check $rule $glob

if ($LASTEXITCODE) {
    throw "eclint terminated with a non-zero exit code ($LASTEXITCODE)."
}
