namespace DalamudPluginProjectTemplateFSharp.Attributes

open System

[<AllowNullLiteral>]
[<AttributeUsage(AttributeTargets.Method)>]
type DoNotShowInHelpAttribute() =
    inherit Attribute()
