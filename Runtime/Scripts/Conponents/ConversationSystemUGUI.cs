using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using Prashalt.Unity.ConversationGraph.Conponents.Base;

namespace Prashalt.Unity.ConversationGraph.Conponents
{
    [RequireComponent(typeof(AudioSource))]
    public class ConversationSystemUGUI : ConversationSystemBase
    {
        [Header("GUI")]
        [SerializeField] private TextMeshProUGUI mainText;
        [SerializeField] private TextMeshProUGUI speaker;

        [Header("Parameter")]
        [SerializeField] private int textAnimationSpeed;


        private AudioSource audioSource;

        public void Start()
        {
            audioSource = GetComponent<AudioSource>();
            OnNodeChangeAction += OnNodeChange;
            OnShowOptionsAction += OnShowOptions;
            OnConversationFinishedAction += OnConvasationFinished;
        }

        private async UniTask OnNodeChange(ConversationData data)
        {
            if (data.textList == null || data.textList.Count == 0) return;

            //Update Text
            speaker.text = data.speakerName;

            foreach (var text in data.textList)
            {
                audioSource.Play();

                mainText.maxVisibleCharacters = 0;
                mainText.text = text;

                //アニメーション
                for (var i = 1; i <= mainText.text.Length; i++)
                {
                    mainText.maxVisibleCharacters = i;
                    await UniTask.Delay(textAnimationSpeed);
                    //TODO クリックしてたら全部にする
                }

                await WaitClick();
                audioSource.Stop();
            }
        }
        private async UniTask OnShowOptions(ConversationData data)
        {
            foreach(var option in data.textList)
            {
                
            }
        }
        private void OnConvasationFinished()
        {
            speaker.text = "";
            mainText.text = "";
        }
    }

}