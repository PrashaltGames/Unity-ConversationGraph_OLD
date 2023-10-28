using Cysharp.Threading.Tasks;
using MagicTween;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Prashalt.Unity.ConversationGraph.Animation
{
	public abstract class LetterAnimation : ConversationAnimation
	{
		protected LetterAnimation(TextMeshProUGUI textMeshPro)
		{
			TextMeshPro = textMeshPro;
		}

		public TextMeshProUGUI TextMeshPro { get; private set; }

		private static bool isAnimationInit;
		private static List<Tween> animations = new();
		protected static int letterCount;

		protected abstract Tween GenerateAnimation(int letterIndex);

		public override List<Tween> SetAnimation()
		{	
			//すでに同じアニメーションがあるなら削除しない。
			if (letterCount == TextMeshPro.GetCharCount() && isAnimationInit)
			{
				return animations;
			}

			if(isAnimationInit)
			{
				//foreach(var animation in animations)
				//{
				//	animation.Kill();
				//}
				animations.Clear();
			}
			isAnimationInit = true;

			TextMeshPro.GetTMPTweenAnimator().Update();
			for(var i = 0; i < TextMeshPro.GetCharCount(); i++)
			{
				animations.Add(GenerateAnimation(i));
			}
			TextMeshPro.GetTMPTweenAnimator().Update();

			return animations;
		}
	}
}
