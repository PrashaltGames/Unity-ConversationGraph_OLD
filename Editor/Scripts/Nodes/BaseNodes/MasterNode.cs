using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Graphs;
using UnityEngine;
using Edge = UnityEditor.Experimental.GraphView.Edge;
using Node = UnityEditor.Experimental.GraphView.Node;

[Serializable]
public abstract class MasterNode : Node
{
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
    protected Port AddInputPort(Type portType, string portName = "Input", Port.Capacity capacity = Port.Capacity.Single)
    {
        //入力ポート
        var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, capacity, portType);
        inputPort.portName = portName;
        inputContainer.Add(inputPort);

        return inputPort;
    }
    protected Port AddInputPort(Type portType, Color portColor, string portName = "Input", Port.Capacity capacity = Port.Capacity.Single)
    {
        //入力ポート
        var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, capacity, portType);
        inputPort.portName = portName;
        inputPort.portColor = portColor;
        inputContainer.Add(inputPort);

        return inputPort;
    }
    protected Port AddOutputPort(Type portType, string portName = "Output", Port.Capacity capacity = Port.Capacity.Single)
    {
        //出力ポート
        var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, capacity, portType);
        outputPort.portName = portName;
        outputContainer.Add(outputPort);

        return outputPort;
    }
    protected Port AddOutputPort(Type portType, Color portColor, string portName = "Output", Port.Capacity capacity = Port.Capacity.Single)
    {
        //出力ポート
        var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, capacity, portType);
        outputPort.portName = portName;
        outputPort.portColor = portColor;
        outputContainer.Add(outputPort);

        return outputPort;
    }
}
