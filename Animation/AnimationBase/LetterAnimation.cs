using Cysharp.Threading.Tasks;
using MagicTween;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Prashalt.Unity.ConversationGraph.Animation
{
	public abstract class LetterAnimation : ConversationAnimationGenerator
	{
		protected LetterAnimation(TextMeshProUGUI textMeshPro)
		{
			TextMeshPro = textMeshPro;
		}

		public TextMeshProUGUI TextMeshPro { get; private set; }

		private static bool isAnimationInit;
		private static ConversationAnimation conversationAnimation = new();
		protected static int letterCount;

		protected abstract ConversationAnimation GenerateAnimation(int letterIndex);

		public override ConversationAnimation SetAnimation()
		{	
			//すでに同じアニメーションがあるなら削除しない。
			if (letterCount == TextMeshPro.GetCharCount() && isAnimationInit)
			{
				return conversationAnimation;
			}

			if(isAnimationInit)
			{
				//foreach(var animation in animations)
				//{
				//	animation.Kill();
				//}
				conversationAnimation.Clear();
			}
			isAnimationInit = true;

			TextMeshPro.GetTMPTweenAnimator().Update();
			for(var i = 0; i < TextMeshPro.GetCharCount(); i++)
			{
				conversationAnimation.Add(GenerateAnimation(i));
			}
			TextMeshPro.GetTMPTweenAnimator().Update();

			return conversationAnimation;
		}
	}
}
