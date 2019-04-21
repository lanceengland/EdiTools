Clear-Host

$ModuleBase = Split-Path -Path $PSScriptRoot -Parent
Remove-Module EdiTools -ErrorAction Ignore
Import-Module "$ModuleBase\EdiTools.psm1"

Invoke-Pester -Path "$ModuleBase\Tests\"
