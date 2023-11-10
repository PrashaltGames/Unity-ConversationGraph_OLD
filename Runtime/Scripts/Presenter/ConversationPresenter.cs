using UniRx;
using Prashalt.Unity.ConversationGraph;
using System;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine;
using Prashalt.Unity.ConversationGraph.Animation;
using Packages.com.prashalt.unity.conversationgraph.Animation;

public class ConversationPresenter
{
	public ReactiveProperty<ConversationGraphAsset> asset;

	public IObservable<Unit> OnConversationFinishedEvent
	{
		get { return _onConversationFinishedEvent; }
	}
	public IObservable<ConversationInfoWithAnimation> OnConversationNodeEvent
	{
		get { return _onConversationNodeEvent; }
	}
	public IObservable<ConversationData> OnShowOptionsEvent
	{
		get { return _onShowOptionsEvent; }
	}
	public IObserver<ConversationGraphAsset> StartConversationObservable
	{
		get { return _startConversation; }
	}
	public IObserver<NodeData> SetLetterAnimationObserver
	{
		get { return _setLetterAnimationObserver; }
	}
	public LetterAnimation LetterAnimation { get; private set; }

	private Subject<NodeData> _setLetterAnimationObserver;

	//NodeProcess
	private Subject<Unit> _onConversationFinishedEvent = new();
	private Subject<ConversationInfoWithAnimation> _onConversationNodeEvent = new();
	private Subject<ConversationData> _onShowOptionsEvent = new();

	private Subject<ConversationGraphAsset> _startConversation = new();

	private bool _isOptionSelected = false;
	private bool _isSkipText;
	private bool _isStartAnimation = false;
	private bool _isWaitClick = false;
	public ConversationPresenter()
	{
		//会話を始めるメソッドを設定
		_startConversation.Subscribe(asset => ConversationLogic.NodeProcess.StartConversationObserver.OnNext(asset));
		ConversationLogic.NodeProcess.GenerateLetterAnimation.Subscribe(data => SetLetterAnimation(data));

		//次のノード
		

		//処理が終わった時のメソッドを設定
		ConversationLogic.NodeProcess.OnConversationNodeEvent.Subscribe(info => OnChangeText(info));
		ConversationLogic.NodeProcess.OnShowOptionsEvent.Subscribe(data => _onShowOptionsEvent.OnNext(data));
		ConversationLogic.NodeProcess.OnConversationFinishedEvent.Subscribe(data => _onConversationFinishedEvent.OnNext(data));

		//_setLetterAnimationObserver.Subscribe(animationNodeData => SetLetterAnimation(animationNodeData));

#if ENABLE_INPUT_SYSTEM
		var action = new ConversationAction();
		action.Enable();
		action.ClickAction.Click.performed += OnClick;
#elif ENABLE_LEGACY_INPUT_MANAGER
		Observable.EveryUpdate().Subscribe(_ =>
		{
			if (Input.GetMouseButtonDown(0))
			{
				ConversationLogic.ConversationInput.OnClickObserevr.OnNext(Unit.Default);
			}
		});
#endif
	}
#if ENABLE_INPUT_SYSTEM
	private void OnClick(CallbackContext _)
	{
		ConversationLogic.ConversationInput.OnClickObserevr.OnNext(Unit.Default);
	}
#endif

	private async void OnChangeText(ConversationInfoWithAnimationData info)
	{
		Debug.Log(info.AnimationData.animationName);
		var animation = GetObjectAnimation(info.AnimationData);

		var infoWithAnimation = new ConversationInfoWithAnimation(info.ConversationInfo, animation);
		_onConversationNodeEvent.OnNext(infoWithAnimation);
		await ConversationLogic.ConversationInput.WaitClick();
	}
	private void SetLetterAnimation(AnimationData animationData)
	{
		//アニメーションを設定
		LetterAnimation = GetLetterAnimation(animationData);
	}

	//ソースジェネレーターチャンス
	private LetterAnimation GetLetterAnimation(AnimationData animationData)
	{
		var animation = animationData.animationName switch
		{
			nameof(LetterFadeInAnimation) => new LetterFadeInAnimation(),
			nameof(LetterFadeInOffsetYAnimation) => new LetterFadeInOffsetYAnimation(),
			_ => null
		};

		SetAnimationProperty(animationData, animation);
		return animation;
	}
	protected ObjectAnimation GetObjectAnimation(AnimationData animationData)
	{
		var animation = animationData.animationName switch
		{
			nameof(ObjectShakeAnimation) => new ObjectShakeAnimation(),
			_ => null
		};

		SetAnimationProperty(animationData, animation);
		return animation;
	}
	private void SetAnimationProperty(AnimationData animationData, ConversationAnimationGenerator animation)
	{
		//animationのプロパティを登録
		var intIndex = 0;
		var floatIndex = 0;

		foreach (var info in animation.GetType().GetFields())
		{
			if (info.FieldType == typeof(int))
			{
				info.SetValue(animation, animationData.intProperties[intIndex]);
				intIndex++;
			}
			else if (info.FieldType == typeof(float))
			{
				info.SetValue(animation, animationData.floatProperties[floatIndex]);
				floatIndex++;
			}
		}
	}
	public struct ConversationInfoWithAnimation
	{
		public ConversationInfo Info { get; private set; }
		public ConversationAnimationGenerator Animation { get; private set; }

		internal ConversationInfoWithAnimation(ConversationInfo info, ConversationAnimationGenerator animation)
		{
			Info = info;
			Animation = animation;
		}
	}
}
