//using Cysharp.Threading.Tasks;
//using Packages.com.prashalt.unity.conversationgraph.Animation;
//using Prashalt.Unity.ConversationGraph.Animation;
//using System;
//using System.Collections.Generic;
//using System.Reflection;
//using System.Text.RegularExpressions;
//using TMPro;
//using UnityEngine;

//namespace Prashalt.Unity.ConversationGraph.Components.Base
//{
//    public abstract class ConversationSystemBase : MonoBehaviour
//    {
//        [Header("Data")]
//        [SerializeField] protected ConversationGraphAsset conversationAsset;

//        public bool IsBusy { get; private set; } = false;
//        public Func<ConversationData, UniTask> OnNodeChangeEvent { get; set; }
//        public Func<ConversationData, UniTask> OnShowOptionsEvent { get; set; }
//        public Action OnConversationFinishedEvent { get; set; }
//        public Action OnConversationStartEvent { get; set; }
//        public Action<ConversationData> OnStartNodeEvent { get; set; }

//		public IReadOnlyList<ConversationData> TextHistory
//		{
//			get
//			{
//				return textHistory;
//			}
//		}

//		private bool _isLogicMode = false;
//        private bool _isLogicEnd = false;
//        protected int optionId;
//        protected Animation.ConversationAnimationGenerator letterAnimation;
//		protected List<ConversationData> textHistory = new();

//		private bool isFinishInit;

//        protected virtual void Start()
//        {
//            isFinishInit = true;
//        }
//        /// <summary>
//        /// Asset Set Method
//        /// </summary>
//        /// <param name="asset"></param>
//        /// <returns>成功しているか（assetがnullの時は失敗する）</returns>
//        public bool SetConversationAsset(ConversationGraphAsset asset)
//        {
//            if(asset is null)
//            {
//                return false;
//            }

//            conversationAsset = asset;
//            return true;
//        }
//        public async void StartConversation()
//        {
//            if (IsBusy) return;
//            await UniTask.WaitUntil(() => isFinishInit);

//            if(conversationAsset is null)
//            {
//                return;
//            }

//			OnConversationStartEvent?.Invoke();
//			await ProccesConversationAsset(conversationAsset);
//        }
//        private async UniTask ProccesConversationAsset(ConversationGraphAsset asset)
//        {
//			IsBusy = true;

//			var previousNodeData = asset.StartNode;
//			var startNodeData = JsonUtility.FromJson<ConversationData>(previousNodeData.json);
//            OnStartNodeEvent?.Invoke(startNodeData);

//			while (true)
//			{
//				var nodeDataList = asset.GetNextNode(previousNodeData);
//				if (nodeDataList.Count <= 0)
//				{
//					Debug.LogError("次のノードが取得できませんでした。");
//					return;
//				}
//				int nodeCount = 0;
//				_isLogicEnd = true;
//				foreach (var nodeData in nodeDataList)
//				{
//					//Logic系（Selectは例外的に含む：修正必要かも）の時はその番号のみを再生する
//					if (_isLogicMode && optionId != nodeCount)
//					{
//						nodeCount++;

//						continue;
//					}

//					var typeName = nodeData.typeName.Split(".")[4];
//					//ノードを分析
//					switch (typeName)
//					{
//						case "Conversation":
//							await OnConversationNode(nodeData);
//							break;
//						case "Logic":
//							OnLogicNode(nodeData);
//							break;
//                        case "SubGraphNode":
//                            await OnSubGraphNode(nodeData);
//                            break;
//						//EndNodeのとき
//						case "EndNode":
//							OnConversationFinishedEvent.Invoke();
//							IsBusy = false;
//							return;
//					}
//					nodeCount++;
//					previousNodeData = nodeData;
//				}
//				if (_isLogicEnd)
//				{
//					_isLogicMode = false;
//				}
//			}
//		}
//        private async UniTask OnConversationNode(NodeData nodeData)
//        {
//			var conversationData = JsonUtility.FromJson<ConversationData>(nodeData.json);

//			if (nodeData.typeName.Split(".")[5] == "SelectNode")
//			{
//				await OnShowOptionsEvent.Invoke(conversationData);
//                _isLogicMode = true;
//                _isLogicEnd = false;
//			}
//			else
//			{
//				await OnNodeChangeEvent.Invoke(conversationData);
//			}
//		}
//        private void OnLogicNode(NodeData nodeData)
//        {
//            //データを読み取る。
//			var logicData = JsonUtility.FromJson<LogicData>(nodeData.json);

//            var boolGuid = logicData.inputNodeGuids[0];
//            var boolNodeData = conversationAsset.FindNode(boolGuid);
//            var boolNode = JsonUtility.FromJson<PropertyData>(boolNodeData.json);

//            //boolNodeの情報を取得
//            var boolInfo = ConversationGraphUtility.ConversationProperties[boolNode.memberName];
//            bool value = false;
//            if(boolInfo is PropertyInfo property)
//            {
//                value = (bool)property.GetValue(null);
//            }
//            else if(boolInfo is FieldInfo field)
//            {
//                value = (bool)field.GetValue(null);
//            }

//			if (value)
//            {
//                optionId = 0;
//			}
//            else
//            {
//                optionId = 1;
//			}
//            _isLogicMode = true;
//		}
//        public async UniTask OnSubGraphNode(NodeData nodeData)
//        {
//			//データを読み取る。
//			var subGraphData = JsonUtility.FromJson<SubGraphData>(nodeData.json);

//            await ProccesConversationAsset(subGraphData.subGraph);
//		}
//        protected async UniTask WaitClick()
//        {
//#if ENABLE_LEGACY_INPUT_MANAGER
//            await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
//#elif ENABLE_INPUT_SYSTEM
        
//#endif
//        }
//        protected string ReflectProperty(string text)
//        {
//            if (text is null || text == "") return "";
//			var Matches = new Regex(@"\{(.+?)\}").Matches(text);

//			foreach (Match propertyNameMatch in Matches)
//			{
//                //正規表現分からないので、ゴリ押す
//                var propertyName = propertyNameMatch.Value.Replace("{", "");
//				propertyName = propertyName.Replace("}", "");

//				var hasProperty = ConversationGraphUtility.ConversationProperties.TryGetValue(propertyName, out var member);
//                if(!hasProperty)
//                {
//                    Debug.LogError("Property is missing maybe");
//                    continue;
//                }
//                string value = ""; 
//                if(member is PropertyInfo property) 
//                {
//                    value = property.GetValue(null).ToString();
//                }
//                else if(member is FieldInfo field)
//                {
//                    value = field.GetValue(null).ToString();
//                }
//				text = text.Replace($"{{{propertyName}}}", value);
//			}
//            return text;
//		}

//		//ソースジェネレーターチャンス
//		protected LetterAnimation GetLetterAnimation(AnimationData animationData, TextMeshProUGUI text)
//		{
//			var animation = animationData.animationName switch
//			{
//				nameof(LetterFadeInAnimation) => new LetterFadeInAnimation(),
//				nameof(LetterFadeInOffsetYAnimation) => new LetterFadeInOffsetYAnimation(),
//				_ => null
//			};

//			SetAnimationProperty(animationData, animation);
//			return animation;
//		}
//        protected ObjectAnimation GetObjectAnimation(AnimationData animationData, Transform text)
//        {
//			var animation = animationData.animationName switch
//			{
//				nameof(ObjectShakeAnimation) => new ObjectShakeAnimation(),
//				_ => null
//			};

//			SetAnimationProperty(animationData, animation);
//			return animation;
//		}
//		private void SetAnimationProperty(AnimationData animationData, Animation.ConversationAnimationGenerator animation)
//		{
//			//animationのプロパティを登録
//			var intIndex = 0;
//			var floatIndex = 0;

//			foreach (var info in animation.GetType().GetFields())
//			{
//				if (info.FieldType == typeof(int))
//				{
//					info.SetValue(animation, animationData.intProperties[intIndex]);
//					intIndex++;
//				}
//				else if (info.FieldType == typeof(float))
//				{
//					info.SetValue(animation, animationData.floatProperties[floatIndex]);
//					floatIndex++;
//				}
//			}
//		}
//	}

//}
