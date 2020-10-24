namespace DalamudPluginProjectTemplateFSharp.Attributes

open System

[<AllowNullLiteral>]
[<AttributeUsage(AttributeTargets.Method)>]
type HelpMessageAttribute(helpMessage : string) =
    inherit Attribute()
    member this.HelpMessage = helpMessage
