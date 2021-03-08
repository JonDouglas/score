![](Resources/logo.png)
# score
A tool that scores your NuGet packages & gives you suggestions to improve them.

**_This tool is under active development & results should be taken with a grain of salt._**

## Installation
```
> dotnet tool install --global score
```
_[![Nuget](https://img.shields.io/nuget/v/score)](https://www.nuget.org/packages/score/)_

## Usage
*Examine a specific package.*

<!-- snippet: Tests.Usage.verified.txt -->
<a id='snippet-Tests.Usage.verified.txt'></a>
```txt
USAGE:
    score <PACKAGE_NAME> <VERSION> [OPTIONS]

EXAMPLES:
    score <PACKAGE_NAME> <VERSION>

ARGUMENTS:
    <PACKAGE_NAME>    The package name to score          
    <VERSION>         The version of the package to score
                                                         
OPTIONS:
    -h, --help    Prints help information
                                         
```
<sup><a href='/Tests/Tests.Usage.verified.txt#L1-L13' title='Snippet source file'>snippet source</a> | <a href='#snippet-Tests.Usage.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

## Example
```
score Autofac 6.1.0
```

<!-- snippet: Tests.DumpScore.verified.txt -->
<a id='snippet-Tests.DumpScore.verified.txt'></a>
```txt
           _   _   _____   _____     ____     ____    ___    ____    _____      
          | \ | | | ____| |_   _|   / ___|   / ___|  / _ \  |  _ \  | ____|     
          |  \| | |  _|     | |     \___ \  | |     | | | | | |_) | |  _|       
       _  | |\  | | |___    | |      ___) | | |___  | |_| | |  _ <  | |___      
      (_) |_| \_| |_____|   |_|     |____/   \____|  \___/  |_| \_\ |_____|     
                                                                                
┌────────────────────────────────────────────────────────┬───────┬─────────────┐
│ Follows NuGet Conventions                              │ Score │ Total Score │
├────────────────────────────────────────────────────────┼───────┼─────────────┤
│ Has valid .nuspec                                      │ 9     │ 10          │
│ `-- <iconUrl> is deprecated. Use <icon> instead.       │       │             │
│     `-- Add an icon to your package and reference it   │       │             │
│         within an <icon> element.                      │       │             │
│ Provide valid release notes                            │ 0     │ 10          │
│ `-- <releaseNotes> is empty.                           │       │             │
│     `-- Add a <releaseNotes> element to your .nuspec   │       │             │
├────────────────────────────────────────────────────────┼───────┼─────────────┤
│ .NET Score                                             │ 9     │ 20          │
└────────────────────────────────────────────────────────┴───────┴─────────────┘
┌────────────────────────────────────────────────────────┬───────┬─────────────┐
│ Provides Documentation                                 │ Score │ Total Score │
├────────────────────────────────────────────────────────┼───────┼─────────────┤
│ Public API has XML Documentation                       │ 10    │ 10          │
├────────────────────────────────────────────────────────┼───────┼─────────────┤
│ .NET Score                                             │ 10    │ 10          │
└────────────────────────────────────────────────────────┴───────┴─────────────┘
┌────────────────────────────────────────────────────────┬───────┬─────────────┐
│ Supports Multiple Platforms                            │ Score │ Total Score │
├────────────────────────────────────────────────────────┼───────┼─────────────┤
│ Target the most compatible & latest frameworks.        │ 10    │ 20          │
│ `-- Target the latest target framework                 │       │             │
│     `-- Multi-target your library for 'net5.0'         │       │             │
├────────────────────────────────────────────────────────┼───────┼─────────────┤
│ .NET Score                                             │ 10    │ 20          │
└────────────────────────────────────────────────────────┴───────┴─────────────┘
┌────────────────────────────────────────────────────────┬───────┬─────────────┐
│ Up-to-date Dependencies                                │ Score │ Total Score │
├────────────────────────────────────────────────────────┼───────┼─────────────┤
│ Has up-to-date dependencies                            │ 0     │ 10          │
│ |-- netstandard1.0/Microsoft.CSharp is not the latest  │       │             │
│ |   version (4.3.0).                                   │       │             │
│ |   `-- Update netstandard1.0/Microsoft.CSharp to the  │       │             │
│ |       latest stable version (4.7.0).                 │       │             │
│ |-- netstandard1.0/NETStandard.Library is not the      │       │             │
│ |   latest version (1.6.1).                            │       │             │
│ |   `-- Update netstandard1.0/NETStandard.Library to   │       │             │
│ |       the latest stable version (2.0.3).             │       │             │
│ |-- netstandard1.3/Microsoft.CSharp is not the latest  │       │             │
│ |   version (4.3.0).                                   │       │             │
│ |   `-- Update netstandard1.3/Microsoft.CSharp to the  │       │             │
│ |       latest stable version (4.7.0).                 │       │             │
│ `-- netstandard1.3/NETStandard.Library is not the      │       │             │
│     latest version (1.6.1).                            │       │             │
│     `-- Update netstandard1.3/NETStandard.Library to   │       │             │
│         the latest stable version (2.0.3).             │       │             │
├────────────────────────────────────────────────────────┼───────┼─────────────┤
│ .NET Score                                             │ 0     │ 10          │
└────────────────────────────────────────────────────────┴───────┴─────────────┘
```
<sup><a href='/Tests/Tests.DumpScore.verified.txt#L1-L58' title='Snippet source file'>snippet source</a> | <a href='#snippet-Tests.DumpScore.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

## Why
Scores are created based on the following categories([heavily inspired by pana](https://pub.dev/packages/pana)):

- [Following NuGet Conventions](#following-nuget-conventions)
- [Providing Documentation](#providing-documentation)
- [Supporting Multiple Platforms](#supporting-multiple-platforms)
- [Passing Static Analysis](#passing-static-analysis)
- [Supporting Up-To-Date Dependencies](#supporting-up-to-date-dependencies)

## Following NuGet Conventions
NuGet packages are expected to follow certain file layouts when organizing a package. Most importantly make sure you:

- Provide a valid .nuspec or project properties. 
    - Ensure all Urls are valid and use https. (TODO)
- Provide a valid LICENSE. Preferably a permissive open source license.
- Provide a README.md. (Upcoming)
- Provide a RELEASENOTES/CHANGELOG.md (Upcoming)

## Providing Documentation
Packages can include documentation in two main areas:

1. Including an example inside your package. (TODO)
2. At least 20% of the public API members containing [API documentation](https://docs.microsoft.com/dotnet/csharp/codedoc).

## Supporting Multiple Platforms
Packages are encouraged to support multiple platforms to allow developers to support a wide variety of platforms. [.NET supports many platforms](https://dotnet.microsoft.com/learn/dotnet/what-is-dotnet). 

score checks for two portable frameworks to ensure packages are both compatible and provide the latest functionality:

- **.NET Standard 2.0:** The most compatible framework that supports majority of platforms.
- **.NET 5:** The latest framework that allows you to leverage the newest platform features.

For more information on .NET 5 and .NET Standard, see the [following docs](https://docs.microsoft.com/dotnet/standard/net-standard#when-to-target-net50-vs-netstandard).

## Passing Static Analysis
Packages go through a static analysis tool to determine if the package has any major health or security issues. These checks help ensure you are using the latest tooling to secure your software supply chain such as reproducible builds, compiler flags, source link, and much more. (TODO)

**This section is not implemented.**

## Supporting Up-To-Date Dependencies
Packages will be checked to ensure their dependencies are the latest version. 

Additionally they will be scanned for any known vulnerabilities or deprecated packages in those dependencies and warn you if you should take on a later version or suggested alternative. (TODO)

Wonderful tools already exist that do some of this such as [dotnet-outdated](https://github.com/dotnet-outdated/dotnet-outdated) which will check outdated dependencies. [snitch](https://github.com/spectresystems/snitch) which will check to see if you can remove unneeded transitive packages. 

## Best Practices
This tool does it's best job to provide many of the best practices of authoring NuGet packages. There are many best practices that do not exist in this tool today & will be considered in a future release. Please create a [new issue](https://github.com/JonDouglas/score/issues) with your favorite best practice if one is missing here!

- [Open Source Library Guidance](https://docs.microsoft.com/en-us/dotnet/standard/library-guidance/)
- [NuGet Library Guidance](https://docs.microsoft.com/en-us/dotnet/standard/library-guidance/nuget)
- [Package Authoring Best Practices](https://docs.microsoft.com/nuget/create-packages/package-authoring-best-practices)

## Contribute!
Please feel free to test this tool against any package & report any [issues](https://github.com/JonDouglas/score/issues)! This tool is a **proof of concept** & likely doesn't support every case. It's goal is to work against NuGet package `Dependency` types, but NuGet hosts more than just dependencies, so you can also try running this tool against `Template` and `DotNetTool` packages as well to see what happens! If you'd like to support these cases more, open up a issue or PR! 

**Any compelling package best practice check will be considered & PRs are gladly accepted!**



