using Prashalt.Unity.ConversationGraph;
using Prashalt.Unity.ConversationGraph.Attributes;
using Prashalt.Unity.ConversationGraph.Editor;
using Prashalt.Unity.ConversationGraph.Nodes.Property;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_2022_1_OR_NEWER
#else
using UnityEditor.UIElements;
#endif

public class GraphInspectorNode : Node
{
	public PrashaltConversationGraph GraphView { get; private set; }
	
    private VisualElement _propertiesContainer;

	private const string PropertyButtonElementPath = ConversationGraphEditorUtility.packageFilePath + "Editor/UXML/PropertyButton.uxml";
	private const string ElementPath = ConversationGraphEditorUtility.packageFilePath + "Editor/UXML/GraphInspector.uxml";

	public GraphInspectorNode(PrashaltConversationGraph graphView)
	{
		title = "Graph Inspector";

		GraphView = graphView;
		//右上のボタンとinputContainerを消す
		titleButtonContainer.Q<VisualElement>().style.display = DisplayStyle.None;

		//エレメントを追加
		var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(ElementPath);
		var defaultContainer = visualTree.Instantiate();
		mainContainer.Add(defaultContainer);

		_propertiesContainer = mainContainer.Q<VisualElement>("propertiesContainer");

		//Propertiesの設定
		var propertyButtonAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PropertyButtonElementPath);
		foreach (var conversationProperty in ConversationGraphUtility.ConversationProperties)
		{
			var isBool = IsMemberInfoTypeBool(conversationProperty.Value);

			GenerateButton(conversationProperty.Value, propertyButtonAsset, _propertiesContainer, isBool);
		}
	}

	#region PropertiesMethods
	public static IEnumerable<System.Reflection.TypeInfo> GetHasConversationPropertyClasses(Assembly assembly)
	{
		foreach (Type type in assembly.GetTypes())
		{
			if (type.GetCustomAttributes(typeof(HasConversationPropertyAttribute), true).Length > 0)
			{
				yield return type.GetTypeInfo();
			}
		}
	}
    public bool IsMemberInfoTypeBool(MemberInfo member)
    {
		if (member is PropertyInfo property)
		{
			if (property.GetValue(null) is bool)
			{
				return true;
			}
			else
			{
                return false;
			}
		}
		else if (member is FieldInfo field)
		{
			if (field.GetValue(null) is bool)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
        else { return false; }
	}
    public void GenerateButton(MemberInfo info, VisualTreeAsset propertyButtonAsset, VisualElement container, bool active)
    {
		var propertyButton = propertyButtonAsset.Instantiate();
        var button = propertyButton.Q<Button>();

		button.text = info.Name;
        button.clicked += () => GeneratePropertyNode(info);
        button.SetEnabled(active);

		container.Add(propertyButton);
	}
    public void GeneratePropertyNode(MemberInfo info)
	{
        // マウスの位置にノードを追加
        var node = new FlagNode();
        node.SetTitle(info.Name);
		var worldMousePosition = GraphView._window.rootVisualElement.ChangeCoordinatesTo(GraphView._window.rootVisualElement.parent, GUIUtility.GUIToScreenPoint(Event.current.mousePosition) - GraphView._window.position.position);
		var localMousePosition = GraphView.WorldToLocal(worldMousePosition);
		var nodePosition = new Rect(localMousePosition, new Vector2(100, 100));
		node.Initialize(node.guid, nodePosition, "");
        GraphView.AddElement(node);
	}
	#endregion
}
