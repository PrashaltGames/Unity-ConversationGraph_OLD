using Prashalt.Unity.ConversationGraph.Nodes;
using System;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Prashalt.Unity.ConversationGraph.Nodes.Property
{
	[Serializable]
	public class FlagNode : PropertyNode
	{
		public FlagNode()
		{
			title = $"(bool)";

			// 出力用のポートを作る
			AddOutputPort(typeof(bool), Color.magenta, "Flag");
		}
		public override void Initialize(string guid, Rect rect, string json)
		{
			base.Initialize(guid, rect, json);
			if (json is not null && json is not "")
			{
				var jsonObj = JsonUtility.FromJson<FlagNode>(json);
				this.memberName = jsonObj.memberName;
			}
		}
		public override string ToJson()
		{
			return JsonUtility.ToJson(this);
		}
	}
}
