# EdiTools

PowerShell scripts for managing and parsing EDI files. Initially, only base functionality and Edi 835 is implemented. More will be added on a as-needed basis.

## Project goals

### Composable

To build functions in a composable way ala the pipeline, such that each function adds more useful properties.

For example, Get-EdiFile returns the data for an EDI X12 file, with the ISA header information as properties in addition to a few convenience properties.

Piping the out of that into Get-EdiTransactionSet splits the file into transaction sets and adds a few transaction set-specific properties.

Piping the output of that into Get-Edi835 (assuming the transaction set is from an 835), adds a few 835-specific properties.

### Performant

Parsing large text files is expensive, especially for the targeted use case of searching through an archive directory for a specific file and/or transaction set. I wanted to be able to filter as far to the "left side" of the pipeline as possible. In particular, I wanted to be able to leverage the power of the Select-String function, and use the match results further down the pipeline.

For example, Get-ChildItem can be piped to Where-Object to filter on a date range, then piped to Select-String to search for a regular expression. The output of Select-String is a MatchInfo object which can be piped to Get-EdiFile. The MatchInfo object will be attached to the output from Get-EdiFile. When this is piped to Get-EdiTransaction set with the -FilterOutput flag enabled, it uses the MatchInfo object passed in to only output the transaction sets that have contain a match.

Another design principal was to keep the file contents intact as a single string. An earlier implementation of these functions split each file into arrays of segments. While it made for more convenient coding in places, the current implementation keeps the contents as a string, and implements an expression property that splits the string on demand. A possible future change would make this a lazy-loaded property instead so to trade the cost of parsing each property access with the additional memory storage of the file contents as array.

## Project Roadmap

Development of the project could see the following

* Additional EDI document type functions e.g. Get-Edi999, Get-Edi837, etc.
* Implementing Pester tests (this is my next focus)
* Making the module PowerShell Core-compatible (if its not already)
