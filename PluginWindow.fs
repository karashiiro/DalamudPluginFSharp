namespace DalamudPluginProjectTemplateFSharp

open Dalamud.Interface.Windowing
open ImGuiNET
open System.Numerics

type PluginWindow() =
    inherit Window("TemplateWindow")

    do base.IsOpen <- true
    do base.Size <- Vector2(810f, 520f)
    do base.SizeCondition <- ImGuiCond.FirstUseEver

    override _.Draw() =
        ImGui.Text("Hello, world!")
