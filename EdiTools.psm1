function Get-EdiFile {
    <#
        .SYNOPSIS
            Provides functions to read Edi X12 files, split into transaction sets, and adds properties to enable filtering
        .DESCRIPTION
            Provides functions to read Edi X12 files, split into transaction sets, and adds properties to enable filtering 
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
            [parameter(ValueFromPipeline = $true)] [System.Object] $InputObject
        )

        Begin {
            # Maintain a string dictionary so we don't waster time processing the same file multiple times (mainly from Select-String input)
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
                Write-Verbose "File $fileName does not exist"    
                return 
            }

            if ($fileList.ContainsKey($fileName)) {
                Write-Verbose "$fileName has already been processed"
                return
            }

            $fileList.Add($fileName, $null)

            $fileContents = [System.IO.File]::ReadAllLines($fileName)
            if ($fileContents[0].Substring(0, 3) -ne 'ISA') { 
                Write-Warning "Skipping $($InputObject.FullName). Not an ISA file"; 
            }
            else {
                <#  Todo: Add this to the inline doc
                    character 104 is always the element delimiter, 
                    105 is the sub-element delimiter, and 
                    106 is the segment delimiter.
                #>
                $elementDelimiter = $fileContents[0][103]
                $componentDelimiter = $fileContents[0][104]
                $segmentDelimiter = $fileContents[0][105]

                # files with new lines are read as arrays. If not, use the EDI segment delimiter to create array of lines
                [string[]] $lines = $null
                if ($fileContents.Count -eq 1) {
                    $lines = $fileContents[0].Split($segmentDelimiter)
                }
                else {
                    $lines = $fileContents
                    # if file was already unwrapped, then we need to remove the trailing sehment delimiter from each line
                    for($i=0; $i -lt $lines.Count; $i++) {
                        $lines[$i] = $lines[$i].Substring(0, $lines[$i].Length - 1) 
                    }
                }

         
                $outputObject = New-Object –TypeName PSObject
                # Basic file properties
                $outputObject | Add-Member –MemberType NoteProperty –Name Name –Value ([System.Io.Path]::GetFileName($fileName)) |Out-Null
                $outputObject | Add-Member –MemberType NoteProperty –Name DirectoryName –Value ([System.Io.Path]::GetDirectoryName($fileName)) |Out-Null
                $outputObject | Add-Member –MemberType NoteProperty –Name Lines –Value $lines |Out-Null
                
                #EDI parsing properties
                $outputObject | Add-Member –MemberType NoteProperty –Name ElementDelimiter –Value $elementDelimiter |Out-Null
                $outputObject | Add-Member –MemberType NoteProperty –Name ComponentDelimiter –Value $componentDelimiter |Out-Null

                # ISA values
                $isaSegments = $lines[0].Split($elementDelimiter)
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
                
                Write-Output $outputObject
            }
        }
    
        End {}
}

function Get-EdiTransactionSet {
[cmdletbinding()]
    Param (
        [parameter(ValueFromPipeline = $true, Mandatory = $true)][PSObject] $InputObject,
        [parameter()][switch] $DontCopyInputProperties = $false
    )
    Begin {}

    Process {
        Write-Verbose "Processing $($InputObject.Name)"

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
                if (-Not $DontCopyInputProperties) {
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
[cmdletbinding()]
    Param (
        [parameter(ValueFromPipeline = $true)][PSObject] $InputObject,
        [parameter()][switch] $DontCopyInputProperties
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