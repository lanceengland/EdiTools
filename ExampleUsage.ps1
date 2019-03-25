Import-Module EdiTools -Verbose

# Example 1
Clear-Host
Get-ChildItem |Where-Object {$_.PsIsContainer -EQ $true -and $_.Name -eq 'Sample Files'} |Select-Object -ExpandProperty FullName -OutVariable sampleFiles

Get-ChildItem -Path $sampleFiles | 
    Select-String -Pattern 'TRN\*1\*051036622050010' |
    Get-EdiFile | 
    Get-EdiTransactionSet |
    Select-Object *

# Example 2
Get-ChildItem -Path $samplesDirectory |
    Where-Object {$_.CreationTime -ge '2019/01/01' -and $_.CreationTime -lt '2019/12/31'} |
    Select-String -Pattern "TRN\*1\*051036622050010" |
    Get-EdiFile |
    Get-EdiTransactionSet |
    Get-Edi835 |
    Where-Object { $_.TRN02 -eq '051036622050010' } |
    Select-Object -ExpandProperty Segments
