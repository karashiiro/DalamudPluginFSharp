namespace DalamudPluginProjectTemplateFSharp.Attributes

open System

/// <summary>
/// A command that is invokable as a slash-command by the player.
/// The <paramref name="command">command</paramref> value must start with a slash.
/// </summary>
/// <remarks>
/// All Commands must have <c>string * string -> unit</c> signature.
/// </remarks>
[<AllowNullLiteral; AttributeUsage(AttributeTargets.Method)>]
type CommandAttribute(command: string) =
    inherit Attribute()
    member this.Command = command
