using MagicTween;
using TMPro;
using UnityEngine;

namespace Prashalt.Unity.ConversationGraph.Animation
{
	public partial class LetterFadeInAnimation : LetterAnimation
	{
		public float animationTime = 0.2f;
		public float delay = 0.2f;

		public LetterFadeInAnimation(TextMeshProUGUI textMeshPro) : base(textMeshPro)
		{
		}

		protected override Tween GenerateAnimation(int letterIndex)
		{
			var	tween = TextMeshPro.TweenCharColorAlpha(letterIndex, 0, animationTime).SetInvert().SetDelay(letterIndex * delay);
			return tween;
		}
	}
}
