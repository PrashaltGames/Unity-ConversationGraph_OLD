using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Prashalt.Unity.ConversationGraph.Nodes
{
    public class StartNode : MasterNode
    {
        public StartNode()
        {
            title = "Start";

            // 出力用のポートを作る
            var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
            outputPort.portName = "Value";
            outputContainer.Add(outputPort); // 出力用ポートはoutputContainerに追加する

            capabilities &= ~Capabilities.Deletable;
        }
    }
}
