using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;

namespace Prashalt.Unity.ConvasationGraph.Editor
{
    public class PrashaltConvasationWindow : EditorWindow
    {
        public ConvasationGraphAsset ConvasationGraphAsset { set; get; }
        public PrashaltConvasationGraph convasationGraphView;



        public void Open(ConvasationGraphAsset convasationGraphAsset)
        {
            ConvasationGraphAsset = convasationGraphAsset;
            var graphView = new PrashaltConvasationGraph(this);
            rootVisualElement.Add(graphView);

            var toolvar = new Toolbar();
            var saveButton = new ToolbarButton(OnSave) { text = "Save", name = "save-button" };
            toolvar.Add(saveButton);
            rootVisualElement.Add(toolvar);

            Show();
        }
        private void OnEnable()
        {

        }

        [OnOpenAsset()]
        public static bool OnOpenAsset(int instanceId, int _)
        {
            if (EditorUtility.InstanceIDToObject(instanceId) is ConvasationGraphAsset)
            {
                var convasationGraphAsset = EditorUtility.InstanceIDToObject(instanceId) as ConvasationGraphAsset;

                if (HasOpenInstances<PrashaltConvasationWindow>())
                {
                    var window = GetWindow<PrashaltConvasationWindow>(convasationGraphAsset.name, typeof(SceneView));

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
                    var window = GetWindow<PrashaltConvasationWindow>(convasationGraphAsset.name, typeof(SceneView));

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
                if (node is not MasterNode) continue;
                ConvasationGraphAsset.SaveNode(ConvasationGraphEditorUtility.NodeToData(node as MasterNode));
            }

            ConvasationGraphAsset.ClearEdges();
            foreach(var edge in convasationGraphView.edges)
            {
                var edgeData = ConvasationGraphEditorUtility.EdgeToData(edge);
                if (edgeData is null) continue;

                ConvasationGraphAsset.SaveEdge(edgeData);
            }
            EditorUtility.SetDirty(ConvasationGraphAsset);
            AssetDatabase.SaveAssets();
        }
    }

}