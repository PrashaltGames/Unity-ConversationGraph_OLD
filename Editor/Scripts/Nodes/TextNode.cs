using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Prashalt.Unity.ConvasationGraph.Nodes
{
    [Serializable]
    public class TextNode : MasterNode
    {
        [SerializeField] protected string text;
        [NonSerialized] protected TextField _textField; 
        public TextNode()
        {
            title = "Text";

            // 入力用のポートを作成
            var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float)); // 第三引数をPort.Capacity.Multipleにすると複数のポートへの接続が可能になる
            inputPort.portName = "Input";
            inputContainer.Add(inputPort); // 入力用ポートはinputContainerに追加する

            //出力ポート
            var outputPort = Port.Create<Edge>(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(float));
            outputPort.portName = "Output";
            outputContainer.Add(outputPort);

            _textField = new TextField();
            extensionContainer.Add(_textField);
            RefreshExpandedState();
        }

        public override void Initialize(string guid, Rect rect, string json)
        {
            base.Initialize(guid, rect, json);
            var jsonObj = JsonUtility.FromJson<TextNode>(json);
            _textField.SetValueWithoutNotify(jsonObj?.text);
        }
        public override string ToJson()
        {
            text = _textField.text;
            return JsonUtility.ToJson(this);
        }
    }
}