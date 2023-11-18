using Prashalt.Unity.ConversationGraph.Animation;
using Prashalt.Unity.ConversationGraph.Components;
using Prashalt.Unity.ConversationGraph.Editor;
using Prashalt.Unity.ConversationGraph.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public abstract class ConversationNode : MasterNode
{
	[SerializeField] protected List<string> textList;
	[SerializeField] protected string animationGuid;

	[NonSerialized] protected VisualElement selectedTextField;
	[NonSerialized] protected VisualElement buttonContainer;
	[NonSerialized] protected TemplateContainer defaultContainer;
	[NonSerialized] protected List<TextField> textFieldList = new();

	protected string mainText;

	private Port _animationPort;

	public ConversationNode(string elementPath, string mainText)
	{
		this.mainText = mainText;
		// 入力用のポートを作成
		AddInputPort(typeof(float), "Input", Port.Capacity.Multi);

		// 入力用のポートを作成
		_animationPort = AddInputPort(typeof(ObjectAnimation) ,Color.red, "Animation");

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
	public override string ToJson()
	{
		base.ToJson();
		if (_animationPort.connected)
		{
			var edge = _animationPort.connections.FirstOrDefault();

			var animationNode = edge.output.node as MasterNode;
			animationGuid = animationNode.guid;
		}
		return JsonUtility.ToJson(this);
	}
}
