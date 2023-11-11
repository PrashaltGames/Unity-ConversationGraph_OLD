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
	public IObservable<OptionData> OnAddOption
	{
		get { return _onAddOption; }
	}
	public IObserver<ConversationGraphAsset> StartConversationObservable
	{
		get { return _startConversation; }
	}
	public IObservable<Unit> OnAnimationSkiped
	{
		get { return _onAnimationSkiped; }
	}
	public IObservable<Unit> OnSelecedOption
	{
		get { return _onSelectedOption; }
	}
	public LetterAnimation LetterAnimation { get; private set; }

	//NodeProcess
	private Subject<Unit> _onConversationFinishedEvent = new();
	private Subject<ConversationInfoWithAnimation> _onConversationNodeEvent = new();
	private Subject<OptionData> _onAddOption = new();
	private Subject<Unit> _onSelectedOption = new();

	private Subject<ConversationGraphAsset> _startConversation = new();

	//Input
	private Subject<Unit> _onAnimationSkiped = new();

	public ConversationPresenter()
	{
		//会話を始めるメソッドを設定
		_startConversation.Subscribe(asset => ConversationLogic.NodeProcess.StartConversationObserver.OnNext(asset));
		ConversationLogic.NodeProcess.GenerateLetterAnimation.Subscribe(data => SetLetterAnimation(data));

		//次のノード
		

		//処理が終わった時のメソッドを設定
		ConversationLogic.NodeProcess.OnConversationNodeEvent.Subscribe(info => OnChangeText(info));
		ConversationLogic.NodeProcess.OnShowOptionsEvent.Subscribe(data => AddOptions(data));
		ConversationLogic.NodeProcess.OnConversationFinishedEvent.Subscribe(data => _onConversationFinishedEvent.OnNext(data));
		ConversationLogic.ConversationInput.OnAnimationSkiped.Subscribe(_ => _onAnimationSkiped.OnNext(Unit.Default));

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
	public void OnSelectOption(int optionIndex)
	{
		ConversationLogic.ConversationInput.OnSelectOption.OnNext(optionIndex);
		_onSelectedOption.OnNext(Unit.Default);
	}
#if ENABLE_INPUT_SYSTEM
	private void OnClick(CallbackContext _)
	{
		ConversationLogic.ConversationInput.OnClickObserevr.OnNext(Unit.Default);
	}
#endif

	private void OnChangeText(in ConversationInfoWithAnimationData info)
	{
		var animation = GetObjectAnimation(info.AnimationData);

		var infoWithAnimation = new ConversationInfoWithAnimation(info.ConversationInfo, animation);
		_onConversationNodeEvent.OnNext(infoWithAnimation);
	}
	private void AddOptions(in ConversationData data)
	{
		int id = 0;
		foreach (var option in data.textList)
		{
			var optionData = new OptionData(option, id);
			_onAddOption.OnNext(optionData);
			id++;
		}
	}
	private void SetLetterAnimation(in AnimationData animationData)
	{
		//アニメーションを設定
		LetterAnimation = GetLetterAnimation(animationData);
	}

	//ソースジェネレーターチャンス
	private LetterAnimation GetLetterAnimation(in AnimationData animationData)
	{
		var animation = animationData.animationName switch
		{
			nameof(LetterFadeInAnimation) => new LetterFadeInAnimation(),
			nameof(LetterFadeInOffsetYAnimation) => new LetterFadeInOffsetYAnimation(),
			_ => null
		};
		if (animation is null) return null;

		SetAnimationProperty(animationData, animation);
		return animation;
	}
	protected ObjectAnimation GetObjectAnimation(in AnimationData animationData)
	{
		var animation = animationData.animationName switch
		{
			nameof(ObjectShakeAnimation) => new ObjectShakeAnimation(),
			_ => null
		};
		if(animation is null) return null;

		SetAnimationProperty(animationData, animation);
		return animation;
	}
	private void SetAnimationProperty(in AnimationData animationData, ConversationAnimationGenerator animation)
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
public struct OptionData
{
	internal OptionData(string text, int optionIndex)
	{
		Text = text;
		OptionIndex = optionIndex;
	}

	public string Text { get; private set; }
	public int OptionIndex { get; private set; }
}
