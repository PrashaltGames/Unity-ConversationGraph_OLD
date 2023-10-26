using Cysharp.Threading.Tasks;
using MagicTween;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Prashalt.Unity.ConversationGraph.Animation
{
	public abstract class LetterAnimation : ConversationAnimation
	{
		public TextMeshProUGUI TextMeshPro { get; private set; }

		private static bool isAnimationInit;
		private static List<Tween> animations = new();
		protected static int letterCount;

		protected LetterAnimation(TextMeshProUGUI textMeshPro)
		{
			TextMeshPro = textMeshPro;
		}

		protected abstract Tween GenerateAnimation(int letterIndex);

		public override async UniTask<List<Tween>> SetAnimation()
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
				await UniTask.Delay(1);
			}
			TextMeshPro.GetTMPTweenAnimator().Update();

			return animations;
		}
	}
}
