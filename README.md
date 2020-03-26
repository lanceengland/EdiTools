# EdiTools

EdiTools is a PowerShell module creating for a spcific use case: an easy way to search, filter, and display EDI file contents.

## Design Goals

When creating EdiTools, I had specific design goals in mind:

* Fast, without sacrificing usability
* Composable
* Narrow scope
* PowerShell 2 compatible
* Line-break agnostic

### Fast

The "heavy lifting" of the file parsing is done in C\# code. PowerShell provides the usability with the pipeline and cmdlets.

### Composable

Each cmdlet works together, using the previous cmdlet output as input. For example, Get-EdiFile | Get-TransactionSet | Get-Edi835. The first cmdlets are general EDI functions, where the last cmdlet is a specific EDI format, in this case 835. The Get-Edi835 is the only format-specific cmdlet at this time.

Each cmdlet exposes more properties on the output object. In addition, the first cmdlet, Get-EdiFile, support piped input from both Get-ChildItem and Select-String, to simplify searching for specific files while leveraging this module.

## Narrow Scope

This is not a full-fledged EDI parser. It has one purpose, search, filter, and display EDI file contents. As such, only frequently needed  properties are exposed.

## PowerShell 2 Compatible

This is a personal constraint, but as of now, I needed the module to be PowerShell 2 compatible.

## Line-break Agnostic

The parsing process needed to be agnostic to "wrapped" (single line), or "unwrapped" (CR/LF, or LF-only) files

## Project Roadmap

Future development may include:

* Additional EDI document type functions e.g. Get-Edi999, Get-Edi837, etc.
* Implementing Pester tests (this is my next focus)
* Create a .net Core and PowerShell Core version
