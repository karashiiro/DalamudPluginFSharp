namespace DalamudPluginProjectTemplateFSharp

open ImGuiNET

type PluginUI() =
    [<DefaultValue>]
    val mutable IsVisible: bool

    member this.Draw() = if this.IsVisible then ()
