using Prashalt.Unity.ConversationGraph.Nodes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SelectNode : MasterNode
{
    public SelectNode() : base()
    {
        title = "Speaker";

        RefreshExpandedState();
    }

    public override void Initialize(string guid, Rect rect, string json)
    {
        rect.width = 200;
        base.Initialize(guid, rect, json);
    }
    public override string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
