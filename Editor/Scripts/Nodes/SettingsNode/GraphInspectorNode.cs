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
    public bool shouldTextAnimation { get; private set; } = true;
    public bool isNeedClick { get; private set; } = true;
    public int switchingSpeed { get; private set; }
    public int animationSpeed { get; private set; }

    public PrashaltConversationGraph GraphView { get; private set; }

    private IntegerField timeToWaitField;
    private IntegerField animationSpeedField;

    private VisualElement graphSettingsContainer;
    private VisualElement propertiesContainer;

	private const string propertyButtonElementPath = ConversationGraphEditorUtility.packageFilePath + "Editor/UXML/PropertyButton.uxml";
#if UNITY_2022_1_OR_NEWER
	private const string elementPath = ConversationGraphEditorUtility.packageFilePath + "Editor/UXML/GraphInspector.uxml";
#else
    private const string elementPath = ConversationGraphEditorUtility.packageFilePath + "Editor/UXML/GraphInspector2021.uxml";
#endif
    public GraphInspectorNode(ConversationGraphAsset asset, PrashaltConversationGraph graphView)
    {
		title = "Graph Inspector";

        //右上のボタンとinputContainerを消す
        titleButtonContainer.Q<VisualElement>().style.display = DisplayStyle.None;

		//エレメントを追加
		var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(elementPath);
        var defaultContainer = visualTree.Instantiate();
        mainContainer.Add(defaultContainer);

		//上部のボタンの設定
		var graphSettingsButton = mainContainer.Q<Toolbar>().Q<Button>("graphSettings");
		var propertiesButton = mainContainer.Q<Toolbar>().Q<Button>("properties");

        graphSettingsButton.RegisterCallback<ClickEvent>(x => OnChangeToSettingsMode(x));
        propertiesButton.RegisterCallback<ClickEvent>(x => OnChangeToPropertiesMode(x));

        graphSettingsContainer = mainContainer.Q<VisualElement>("graphSettingsContainer");
        propertiesContainer = mainContainer.Q<VisualElement>("propertiesContainer");

		//Propertiesの設定
        var propertyButtonAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(propertyButtonElementPath);
        foreach(var conversationProperty in ConversationGraphUtility.ConversationProperties)
        {
            var isBool = IsMemberInfoTypeBool(conversationProperty.Value);

			GenerateButton(conversationProperty.Value, propertyButtonAsset, propertiesContainer, isBool);
		}

		//GraphSettingsの設定
		timeToWaitField = defaultContainer.Q<IntegerField>("timeToWait");
        timeToWaitField.RegisterValueChangedCallback(x => OnChangeTimeToWait(x));
        timeToWaitField.SetValueWithoutNotify(asset.settings.switchingSpeed);
        switchingSpeed = asset.settings.switchingSpeed;

        animationSpeedField = defaultContainer.Q<IntegerField>("animationSpeed");
        animationSpeedField.RegisterValueChangedCallback(x => OnChangeAnimationSpeed(x));
        animationSpeedField.SetValueWithoutNotify(asset.settings.animationSpeed);
        animationSpeed = asset.settings.animationSpeed;

        var textAnimationToggle = defaultContainer.Q<Toggle>("textAnimation");
        textAnimationToggle.RegisterValueChangedCallback(x => OnChangeTextAnimationSettings(x));
        textAnimationToggle.value = asset.settings.shouldTextAnimation;
        shouldTextAnimation = asset.settings.shouldTextAnimation;
        ChangeStateSwitchingSpeedEnable(shouldTextAnimation);

        var needClickToggle = defaultContainer.Q<Toggle>("needClick");
        needClickToggle.RegisterValueChangedCallback(x => OnChangeNeedClickSettings(x));
        needClickToggle.value = asset.settings.isNeedClick;
        isNeedClick = asset.settings.isNeedClick;
        ChangeStateSwitchingSpeedEnable(isNeedClick);

        capabilities &= ~Capabilities.Deletable;

        GraphView = graphView;
    }
	#region SettingsMethods
	public void OnChangeTextAnimationSettings(ChangeEvent<bool> e)
    {
        shouldTextAnimation = e.newValue;
        ChangeStateAnimationSpeedEnable(shouldTextAnimation);
    }
    public void OnChangeNeedClickSettings(ChangeEvent<bool> e)
    {
        isNeedClick = e.newValue;
        ChangeStateSwitchingSpeedEnable(isNeedClick);
    }
    public void ChangeStateSwitchingSpeedEnable(bool value)
    {
        if (value == true)
        {
            timeToWaitField.SetEnabled(false);
        }
        else
        {
            timeToWaitField.SetEnabled(true);
        }
    }
    public void ChangeStateAnimationSpeedEnable(bool value)
    {
        if (value == true)
        {
            animationSpeedField.SetEnabled(true);
        }
        else
        {
            animationSpeedField.SetEnabled(false);
        }
    }
    public void OnChangeTimeToWait(ChangeEvent<int> e)
    {
        switchingSpeed = e.newValue;
    }
    public void OnChangeAnimationSpeed(ChangeEvent<int> e)
    {
        animationSpeed = e.newValue;
    }
    public void OnChangeToSettingsMode(ClickEvent _)
    {
		ChangeContainer(true);
	}
	public void OnChangeToPropertiesMode(ClickEvent _)
	{
        ChangeContainer(false);
	}
	#endregion
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
	public void ChangeContainer(bool isSettings)
    {
        if(isSettings)
        {
			graphSettingsContainer.style.display = DisplayStyle.Flex;
            propertiesContainer.style.display = DisplayStyle.None;
		}
        else
        {
			graphSettingsContainer.style.display = DisplayStyle.None;
			propertiesContainer.style.display = DisplayStyle.Flex;
		}
	}
}
