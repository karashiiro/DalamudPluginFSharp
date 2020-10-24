namespace DalamudPluginProjectTemplateFSharp

open ImGuiNET

type PluginUI() =
    [<DefaultValue>] val mutable IsVisible : bool

    member this.Draw() =
        match this.IsVisible with
        | false -> ()
        | true -> ()
