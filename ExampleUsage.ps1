Import-Module EdiTools

# Example 1
Clear-Host

Get-ChildItem -Path 'C:\Users\Lance\Desktop\WORKING FOLDER\EDI' | 
    Select-String -Pattern 'John Doe' |
    Get-EdiFile | 
    Get-EdiTransactionSet |
    Select-Object -ExpandProperty Segments


# Example 2
Get-ChildItem -Path 'C:\Users\Lance\Desktop\WORKING FOLDER\EDI' |
    Where-Object {$_.CreationTime -ge '2019/03/08' -and $_.CreationTime -lt '2019/03/09'} |
    Select-String -Pattern "TRN\*1\*12345" |
    Get-EdiFile |
    Get-EdiTransactionSet |
    Get-Edi835 |
    Where-Object { $_.TRN02 -eq '12345' } |
    Select-Object -ExpandProperty Segments
