namespace DalamudPluginProjectTemplateFSharp.Attributes

open System

[<AllowNullLiteral>]
[<AttributeUsage(AttributeTargets.Method)>]
type CommandAttribute(command : string) =
    inherit Attribute()
    member this.Command = command
