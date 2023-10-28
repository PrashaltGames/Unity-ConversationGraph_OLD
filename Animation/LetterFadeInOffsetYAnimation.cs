using MagicTween;
using Prashalt.Unity.ConversationGraph.Animation;
using TMPro;
using UnityEngine;

public class LetterFadeInOffsetYAnimation : LetterFadeInAnimation
{
	public int offset = 10; 
	public LetterFadeInOffsetYAnimation(TextMeshProUGUI textMeshPro) : base(textMeshPro)
	{
	}

	protected override ConversationAnimation GenerateAnimation(int letterIndex)
	{
		var offsetYAnimation = TextMeshPro.TweenCharOffset(letterIndex, new Vector3(0, offset), animationSpeed).SetDelay(animationSpeed * letterIndex).SetInvert();

		var animation = base.GenerateAnimation(letterIndex);
		
		animation.Add(offsetYAnimation);

		return animation;
	}
}
