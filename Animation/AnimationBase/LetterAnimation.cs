using Cysharp.Threading.Tasks;
using MagicTween;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Prashalt.Unity.ConversationGraph.Animation
{
	public abstract class LetterAnimation : ConversationAnimationGenerator
	{
		private static bool isAnimationInit;
		private static ConversationAnimation conversationAnimation = new();
		protected static int letterCount;

		protected abstract ConversationAnimation GenerateAnimation(int letterIndex, TextMeshProUGUI textMeshPro);

		public override ConversationAnimation SetAnimation(TextMeshProUGUI textMeshPro)
		{	
			//すでに同じアニメーションがあるなら削除しない。
			if (letterCount == textMeshPro.GetCharCount() && isAnimationInit)
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

			textMeshPro.GetTMPTweenAnimator().Update();
			for(var i = 0; i < textMeshPro.GetCharCount(); i++)
			{
				conversationAnimation.Add(GenerateAnimation(i, textMeshPro));
			}
			textMeshPro.GetTMPTweenAnimator().Update();

			return conversationAnimation;
		}
	}
}
