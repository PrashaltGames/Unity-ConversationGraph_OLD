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
			AddInputPort(typeof(float));

			//出力ポート
			AddInputPort(typeof(float));
		}
	}
}

