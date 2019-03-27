Import-Module -Name "<your path here>\EdiTools\EdiTools.psm1"

# No file
Get-EdiFile -InputObject 'c:\foo.txt' -Verbose

# One file
Get-EdiFile -InputObject '<your path here>\EdiTools\Sample Files\Sample1.txt' |
    Get-Member

# Array of file names
Get-EdiFile -InputObject '<your path here>\EdiTools\Sample Files\Sample1.txt', '<your path here>\EdiTools\Sample Files\Sample2.txt'

# Select file content
Get-EdiFile -InputObject '<your path here>\EdiTools\Sample Files\Sample1.txt' |
    Select-Object -ExpandProperty Lines

# Pipe from Get-ChildItem
Get-ChildItem -Path '<your path here>\EdiTools\Sample Files\*.txt' |
    Get-EdiFile

# Pipe from Select-String
Get-ChildItem -Path '<your path here>\EdiTools\Sample Files\*.txt' |
    Select-String -Pattern 'NM1\*QC\*1\*MOUSE\*MINNIE' |
    Get-EdiFile

# Extract transaction sets
Get-ChildItem -Path '<your path here>\EdiTools\Sample Files\*.txt' |
    Select-String -Pattern 'NM1\*QC\*1\*MOUSE\*MINNIE' |
    Get-EdiFile |
    Get-EdiTransactionSet

# Filter on TransactionSet properties
Get-ChildItem -Path '<your path here>\EdiTools\Sample Files\*.txt' |
    Select-String -Pattern 'NM1\*QC\*1\*MOUSE\*MINNIE' |
    Get-EdiFile |
    Get-EdiTransactionSet |
    Where-Object {$_.ST02 -eq '112299'}

# Add 835-specific properties and filter
Get-ChildItem -Path '<your path here>\EdiTools\Sample Files\*.txt' |
    Select-String -Pattern 'NM1\*QC\*1\*MOUSE\*MINNIE' |
    Get-EdiFile |
    Get-EdiTransactionSet |
    Get-Edi835 |
    Where-Object {$_.TransactionNumber -eq '051036622050010'}
