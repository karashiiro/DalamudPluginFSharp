namespace DalamudPluginProjectTemplateFSharp

open Dalamud.Configuration
open Newtonsoft.Json
open Dalamud.Plugin

type Configuration() =
    // Add any other properties or methods here. NOTE: they must be mutable / have setters
    // example: `member val MyProp = defaultValue with get, set`

    // machinery for convenience Save() method
    [<JsonIgnore>]
    let mutable pluginInterface: DalamudPluginInterface = null

    /// Called once from plugin initalization, stores the DalamudPluginInterface reference.
    member internal _.Initialize(pi: DalamudPluginInterface) = pluginInterface <- pi

    /// Saves the current configuration.
    member this.Save() = pluginInterface.SavePluginConfig(this)

    // mandatory interface + config file Version property
    interface IPluginConfiguration with
        member val Version = 0 with get, set
