using Cysharp.Threading.Tasks;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;

namespace Prashalt.Unity.ConversationGraph.Editor
{
    public class PrashaltConversationWindow : EditorWindow
    {
        public ConversationGraphAsset ConvasationGraphAsset { set; get; }
        public PrashaltConversationGraph convasationGraphView;

        private bool isAssetSet = false;

        public void Open(ConversationGraphAsset convasationGraphAsset)
        {
            ConvasationGraphAsset = convasationGraphAsset;

            isAssetSet = true;

            Show();
        }
        private async void OnEnable()
        {
            await UniTask.WaitUntil(() => isAssetSet);
            var graphView = new PrashaltConversationGraph(this);
            rootVisualElement.Add(graphView);

            var toolvar = new Toolbar();
            var saveButton = new ToolbarButton(OnSave) { text = "Save", name = "save-button" };
            toolvar.Add(saveButton);
            rootVisualElement.Add(toolvar);
        }

        [OnOpenAsset()]
        public static bool OnOpenAsset(int instanceId, int _)
        {
            if (EditorUtility.InstanceIDToObject(instanceId) is ConversationGraphAsset)
            {
                var convasationGraphAsset = EditorUtility.InstanceIDToObject(instanceId) as ConversationGraphAsset;

                if (HasOpenInstances<PrashaltConversationWindow>())
                {
                    var window = GetWindow<PrashaltConversationWindow>(convasationGraphAsset.name, typeof(SceneView));

                    if (window.ConvasationGraphAsset == null)
                    {
                        window.Open(convasationGraphAsset);
                        return true;
                    }

                    if (window.ConvasationGraphAsset.GetInstanceID() == convasationGraphAsset.GetInstanceID())
                    {
                        window.Focus();
                        return false;
                    }
                    else
                    {
                        // TODO:êÿÇËë÷Ç¶ëOÇ…ï€ë∂
                        window.Open(convasationGraphAsset);
                        window.titleContent.text = convasationGraphAsset.name;
                        window.Focus();
                        return false;
                    }
                }
                else
                {
                    // êVãKwindowçÏê¨
                    var window = GetWindow<PrashaltConversationWindow>(convasationGraphAsset.name, typeof(SceneView));

                    window.Open(convasationGraphAsset);
                    return true;
                }
            }

            return false;
        }
        public void OnSave()
        {
            if (ConvasationGraphAsset is null) return;

            ConvasationGraphAsset.ClearNodes();

            foreach(var node in convasationGraphView.nodes)
            {
                if (node is MasterNode)
                {
                    ConvasationGraphAsset.SaveNode(ConversationGraphEditorUtility.NodeToData(node as MasterNode));
                }
                else if(node is GraphInspectorNode graphInspector)
                {
                    ConvasationGraphAsset.settings.isNeedClick = graphInspector.isNeedClick;
                    ConvasationGraphAsset.settings.shouldTextAnimation = graphInspector.shouldTextAnimation;
                    ConvasationGraphAsset.settings.time = graphInspector.time;
                }
            }

            ConvasationGraphAsset.ClearEdges();
            foreach(var edge in convasationGraphView.edges)
            {
                var edgeData = ConversationGraphEditorUtility.EdgeToData(edge);
                if (edgeData is null) continue;

                ConvasationGraphAsset.SaveEdge(edgeData);
            }

            EditorUtility.SetDirty(ConvasationGraphAsset);
            AssetDatabase.SaveAssets();
        }
    }

}