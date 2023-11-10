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
        public bool isChanged;
        #region Func_UnityEditor
        public PrashaltConversationGraph(PrashaltConversationWindow window)
        {
            //ScritableObjectから追加する。
            _window = window;
            _window.conversationGraphView = this;

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

            if (_window.ConversationGraphAsset is null || _window.ConversationGraphAsset.Nodes.Count <= 0)
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
                ShowNodesFromAsset(_window.ConversationGraphAsset);
                ShowEdgeFromAsset(_window.ConversationGraphAsset);
            }
            var graphInspector = new GraphInspectorNode(_window.ConversationGraphAsset, this);
            AddElement(graphInspector);

            //グラフビューの変更を検知する
            graphViewChanged += OnGraphViewChanged;
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

                //// ポートの型が一致していない場合は繋げない
                //if (port.portType != startPort.portType)
                //    return false;

                //ポートの型が継承されていなければ繋げない
                if(!startPort.portType.IsSubclassOf(port.portType) && startPort.portType != port.portType)
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
                if(instance is PropertyNode propertyNode)
                {
                    var obj = JsonUtility.FromJson<PropertyNode>(nodeData.json);
                    if(ConversationGraphUtility.ConversationProperties.TryGetValue(obj.memberName, out _))
                    {
						propertyNode.SetTitle(obj.memberName);
					}
                    else
                    {
                        Debug.LogWarning($"Conversation Property is missing. {obj.memberName}");
                        continue;
                    }
                }
                else if(instance is SubGraphNode subGraphNode)
                {
                    var obj = JsonUtility.FromJson<SubGraphNode>(nodeData.json);
                    subGraphNode.SetSubGraphAsset(obj.subGraph);
                }

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
                try
                {
					var baseNode = nodes.Select(x => x as MasterNode).FirstOrDefault(x => x.guid == baseNodeGuidWithCount[0]);
					var targetNode = nodes.Select(x => x as MasterNode).FirstOrDefault(x => x.guid == targetNodeGuidWithCount[0]);

					if (baseNode is null || targetNode is null) return;

					var input = targetNode.inputContainer.Children().Where(x => x is Port).ElementAt(targetNodeGuidWithCount.Length == 1 ? 0 : int.Parse(targetNodeGuidWithCount[1])) as Port;
					var output = baseNode.outputContainer.Children().Where(x => x is Port).ElementAt(baseNodeGuidWithCount.Length == 1 ? 0 : int.Parse(baseNodeGuidWithCount[1])) as Port;

					var edge = new Edge() { input = input, output = output };
					edge.input.Connect(edge);
					edge.output.Connect(edge);
					Add(edge);

					previousBaseNode = baseNode;
				}
                catch (Exception)
                {
                    continue;
                }
            }
            
        }
        public bool DropSubGraph()
        {
            //カーソルがGraph上になかったらスルー
            if(!contentRect.Contains(Event.current.mousePosition))
            {
                return false;
            }

            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            //ドラッグ後ならスルー
            var eventType = Event.current.type;
            if(eventType != EventType.DragExited)
            {
                return false;
            }

			DragAndDrop.AcceptDrag();

            Event.current.Use();

            var subGraphReferences = DragAndDrop.objectReferences.OfType<ConversationGraphAsset>().ToList();
            if (subGraphReferences.Count <= 0)
            {
                return false;
            }

            //自分と同じものはSubGraphとしては利用できないように。
            if (subGraphReferences[0].name == _window.ConversationGraphAsset.name)
            {
                return false;
            }
            //Nodeを生成
            var asset = subGraphReferences[0];
			var subGraphNode = new SubGraphNode();
			subGraphNode.SetSubGraphAsset(asset);

			// マウスの位置にノードを追加
			var worldMousePosition = _window.rootVisualElement.ChangeCoordinatesTo(_window.rootVisualElement.parent, GUIUtility.GUIToScreenPoint(Event.current.mousePosition) - _window.position.position);
			var localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);
			var nodePosition = new Rect(localMousePosition, new Vector2(100, 100));

            subGraphNode.Initialize("", nodePosition, "");

			AddElement(subGraphNode);

            return true;
        }
        public GraphViewChange OnGraphViewChanged(GraphViewChange e)
        {
            if(!isChanged)
            {
				_window.titleContent.text = $"{_window.titleContent.text}*";
                isChanged = true;
			}
            return e;
        }
        #endregion
    }
}
