using Prashalt.Unity.ConversationGraph.Components;
using Prashalt.Unity.ConversationGraph.Editor;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Prashalt.Unity.ConversationGraph.Nodes.Conversation
{
    [Serializable]
    public class NarratorNode : ConversationNode
    {
        private const string elementPath = ConversationGraphEditorUtility.packageFilePath + "Editor/UXML/NarratorNode.uxml";
        
        public NarratorNode() : base(elementPath, "Main Text")
        {
            title = "Narrator";

			//出力ポート
			var outputPort = Port.Create<Edge>(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(float));
			outputPort.portName = "Output";
			outputContainer.Add(outputPort);

            var addTextFieldButton = mainContainer.Q<Button>("addButton");
            addTextFieldButton.clicked += OnAddTextButton;
        }
        public void OnAddTextButton()
        {
            var newTextField = new PrashaltTextFieldButton();

            newTextField.Q<Label>().text = $"Main Text {textFieldList.Count + 1}";
            textFieldList.Add(newTextField.Q<TextField>());

            defaultContainer.Add(newTextField);
			newTextField.Q<Button>().clicked += () => SelectTextButton(newTextField);

			ConversationGraphEditorUtility.MoveDown(defaultContainer, buttonContainer);
        }

        public override void Initialize(string guid, Rect rect, string json)
        {
            base.Initialize(guid, rect, json);
            var jsonObj = JsonUtility.FromJson<NarratorNode>(json);

            if(jsonObj is not null)
            {
                int i = 0;
                foreach (var text in jsonObj.textList)
                {
                    if(i > 0)
                    {
                        OnAddTextButton();
                    }
                    textFieldList[i].SetValueWithoutNotify(text);
                    i++;
                }
            }
        }
        public override string ToJson()
        {
            textList = new();
            foreach ( var item in textFieldList )
            {
                textList.Add(item.text);
            }
            return JsonUtility.ToJson(this);
        }
	}
}