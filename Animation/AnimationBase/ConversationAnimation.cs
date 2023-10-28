using Cysharp.Threading.Tasks;
using MagicTween;
using System.Collections.Generic;

namespace Prashalt.Unity.ConversationGraph.Animation
{
	public abstract class ConversationAnimation
	{
		protected ConversationAnimation()
		{
			
		}
		public abstract List<Tween> SetAnimation();
	}
}
