using Prashalt.Unity.ConversationGraph;
using Prashalt.Unity.ConversationGraph.Editor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphInspectorNode : Node
{
    public bool shouldTextAnimation { get; private set; } = true;
    public bool isNeedClick { get; private set; } = true;
    public int switchingSpeed { get; private set; }
    public int animationSpeed { get; private set; }

    private IntegerField timeToWaitField;
    private IntegerField animationSpeedField;
    private const string elementPath = ConversationGraphEditorUtility.packageFilePath + "Editor/UXML/GraphInspector.uxml";
    public GraphInspectorNode(ConversationGraphAsset asset)
    {
        title = "Graph Inspector";

        //右上のボタンとinputContainerを消す
        titleButtonContainer.Q<VisualElement>().style.display = DisplayStyle.None;

        //エレメントを追加
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(elementPath);
        var defaultContainer = visualTree.Instantiate();
        mainContainer.Add(defaultContainer);

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

        capabilities &= ~Capabilities.Deletable;
    }
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
}
