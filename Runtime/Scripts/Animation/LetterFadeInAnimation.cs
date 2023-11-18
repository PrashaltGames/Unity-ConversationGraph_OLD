using MagicTween;
using TMPro;


namespace Prashalt.Unity.ConversationGraph.Animation.Letter
{ 
	public class LetterFadeInAnimation : LetterAnimation
	{
		public float animationSpeed = 0.2f;
		public float delay = 0.2f;

		protected override ConversationAnimation GenerateAnimation(int letterIndex, TextMeshProUGUI textMeshPro)
		{
			var	tween = textMeshPro.TweenCharColorAlpha(letterIndex, 0, animationSpeed).SetInvert().SetDelay(letterIndex * delay).SetAutoKill(false);
			var animation = new ConversationAnimation();
			animation.Add(tween);

			return animation;
		}
	}
}
