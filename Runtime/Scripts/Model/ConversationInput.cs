using Cysharp.Threading.Tasks;
using System;
using UniRx;
using UnityEngine;

public class ConversationInput
{
	public ReactiveProperty<ConversationState> State { get; set; } = new();
	private bool _onClick = false;
	private bool _enableClick;

	public IObserver<Unit> OnClickObserevr
	{
		get { return _onClickSubject; }
	}
	public IObservable<Unit> OnAnimationSkiped
	{
		get { return _onAnimationSkiped; }
	}
	public IObserver<int> OnSelectOption
	{
		get { return _onSelectOption; }
	}
	private Subject<Unit> _onClickSubject { get; set; } = new();
	private Subject<Unit> _onAnimationSkiped {  get; set; } = new();
	private Subject<int> _onSelectOption {  get; set; } = new();

	internal ConversationInput()
	{
		_onClickSubject.Where(_ => !_enableClick).Subscribe(_ => OnClick());
		State.Subscribe(newState => ChangeState(newState));
		_onSelectOption.Subscribe(index => SelectOption(index));
	}
	private async void ChangeState(ConversationState newState)
	{
		await WaitClick();
		if (newState == ConversationState.WaitClick)
		{
			ConversationLogic.NodeProcess.OnNextNode();
		}
		else if (newState == ConversationState.LetterAnimation)
		{
			_onAnimationSkiped.OnNext(Unit.Default);
			State.Value = ConversationState.WaitClick;
		}
		else if(newState == ConversationState.WaitSelect)
		{

        }
    }
	private void OnClick()
	{
		_onClick = true;
	}
	public async UniTask WaitClick()
	{
		Debug.Log(State);
		await UniTask.WaitUntil(() => _onClick);
		_onClick = false;
		DelayEnableClick();
	}
	private async void DelayEnableClick()
	{
		await UniTask.Delay(200);
		_enableClick = false;
	}
	private void SelectOption(in int optionId)
	{
		ConversationLogic.NodeProcess.optionId = optionId;
		ConversationLogic.NodeProcess.OnNextNode();
	}
}
public enum ConversationState
{
	LetterAnimation,
	WaitClick,
	WaitSelect
}
