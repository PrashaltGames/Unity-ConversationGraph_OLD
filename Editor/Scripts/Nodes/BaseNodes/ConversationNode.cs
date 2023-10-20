using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class ConversationNode : MasterNode
{
	[SerializeField] protected List<string> textList;

	[NonSerialized] protected VisualElement selectedTextField;
	[NonSerialized] protected VisualElement buttonContainer;
	[NonSerialized] protected TemplateContainer defaultContainer;
	[NonSerialized] protected List<TextField> textFieldList;

	public ConversationNode(string elementPath)
	{
		// 入力用のポートを作成
		var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
		inputPort.portName = "Input";
		inputContainer.Add(inputPort);

		//MainContainerをテンプレートからコピー
		var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(elementPath);
		defaultContainer = visualTree.Instantiate();
		mainContainer.Add(defaultContainer);

		var removeOptionButton = mainContainer.Q<Button>("removeButton");
		removeOptionButton.clicked += OnRemoveTextButton;
	}
	public void OnRemoveTextButton()
	{
		if (textFieldList.Count - 1 <= 0)
		{
			return;
		}
		defaultContainer.Remove(selectedTextField);
		textFieldList.Remove(selectedTextField.Q<TextField>());
	}
}
