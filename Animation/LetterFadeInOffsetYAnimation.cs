using MagicTween;
using Prashalt.Unity.ConversationGraph.Animation;
using TMPro;
using UnityEngine;

public class LetterFadeInOffsetYAnimation : LetterFadeInAnimation
{
	public int offset = 10; 

	protected override ConversationAnimation GenerateAnimation(int letterIndex, TextMeshProUGUI textMeshPro)
	{
		var offsetYAnimation = textMeshPro.TweenCharOffset(letterIndex, new Vector3(0, offset), animationSpeed).SetDelay(animationSpeed * letterIndex).SetInvert();

		var animation = base.GenerateAnimation(letterIndex, textMeshPro);
		
		animation.Add(offsetYAnimation);

		return animation;
	}
}
