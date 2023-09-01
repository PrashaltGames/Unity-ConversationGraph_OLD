using Prashalt.Unity.ConvasationGraph;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(AudioSource))]
public class ConvasationSystemUGUI : ConvasationSystemBase
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
        OnConvasationFinishedAction += OnConvasationFinished;
        StartConvasation();
    }

    private async UniTask OnNodeChange(ConvasationData data)
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

            audioSource.Stop();
            await WaitClick();
        }
    }
    private void OnConvasationFinished()
    {
        speaker.text = "";
        mainText.text = "";
    }
}
