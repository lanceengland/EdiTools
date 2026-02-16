Add-Type -Path "$PSScriptRoot\EdiTools.dll"

function Get-EdiFile {
<#
    .SYNOPSIS
        Reads Edi X12 files, and promotes Edi interchange and file properties.
    .DESCRIPTION
        Reads Edi X12 files, and promotes Edi interchange and file properties.
        The EdiFile object gives access to the underlying file contents, but it best used by piping it to Get-EdiTransactionSet
    .PARAMETER Path
        A string, or array of strings representing paths to files to be parsed
    .PARAMETER FileInfo
        A System.IO.FileInfo object. The parameter is best used when piped from Get-ChildItem
    .PARAMETER MatchInfo
        A Microsoft.PowerShell.Commands.MatchInfo object. This parameter is best used when piped from Select-String
    .PARAMETER NoLogo
        Used with the -Verbose flag, does not write the EdiTools logo to the verbose stream
    .NOTES
        Author: Lance England
        Blog: http://lance-england.com
    .EXAMPLE
        Get-EdiFile -Path 'c:\foo.txt'

        Simple version, a single file name parameter
    .EXAMPLE
        Get-EdiFile -Path 'c:\foo.txt', 'c:\bar.txt'

        An array of file names
    .EXAMPLE
        Get-ChildItem -Path 'c:\Sample Files\*.txt' |Get-EdiFile

        Pipe in output from Get-ChildItem
    .EXAMPLE
        Select-String -Path 'c:\Sample Files\*.txt' -Pattern 'NM1\*QC\*1\*MOUSE\*MINNIE' |Get-EdiFile

        Pipe in input from Select-String
#>
    [CmdletBinding(DefaultParameterSetName = 'Path')]
    [OutputType([EdiTools.EdiFile])]
    param
    (
        [Parameter(ParameterSetName = 'Path',
                   Position = 0,
                   Mandatory = $true)]
        [string[]] $Path,

        [Parameter(ParameterSetName = 'FileInfo',
                   Position = 0,
                   Mandatory = $true,
                   ValueFromPipeline = $true)]
        [System.IO.FileInfo] $FileInfo,

        [Parameter(ParameterSetName = 'MatchInfo',
                   Position = 0,
                   Mandatory = $true,
                   ValueFromPipeline = $true)]
        [Microsoft.PowerShell.Commands.MatchInfo] $MatchInfo,

        [Switch] $NoLogo
    )

        Begin {
            # Maintain a string dictionary to not parse the same file multiple times from Select-String input
            $fileList = [System.Collections.Specialized.StringDictionary]::new()

            if (-not $NoLogo) {
                Write-Verbose @"

_______   _ _ ________         __
|  ____|  | (_)__   __|        | |
| |__   __| |_   | | ___   ___ | |___
|  __| / _`` | |  | |/ _ \ / _ \| / __|
| |___| (_| | |  | | (_) | (_) | \__ \
|______\__,_|_|  |_|\___/ \___/|_|___/

https://github.com/lanceengland/EdiTools


"@
            }
        }

        Process {

            [string[]] $Files = $null

            $chosenParameterSet = $PSCmdlet.ParameterSetName
            Switch ($chosenParameterSet)
            {
                'Path'      { $Files = $Path }
                'FileInfo'  { $Files = @($FileInfo.FullName) }
                'MatchInfo' { $Files = @($MatchInfo.Path) }
            }

            foreach($file in $Files) {
                if (-not [System.IO.File]::Exists($file)) {
                    Write-Error "Path not valid: $file"
                    continue
                }

                if ($fileList.ContainsKey($file)) {
                    Write-Verbose "[Get-EdiFile] Skipping $file. Reason: Already parsed"
                    continue
                }

                Write-Verbose "[Get-EdiFile] Parsing $file"
                $fileList.Add($file, $null)
                $ediFile = [EdiTools.EdiFile]::new($file)
                Write-Output $ediFile
            }
        }
}

function Get-EdiTransactionSet {
<#
    .SYNOPSIS
        Returns the individual transaction sets (ST/SE) from an EdiFile object and promotes additional properties
    .DESCRIPTION
        Returns the individual transaction sets (ST/SE) from an EdiFile object and promotes additional properties
    .PARAMETER EdiFile
        An EdiFile object returned from the Get-EdiFile cmdlet
    .NOTES
        Author: Lance England
        Blog: http://lance-england.com
    .EXAMPLE
        Get-EdiFile -Path 'c:\foo.txt' |Get-EdiTransactionSet

        Extract all transaction sets
    .EXAMPLE
        Get-EdiFile -Path 'c:\foo.txt' |Get-EdiTransactionSet |Where-Object {$_.ST02 -eq '112299'}

        Extract all transaction sets and filter on ST02 property
#>
    [CmdletBinding()]
    [OutputType([EdiTools.TransactionSet])]
    Param (
        [parameter(ValueFromPipeline = $true, Mandatory = $true)][EdiTools.EdiFile] $EdiFile
    )

    Process {
        foreach($fg in $EdiFile.FunctionalGroups) {
            foreach($ts in $fg.TransactionSets) {
                Write-Verbose "[Get-EdiTransactionSet] Parsing control number $($ts.ControlNumber)"
                Write-Output $ts
            }
        }
    }
}

function Get-Edi835 {
<#
    .SYNOPSIS
        Accepts a TransactionSet object from Get-EdiTransactionSet and promotes 835-specific properties
    .DESCRIPTION
        Accepts a TransactionSet object from Get-EdiTransactionSet and promotes 835-specific properties
    .PARAMETER TransactionSet
        A TransactionSet returned from the Get-EdiTransactionSet cmdlet
    .NOTES
        Author: Lance England
        Blog: http://lance-england.com
    .EXAMPLE
        Get-EdiFile -Path 'c:\foo.txt' |Get-EdiTransactionSet |Get-Edi835

        Promotes 835-specific properties

    .EXAMPLE
        Get-EdiFile -Path 'c:\foo.txt' |Get-EdiTransactionSet |
            Where-Object {$_.ST02 -eq '112299'} |
            Get-Edi835 |
            Where-Object {$_.TransactionNumber -eq '051036622050010'}

        Filters on 835-specific properties, TRN02 in this example.
#>
    [CmdletBinding()]
    [OutputType([EdiTools.Edi835.TransactionSet])]
    Param (
        [parameter(ValueFromPipeline = $true, Mandatory = $true)][EdiTools.TransactionSet] $TransactionSet
    )

    Process {
        if ($TransactionSet.ID -ne '835') {
            Write-Error "Transaction set $($TransactionSet.ControlNumber) is type $($TransactionSet.ID), not 835. Skipping."
            return
        }

        $edi835 = [EdiTools.Edi835.TransactionSet]::new($TransactionSet)
        Write-Output $edi835
    }
}

function Get-Edi837 {
<#
    .SYNOPSIS
        Accepts a TransactionSet object from Get-EdiTransactionSet and promotes 837-specific properties
    .DESCRIPTION
        Accepts a TransactionSet object from Get-EdiTransactionSet and promotes 837-specific properties
    .PARAMETER TransactionSet
        A TransactionSet returned from the Get-EdiTransactionSet cmdlet
    .NOTES
        Author: Lance England
        Blog: http://lance-england.com
    .EXAMPLE
        Get-EdiFile -Path 'c:\foo.txt' |Get-EdiTransactionSet |Get-Edi837

        Promotes 837-specific properties

    .EXAMPLE
        Get-EdiFile -Path 'c:\foo.txt' |Get-EdiTransactionSet |
            Where-Object {$_.ST02 -eq '112299'} |
            Get-Edi837 |
            Where-Object {$_.VersionShortId -eq 'P'}

        Filters on 837-specific properties, VersionShortId in this example.
#>
    [CmdletBinding()]
    [OutputType([EdiTools.Edi837.TransactionSet])]
    Param (
        [parameter(ValueFromPipeline = $true, Mandatory = $true)][EdiTools.TransactionSet] $TransactionSet
    )

    Process {
        if ($TransactionSet.ID -ne '837') {
            Write-Error "Transaction set $($TransactionSet.ControlNumber) is type $($TransactionSet.ID), not 837. Skipping."
            return
        }

        $edi837 = [EdiTools.Edi837.TransactionSet]::new($TransactionSet)
        Write-Output $edi837
    }
}

function Get-Edi837Hierarchy {
<#
    .SYNOPSIS
        Accepts an 837 TransactionSet object and builds a hierarchy of Billing Provider, Subscriber, Patient, Claim
    .DESCRIPTION
        Accepts an 837 TransactionSet object and builds a hierarchy of Billing Provider, Subscriber, Patient, Claim
    .PARAMETER TransactionSet
        A TransactionSet returned from the Get-EdiTransactionSet cmdlet
    .NOTES
        Author: Lance England
        Blog: http://lance-england.com
    .EXAMPLE
        Get-EdiFile -Path 'c:\foo.txt' |Get-EdiTransactionSet |Get-Edi837Hierarchy

#>
    [CmdletBinding()]
    [OutputType([EdiTools.Edi837.DocumentHierarchy])]
    Param (
        [parameter(ValueFromPipeline = $true, Mandatory = $true)][EdiTools.TransactionSet] $TransactionSet
    )

    Process {
        $edi837hierarchy = [EdiTools.Edi837.DocumentHierarchy]::new($TransactionSet.Segments)
        Write-Output $edi837hierarchy
    }
}

# Defines which function to make public
Export-ModuleMember -Function Get-EdiFile, Get-EdiTransactionSet, Get-Edi835, Get-Edi837, Get-Edi837Hierarchy
