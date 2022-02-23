namespace DalamudPluginProjectTemplateFSharp.Attributes

open System

/// <summary>
/// Hides the command from the information page in <c>/xlplugins</c>.
/// </summary>
[<AllowNullLiteral; AttributeUsage(AttributeTargets.Method)>]
type DoNotShowInHelpAttribute() =
    inherit Attribute()
