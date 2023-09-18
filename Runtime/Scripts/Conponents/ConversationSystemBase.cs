using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Prashalt.Unity.ConversationGraph.Conponents.Base
{
    public abstract class ConversationSystemBase : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] protected ConversationGraphAsset conversationAsset;

        public bool isBusy { get; private set; } = false;
        public Func<ConversationData, UniTask> OnNodeChangeEvent { get; set; }
        public Func<ConversationData, UniTask> OnShowOptionsEvent { get; set; }
        public Action OnConversationFinishedEvent { get; set; }
        public Action OnConversationStartEvent { get; set; }

        private bool isSelectMode = false;
        protected int optionId;

        private bool isFinishInit;

        protected virtual void Start()
        {
            isFinishInit = true;
        }
        /// <summary>
        /// Asset Set Method
        /// </summary>
        /// <param name="asset"></param>
        /// <returns>成功しているか（assetがnullの時は失敗する）</returns>
        public bool SetConversationAsset(ConversationGraphAsset asset)
        {
            if(asset is null)
            {
                return false;
            }

            conversationAsset = asset;
            return true;
        }
        public async void StartConversation()
        {
            await UniTask.WaitUntil(() => isFinishInit);
            isBusy = true;

            OnConversationStartEvent?.Invoke();
            
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
                        nodeCount++;

                        continue;
                    }
                    
                    //ノードを分析
                    var data = JsonUtility.FromJson<ConversationData>(nodeData.json);
                    if (nodeData.typeName.Split(".")[4] == "SelectNode")
                    {
                        await OnShowOptionsEvent.Invoke(data);
                        isSelectMode = true;
                    }
                    else
                    {
                        await OnNodeChangeEvent.Invoke(data);
                        isSelectMode = false;
                    }

                    nodeCount++;
                    previousNodeData = nodeData;
                }
                i += nodeCount;
            }
            OnConversationFinishedEvent.Invoke();
            isBusy = false;
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
