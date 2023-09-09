using Cysharp.Threading.Tasks;
using Prashalt.Unity.ConversationGraph.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Prashalt.Unity.ConversationGraph.Editor
{
    public class PrashaltConversationGraph : GraphView
    {
        public PrashaltConversationWindow _window;
        #region Func_UnityEditor
        public PrashaltConversationGraph(PrashaltConversationWindow window)
        {
            //ScritableObjectから追加する。
            _window = window;
            _window.convasationGraphView = this;

            // 親のサイズに合わせてGraphViewのサイズを設定
            this.StretchToParentSize();

            // MMBスクロールでズームインアウトができるように
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            // MMBドラッグで描画範囲を動かせるように
            this.AddManipulator(new ContentDragger());
            // LMBドラッグで選択した要素を動かせるように
            this.AddManipulator(new SelectionDragger());
            // LMBドラッグで範囲選択ができるように
            this.AddManipulator(new RectangleSelector());

            // 右クリックメニューを追加
            var menuWindowProvider = ScriptableObject.CreateInstance<PrashaltSearchMenuWindowProvider>();
            menuWindowProvider.Initialize(this, _window);
            nodeCreationRequest += context =>
            {
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), menuWindowProvider);
            };

            if (_window.ConvasationGraphAsset is null || _window.ConvasationGraphAsset.Nodes.Count <= 0)
            {
                var startNode = new StartNode();
                AddElement(startNode);
                var endNode = new EndNode();
                AddElement(endNode);

                startNode.Initialize(null, new Rect(100f, 150f, default, default), "");
                endNode.Initialize(null, new Rect(800f, 150f, default, default), "");
            }
            else
            {
                ShowNodesFromAsset(_window.ConvasationGraphAsset);
                ShowEdgeFromAsset(_window.ConvasationGraphAsset);
            }
            var graphInspector = new GraphInspectorNode(_window.ConvasationGraphAsset);
            AddElement(graphInspector);
        }
        // GetCompatiblePortsをオーバーライドする
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            compatiblePorts.AddRange(ports.ToList().Where(port =>
            {
                // 同じノードには繋げない
                if (startPort.node == port.node)
                    return false;

                // Input同士、Output同士は繋げない
                if (port.direction == startPort.direction)
                    return false;

                // ポートの型が一致していない場合は繋げない
                if (port.portType != startPort.portType)
                    return false;

                return true;
            }));

            return compatiblePorts;
        }
        #endregion
        #region Func_Original
        public void ShowNodesFromAsset(ConversationGraphAsset asset)
        {
            foreach(var nodeData in asset.Nodes)
            {
                var t = Type.GetType(nodeData.typeName);
                if (t is null) continue;

                var instance = Activator.CreateInstance(t) as MasterNode;
                if (instance is null) continue;

                AddElement(instance);
                instance.Initialize(nodeData.guid, nodeData.rect, nodeData.json);
            }
        }
        public async void ShowEdgeFromAsset(ConversationGraphAsset asset)
        {
            await UniTask.Delay(10);
            MasterNode previousBaseNode = null;
            foreach (var edgeData in asset.Edges)
            {
                var baseNodeGuidWithCount = edgeData.baseNodeGuid.Split(":");
                var targetNodeGuidWithCount = edgeData.targetNodeGuid.Split(":");
                var baseNode = nodes.Select(x => x as MasterNode).FirstOrDefault(x => x.guid == baseNodeGuidWithCount[0]);
                var targetNode = nodes.Select(x => x as MasterNode).FirstOrDefault(x => x.guid == targetNodeGuidWithCount[0]);
                if (baseNode is null || targetNode is null) return;

                var input = targetNode.inputContainer.Children().Where(x => x is Port).ElementAt(int.Parse(targetNodeGuidWithCount[1])) as Port;
                var output = baseNode.outputContainer.Children().Where(x => x is Port).ElementAt(int.Parse(baseNodeGuidWithCount[1])) as Port;

                var edge = new Edge() { input = input, output = output };
                edge.input.Connect(edge);
                edge.output.Connect(edge);
                Add(edge);

                previousBaseNode = baseNode;
            }
            
        }
        #endregion
    }
}
