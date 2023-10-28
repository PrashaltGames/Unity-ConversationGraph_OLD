using Prashalt.Unity.ConversationGraph;
using System;
using System.Collections.Generic;

[Serializable]
public struct ConversationData
{
    public string speakerName;
    public List<string> textList;
    public string animationGuid;
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
[Serializable]
public struct AnimationData
{
    public string animationName;
    public List<int> intProperties;
    public List<float> floatProperties;
}
