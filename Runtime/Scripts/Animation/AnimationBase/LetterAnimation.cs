using MagicTween;
using TMPro;

namespace Prashalt.Unity.ConversationGraph.Animation
{
	public abstract class LetterAnimation : ConversationAnimation
	{
		public TextMeshProUGUI TextMeshPro { get; private set; }
		//キャッシュが出来るようなAnimationプロパティを作っておく
		public string AnimationId
		{
			get
			{
				//文字が変更されているもしくはアニメーションが生成されていなかったら生成後に渡す。
				if (letterCount != TextMeshPro.GetCharCount() || !isAnimationInit)
				{
					SetAnimation();
				}
				return GetType().Name;
			}
		}
		private bool isAnimationInit;
		protected int letterCount;

		protected LetterAnimation(TextMeshProUGUI textMeshPro)
		{
			TextMeshPro = textMeshPro;
		}

		protected abstract Tween GenerateAnimation(int letterIndex);

		private void SetAnimation()
		{
			isAnimationInit = true;
			for(var i = 0; i < TextMeshPro.GetCharCount(); i++)
			{
				GenerateAnimation(i).SetId(GetType().Name);
			}
		}
	}
}
