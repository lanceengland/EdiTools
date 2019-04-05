Clear-Host
Import-Module -Name "C:\Users\Lance\Desktop\FILES\GitHub\EdiTools\EdiTools.psm1" -Force -Verbose
$pattern = "\r?\n?CLP*"

$mi = Select-String -Path 'C:\Users\Lance\Desktop\FILES\GitHub\EdiTools\Sample Files\Sample1.edi' -Pattern $pattern -AllMatches

$miUnwrapped = Select-String -Path 'C:\Users\Lance\Desktop\FILES\GitHub\EdiTools\Sample Files\Sample1-unwrapped.edi' -Pattern $pattern -AllMatches

$unwrappedString = Get-Content -Path 'C:\Users\Lance\Desktop\FILES\GitHub\EdiTools\Sample Files\Sample1-unwrapped.edi' -Raw

$stMiUnwrapped = Select-String -InputObject $unwrappedString -Pattern ('~' + '\r?\n?ST\*') -AllMatches


foreach($s in ('BPR', 9), ('TRN', 2)) {
    Write-Host "$($s[0]) : $($s[1])"
}
