function Get-EdiFile {
<#
    .SYNOPSIS
        Reads Edi X12 files, promotes Edi and file properties, and exposes the file content as an array of string(s).
    .DESCRIPTION
        Reads Edi X12 files, promotes Edi and file properties, and exposes the file content as an array of string(s).
    .PARAMETER InputObject
        Can be a path, an array of paths, System.IO.FileInfo object, or Microsoft.PowerShell.Commands.MatchInfo object 
    .NOTES
        Author: Lance England
        Blog: http://lance-england.com
    .EXAMPLE
        Get-EdiFile -InputObject 'c:\foo.txt' -Verbose

        Simple version, a single file name parameter
    .EXAMPLE
        Get-EdiFile -InputObject '<your path here>\EdiTools\Sample Files\Sample1.txt', '<your path here>\EdiTools\Sample Files\Sample2.txt'

        An array of file names
    .EXAMPLE
        Get-ChildItem -Path '<your path here>\EdiTools\Sample Files\*.txt' |Get-EdiFile

        Pipe in output from Get-ChildItem
    .EXAMPLE
        Get-ChildItem -Path '<your path here>\EdiTools\Sample Files\*.txt' |Select-String -Pattern 'NM1\*QC\*1\*MOUSE\*MINNIE' |Get-EdiFile
        
        Pipe in input from Select-String
#>
    [cmdletbinding()]
        Param (    
            [parameter(ValueFromPipeline = $true, Mandatory = $true)] [System.Object] $InputObject
        )

        Begin {
            # Maintain a string dictionary so we don't waste time processing the same file multiple times (mainly from Select-String input)
            $fileList = New-Object System.Collections.Specialized.StringDictionary
            if ($InputObject -is [System.Object[]]) {
                $InputObject | Get-EdiFile 
            }
        }
    
        Process {
            if ($InputObject -is [System.Object[]]) {return}
            
            [string] $fileName = $null
            if ($InputObject -is [System.String]) { 
                $fileName = $InputObject 
            }
            if ($InputObject -is [System.IO.FileInfo]) { 
                $fileName = $InputObject.FullName 
            }
            if ($InputObject -is [Microsoft.PowerShell.Commands.MatchInfo]) {
                $fileName = $InputObject.Path
            }
            
            if (-not [System.IO.File]::Exists($fileName)) { 
                Write-Error "File $fileName does not exist"    
                return 
            }

            if ($fileList.ContainsKey($fileName)) {
                Write-Verbose "$fileName has already been processed"
                return
            }

            $fileList.Add($fileName, $null)

            $fileContents = [System.IO.File]::ReadAllText($fileName)
            <#
                Character 104 is the element delimiter
                Character 105 is the sub-element delimiter 
                Character 106 is the segment delimiter
            #>
            $elementDelimiter = $fileContents[103]
            $componentDelimiter = $fileContents[104]
            $segmentDelimiter = $fileContents[105]
            [bool] $isUnwrapped = ($fileContents[106] -eq "`r" -or $fileContents[106] -eq "`n")
            # TODO: Check if EDI X12 file

            $outputObject = New-Object –TypeName PSObject
            # Basic file properties
            $outputObject | Add-Member –MemberType NoteProperty –Name Name –Value ([System.Io.Path]::GetFileName($fileName)) |Out-Null
            $outputObject | Add-Member –MemberType NoteProperty –Name DirectoryName –Value ([System.Io.Path]::GetDirectoryName($fileName)) |Out-Null
            $outputObject | Add-Member –MemberType NoteProperty –Name Body –Value $fileContents |Out-Null
            
            #EDI parsing properties
            $outputObject | Add-Member –MemberType NoteProperty –Name ElementDelimiter –Value $elementDelimiter |Out-Null
            $outputObject | Add-Member –MemberType NoteProperty –Name ComponentDelimiter –Value $componentDelimiter |Out-Null
            $outputObject | Add-Member –MemberType NoteProperty –Name SegmentDelimiter –Value $segmentDelimiter |Out-Null
            $outputObject | Add-Member –MemberType NoteProperty –Name IsUnwrapped –Value $isUnwrapped |Out-Null
            
            # ISA values
            $isaSegments = $fileContents.Substring(0, 105).Split($elementDelimiter)
            $outputObject | Add-Member –MemberType NoteProperty –Name ISA01 –Value $isaSegments[1] |Out-Null
            $outputObject | Add-Member –MemberType NoteProperty –Name ISA02 –Value $isaSegments[2] |Out-Null
            $outputObject | Add-Member –MemberType NoteProperty –Name ISA03 –Value $isaSegments[3] |Out-Null
            $outputObject | Add-Member –MemberType NoteProperty –Name ISA04 –Value $isaSegments[4] |Out-Null
            $outputObject | Add-Member –MemberType NoteProperty –Name ISA05 –Value $isaSegments[5] |Out-Null
            $outputObject | Add-Member –MemberType NoteProperty –Name ISA06 –Value $isaSegments[6] |Out-Null
            $outputObject | Add-Member –MemberType NoteProperty –Name ISA07 –Value $isaSegments[7] |Out-Null
            $outputObject | Add-Member –MemberType NoteProperty –Name ISA08 –Value $isaSegments[8] |Out-Null
            $outputObject | Add-Member –MemberType NoteProperty –Name ISA09 –Value $isaSegments[9] |Out-Null
            $outputObject | Add-Member –MemberType NoteProperty –Name ISA10 –Value $isaSegments[10] |Out-Null
            $outputObject | Add-Member –MemberType NoteProperty –Name ISA11 –Value $isaSegments[11] |Out-Null
            $outputObject | Add-Member –MemberType NoteProperty –Name ISA12 –Value $isaSegments[12] |Out-Null
            $outputObject | Add-Member –MemberType NoteProperty –Name ISA13 –Value $isaSegments[13] |Out-Null
            $outputObject | Add-Member –MemberType NoteProperty –Name ISA14 –Value $isaSegments[14] |Out-Null
            $outputObject | Add-Member –MemberType NoteProperty –Name ISA15 –Value $isaSegments[15] |Out-Null
            
            # Convenience properties
            $outputObject | Add-Member -MemberType AliasProperty -Name InterchangeControlNumber -Value ISA13 |Out-Null
            $interchangeDate = [DateTime]::ParseExact($isaSegments[9], 'yymmdd', [DateTime].CultureInfo.InvariantCulture)
            $outputObject | Add-Member -MemberType NoteProperty -Name InterchangeDate -Value $interchangeDate |Out-Null
            $outputObject | Add-Member -MemberType AliasProperty -Name ReceiverQualifier -Value ISA07 |Out-Null
            $outputObject | Add-Member -MemberType NoteProperty -Name ReceiverId -Value $isaSegments[8].TrimEnd() |Out-Null
            $outputObject | Add-Member -MemberType AliasProperty -Name SenderQualifier -Value ISA05 |Out-Null
            $outputObject | Add-Member -MemberType NoteProperty -Name SenderId -Value $isaSegments[6].TrimEnd() |Out-Null
            $outputObject | Add-Member -MemberType ScriptProperty -Name Lines -Value { $this.Body -split $this.SegmentDelimiter + "\r?\n?" }

            # Pass-thru the Select-String pattern (if applicable)
            if ($InputObject -is [Microsoft.PowerShell.Commands.MatchInfo]) {
                $outputObject | Add-Member -MemberType NoteProperty -Name SearchPattern -Value $InputObject.Pattern |Out-Null
            }

            Write-Output $outputObject
        }
    
        End {}
}
function Get-EdiTransactionSet {
<#
    .SYNOPSIS
        Splits Edi file contents into individual transaction sets (ST/SE) and promotes additional properties
    .DESCRIPTION
        Splits Edi file contents into individual transaction sets (ST/SE) and promotes additional properties
    .PARAMETER InputObject
        A PSObject returned from the Get-EdiFile cmdlet       
    .PARAMETER SkipInputProperties
        A flag to not copy input properties onto the output PSObject. Default is false.
    .NOTES
        Author: Lance England
        Blog: http://lance-england.com
    .EXAMPLE
        Get-EdiFile -InputObject 'c:\foo.txt' |Get-EdiTransactionSet

        Extract all transaction sets

    .EXAMPLE
        Get-EdiFile -InputObject 'c:\foo.txt' |Get-EdiTransactionSet |Where-Object {$_.ST02 -eq '112299'}

        Extract all transaction sets and filter on ST02 property          
#>
[cmdletbinding()]
    Param (
        [parameter(ValueFromPipeline = $true, Mandatory = $true)][PSObject] $InputObject,
        [parameter()][switch] $SkipInputProperties = $false
    )
    Begin {}

    Process {
        Write-Verbose "Processing $($InputObject.Name)"

        <#
            New logic:
            if SearchPattern then
            filtered output
            else
            all output
        #>

        [System.Collections.ArrayList] $segments = $null
        [bool]$inTransactionSet = $false

        foreach($line in $InputObject.Lines) {
            if ($line.StartsWith("ST*835*")) {
                $segments = New-Object System.Collections.ArrayList
                $inTransactionSet = $true
            }
    
            if ($inTransactionSet) {
                $segments.Add($line) |Out-Null
            }

            if ($line.StartsWith("SE*")) {
                $transactionSet = New-Object –TypeName PSObject

                # copy input object note properties (except for Lines string array)
                if (-Not $SkipInputProperties) {
                    foreach($prop in $InputObject.PsObject.Properties) {
                        # Exclude 'Lines' property; is redunadant and unnecessary
                        if( $prop.Name -ne 'Lines') {
                            # Alias properties need to be handled differently than NoteProperties (see the value parameter)
                            switch ($prop.MemberType.value__) {
                                ([System.Management.Automation.PSMemberTypes]::AliasProperty.value__) {$transactionSet | Add-Member –MemberType $prop.MemberType –Name $prop.Name –Value $prop.ReferencedMemberName |Out-Null}  
                                ([System.Management.Automation.PSMemberTypes]::NoteProperty.value__) {$transactionSet | Add-Member –MemberType $prop.MemberType –Name $prop.Name –Value $prop.Value |Out-Null}  
                            }
                        }
                    }
                }
                # ST properties
                $st01 = $segments[0].ToString().Split($InputObject.ElementDelimiter).Get(1)
                $transactionSet | Add-Member –MemberType NoteProperty –Name ST01 -Value $st01 |Out-Null
                    
                $st02 = $segments[0].ToString().Split($InputObject.ElementDelimiter).Get(2)
                $transactionSet | Add-Member –MemberType NoteProperty –Name ST02 -Value $st02 |Out-Null

                # ST aliases
                $transactionSet | Add-Member -MemberType AliasProperty -Name TransactionSetIdentifierCode -Value ST01 |Out-Null
                $transactionSet | Add-Member -MemberType AliasProperty -Name TransactionSetControlNumber -Value ST02 |Out-Null

                $transactionSet | Add-Member –MemberType NoteProperty –Name Segments –Value $segments |Out-Null
                    
                Write-Output $transactionSet
                $segments = $null
                $inTransactionSet = $false
            }
        }
    }
    End {}  
}
function Get-Edi835 {
<#
    .SYNOPSIS
        Accepts a PSObject from Get-EdiTransactionSet and promotes 835-specifc properties
    .DESCRIPTION
        Accepts a PSObject from Get-EdiTransactionSet and promotes 835-specifc properties
    .PARAMETER InputObject
        A PSObject returned from the Get-EdiTransactionSet cmdlet       
    .NOTES
        Author: Lance England
        Blog: http://lance-england.com
    .EXAMPLE
        Get-EdiFile -InputObject 'c:\foo.txt' |Get-EdiTransactionSet |Get-Edi835

        Promotes 835-specifc properties

    .EXAMPLE
        Get-EdiFile -InputObject 'c:\foo.txt' |Get-EdiTransactionSet |Where-Object {$_.ST02 -eq '112299'} |Get-Edi835 |Where-Object {$_.TransactionNumber -eq '051036622050010'}

        Filters on 835-specifc properties, TRN02 in this example.     
#>
[cmdletbinding()]
    Param (
        [parameter(ValueFromPipeline = $true, Mandatory = $true)][PSObject] $InputObject
    )
    Begin {}

    Process {
        Write-Verbose "Processing $($InputObject.Name) ST02=$($InputObject.ST02)"

        # Add 835 specifc properties
        $bpr09 = $InputObject.Segments[1].ToString().Split($InputObject.ElementDelimiter).Get(9)
        $InputObject | Add-Member –MemberType NoteProperty –Name BPR09 -Value $bpr09 |Out-Null

        $trn02 = $InputObject.Segments[2].ToString().Split($InputObject.ElementDelimiter).Get(2)
        $InputObject | Add-Member –MemberType NoteProperty –Name TRN02 -Value $trn02 |Out-Null

        $InputObject | Add-Member -MemberType AliasProperty -Name BankAccount -Value BPR09
        $InputObject | Add-Member -MemberType AliasProperty -Name TransactionNumber -Value TRN02
        
        Write-Output $InputObject
    }
    End {}  
}

Export-ModuleMember -Function Get-EdiFile, Get-EdiTransactionSet, Get-Edi835

