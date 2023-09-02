using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using Prashalt.Unity.ConversationGraph.Nodes;

namespace Prashalt.Unity.ConversationGraph.Editor 
{
    public class PrashaltSearchMenuWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        private PrashaltConversationGraph _graphView;
        private EditorWindow _editorWindow;

        public void Initialize(PrashaltConversationGraph graphView, EditorWindow editorWindow)
        {
            _graphView = graphView;
            _editorWindow = editorWindow;
        }

        List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent("Create Node")),

            new SearchTreeGroupEntry(new GUIContent("Text")) { level = 1 },

            new SearchTreeEntry(new GUIContent(nameof(NarratorNode))) { level = 2, userData = typeof(NarratorNode) },
            new SearchTreeEntry(new GUIContent(nameof(SpeakerNode))) { level = 2, userData = typeof(SpeakerNode) },

            new SearchTreeGroupEntry(new GUIContent("Logic")) { level = 1 },

            new SearchTreeEntry(new GUIContent(nameof(SelectNode))) { level = 2, userData = typeof(SelectNode) },

            new SearchTreeGroupEntry(new GUIContent("Other")) { level = 1 },

            new SearchTreeEntry(new GUIContent(nameof(EndNode))) { level = 2, userData = typeof(EndNode) },
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