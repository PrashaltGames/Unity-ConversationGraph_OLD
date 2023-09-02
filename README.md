# Unity-ConversationGraph
Node-based package for creating conversation for Unity.
## Developer
Develop by AtsuAtsu0120(X:@KaidoAtsuya)
## Warning
**It is experimental package.**

In addition, secondary distribution and secondary processing are prohibited, at least until the creator's job search is complete.

## Suppoted
- UGUI
# Getting started
Install this package with UPM.

`https://github.com/PrashaltGames/Unity-ConvasationGraph.git`
# How to Use
1. Make `Conversation Graph Asset` in ProjectWindow.
2. Create your Conversation using Nodes.
3. Use `ConversationSystemUGUI` in Runtime with your Conversation Graph Asset.
## Custom
You can create custom components for ConvasationGraph using `ConversationSystemBase`.
This class provided 2 actions.
1. `OnNodeChangeAction`
2. `OnConversationFinishedAction`
# NodeList
- SpeakerNode : Normal node, enable to set speaker name and text.
- NarratorNode : SpeakerNode without speaker name.

