using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Prashalt.Unity.ConvasationGraph.Nodes
{
    [Serializable]
    public class EndNode : MasterNode
    {
        public EndNode()
        {
            title = "End";

            // 入力用のポートを作成
            var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float)); // 第三引数をPort.Capacity.Multipleにすると複数のポートへの接続が可能になる
            inputPort.portName = "Input";
            inputContainer.Add(inputPort); // 入力用ポートはinputContainerに追加する
        }

        public override void Initialize(string guid, Rect rect, string json)
        {
            base.Initialize(guid, rect, json);
        }
    }
}