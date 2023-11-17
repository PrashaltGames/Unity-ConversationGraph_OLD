using Cysharp.Threading.Tasks;
using MagicTween;
using System.Collections.Generic;
using TMPro;

namespace Prashalt.Unity.ConversationGraph.Animation
{
	public abstract class ConversationAnimationGenerator
	{
		public abstract ConversationAnimation SetAnimation(TextMeshProUGUI textMeshPro);
	}
}
