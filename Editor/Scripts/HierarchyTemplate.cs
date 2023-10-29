using Prashalt.Unity.ConversationGraph.Editor;
using UnityEditor;
using UnityEngine;

public class HierarchyTemplate
{
	[MenuItem("GameObject/ConversationGraph/UGUI", false, 0)]
	public static void CreateTemplateUGUI()
	{
		var asset = AssetDatabase.LoadAssetAtPath<GameObject>(ConversationGraphEditorUtility.packageFilePath + "Assets/Template.prefab");
		Selection.activeObject = asset;
	}
}
