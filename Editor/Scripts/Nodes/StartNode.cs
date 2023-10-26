using Prashalt.Unity.ConversationGraph.Animation;
using System;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Prashalt.Unity.ConversationGraph.Nodes
{
    [Serializable]
    public class StartNode : MasterNode
    {
        [SerializeField] private AnimationData animation;

		[NonSerialized] private Port animationPort;
        public StartNode()
        {
            title = "Start";

            // 出力用のポートを作る
            var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
            outputPort.portName = "Start";
            outputContainer.Add(outputPort); // 出力用ポートはoutputContainerに追加する

			// 入力用のポートを作成
			animationPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(LetterAnimation));
			animationPort.portName = "Letters Animation";
			animationPort.portColor = Color.red;
			inputContainer.Add(animationPort);

			capabilities &= ~Capabilities.Deletable;
		}
		public override string ToJson()
		{
			base.ToJson();
			if (animationPort.connected)
			{
				var edge = animationPort.connections.FirstOrDefault();

				var animationNode = edge.output.node as AnimationNode<LetterFadeInAnimation>;
				animation.name = animationNode.AnimationName;
				animation.intProperties = animationNode.intProperties;
				animation.floatProperties = animationNode.floatProperties;
			}
			return JsonUtility.ToJson(this);
		}
	}
}
