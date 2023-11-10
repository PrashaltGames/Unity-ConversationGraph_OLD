using Cysharp.Threading.Tasks;
using System;
using UniRx;

public class ConversationInput
{
	internal ConversationState State { get; set; }

	public IObserver<Unit> OnClickObserevr
	{
		get { return _onClickSubject; }
	}
	private Subject<Unit> _onClickSubject { get; set; } = new();

	internal ConversationInput()
	{
		_onClickSubject.Subscribe(_ => OnClick());
	}
	private void OnClick()
	{
		if (State == ConversationState.WaitClick)
		{
			//TODO: 次へ
			//TODO: 次へ
			ConversationLogic.NodeProcess.OnNextNode();
			State = ConversationState.Animation;
		}
		else if(State == ConversationState.Animation)
		{
			//TODO: アニメーションをスキップ
			State = ConversationState.WaitClick;
		}
		else if(State == ConversationState.WaitSelect)
		{
			//TODO: Select
			State = ConversationState.Animation;
		}
	}
	public async UniTask WaitClick()
	{
		State = ConversationState.WaitClick;
		await UniTask.WaitWhile(() => State == ConversationState.WaitClick);
	}
}
internal enum ConversationState
{
	Animation,
	WaitClick,
	WaitSelect
}
