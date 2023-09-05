using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Prashalt.Unity.ConversationGraph.Editor
{
    [CustomEditor(typeof(ConversationGraphAsset))]
    public class ConversationGraphAssetInspector : UnityEditor.Editor
    {
        private const string elementPath = ConversationGraphEditorUtility.packageFilePath + "";
        public override void OnInspectorGUI()
        {

        }
    }
}
