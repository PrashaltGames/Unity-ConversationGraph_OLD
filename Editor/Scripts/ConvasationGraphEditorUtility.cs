using Codice.Utils;
using Prashalt.Unity.ConvasationGraph;
using Prashalt.Unity.ConvasationGraph.Nodes;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Prashalt.Unity.ConvasationGraph.Editor
{
    public static class ConvasationGraphEditorUtility
    {
        public static NodeData NodeToData(MasterNode node)
        {
            var guid = node.guid;
            var rect = node.GetPosition();
            var json = node.ToJson();
            var type = node.GetType().FullName;

            return new NodeData { guid = guid, rect = rect, json = json, typeName = type };
        }
        public static EdgeData EdgeToData(Edge edge)
        {
            var targetNode = edge.input.node as MasterNode;
            var baseNode = edge.output.node as MasterNode;

            if (baseNode is null || targetNode is null) return null;

            var edgeData = new EdgeData("", baseNode.guid, targetNode.guid);

            return edgeData;
        }
        public static void MoveUp(VisualElement parent, VisualElement target)
        {
            var index = parent.IndexOf(target);
            var next = parent.ElementAt(index - 1);
            target.PlaceBehind(next);
        }
        public static void MoveDown(VisualElement parent, VisualElement target)
        {
            var index = parent.IndexOf(target);
            var next = parent.ElementAt(index + 1);
            target.PlaceInFront(next);
        }
    }
}
