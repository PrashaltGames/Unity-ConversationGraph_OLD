using Cysharp.Threading.Tasks;
using Prashalt.Unity.ConversationGraph;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConversationSystemBase : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private ConversationGraphAsset conversationAsset;

    protected Func<ConversationData, UniTask> OnNodeChangeAction;
    protected Action OnConversationFinishedAction;
    public async void StartConversation()
    {
        var previousNodeData = conversationAsset.StartNode;
        for (var i = 0; i < conversationAsset.Nodes.Count; i++)
        {
            var nodeDataList = conversationAsset.GetNextNode(previousNodeData);
            int nodeCount = 0;
            foreach (var nodeData in nodeDataList)
            {
                var data = JsonUtility.FromJson<ConversationData>(nodeData.json);
                await OnNodeChangeAction.Invoke(data);

                nodeCount++;
                previousNodeData = nodeData;
            }
            i += nodeCount;
        }
        OnConversationFinishedAction.Invoke();
    }
    
    protected async UniTask WaitClick()
    {
#if ENABLE_LEGACY_INPUT_MANAGER
        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
#elif ENABLE_INPUT_SYSTEM
        
#endif
    }
}
