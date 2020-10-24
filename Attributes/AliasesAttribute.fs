namespace DalamudPluginProjectTemplateFSharp.Attributes

open System

[<AllowNullLiteral>]
[<AttributeUsage(AttributeTargets.Method)>]
type AliasesAttribute([<ParamArray>] aliases : string[]) =
    inherit Attribute()
    member this.Aliases = aliases
