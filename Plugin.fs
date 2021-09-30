namespace DalamudPluginProjectTemplateFSharp

open System
open Dalamud.Plugin
open Dalamud.Logging
open Dalamud.Game.Command
open Dalamud.Game.Gui
open Dalamud.Game.ClientState
open Dalamud.IoC
open DalamudPluginProjectTemplateFSharp.Attributes

type Plugin([<RequiredVersion("1.0")>] pluginInterface : DalamudPluginInterface,
            [<RequiredVersion("1.0")>] commands : CommandManager,
            [<RequiredVersion("1.0")>] chat : ChatGui,
            [<RequiredVersion("1.0")>] clientState : ClientState) as this =
    let mutable commandManager : PluginCommandManager<Plugin> = null
    let mutable config = Configuration()
    let ui = PluginUI()

    // Initialization
    let loadedConfig = pluginInterface.GetPluginConfig()
    do if (loadedConfig <> null) then
        config <- downcast loadedConfig
    do config.Initialize(pluginInterface)
    do pluginInterface.UiBuilder.add_Draw(fun() -> ui.Draw())
    do commandManager <- new PluginCommandManager<Plugin>(this, commands)

    [<Command("/example1")>]
    [<HelpMessage("Example help message.")>]
    member this.ExampleCommand1(command : string, args : string) =
        let world = clientState.LocalPlayer.CurrentWorld.GameData
        chat.Print(String.Format("Hello {0}!", world.Name))
        PluginLog.Log("Message sent successfully.")
        
    member private this.Dispose(disposing : bool) =
        match disposing with
        | true ->
            (commandManager :> IDisposable).Dispose()
            config.Save()
            pluginInterface.UiBuilder.remove_Draw(fun() -> ui.Draw())
            pluginInterface.Dispose()
        | false -> ()

    interface IDalamudPlugin with
        member this.Name = "Your Plugin's Display Name"
        member this.Dispose() =
            this.Dispose(true)