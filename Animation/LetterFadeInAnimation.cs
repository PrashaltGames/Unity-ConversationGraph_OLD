using MagicTween;
using TMPro;


namespace Prashalt.Unity.ConversationGraph.Animation
{ 
	public partial class LetterFadeInAnimation : LetterAnimation
	{
		public float animationSpeed = 0.2f;
		public float delay = 0.2f;

		public LetterFadeInAnimation(TextMeshProUGUI textMeshPro) : base(textMeshPro)
		{
			
		}

		protected override ConversationAnimation GenerateAnimation(int letterIndex)
		{
			var	tween = TextMeshPro.TweenCharColorAlpha(letterIndex, 0, animationSpeed).SetInvert().SetDelay(letterIndex * delay).SetAutoKill(false);
			var animation = new ConversationAnimation();
			animation.Add(tween);

			return animation;
		}
	}
}
