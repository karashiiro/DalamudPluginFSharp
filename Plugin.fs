namespace DalamudPluginProjectTemplateFSharp

open System
open Dalamud.Plugin
open Dalamud.Logging
open Dalamud.Game.Command
open Dalamud.Game.Gui
open Dalamud.Game.ClientState
open Dalamud.IoC
open DalamudPluginProjectTemplateFSharp.Attributes

type Plugin
    (
        [<RequiredVersion("1.0")>] pluginInterface: DalamudPluginInterface,
        [<RequiredVersion("1.0")>] commands: CommandManager,
        [<RequiredVersion("1.0")>] chat: ChatGui,
        [<RequiredVersion("1.0")>] clientState: ClientState
    ) as this =
    let config =
        match pluginInterface.GetPluginConfig() with
        | null -> Configuration()
        | config -> downcast config

    do config.Initialize(pluginInterface)

    /// Implement your plugin UI in the PluginUI class.
    let ui = PluginUI()
    /// Delegate instance for ui.Draw; reference saved for Dispose().
    let drawHandler = Action(ui.Draw)
    do pluginInterface.UiBuilder.add_Draw drawHandler

    /// Implements the command attributes; reference only used to Dispose().
    let commandManager = new PluginCommandManager<Plugin>(this, commands)

    [<Command("/example1")>]
    [<HelpMessage("Example help message.")>]
    member _.ExampleCommand1(command: string, args: string) : unit =
        // there's no nice way to handle all the unmarked nulls in the API; options are:
        // - risk NullReferenceException and hope for the best
        // - "if not (isNull x) then" checks (don't use "<> null")
        // - match and bind on the not-null case
        // - use Option.ofObj and handle it like a regular Option value
        // - use the provided maybe monad and read all intermediate values using let!
        //   the CE will short-circuit away should any let! find a null or None value
        //   don't forget to pipe it to "ignore" if used imperatively as commands must return unit
        maybe {
            // the block only continues if the value of a let!, match!, do! or use! is not null
            // should a result be null, the entire block automatically resolves to None
            let! localPlayer = clientState.LocalPlayer
            let! world = localPlayer.CurrentWorld.GameData

            chat.Print $"Hello %s{world.Name.ToString()}!"
            PluginLog.Log "Message sent successfully."
        }
        |> ignore

    interface IDalamudPlugin with
        member _.Name = "Your Plugin's Display Name"

        member _.Dispose() =
            (commandManager :> IDisposable).Dispose()
            config.Save()
            pluginInterface.UiBuilder.remove_Draw drawHandler
            // NOTE: pluginInterface.Dispose() exists but is [<Obsolete>]
            (pluginInterface :> IDisposable).Dispose()
