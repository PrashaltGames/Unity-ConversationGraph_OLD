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
    public class SelectNode : ConversationNode
    {
        [NonSerialized] private List<TextField> selectOptionTextList = new();
        [NonSerialized] private VisualElement buttonContainer;
        [NonSerialized] private TemplateContainer defaultContainer;

        private const string elementPath = ConversationGraphEditorUtility.packageFilePath + "Editor/UXML/SelectNode.uxml";
        public SelectNode() : base()
        {
            title = "Select";

            //出力ポート
            var outputPort = Port.Create<Edge>(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(float));
            outputPort.portName = "Option1";
            outputContainer.Add(outputPort);

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(elementPath);
            defaultContainer = visualTree.Instantiate();
            mainContainer.Add(defaultContainer);

            var textField = new PrashaltTextFieldButton();
            textField.Q<Label>().text = "Option1 Text";
			textField.Q<Button>().clicked += () => SelectTextButton(textField);
			defaultContainer.Add(textField);

            selectOptionTextList.Add(textField.Q<TextField>());

            buttonContainer = mainContainer.Q<VisualElement>("buttonContainer");

            var addOptionButton = mainContainer.Q<Button>("addButton");
            addOptionButton.clicked += OnAddOptionButton;

            var removeOptionButton = mainContainer.Q<Button>("removeButton");
            removeOptionButton.clicked += OnRemoveTextButton;

			ConversationGraphEditorUtility.MoveUp(defaultContainer, textField);

            RefreshExpandedState();
        }
        public void OnAddOptionButton()
        {
            //出力ポートを追加
            var outputPort = Port.Create<Edge>(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(float));
            outputPort.portName = $"Option{outputContainer.childCount + 1}";
            outputContainer.Add(outputPort);

            //入力欄を追加
            var textField = new PrashaltTextFieldButton();
            textField.Q<Label>().text = $"Option{outputContainer.childCount} Text";
            textField.Q<Button>().clicked += () => SelectTextButton(textField);

            defaultContainer.Add(textField);
            ConversationGraphEditorUtility.MoveDown(defaultContainer, buttonContainer);

            selectOptionTextList.Add(textField.Q<TextField>());
        }
		public void OnRemoveTextButton()
		{
			if (selectOptionTextList.Count - 1 <= 1)
			{
				return;
			}
			defaultContainer.Remove(selectedTextField);
			selectOptionTextList.Remove(selectedTextField.Q<TextField>());
		}
		public void SelectTextButton(VisualElement element)
		{
			if (selectedTextField is not null)
			{
				selectedTextField.style.backgroundColor = Color.gray;
			}

			selectedTextField = element;

			selectedTextField.style.backgroundColor = Color.green;
		}

		public override void Initialize(string guid, Rect rect, string json)
        {
            base.Initialize(guid, rect, json);
            var jsonObj = JsonUtility.FromJson<SelectNode>(json);
            int i = 0;
            if (jsonObj is null) return;
            foreach(var text in jsonObj.textList)
            {
                if (i > 0)
                {
                    OnAddOptionButton();
                }
                selectOptionTextList[i].SetValueWithoutNotify(text);
                i++;
            }
        }
        public override string ToJson()
        {
            textList = new();
            foreach(var textField in selectOptionTextList)
            {
                textList.Add(textField.text);
            }
            return JsonUtility.ToJson(this);
        }
    }
}
