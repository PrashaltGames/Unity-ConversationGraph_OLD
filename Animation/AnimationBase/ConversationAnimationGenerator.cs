using Cysharp.Threading.Tasks;
using MagicTween;
using System.Collections.Generic;

namespace Prashalt.Unity.ConversationGraph.Animation
{
	public abstract class ConversationAnimationGenerator
	{
		protected ConversationAnimationGenerator()
		{
			
		}
		public abstract ConversationAnimation SetAnimation();
	}
}
