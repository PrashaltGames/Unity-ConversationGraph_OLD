using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Prashalt.Unity.ConversationGraph.Editor
{
    [CustomEditor(typeof(ConversationGraphAsset))]
    public class ConversationGraphAssetInspector : UnityEditor.Editor
    {
        private const string elementPath = ConversationGraphEditorUtility.packageFilePath + "Editor/UXML/";
        public override VisualElement CreateInspectorGUI()
        {
            var visualElement = new VisualElement();
            var asset = target as ConversationGraphAsset;

            List<NodeData> previousNodeDataList = new() { asset.StartNode };
            var count = 1;
            for (var i = 0; i < asset.Nodes.Count; i++)
            {
                List<NodeData> nodeDataLists = new();
                foreach(var previousNodeData in previousNodeDataList)
                {
                    var nodeDataList = asset.GetNextNode(previousNodeData);

                    var borderElement = new VisualElement()
                    {
                        style =
                    {
                        borderBottomColor = new Color(0.15f, 0.15f, 0.15f),
                        borderLeftColor = new Color(0.15f, 0.15f, 0.15f),
                        borderRightColor = new Color(0.15f, 0.15f, 0.15f),
                        borderTopColor = new Color(0.15f, 0.15f, 0.15f),
                        borderBottomWidth = 2,
                        borderLeftWidth = 2,
                        borderRightWidth = 2,
                        borderTopWidth = 2,
                        marginBottom = 5,
                        marginLeft = 5,
                        marginRight = 5,
                        marginTop = 5,
                    }
                    };

                    foreach (var nodeData in nodeDataList)
                    {
                        var type = nodeData.typeName.Split(".")[4];
                        if (type == "EndNode") continue;
                        var typeLabel = new Label($"{count}.{type}");

                        typeLabel.style.fontSize = 14;
                        typeLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
                        typeLabel.style.marginBottom = 3;

                        borderElement.Add(typeLabel);
                        foreach (var text in JsonUtility.FromJson<ConversationData>(nodeData.json).textList)
                        {
                            var textElenet = new Label(text);
                            textElenet.style.marginLeft = 10;
                            textElenet.style.marginTop = 2;
                            borderElement.Add(textElenet);
                        }
                    }
                    if(borderElement.childCount != 0)
                    {
                        visualElement.Add(borderElement);
                        nodeDataLists.AddRange(nodeDataList);
                    }
                }
                count++;
                previousNodeDataList = nodeDataLists;
            }

            return visualElement;
        }
    }
}
