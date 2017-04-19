
#region Class Definitions
class X12Loop {} <# base class for common loop pointer #>
class SegmentContext : X12Loop
{
    [string] $ContextName
    [string] $ContextReference

    [string] $SegmentIDCode
    [string] $SegmentPositionInTransactionSet
    [string] $LoopIdentifierCode

    [string] $ElementPositionInSegment
    [string] $ComponentDataElementPositioninComposite
    [string] $RepeatingDataElementPosition

    #[string] $DataElementReferenceNumber
    #[string] $DataElementReferenceNumber

    SegmentContext([string] $RawString)
    {
        $Elements = $RawString.Split('*')
        $this.ContextName = $Elements[1]
        $this.ContextReference = $Elements[2]

        $this.SegmentIDCode = $Elements[3]
        $this.SegmentPositionInTransactionSet = $Elements[4]
        $this.LoopIdentifierCode = $Elements[5]

        $this.ElementPositionInSegment = $Elements[6]
        $this.ComponentDataElementPositioninComposite = $Elements[7]
        $this.RepeatingDataElementPosition = $Elements[8]

        #$this.DataElementReferenceNumber = $Elements[9]
        #$this.DataElementReferenceNumber = $Elements[10]
    }
}

class ErrorIdentification : X12Loop
{
    [string] $SegmentIDCode
    [string] $SegmentPositionInTransactionSet
    [string] $LoopIdentifierCode
    [string] $SegmentSyntaxErrorCode
    [string] $SegmentSyntaxErrorDescription

    ErrorIdentification([string] $RawString)
    {
        $Elements = $RawString.Split('*')
        $this.SegmentIDCode = $Elements[1]
        $this.SegmentPositionInTransactionSet = $Elements[2]
        $this.LoopIdentifierCode = $Elements[3]
        $this.SegmentSyntaxErrorCode = $Elements[4]

        switch ($this.SegmentSyntaxErrorCode)
        {
            '1' { $this.SegmentSyntaxErrorDescription = 'Unrecognized segment ID' }
            '2' { $this.SegmentSyntaxErrorDescription = 'Unexpected segment' }
            '3' { $this.SegmentSyntaxErrorDescription = 'Required segment missing' }
            '4' { $this.SegmentSyntaxErrorDescription = 'Loop occurs over maximum times' }
            '5' { $this.SegmentSyntaxErrorDescription = 'Segment exceeds maximum use' }
            '6' { $this.SegmentSyntaxErrorDescription = 'Segment not in defined transaction set' }
            '7' { $this.SegmentSyntaxErrorDescription = 'Segment not in proper sequence' }
            '8' { $this.SegmentSyntaxErrorDescription = 'Segment has data element errors' }
            'I4' { $this.SegmentSyntaxErrorDescription = 'Implementation “Not Used” segment present' }
            'I6' { $this.SegmentSyntaxErrorDescription = 'Implementation dependent segment missing' }
            'I7' { $this.SegmentSyntaxErrorDescription = 'Implementation loop occurs under minimum times' }
            'I8' { $this.SegmentSyntaxErrorDescription = 'Implementation segment below minimum use' }
            'I9' { $this.SegmentSyntaxErrorDescription = 'Implementation' }
        }
    }
}

class TransactionSetResponse : X12Loop
{
    [string] $FilePath;
    [string] $FunctionalIdentifierCode
    [string] $GroupControlNumber
    [string] $VersionReleaseIndustryIdentifierCode

    [string] $TransactionSetAcknowledgmentCode
    [string] $TransactionSetSyntaxErrorCode

    [System.Collections.Generic.List[ErrorIdentification]] $Errors = [System.Collections.Generic.List[ErrorIdentification]]::new()

    TransactionSetResponse([string] $RawString, [string] $FilePath)
    {
        $this.FilePath = $FilePath
        $this.Init($RawString)
    }

    Init([string] $RawString)
    {
        $Elements = $RawString.Split('*')
        $this.FunctionalIdentifierCode = $Elements[1]
        $this.GroupControlNumber = $Elements[2]
        $this.VersionReleaseIndustryIdentifierCode = $Elements[3]      
    }

    TransactionSetResponse([string] $RawString)
    {
        $this.Init($RawString)
    }

    AddTransactionSetResponse([string] $RawString)
    {
        $Elements = $RawString.Split('*')
        $this.TransactionSetAcknowledgmentCode = $Elements[1]
        $this.TransactionSetSyntaxErrorCode = $Elements[2]
    }
}
#endregion

function Get-TransactionSetResponse
{
    [CmdletBinding()]
    param
    (
        [Parameter( Mandatory=$True,
                    ValueFromPipeline=$True,
                    ValueFromPipelineByPropertyName=$True,
                    HelpMessage='The full path to the EDI 999 file')]
        [string] $FilePath
    )
    process
    {
        $raw = Get-Content $FilePath 
    
        Write-Verbose "Read $FilePath"
        [System.Text.StringBuilder] $sb = [System.Text.StringBuilder]::new($raw.Substring($raw.IndexOf("~AK2"), $raw.IndexOf("~AK9") - $raw.IndexOf("~AK2")))
        $sb.Replace('~AK2', '|AK2') |Out-Null

        [X12Loop] $CurrentLoopReference = $null
        [TransactionSetResponse] $TransactionSetResponse = $null
        [ErrorIdentification] $ErrorIdentification = $null
        [SegmentContext] $SegmentContext = $null

        foreach($TransactionSetResponseRawString in $sb.ToString().Split('|'))
        {
            foreach($Segment in $TransactionSetResponseRawString.Split('~'))
            {
                if ($Segment.StartsWith('AK2')) 
                { 
                    # write out previous object (new segment indicates end of previous segment
                    if ($TransactionSetResponse -ne $null)
                    {
                        Write-Output $TransactionSetResponse
                    } 
                    $TransactionSetResponse = [TransactionSetResponse]::new($Segment, $FilePath)
                    $CurrentLoopReference = $TransactionSetResponse
                }

                if ($Segment.StartsWith('IK3')) 
                {
                    $ErrorIdentification = [ErrorIdentification]::new($Segment)
                    $TransactionSetResponse.Errors.Add($ErrorIdentification)

                }

                if ($Segment.StartsWith('IK4')) 
                {
                }

                if ($Segment.StartsWith('IK5')) 
                {
                    $TransactionSetResponse.AddTransactionSetResponse($Segment)
                }
            } # foreach $Segment
        } # foreach $TransactionSetResponseRawString
    }
}
