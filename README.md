# EdiTools

EdiTools is a PowerShell module creating for a specific use case: an easy way to search, filter, and display EDI file contents.

## Design Goals

When creating EdiTools, I had specific design goals in mind:

* Fast, without sacrificing usability
* Composable
* Narrow scope
* PowerShell 2 compatible
* Line-break agnostic
* Convenience properties with accurate data type

### Fast

The "heavy lifting" of the file parsing is done in C\# code. PowerShell provides the usability with the pipeline and cmdlets.

### Composable

Each cmdlet works together, using the previous cmdlet output as input. For example, Get-EdiFile | Get-TransactionSet | Get-Edi835. The first cmdlets are general EDI functions, where the last cmdlet is a specific EDI format, in this case 835. The Get-Edi835 is the only format-specific cmdlet currently.

Each cmdlet exposes more properties on the output object. In addition, the first cmdlet, Get-EdiFile, support piped input from both Get-ChildItem and Select-String, to simplify searching for specific files while leveraging this module.

### Narrow Scope

This is not a full-fledged EDI parser. It has one purpose, search, filter, and display EDI file contents. As such, only frequently needed properties are exposed.

### PowerShell 2 Compatible

This is a personal constraint, but as of now, I needed the module to be PowerShell 2 compatible.

### Line-break Agnostic

The parsing process needed to be agnostic to "wrapped" (single line), or "unwrapped" (CR/LF, or LF-only) files

### Convenience properties with accurate data type

Certain segment/element values from the EDI file are promoted for convenience. These convenience properties are converted to data types designated in the specification.

For example, the Interchange segments ISA09 and ISA10 are interchange date and time, respectively. These properties are combined into property Interchange.Date of type System.DateTime.

## Installation

If you don't want to clone the repo, and just want to install the PowerShell module:

1. [Download the EdiTools.psm1 file](https://github.com/lanceengland/EdiTools/raw/master/PowerShell/EdiTools.psm1)

2. Save the file to c:\Users\<user name>Documents\WindowsPowerShell\Modules\EdiTools

3. Right-click on the file and choose Properties -> Click the Unblock button

4. From a PowerShell prompt, type "Import-Module EdiTools -Verbose" (the -Verbose is optional, but will give you conformation the cmdlets installed).

## Project Roadmap

Future development may include:

* Additional EDI document type functions e.g. Get-Edi999, Get-Edi837, etc.
* Implementing Pester tests (this is my next focus)
* Create a .net Core and PowerShell Core version
