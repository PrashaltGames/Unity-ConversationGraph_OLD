using Prashalt.Unity.ConversationGraph.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PropertiesPopupWindow : PopupWindowContent
{
	private const string elementPath = ConversationGraphEditorUtility.packageFilePath + "Editor/UXML/PropertiesPopup";

	public override Vector2 GetWindowSize()
	{
		return new(200, 100);
	}

	public override void OnGUI(Rect rect)
	{
		
	}

	public override void OnOpen()
	{
		var elementAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(elementPath);
		var element = elementAsset.Instantiate();
		editorWindow.rootVisualElement.Add(element);
	}
}
