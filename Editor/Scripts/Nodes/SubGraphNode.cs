using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Prashalt.Unity.ConversationGraph.Nodes
{
	[Serializable]
	public class SubGraphNode : MasterNode
	{
		[SerializeField] public ConversationGraphAsset subGraph;
		public SubGraphNode()
		{
			var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
			inputPort.portName = "Input";
			inputContainer.Add(inputPort);

			// 出力用のポートを作る
			var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
			outputPort.portName = "Output";
			outputContainer.Add(outputPort);
		}
		public void SetSubGraphAsset(ConversationGraphAsset subGraph)
		{
			this.subGraph = subGraph;
			title = $"{subGraph.name} (Sub Graph)";
		}
		public override string ToJson()
		{
			return JsonUtility.ToJson(this);
		}
	}
}
