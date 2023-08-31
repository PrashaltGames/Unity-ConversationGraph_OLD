using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using Prashalt.Unity.ConvasationGraph.Nodes;

namespace Prashalt.Unity.ConvasationGraph.Editor 
{
    public class PrashaltSearchMenuWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        private PrashaltConvasationGraph _graphView;
        private EditorWindow _editorWindow;

        public void Initialize(PrashaltConvasationGraph graphView, EditorWindow editorWindow)
        {
            _graphView = graphView;
            _editorWindow = editorWindow;
        }

        List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent("Create Node")),

            // Exampleというグループを追加
            new SearchTreeGroupEntry(new GUIContent("Example")) { level = 1 },

            // Exampleグループの下に各ノードを作るためのメニューを追加
            new SearchTreeEntry(new GUIContent(nameof(TextNode))) { level = 2, userData = typeof(TextNode) },
        };


            return entries;
        }

        bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var type = searchTreeEntry.userData as Type;
            var node = Activator.CreateInstance(type) as Node;

            // マウスの位置にノードを追加
            var worldMousePosition = _editorWindow.rootVisualElement.ChangeCoordinatesTo(_editorWindow.rootVisualElement.parent, context.screenMousePosition - _editorWindow.position.position);
            var localMousePosition = _graphView.contentViewContainer.WorldToLocal(worldMousePosition);
            var nodePosition = new Rect(localMousePosition, new Vector2(100, 100));
            node.SetPosition(nodePosition);

            if (node is MasterNode masterNode)
            {
                masterNode.Initialize(masterNode.guid, nodePosition, "");
            }
            _graphView.AddElement(node);
            return true;
        }
    }
}