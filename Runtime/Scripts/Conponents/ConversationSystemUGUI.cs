using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using Prashalt.Unity.ConversationGraph.Components.Base;
using UnityEngine.UI;
using MagicTween;
using System.Collections.Generic;
using Packages.com.prashalt.unity.conversationgraph.Animation;
using Prashalt.Unity.ConversationGraph.Animation;
using System;

namespace Prashalt.Unity.ConversationGraph.Components
{
    [RequireComponent(typeof(AudioSource))]
    public class ConversationSystemUGUI : ConversationSystemBase
    {
        [Header("GUI-Text")]
        [SerializeField] private TextMeshProUGUI mainText;
        [SerializeField] private TextMeshProUGUI speaker;
        [Header("GUI-Option")]
        [SerializeField] private GameObject optionObjParent;
        [SerializeField] private GameObject optionPrefab;

        private AudioSource audioSource;
        private bool isOptionSelected = false;
        private bool isSkipText;
        private bool isStartAnimation = false;
        private bool isWaitClick = false;

        protected override void Start()
        {
            audioSource = GetComponent<AudioSource>();
            OnNodeChangeEvent += OnNodeChange;
            OnShowOptionsEvent += OnShowOptions;
            OnConversationFinishedEvent += OnConvasationFinished;
            OnStartNodeEvent += OnStartNode;

            base.Start();
        }

        private void Update()
        {
            //DIで書き直してもいいかも
#if ENABLE_LEGACY_INPUT_MANAGER
            if(Input.GetMouseButtonDown(0) && isStartAnimation && !isWaitClick)
            {
                isSkipText = true;
            }
#elif ENABLE_INPUT_SYSTEM
        
#endif
        }
        private void OnStartNode(ConversationData data)
        {
			var animationNode = conversationAsset.FindNode(data.animationGuid);
			var animationData = JsonUtility.FromJson<AnimationData>(animationNode.json);
			letterAnimation = GetLetterAnimation(animationData, mainText);
		}
        private async UniTask OnNodeChange(ConversationData data)
        {
			if (data.textList == null || data.textList.Count == 0) return;

            var speakerName = ReflectProperty(data.speakerName);

			//Update Text => MagicTween内のテキスト更新されない…
			speaker.text = speakerName;

            foreach (var text in data.textList)
            {
				isSkipText = false;

				var reflectPropertyText = ReflectProperty(text);
				audioSource.Play();
				//Update Text => MagicTween内のテキスト更新されない…
				mainText.SetText(reflectPropertyText);
                mainText.ForceMeshUpdate();

                if (conversationAsset.settings.shouldTextAnimation)
                {
					// LetterAnimation
					await LetterAnimation();

					if (data.animationGuid != "" && data.animationGuid is not null)
					{
						var animationNode = conversationAsset.FindNode(data.animationGuid);
						var animationData = JsonUtility.FromJson<AnimationData>(animationNode.json);
						var objectAnimation = GetObjectAnimation(animationData, mainText.transform);
						ObjectAnimation(objectAnimation);
					}

					isStartAnimation = false;
                }
                else
                {
                    mainText.maxVisibleCharacters = mainText.text.Length;
                }

                if(conversationAsset.settings.isNeedClick)
                {
                    isWaitClick = true;
                    await WaitClick();
                    DelayEnableSkip();
				}
                else
                {
                    await UniTask.Delay(conversationAsset.settings.switchingSpeed);
                }
                audioSource.Stop();
            }
        }
        protected async UniTask OnShowOptions(ConversationData data)
        {
            int id = 0;
            isOptionSelected = false;
            foreach(var option in data.textList)
            {
                var gameObj = Instantiate(optionPrefab, optionObjParent.transform);

                gameObj.GetComponentInChildren<TextMeshProUGUI>().text = option;

                //値型のはずなのに、新しい変数に格納してからAddListenerしないとなぜか全て値が２になる（＝参照型みたいな動作をする。）
                //これは実行時の値で実行されるから。そりゃそう
                int optionId = id;
                gameObj.GetComponent<Button>().onClick.AddListener(() => OnSelectOptionButton(optionId));
                id++;
            }
            await UniTask.WaitUntil(() => isOptionSelected);
        }
        protected void OnSelectOptionButton(int optionId)
        {
            foreach (Transform button in optionObjParent.transform)
            {
                Destroy(button.gameObject);
            }
            this.optionId = optionId;

            isOptionSelected = true;
        }
        protected void OnConvasationFinished()
        {
            speaker.text = "";
            mainText.text = "";
        }
		private async UniTask PlayAnimation(List<Tween> animations)
		{
			foreach (var animation in animations)
			{
				animation.Play();
			}
            isStartAnimation = true;

			await UniTask.WaitUntil(() => !animations.Exists(x => x.IsPlaying()) || isSkipText);
			mainText.ResetCharTweens();
		}
        public async UniTask LetterAnimation()
        {
			if (letterAnimation is not null)
			{
				//アニメーションを今の文字列の長さで生成
				var tweenList = letterAnimation.SetAnimation();

				//アニメーションを再生
				await PlayAnimation(tweenList);
			}
			else
			{
				mainText.maxVisibleCharacters = 0;
				//アニメーション
				for (var i = 1; i <= mainText.text.Length; i++)
				{
					mainText.maxVisibleCharacters = i;
					await UniTask.Delay(conversationAsset.settings.animationSpeed);

					//クリックしてたら全部にする
					if (isSkipText)
					{
						mainText.maxVisibleCharacters = mainText.text.Length;
						break;
					}
					else
					{
						isStartAnimation = true;
					}
				}
			}
		}
        public void ObjectAnimation(ObjectAnimation animation)
        {
            var animations = animation.SetAnimation();
            animations[0].Play();
        }
        public async void DelayEnableSkip()
        {
            await UniTask.Delay(200);
            isWaitClick = false;
        }
	}
}