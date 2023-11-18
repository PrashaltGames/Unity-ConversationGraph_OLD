using Prashalt.Unity.ConversationGraph;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using MagicTween;
using Prashalt.Unity.ConversationGraph.Animation;

public class ConversationSystem : MonoBehaviour
{
	[Header("GUI")]
	[SerializeField] private Canvas conversationCanvas;
	[Header("GUI-Text")]
	[SerializeField] private TextMeshProUGUI mainText;
	[SerializeField] private TextMeshProUGUI speaker;
	[Header("GUI-Option")]
	[SerializeField] private GameObject optionObjParent;
	[SerializeField] private GameObject optionPrefab;
	[Header("Other")]
	[SerializeField] private CanvasGroup arrow;
	[SerializeField] private float arrowAnimationSpeed;

	private ConversationPresenter _conversationPresenter;
	private Tween _arrowTween;

	private ConversationAnimation _objAnimation;
	public void Start()
	{
		_conversationPresenter = new();

		_conversationPresenter.OnConversationNodeEvent.Subscribe(data => ChangeTextWithAnimation(data.Info, data.Animation));
		_conversationPresenter.OnAddOption.Subscribe(data => AddOption(data));
		_conversationPresenter.OnSelecedOption.Subscribe(_ => HideOptions());
		_conversationPresenter.OnConversationFinishedEvent.Subscribe(_ => EndConversation());

		_conversationPresenter.OnAnimationSkipped.Subscribe(_ => SkipAnimation());
		_conversationPresenter.OnAnimationSkipped.Subscribe(_ => StartObjectAnimation());

		//arrowTweenを事前に作成しておく。
		_arrowTween = arrow.TweenAlpha(0, arrowAnimationSpeed).SetLoops(-1, LoopType.Yoyo).SetAutoPlay(false).SetInvert();
		arrow.alpha = 0;
	}
	private void ChangeTextWithAnimation(in ConversationInfo info, in ConversationAnimationGenerator animationGenerator)
	{
		//その前のアニメーションを止める
		_objAnimation?.Puase();
		mainText.transform.rotation = Quaternion.identity;

		//矢印もリセット
		_arrowTween.Pause();
		arrow.alpha = 0;

		speaker.SetText(info.SpeakerName);
		mainText.SetText(info.Text);

		mainText.ForceMeshUpdate();

		var letterAnimation = _conversationPresenter.LetterAnimation?.SetAnimation(mainText);
		letterAnimation?.Play();

		_objAnimation = animationGenerator?.SetAnimation(mainText);
	}
	private void HideOptions()
	{
		optionObjParent.SetActive(false);
	}
	private void StartObjectAnimation()
	{
		_objAnimation?.Play();
	}
	private void SkipAnimation()
	{
		_arrowTween.Restart();
		mainText.ResetCharTweens();
	}
	private void AddOption(OptionData data)
	{
		//TODO: オブジェクトプール？
		var gameObj = Instantiate(optionPrefab, optionObjParent.transform);

		gameObj.GetComponentInChildren<TextMeshProUGUI>().text = data.Text;

		gameObj.GetComponent<Button>().onClick.AddListener(() => _conversationPresenter.OnSelectOption(data.OptionIndex));
	}
	public void StartConversation(in ConversationGraphAsset conversationAsset)
	{
		conversationCanvas.gameObject.SetActive(true);
		_conversationPresenter.StartConversationObservable.OnNext(conversationAsset);
	}
	private void EndConversation()
	{
		conversationCanvas.gameObject.SetActive(false);
		foreach(Transform child in optionObjParent.transform)
		{
			Destroy(child.gameObject);
		}
	}
}
