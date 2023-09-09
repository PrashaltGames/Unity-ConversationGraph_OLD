using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Prashalt.Unity.ConversationGraph.Editor
{
    public class PrashaltConversationWindow : EditorWindow
    {
        public ConversationGraphAsset ConvasationGraphAsset { set; get; }
        public PrashaltConversationGraph convasationGraphView;

        public static List<PrashaltConversationWindow> activeWindowList = new();

        private bool isAssetSet = false;

        private const string iconPath = ConversationGraphEditorUtility.packageFilePath + "Editor/Icon/ConversationGraphTab.png";

        public void Open(ConversationGraphAsset convasationGraphAsset)
        {
            ConvasationGraphAsset = convasationGraphAsset;

            isAssetSet = true;

            Show();
        }
        private async void OnEnable()
        {
            rootVisualElement.Clear();

            await UniTask.WaitUntil(() => isAssetSet);
            var graphView = new PrashaltConversationGraph(this);
            rootVisualElement.Add(graphView);

            var toolvar = new Toolbar();
            var saveButton = new ToolbarButton(OnSave) { text = "Save", name = "save-button" };
            toolvar.Add(saveButton);
            rootVisualElement.Add(toolvar);
        }
        private void OnDisable()
        {
            activeWindowList.Remove(this);
        }
        [OnOpenAsset()]
        public static bool OnOpenAsset(int instanceId, int _)
        {
            if (EditorUtility.InstanceIDToObject(instanceId) is ConversationGraphAsset)
            {
                var conversationGraphAsset = EditorUtility.InstanceIDToObject(instanceId) as ConversationGraphAsset;

                if (HasOpenInstances<PrashaltConversationWindow>())
                {
                    foreach (var window in activeWindowList)
                    {
                        if (window.ConvasationGraphAsset.GetInstanceID() == conversationGraphAsset.GetInstanceID())
                        {
                            window.Focus();
                            return false;
                        }
                    }
                    CreateNewWindow(conversationGraphAsset);
                    return false;
                }
                else
                {
                    // êVãKwindowçÏê¨
                    CreateNewWindow(conversationGraphAsset);
                    return true;
                }
            }

            return false;
        }
        public void OnSave()
        {
            if (ConvasationGraphAsset is null) return;

            ConvasationGraphAsset.ClearNodes();
            ConvasationGraphAsset.ClearEdges();

            foreach (var node in convasationGraphView.nodes)
            {
                if (node is MasterNode masterNode)
                {
                    var nodeData = ConversationGraphEditorUtility.NodeToData(masterNode);
                    ConvasationGraphAsset.SaveNode(nodeData);
                }
                else if(node is GraphInspectorNode graphInspector)
                {
                    ConvasationGraphAsset.settings.isNeedClick = graphInspector.isNeedClick;
                    ConvasationGraphAsset.settings.shouldTextAnimation = graphInspector.shouldTextAnimation;
                    ConvasationGraphAsset.settings.switchingSpeed = graphInspector.switchingSpeed;
                    ConvasationGraphAsset.settings.animationSpeed = graphInspector.animationSpeed;
                }
            }

            ConvasationGraphAsset.ClearEdges();
            foreach (var edge in convasationGraphView.edges)
            {
                var edgeData = ConversationGraphEditorUtility.EdgeToData(edge);
                if (edgeData is null) continue;

                var inputOptionId = 0;
                foreach (var parentChild in edge.input.parent.Children())
                {
                    if (parentChild == edge.input)
                    {
                        break;
                    }
                    inputOptionId++;
                }
                edgeData.targetNodeGuid += $":{inputOptionId}";

                var outputOptionId = 0;
                foreach (var parentChild in edge.output.parent.Children())
                {
                    if (parentChild == edge.output)
                    {
                        break;
                    }
                    outputOptionId++;
                }
                edgeData.baseNodeGuid += $":{outputOptionId}";

                ConvasationGraphAsset.SaveEdge(edgeData);
            }

            EditorUtility.SetDirty(ConvasationGraphAsset);
            AssetDatabase.SaveAssets();
        }
        private static void CreateNewWindow(ConversationGraphAsset conversationGraphAsset)
        {
            var newWindow = CreateWindow<PrashaltConversationWindow>(typeof(SceneView));
            var icon = AssetDatabase.LoadAssetAtPath<Texture>(iconPath);

            newWindow.Open(conversationGraphAsset);
            newWindow.titleContent = new(conversationGraphAsset.name, icon);
            newWindow.Focus();

            activeWindowList.Add(newWindow);
        }
    }

}