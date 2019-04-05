Clear-Host
Import-Module -Name "C:\Users\Lance\Desktop\FILES\GitHub\EdiTools\EdiTools.psm1" -Force -Verbose

Get-EdiFile -InputObject 'C:\Users\Lance\Desktop\FILES\GitHub\EdiTools\Sample Files\Sample1.edi' -OutVariable ovFile |
    Get-EdiTransactionSet |Get-Edi835 -OutVariable ov835


# Test #1: wrapped file, no match info
Get-EdiFile -InputObject 'C:\Users\Lance\Desktop\FILES\GitHub\EdiTools\Sample Files\Sample1.edi' -OutVariable ovFile |
    Get-EdiTransactionSet -OutVariable ovTranSet |Measure-Object

# Test #2: unwrapped file, no match info
Get-EdiFile -InputObject 'C:\Users\Lance\Desktop\FILES\GitHub\EdiTools\Sample Files\Sample1-unwrapped.edi' -OutVariable ovFile |
    Get-EdiTransactionSet -OutVariable ovTranSet |Measure-Object

# Test #3: wrapped file, match info
Select-String -Path 'C:\Users\Lance\Desktop\FILES\GitHub\EdiTools\Sample Files\Sample1.edi' -Pattern "MICKEY" -AllMatches -OutVariable ovMatchInfo |
    Get-EdiFile -OutVariable ovFile |
    Get-EdiTransactionSet -FilteredOutput -OutVariable ovTranSet |Measure-Object

# Test #4: unwrapped file, match info
Select-String -Path 'C:\Users\Lance\Desktop\FILES\GitHub\EdiTools\Sample Files\Sample1.edi' -Pattern "MICKEY" -AllMatches -OutVariable ovMatchInfo |
    Get-EdiFile -OutVariable ovFile |
    Get-EdiTransactionSet -FilteredOutput -OutVariable ovTranSet |Measure-Object

# big file for perf test
# no filter
Measure-Command -Expression {
Select-String -Path "C:\Users\Lance\Desktop\WORKING FOLDER\EDI\835_1905_UCAREMN.txt" -Pattern "COURNEYA\*DANIEL" -AllMatches -OutVariable ovMatchInfo |
     Get-EdiFile -OutVariable ovFile |
     Get-EdiTransactionSet -OutVariable ovTranSet
}

# filter
Measure-Command -Expression {
    Select-String -Path "C:\Users\Lance\Desktop\WORKING FOLDER\EDI\835_1905_UCAREMN.txt" -Pattern "COURNEYA\*DANIEL" -AllMatches -OutVariable ovMatchInfo |
         Get-EdiFile -OutVariable ovFile |
         Get-EdiTransactionSet -FilteredOutput -OutVariable ovTranSet
}

# No file
#Get-EdiFile -InputObject 'C:\Users\Lance\Desktop\FILES\GitHub\EdiTools\Sample Files\Sample1.edi' -Verbose -OutVariable ov
#Select-String -Path 'C:\Users\Lance\Desktop\FILES\GitHub\EdiTools\Sample Files\Sample1.edi' -Pattern "MOUSE" -AllMatches -OutVariable ovMatchInfo |
    # Get-EdiFile -InputObject 'C:\Users\Lance\Desktop\FILES\GitHub\EdiTools\Sample Files\Sample1.edi' -OutVariable ovFile |
    # Get-EdiTransactionSet -OutVariable ovTranSet


# # One file
# Get-EdiFile -InputObject '<your path here>\EdiTools\Sample Files\Sample1.edi' |
#     Get-Member

# # Array of file names
# Get-EdiFile -InputObject '<your path here>\EdiTools\Sample Files\Sample1.edi', '<your path here>\EdiTools\Sample Files\Sample2.edi'

# # Select file content
# Get-EdiFile -InputObject '<your path here>\EdiTools\Sample Files\Sample1.edi' |
#     Select-Object -ExpandProperty Lines

# # Pipe from Get-ChildItem
# Get-ChildItem -Path '<your path here>\EdiTools\Sample Files\*.edi' |
#     Get-EdiFile

# # Pipe from Select-String
# Get-ChildItem -Path '<your path here>\EdiTools\Sample Files\*.edi' |
#     Select-String -Pattern 'NM1\*QC\*1\*MOUSE\*MINNIE' |
#     Get-EdiFile

# # Extract transaction sets
# Get-ChildItem -Path '<your path here>\EdiTools\Sample Files\*.edi' |
#     Select-String -Pattern 'NM1\*QC\*1\*MOUSE\*MINNIE' |
#     Get-EdiFile |
#     Get-EdiTransactionSet

# # Filter on TransactionSet properties
# Get-ChildItem -Path '<your path here>\EdiTools\Sample Files\*.edi' |
#     Select-String -Pattern 'NM1\*QC\*1\*MOUSE\*MINNIE' |
#     Get-EdiFile |
#     Get-EdiTransactionSet |
#     Where-Object {$_.ST02 -eq '112299'}

# # Add 835-specific properties and filter
# Get-ChildItem -Path '<your path here>\EdiTools\Sample Files\*.edi' |
#     Select-String -Pattern 'NM1\*QC\*1\*MOUSE\*MINNIE' |
#     Get-EdiFile |
#     Get-EdiTransactionSet |
#     Get-Edi835 |
#     Where-Object {$_.TransactionNumber -eq '051036622050010'}
