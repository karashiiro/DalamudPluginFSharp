namespace DalamudPluginProjectTemplateFSharp.Attributes

open System

/// List of other command names that also invoke this command.
[<AllowNullLiteral; AttributeUsage(AttributeTargets.Method)>]
type AliasesAttribute([<ParamArray>] aliases: string []) =
    inherit Attribute()
    member this.Aliases = aliases
