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
                    //SelectMode‚ÌŽž‚Í‚»‚Ì”Ô†‚Ì‚Ý‚ðÄ¶‚·‚é
                    if(isSelectMode && optionId != nodeCount)
                    {
                        Debug.Log($"‘I‘ðŽˆ‚Å‚È‚¢:{nodeCount}");
                        nodeCount++;

                        continue;
                    }
                    
                    //ƒm[ƒh‚ð•ªÍ
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
