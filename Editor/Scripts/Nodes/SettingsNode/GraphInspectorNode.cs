using Prashalt.Unity.ConversationGraph;
using Prashalt.Unity.ConversationGraph.Editor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class GraphInspectorNode : Node
{
    public bool shouldTextAnimation { get; private set; } = true;
    public bool isNeedClick { get; private set; } = true;
    public int time { get; private set; }

    private IntegerField timeField;
    private const string elementPath = ConversationGraphEditorUtility.packageFilePath + "Editor/UXML/GraphInspector.uxml";
    public GraphInspectorNode(ConversationGraphAsset asset)
    {
        title = "Graph Inspector";

        //ÉGÉåÉÅÉìÉgÇí«â¡
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(elementPath);
        var defaultContainer = visualTree.Instantiate();
        mainContainer.Add(defaultContainer);

        timeField = defaultContainer.Q<IntegerField>("timeToWait");
        timeField.RegisterValueChangedCallback(x => OnChangeTimeToWait(x));
        timeField.SetValueWithoutNotify(asset.settings.time);
        time = asset.settings.time;

        var textAnimationToggle = defaultContainer.Q<Toggle>("textAnimation");
        textAnimationToggle.RegisterValueChangedCallback(x => OnChangeTextAnimationSettings(x));
        textAnimationToggle.value = asset.settings.shouldTextAnimation;
        shouldTextAnimation = asset.settings.shouldTextAnimation;
        ChangeWaitToTime(shouldTextAnimation);

        var needClickToggle = defaultContainer.Q<Toggle>("needClick");
        needClickToggle.RegisterValueChangedCallback(x => OnChangeNeedClickSettings(x));
        needClickToggle.value = asset.settings.isNeedClick;
        isNeedClick = asset.settings.isNeedClick;

        capabilities &= ~Capabilities.Deletable;
    }
    public void OnChangeTextAnimationSettings(ChangeEvent<bool> e)
    {
        shouldTextAnimation = e.newValue;
        ChangeWaitToTime(shouldTextAnimation);
    }
    public void OnChangeNeedClickSettings(ChangeEvent<bool> e)
    {
        isNeedClick = e.newValue;
    }
    public void ChangeWaitToTime(bool value)
    {
        if (value == true)
        {
            timeField.Q<Label>().text = "Animation Speed (char/msec)";
        }
        else
        {
            timeField.Q<Label>().text = "Time to Wait (msec)";
        }
    }
    public void OnChangeTimeToWait(ChangeEvent<int> e)
    {
        time = e.newValue;
    }
}
