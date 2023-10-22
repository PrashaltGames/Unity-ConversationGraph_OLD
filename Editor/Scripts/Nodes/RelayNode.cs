using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Prashalt.Unity.ConversationGraph.Nodes
{
	public class RelayNode : MasterNode
	{
		public RelayNode()
		{
			title = "Relay";

			// 入力用のポートを作成
			var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float));
			inputPort.portName = "Input";
			inputContainer.Add(inputPort);

			//出力ポート
			var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
			outputPort.portName = "Option1";
			outputContainer.Add(outputPort);
		}
	}
}

