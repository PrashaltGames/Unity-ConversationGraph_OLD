using Prashalt.Unity.ConversationGraph.Components;
using Prashalt.Unity.ConversationGraph.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public abstract class ConversationNode : MasterNode
{
	[SerializeField] protected List<string> textList;

	[NonSerialized] protected VisualElement selectedTextField;
	[NonSerialized] protected VisualElement buttonContainer;
	[NonSerialized] protected TemplateContainer defaultContainer;
	[NonSerialized] protected List<TextField> textFieldList = new();

	protected string mainText;

	public ConversationNode(string elementPath, string mainText)
	{
		this.mainText = mainText;
		// 入力用のポートを作成
		var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
		inputPort.portName = "Input";
		inputContainer.Add(inputPort);

		//MainContainerをテンプレートからコピー
		var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(elementPath);
		defaultContainer = visualTree.Instantiate();
		mainContainer.Add(defaultContainer);

		var textField = new PrashaltTextFieldButton();
		textField.Q<Label>().text = mainText + " 1";
		textField.Q<Button>().clicked += () => SelectTextButton(textField);
		defaultContainer.Add(textField);

		textFieldList.Add(textField.Q<TextField>());
		ConversationGraphEditorUtility.MoveUp(defaultContainer, textField);

		var removeOptionButton = mainContainer.Q<Button>("removeButton");
		removeOptionButton.clicked += OnRemoveTextButton;


		buttonContainer = mainContainer.Q<VisualElement>("buttonContainer");
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
	public void OnRemoveTextButton()
	{
		if (textFieldList.Count - 1 <= 0)
		{
			return;
		}
		defaultContainer.Remove(selectedTextField);
		textFieldList.Remove(selectedTextField.Q<TextField>());

		int i = 0;
		foreach(var textField in textFieldList)
		{
			i++;
			textField.parent.Q<Label>().text = $"{mainText} {i}";
		}
	}
}
