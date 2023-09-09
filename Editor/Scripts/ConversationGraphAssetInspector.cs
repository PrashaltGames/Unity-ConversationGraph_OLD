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

                        borderBottomLeftRadius = 8,
                        borderBottomRightRadius = 8,
                        borderTopLeftRadius = 8,
                        borderTopRightRadius = 8,

                        paddingBottom = 4,
                        paddingLeft = 4,
                        paddingRight = 4,
                        paddingTop = 4,

                        marginBottom = 5,
                        marginTop = 5,
                        }
                    };

                    foreach (var nodeData in nodeDataList)
                    {
                        var type = nodeData.typeName.Split(".")[4];
                        if (type == "EndNode") continue;
                        var typeLabel = new Label($"{count}.{type}");

                        typeLabel.style.fontSize = 15;
                        typeLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
                        typeLabel.style.marginBottom = 3;

                        borderElement.Add(typeLabel);

                        var followTextDescription = type switch
                        {
                            "SelectNode" => "Options",
                            _ => "Main Texts"
                        };
                        var descriptionLabel = new Label(followTextDescription);
                        descriptionLabel.style.fontSize = 13;
                        borderElement.Add(descriptionLabel);

                        var textCount = 1;
                        var conversation = JsonUtility.FromJson<ConversationData>(nodeData.json);

                        if(conversation.speakerName != "" && conversation.speakerName is not null)
                        {
                            var speakerLabel = new Label($"Speaker: {conversation.speakerName}");
                            speakerLabel.style.marginBottom = 3;
                            speakerLabel.style.fontSize = 13;
                            borderElement.Add(speakerLabel);

                            ConversationGraphEditorUtility.MoveUp(borderElement, speakerLabel);
                        }
                        foreach (var text in conversation.textList)
                        {
                            var textElenet = new Label($"{textCount}: {text}");
                            textElenet.style.marginLeft = 10;
                            textElenet.style.marginTop = 2;
                            borderElement.Add(textElenet);

                            textCount++;
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
