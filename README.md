# Unity-ConversationGraph
Node-based package for creating conversation for Unity.
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
## Basic
1. Make `Conversation Graph Asset` in ProjectWindow.
2. Create your Conversation using Nodes.
3. Use `ConversationSystemUGUI` in Runtime with your Conversation Graph Asset.
4. Use `StartConvesation` Method in ConversationSystem Component.

## Property
You can use `ConversationProperty`Attribute.
It is only for static field or property.

In Graph, You write `{PropertyName}`, it replace value.

Also, Bool property can use as Node.
This node use in Branch Node.
Click property button in GraphInspector, Create Bool property Node.

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
- BranchNode ： Conversation branching by FlagNode(Bool Property Node)
