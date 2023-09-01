using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[Serializable]
public abstract class MasterNode : Node
{
    [SerializeField] protected List<string> textList;
    [NonSerialized] public string guid;

    public virtual string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
    public virtual void Initialize(string guid, Rect rect, string json)
    {
        if (guid == null || guid == "")
        {
            this.guid = Guid.NewGuid().ToString("N");
        }
        else
        {
            this.guid = guid;
        }
        SetPosition(rect);
    }
}
