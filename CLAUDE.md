# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

EdiTools is a C#/PowerShell EDI (Electronic Data Interchange) parser for X12 healthcare transaction sets (835, 837). C# handles parsing performance; PowerShell provides pipeline composability.

## Build and Test

```bash
# Build
cd CSharp
dotnet build csharp.sln

# Run all tests (NUnit 3)
dotnet test CSharp/TestEdiToolsClassLibrary/TestEdiToolsClassLibrary.csproj

# Run a single test
dotnet test CSharp/TestEdiToolsClassLibrary/TestEdiToolsClassLibrary.csproj --filter "FullyQualifiedName~TestName"
```

The solution targets .NET Framework 4.7.2. The main library is `CSharp/EdiTools/EdiTools.csproj`.

## Architecture

### EDI Hierarchical Model

```
EdiFile                         # Entry point; parses raw EDI text
  └── Interchange (ISA...IEA)   # Envelope level
      └── FunctionalGroup (GS...GE)
          └── TransactionSet (ST...SE)   # Base class
              └── Segment                # Individual EDI lines → Elements
```

Each level has a static factory/parse method that extracts its children from a segment list. Segments use lazy element parsing with copy-on-write semantics — `OriginalText` is preserved, and `Text` reconstructs from elements only if modified.

### Transaction-Specific Subclasses

- `Edi835.TransactionSet` — Health Care Claim Payment
- `Edi837.TransactionSet` — Health Care Claim, with additional hierarchy: `DocumentHierarchy` → `BillingProvider` → `Subscriber` → `Patient` → `Claim`

### Utilities (`CSharp/EdiTools/Utilities/`)

- `FileOperations` — Split 837 files by individual claims
- `DataDeidentification` — PII/PHI scrubbing for HIPAA compliance
- `ListExtensions` — `ToText()` extension to convert segment lists back to EDI text

### PowerShell Module (`PowerShell/`)

Pipeline cmdlets: `Get-EdiFile` → `Get-EdiTransactionSet` → `Get-Edi835` / `Get-Edi837` → `Get-Edi837Hierarchy`

`Get-EdiFile` accepts file paths, `FileInfo` (from `Get-ChildItem`), and `MatchInfo` (from `Select-String`).

## Key Design Constraints

- **PowerShell 2.0 compatibility**: C# code avoids modern language features to maintain PS 2.0 compat.
- **Line-break agnostic**: Handles both wrapped (single-line) and unwrapped (CR/LF) EDI files. Delimiter detection uses ISA segment positions 103-106.
- **Not a full EDI parser**: Only certain segment/element values are promoted as typed convenience properties (e.g., dates as `DateTime`, amounts as `decimal`).
