namespace DalamudPluginProjectTemplateFSharp

open System
open Dalamud.Plugin
open DalamudPluginProjectTemplateFSharp.Attributes

type Plugin() =
    let mutable pluginInterface : DalamudPluginInterface = null
    let mutable commandManager : PluginCommandManager<Plugin> = null
    let mutable config = Configuration()
    let ui = PluginUI()

    [<Command("/example1")>]
    [<HelpMessage("Example help message.")>]
    member this.ExampleCommand1(command : string, args : string) =
        let chat = pluginInterface.Framework.Gui.Chat
        let world = pluginInterface.ClientState.LocalPlayer.CurrentWorld.GameData
        chat.Print(String.Format("Hello {0}!", world.Name))
        PluginLog.Log("Message sent successfully.")
        
    member private this.Dispose(disposing : bool) =
        match disposing with
        | true ->
            (commandManager :> IDisposable).Dispose()
            config.Save()
            pluginInterface.UiBuilder.remove_OnBuildUi(fun() -> ui.Draw())
            pluginInterface.Dispose()
        | false -> ()

    interface IDalamudPlugin with
        member this.Name = "Your Plugin's Display Name"
        member this.Initialize(pi : DalamudPluginInterface) =
            pluginInterface <- pi
                
            let loadedConfig = pi.GetPluginConfig()
            if (loadedConfig <> null) then
                config <- downcast loadedConfig
            config.Initialize(pi)

            pi.UiBuilder.add_OnBuildUi(fun() -> ui.Draw())

            commandManager <- new PluginCommandManager<Plugin>(this, pi)
        member this.Dispose() =
            this.Dispose(true)