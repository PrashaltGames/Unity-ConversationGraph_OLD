using MagicTween;
using Prashalt.Unity.ConversationGraph.Animation.Letter;
using TMPro;

namespace Prashalt.Unity.ConversationGraph.Animation.Object
{
	public class ObjectShakeAnimation : ObjectAnimation
	{
		public float stlength;
		public float duration;

		public override ConversationAnimation SetAnimation(TextMeshProUGUI textMeshPro)
		{
			var list = new ConversationAnimation();
			list.Add(textMeshPro.transform.ShakeEulerAnglesZ(stlength, duration).SetAutoKill(false).SetAutoPlay(false));
			return list;
		}
	}
}
