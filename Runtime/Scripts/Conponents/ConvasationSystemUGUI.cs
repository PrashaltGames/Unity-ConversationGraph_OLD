using Prashalt.Unity.ConvasationGraph;
using UnityEngine;
using TMPro;
using System;

public class ConvasationSystemUGUI : MonoBehaviour
{
    [Header("GUI")]
    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private TextMeshProUGUI speaker;
    [Header("Data")]
    [SerializeField] private ConvasationGraphAsset convasation;

    public void Start()
    {
        StartConvasation();
    }
    public void StartConvasation()    
    {
        var previousNodeData = convasation.StartNode;
        for (var i = 0; i < convasation.Nodes.Count; i++)
        {
            var nodeDataList = convasation.GetNextNode(previousNodeData);
            int nodeCount = 0;
            foreach(var nodeData in nodeDataList)
            {
                var data = JsonUtility.FromJson<ConvasationData>(nodeData.json);
                if (data.text == null || data.text == "") continue;

                mainText.text = data.text;

                nodeCount++;
                previousNodeData = nodeData;
            }
            i += nodeCount;
        }
    }
}
