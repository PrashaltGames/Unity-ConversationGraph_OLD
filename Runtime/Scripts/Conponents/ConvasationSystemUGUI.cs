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


    private AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        OnTextChangeAction += OnTextChange;
        StartConvasation();
    }

    protected async UniTask OnTextChange(ConvasationData data)
    {
        if (data.text == null || data.text == "") return;

        //Update Text
        speaker.text = data.speakerName;

        mainText.maxVisibleCharacters = 0;
        mainText.text = data.text;
        for(var i = 1; i < mainText.text.Length; i++)
        {
            mainText.maxVisibleCharacters = i;
            await UniTask.Delay(100);
        }
        await WaitClick();
        audioSource.Play();
    }
}
