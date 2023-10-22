using Prashalt.Unity.ConversationGraph.Animation;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Prashalt.Unity.ConversationGraph.Nodes
{
	public class AnimationNode : MasterNode
	{
		public string AnimationName { get; private set; }
		public AnimationNode()
		{
			title = "(Animation)";

			//出力ポート
			var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(ConversationAnimation));
			outputPort.portName = "Output";
			outputPort.portColor = Color.red;
			outputContainer.Add(outputPort);

			AnimationName = nameof(LetterFadeInAnimation);
		}
	}
}
