Clear-Host

Import-Module -Name "C:\Users\Lance\Desktop\FILES\GitHub\EdiTools\EdiTools.psm1" -Force -Verbose

Get-EdiFile -InputObject 'C:\Users\Lance\Desktop\FILES\GitHub\EdiTools\Sample Files\Sample1-unwrapped.edi' -OutVariable ovFile

$stMatchInfo = Select-String -InputObject $ovFile.Body -Pattern ($ovFile.SegmentDelimiter + '\r?\n?ST\*') -AllMatches

$stMatchInfo2 = Select-String -InputObject $ovFile.Body -Pattern ($ovFile.SegmentDelimiter + 'ST\*') -AllMatches


# calculate the length of new line char(s), if any. Could be 0, 1, or 2
$newlineLength = 0
if ($InputObject.HasCarriageReturn) {
    $newlineLength += 1
}
if ($InputObject.HasNewLine) {
    $newlineLength += 1
}
