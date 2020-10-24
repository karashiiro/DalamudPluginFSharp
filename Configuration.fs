namespace DalamudPluginProjectTemplateFSharp

open Dalamud.Configuration
open Newtonsoft.Json
open Dalamud.Plugin

type Configuration() =
    let mutable version : int = 0

    // Add any other properties or methods here.

    [<JsonIgnore>]
    let mutable pluginInterface : DalamudPluginInterface = null

    member this.Initialize(pi : DalamudPluginInterface) =
        pluginInterface <- pi

    member this.Save() =
        pluginInterface.SavePluginConfig(this)

    interface IPluginConfiguration with
        member this.Version
            with get (): int =
                version
            and set (v : int): unit = 
                version <- v