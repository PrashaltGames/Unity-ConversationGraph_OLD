using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using Prashalt.Unity.ConversationGraph.Conponents.Base;
using UnityEngine.UI;
using Prashalt.Unity.ConversationGraph.Animation;
using MagicTween;
using System.Collections.Generic;

namespace Prashalt.Unity.ConversationGraph.Conponents
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
#if ENABLE_LEGACY_INPUT_MANAGER
            if(Input.GetMouseButtonDown(0) && isStartAnimation)
            {
                isSkipText = true;
            }
#elif ENABLE_INPUT_SYSTEM
        
#endif
        }
        private void OnStartNode(ConversationData data)
        {
			letterAnimation = GetAnimation(data.animation.name, mainText);

			//animationのプロパティを登録
			var intIndex = 0;
			var floatIndex = 0;

			foreach (var info in letterAnimation.GetType().GetFields())
			{
				if (info.FieldType == typeof(int))
				{
					info.SetValue(letterAnimation, data.animation.intProperties[intIndex]);
					intIndex++;
				}
				else if (info.FieldType == typeof(float))
				{
					info.SetValue(letterAnimation, data.animation.floatProperties[floatIndex]);
					floatIndex++;
				}
			}
		}
        private async UniTask OnNodeChange(ConversationData data)
        {
            isSkipText = false;
			if (data.textList == null || data.textList.Count == 0) return;

            var speakerName = ReflectProperty(data.speakerName);

			//Update Text => MagicTween内のテキスト更新されない…
			speaker.text = speakerName;

            foreach (var text in data.textList)
            {
				var reflectPropertyText = ReflectProperty(text);
				audioSource.Play();
				//Update Text => MagicTween内のテキスト更新されない…
				mainText.SetText(reflectPropertyText);
                mainText.ForceMeshUpdate();

                if (conversationAsset.settings.shouldTextAnimation)
                {
					if (letterAnimation is not null)
                    {
                        Debug.Log("Animation");

                        //アニメーションを今の文字列の長さで生成
                        var tweenList = await letterAnimation.SetAnimation();

                        //アニメーションを再生
						isStartAnimation = true;
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
                    isStartAnimation = false;
                }
                else
                {
                    mainText.maxVisibleCharacters = mainText.text.Length;
                }
                
                if(conversationAsset.settings.isNeedClick)
                {
                    await WaitClick();
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
			await UniTask.WaitUntil(() => !animations.Exists(x => x.IsPlaying()) || isSkipText);
            mainText.ResetCharTweens();   
		}
	}
}