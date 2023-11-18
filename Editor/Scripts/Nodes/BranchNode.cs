using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Prashalt.Unity.ConversationGraph.Nodes.Logic
{
	[Serializable]
	public class BranchNode : LogicNode
	{
		public BranchNode()
		{
			title = "Branch";

			// 入力用のポートを作成
			AddInputPort(typeof(float));
			AddInputPort(typeof(bool), Color.magenta);

			//出力ポート
			AddOutputPort(typeof(float), "True");
			AddOutputPort(typeof(float), "False");
		}
		public override string ToJson()
		{
			int i = 0;
			foreach (Port input in inputContainer.Children())
			{
				foreach(var edge in input.connections)
				{
					if(edge.output.node is MasterNode masterNode)
					{
						switch(i)
						{
							case 0:
								break;
							case 1:
								inputNodeGuids.Add($"B:{masterNode.guid}");
								break;
						}
					}
				}
				i++;
			}
			i = 0;
			foreach(Port output in outputContainer.Children())
			{
				foreach (var edge in output.connections)
				{
					if (edge.output.node is MasterNode masterNode)
					{
						switch (i)
						{
							case 0:
								outputNodeGuids.Add($"T:{masterNode.guid}");
								break;
							case 1:
								outputNodeGuids.Add($"F:{masterNode.guid}");
								break;
						}
					}
				}
				i++;
			}
			return JsonUtility.ToJson(this);
		}
	}
}