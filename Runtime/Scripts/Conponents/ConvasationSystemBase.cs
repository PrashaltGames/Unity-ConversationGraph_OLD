using Cysharp.Threading.Tasks;
using Prashalt.Unity.ConvasationGraph;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConvasationSystemBase : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private ConvasationGraphAsset convasation;

    protected Func<ConvasationData, UniTask> OnNodeChangeAction;
    protected Action OnConvasationFinishedAction;
    public async void StartConvasation()
    {
        var previousNodeData = convasation.StartNode;
        for (var i = 0; i < convasation.Nodes.Count; i++)
        {
            var nodeDataList = convasation.GetNextNode(previousNodeData);
            int nodeCount = 0;
            foreach (var nodeData in nodeDataList)
            {
                var data = JsonUtility.FromJson<ConvasationData>(nodeData.json);
                await OnNodeChangeAction.Invoke(data);

                nodeCount++;
                previousNodeData = nodeData;
            }
            i += nodeCount;
        }
        OnConvasationFinishedAction.Invoke();
    }
    
    protected async UniTask WaitClick()
    {
#if ENABLE_LEGACY_INPUT_MANAGER
        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
#elif ENABLE_INPUT_SYSTEM
        
#endif
    }
}
