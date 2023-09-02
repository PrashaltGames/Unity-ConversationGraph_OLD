using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Prashalt.Unity.ConversationGraph.Conponents.Base
{
    public abstract class ConversationSystemBase : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private ConversationGraphAsset conversationAsset;

        public Func<ConversationData, UniTask> OnNodeChangeAction;
        public Func<ConversationData, UniTask> OnShowOptionsAction;
        public Action OnConversationFinishedAction;

        private bool isSelectMode = false;
        protected int optionId;
        public async void StartConversation()
        {
            var previousNodeData = conversationAsset.StartNode;
            for (var i = 0; i < conversationAsset.Nodes.Count; i++)
            {
                var nodeDataList = conversationAsset.GetNextNode(previousNodeData);
                int nodeCount = 0;
                foreach (var nodeData in nodeDataList)
                {
                    //SelectModeの時はその番号のみを再生する
                    if(isSelectMode && optionId != nodeCount)
                    {
                        Debug.Log($"選択肢でない:{nodeCount}");
                        nodeCount++;

                        continue;
                    }
                    
                    //ノードを分析
                    var data = JsonUtility.FromJson<ConversationData>(nodeData.json);
                    if (nodeData.typeName.Split(".")[4] == "SelectNode")
                    {
                        await OnShowOptionsAction.Invoke(data);
                        isSelectMode = true;
                    }
                    else
                    {
                        await OnNodeChangeAction.Invoke(data);
                        isSelectMode = false;
                    }

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
}
