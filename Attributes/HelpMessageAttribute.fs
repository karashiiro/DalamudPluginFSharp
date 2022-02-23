namespace DalamudPluginProjectTemplateFSharp.Attributes

open System

/// <summary>
/// One-liner description of the command for the information page in <c>/xlplugins</c>.
/// </summary>
[<AllowNullLiteral; AttributeUsage(AttributeTargets.Method)>]
type HelpMessageAttribute(helpMessage: string) =
    inherit Attribute()
    member this.HelpMessage = helpMessage
