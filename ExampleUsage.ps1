Clear-Host
Import-Module -Name "C:\Users\Lance\Desktop\FILES\GitHub\EdiTools\EdiTools.psm1" -Force -Verbose

# No file
Get-EdiFile -InputObject 'C:\Users\Lance\Desktop\FILES\GitHub\EdiTools\Sample Files\Sample1.edi' -Verbose -OutVariable ov
Select-String -Path 'C:\Users\Lance\Desktop\FILES\GitHub\EdiTools\Sample Files\Sample1.edi' -Pattern "MOUSE" -AllMatches -OutVariable ovMatchInfo |
    Get-EdiFile -InputObject 'C:\Users\Lance\Desktop\FILES\GitHub\EdiTools\Sample Files\Sample1.edi' -OutVariable ovFile |
    Get-EdiTransactionSet -OutVariable ovTranSet


# One file
Get-EdiFile -InputObject '<your path here>\EdiTools\Sample Files\Sample1.edi' |
    Get-Member

# Array of file names
Get-EdiFile -InputObject '<your path here>\EdiTools\Sample Files\Sample1.edi', '<your path here>\EdiTools\Sample Files\Sample2.edi'

# Select file content
Get-EdiFile -InputObject '<your path here>\EdiTools\Sample Files\Sample1.edi' |
    Select-Object -ExpandProperty Lines

# Pipe from Get-ChildItem
Get-ChildItem -Path '<your path here>\EdiTools\Sample Files\*.edi' |
    Get-EdiFile

# Pipe from Select-String
Get-ChildItem -Path '<your path here>\EdiTools\Sample Files\*.edi' |
    Select-String -Pattern 'NM1\*QC\*1\*MOUSE\*MINNIE' |
    Get-EdiFile

# Extract transaction sets
Get-ChildItem -Path '<your path here>\EdiTools\Sample Files\*.edi' |
    Select-String -Pattern 'NM1\*QC\*1\*MOUSE\*MINNIE' |
    Get-EdiFile |
    Get-EdiTransactionSet

# Filter on TransactionSet properties
Get-ChildItem -Path '<your path here>\EdiTools\Sample Files\*.edi' |
    Select-String -Pattern 'NM1\*QC\*1\*MOUSE\*MINNIE' |
    Get-EdiFile |
    Get-EdiTransactionSet |
    Where-Object {$_.ST02 -eq '112299'}

# Add 835-specific properties and filter
Get-ChildItem -Path '<your path here>\EdiTools\Sample Files\*.edi' |
    Select-String -Pattern 'NM1\*QC\*1\*MOUSE\*MINNIE' |
    Get-EdiFile |
    Get-EdiTransactionSet |
    Get-Edi835 |
    Where-Object {$_.TransactionNumber -eq '051036622050010'}
