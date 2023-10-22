using Prashalt.Unity.ConversationGraph;
using System;
using System.Collections.Generic;

[Serializable]
public struct ConversationData
{
    public string speakerName;
    public List<string> textList;
    public string animationName;
}
[Serializable]
public struct PropertyData
{
    public string memberName;
}
[Serializable]
public struct LogicData
{
    public List<string> inputNodeGuids;
    public List<string> outputNodeGuids;
}
[Serializable]
public struct SubGraphData
{
	public ConversationGraphAsset subGraph;
} 
