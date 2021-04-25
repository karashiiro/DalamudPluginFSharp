# DalamudPluginProjectTemplate
An opinionated Visual Studio project template for Dalamud plugins, with attributes for more maintainable command setup and teardown.

## Attributes
This template includes an attribute framework reminiscent of [Discord.Net](https://github.com/discord-net/Discord.Net).

```fsharp
[<Command("/example1")>]
[<Aliases("/ex1", "/e1")>]
[<HelpMessage("Example help message.")>]
member this.ExampleCommand1(command : string, args : string) =
    let chat = pluginInterface.Framework.Gui.Chat
    let world = pluginInterface.ClientState.LocalPlayer.CurrentWorld.GameData
    chat.Print(String.Format("Hello {0}!", world.Name))
    PluginLog.Log("Message sent successfully.")

[<Command("/example2")>]
[<DoNotShowInHelp>]
member this.ExampleCommand2(command : string, args : string) =
    PluginLog.Log("Hello, world!")
```

This automatically registers and unregisters the methods that they're attached to on initialization and dispose.

## GitHub Actions
Running the shell script `DownloadGithubActions.sh` will download some useful GitHub actions for you. You can also delete this file if you have no need for it.

### Current Actions
  * dotnet build/test
