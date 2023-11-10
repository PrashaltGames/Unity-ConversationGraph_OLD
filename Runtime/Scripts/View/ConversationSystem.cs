using Cysharp.Threading.Tasks;
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
	public void Start()
	{
		_conversationPresenter = new();

		_conversationPresenter.OnConversationNodeEvent.Subscribe(data => ChangeTextWithAnimation(data.Info, data.Animation));

		//arrowTweenÇéñëOÇ…çÏê¨ÇµÇƒÇ®Ç≠ÅB
		_arrowTween = arrow.TweenAlpha(0, arrowAnimationSpeed).SetLoops(-1, LoopType.Yoyo).SetAutoPlay(false).SetInvert();
		arrow.alpha = 0;
	}
	public void ChangeTextWithAnimation(ConversationInfo info, ConversationAnimationGenerator animationGenerator)
	{
		speaker.SetText(info.SpeakerName);
		mainText.SetText(info.Text);

		mainText.ForceMeshUpdate();

		var letterAnimation = _conversationPresenter.LetterAnimation.SetAnimation(mainText);
		letterAnimation.Play();

		var animation = animationGenerator.SetAnimation(mainText);
		animation.Play();
	}
	public void AddOption(int optionIndex, string text)
	{

	}
	public void StartConversation(ConversationGraphAsset conversationAsset)
	{
		conversationCanvas.gameObject.SetActive(true);
		_conversationPresenter.StartConversationObservable.OnNext(conversationAsset);
	}
}
