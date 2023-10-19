using Cysharp.Threading.Tasks;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

        private bool isLogicMode = false;
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
            if (isBusy) return;
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
                    //Logic系の時はその番号のみを再生する
                    if(isLogicMode && optionId != nodeCount)
                    {
                        nodeCount++;

                        continue;
                    }

                    var typeName = nodeData.typeName.Split(".")[4];
                    //ノードを分析
                    switch (typeName)
                    {
                        case "Conversation":
                            await OnConversationNode(nodeData);
                            break;
                        case "Logic":
                            OnLogicNode(nodeData);
                            break;
                        case "Property":
                            OnPropertyNode(nodeData);
                            break;
                        //EndNodeのとき
                        default:
							OnConversationFinishedEvent.Invoke();
							isBusy = false;
							return;
                    }
                    nodeCount++;
                    previousNodeData = nodeData;
                }
                i += nodeCount;
            }
        }
        private async UniTask OnConversationNode(NodeData nodeData)
        {
			var conversationData = JsonUtility.FromJson<ConversationData>(nodeData.json);

			if (nodeData.typeName.Split(".")[5] == "SelectNode")
			{
				await OnShowOptionsEvent.Invoke(conversationData);
				isLogicMode = true;
			}
			else
			{
				await OnNodeChangeEvent.Invoke(conversationData);
				isLogicMode = false;
			}
		}
        private void OnLogicNode(NodeData nodeData)
        {
            //データを読み取る。
			var logicData = JsonUtility.FromJson<LogicData>(nodeData.json);

            var boolGuid = logicData.inputNodeGuids[0];
            var boolNodeData = conversationAsset.FindNode(boolGuid);
            var boolNode = JsonUtility.FromJson<PropertyData>(boolNodeData.json);

            //boolNodeの情報を取得
            var boolInfo = ConversationGraphUtility.ConversationProperties[boolNode.memberName];
            bool value = false;
            if(boolInfo is PropertyInfo property)
            {
                value = (bool)property.GetValue(null);
            }
            else if(boolInfo is FieldInfo field)
            {
                value = (bool)field.GetValue(null);
            }

			if (value)
            {
                optionId = 0;
			}
            else
            {
                optionId = 1;
			}
            isLogicMode = true;
		}
        private void OnPropertyNode(NodeData nodeData)
        {
			var data = JsonUtility.FromJson<PropertyData>(nodeData.json);
		}

        protected async UniTask WaitClick()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
#elif ENABLE_INPUT_SYSTEM
        
#endif
        }
        protected string ReflectProperty(string text)
        {
            if (text is null || text == "") return "";
			var Matches = new Regex(@"\{(.+?)\}").Matches(text);

			foreach (Match propertyNameMatch in Matches)
			{
                //正規表現分からないので、ゴリ押す
                var propertyName = propertyNameMatch.Value.Replace("{", "");
				propertyName = propertyName.Replace("}", "");

				var member = ConversationGraphUtility.ConversationProperties[propertyName];
                string value = ""; 
                if(member is PropertyInfo property) 
                {
                    value = property.GetValue(null).ToString();
                }
                else if(member is FieldInfo field)
                {
                    value = field.GetValue(null).ToString();
                }
				text = text.Replace($"{{{propertyName}}}", value);
			}
            return text;
		}
    }
}
