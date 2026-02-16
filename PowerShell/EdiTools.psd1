@{
    # Module manifest data
    RootModule = 'EdiTools.psm1'
    ModuleVersion = '1.0'
    Author = 'Lance England'
    Description = 'PowerShell module for searching, filtering, and displaying EDI X12 file contents'
    FunctionsToExport = @('Get-EdiFile', 'Get-EdiTransactionSet', 'Get-Edi835', 'Get-Edi837', 'Get-Edi837Hierarchy')
    CmdletsToExport = @()
    VariablesToExport = @()
    AliasesToExport = @()
    PowerShellVersion = '5.1'

    # Custom formatting file
    FormatsToProcess = @('EdiTools.ps1xml')
}
