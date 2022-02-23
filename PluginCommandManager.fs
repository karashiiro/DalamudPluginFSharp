namespace DalamudPluginProjectTemplateFSharp

open System
open System.Reflection
open Dalamud.Game.Command
open DalamudPluginProjectTemplateFSharp.Attributes

type private LoadedCommandInfo = (string * CommandInfo)

type PluginCommandManager<'THost>(host: 'THost, commandManager: CommandManager) =
    let commands =
        host
            .GetType()
            .GetMethods(
                BindingFlags.NonPublic
                ||| BindingFlags.Public
                ||| BindingFlags.Static
                ||| BindingFlags.Instance
            )

    let getCommandInfoTuple (method: MethodInfo) : LoadedCommandInfo list =
        let handlerDelegate: CommandInfo.HandlerDelegate =
            downcast Delegate.CreateDelegate(typeof<CommandInfo.HandlerDelegate>, host, method)

        let command = handlerDelegate.Method.GetCustomAttribute<CommandAttribute>()
        let aliases = handlerDelegate.Method.GetCustomAttribute<AliasesAttribute>()
        let helpMessage = handlerDelegate.Method.GetCustomAttribute<HelpMessageAttribute>()

        let doNotShowInHelp =
            handlerDelegate.Method.GetCustomAttribute<DoNotShowInHelpAttribute>()

        let commandInfo = CommandInfo(handlerDelegate)
        commandInfo.ShowInHelp <- not (isNull doNotShowInHelp)

        if (not (isNull helpMessage)
            && not (isNull helpMessage.HelpMessage)) then
            commandInfo.HelpMessage <- helpMessage.HelpMessage
        else
            commandInfo.HelpMessage <- ""

        (command.Command, commandInfo)
        :: if isNull aliases then
               []
           else
               [ for alias in aliases.Aliases -> (alias, commandInfo) ]

    let pluginCommands =
        commands
        |> Array.filter (fun method -> not (isNull (method.GetCustomAttribute<CommandAttribute>())))
        |> Array.collect (fun item -> List.toArray (getCommandInfoTuple item))

    do
        for command, commandInfo in pluginCommands do
            commandManager.AddHandler(command, commandInfo)
            |> ignore

    interface IDisposable with
        member _.Dispose() =
            for command, _ in pluginCommands do
                commandManager.RemoveHandler(command) |> ignore
