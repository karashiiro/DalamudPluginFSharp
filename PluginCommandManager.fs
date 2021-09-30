namespace DalamudPluginProjectTemplateFSharp

open System
open System.Collections.Generic
open System.Linq
open System.Reflection
open Dalamud.Game.Command
open DalamudPluginProjectTemplateFSharp.Attributes

type private LoadedCommandInfo = (string*CommandInfo)

[<AllowNullLiteral>]
type PluginCommandManager<'THost>(host : 'THost, commandManager : CommandManager) as this =
    let GetCommandInfoTuple(method : MethodInfo) : IEnumerable<LoadedCommandInfo> =
        let handlerDelegate : CommandInfo.HandlerDelegate = downcast Delegate.CreateDelegate(typeof<CommandInfo.HandlerDelegate>, host, method)

        let command = handlerDelegate.Method.GetCustomAttribute<CommandAttribute>()
        let aliases = handlerDelegate.Method.GetCustomAttribute<AliasesAttribute>()
        let helpMessage = handlerDelegate.Method.GetCustomAttribute<HelpMessageAttribute>()
        let doNotShowInHelp = handlerDelegate.Method.GetCustomAttribute<DoNotShowInHelpAttribute>()

        let commandInfo = CommandInfo(handlerDelegate)
        commandInfo.ShowInHelp <- doNotShowInHelp <> null
        if (helpMessage <> null && helpMessage.HelpMessage <> null) then
            commandInfo.HelpMessage <- helpMessage.HelpMessage
        else
            commandInfo.HelpMessage <- ""

        let commandInfoTuples = List<LoadedCommandInfo>()
        commandInfoTuples.Add((command.Command, commandInfo))

        upcast commandInfoTuples

    let methods = host.GetType().GetMethods(BindingFlags.NonPublic ||| BindingFlags.Public ||| BindingFlags.Static ||| BindingFlags.Instance)
    let mutable pluginCommands : LoadedCommandInfo[] = methods.Where(fun method -> method.GetCustomAttribute<CommandAttribute>() <> null).SelectMany(fun method -> GetCommandInfoTuple(method)).ToArray()
        
    do this.AddCommandHandlers()

    member private this.AddCommandHandlers() =
        for i = 0 to pluginCommands.Length - 1 do
            let (command, commandInfo) = pluginCommands.[i]
            commandManager.AddHandler(command, commandInfo) |> ignore
    member private this.RemoveCommandHandlers() =
        for i = 0 to pluginCommands.Length - 1 do
            let (command, _) = pluginCommands.[i]
            commandManager.RemoveHandler(command) |> ignore

    interface IDisposable with
        member this.Dispose() =
            this.RemoveCommandHandlers()