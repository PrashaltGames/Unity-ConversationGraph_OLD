# Unity-ConversationGraph
Node-based package for creating conversation for Unity.

SourceGenerator Repository: https://github.com/PrashaltGames/Unity-ConversationGraphSourceGenerator
## Developer
Develop by AtsuAtsu0120(X:[@KaidoAtsuya](https://twitter.com/KaidoAtsuya))
## Warning
**It is experimental package.**

In addition, secondary distribution and secondary processing are prohibited, at least until the creator's job search is complete.

## Suppoted
- Unity 2021 or newer (with .NET Framework)
- Legacy Input Manager
  - Input System (new) will be suppot maybe.
- uGUI
# Getting started
Install this package with UPM.

`https://github.com/PrashaltGames/Unity-ConversationGraph.git`
# How to Use
1. Make `Conversation Graph Asset` in ProjectWindow.
2. Create your Conversation using Nodes.
3. Use `ConversationSystemUGUI` in Runtime with your Conversation Graph Asset.
4. Use `StartConvesation` Method in ConversationSystem Component.
## Custom
You can create custom components for ConvasationGraph using `ConversationSystemBase`.
This class provided 3 Events.
1. `OnStartConvesationEvent`
2. `OnNodeChangeEvent`
3. `OnShowOptionsEvent`
4. `OnConversationFinishedEvent`
# NodeList
- SpeakerNode : Normal node, enable to set speaker name and text.
- NarratorNode : SpeakerNode without speaker name.
- SelectNode : Node for making the branch by options.
