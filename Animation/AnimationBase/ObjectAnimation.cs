using Prashalt.Unity.ConversationGraph.Animation;
using UnityEngine;

namespace Packages.com.prashalt.unity.conversationgraph.Animation
{
	public abstract class ObjectAnimation : ConversationAnimationGenerator
	{
		protected ObjectAnimation(Transform text)
		{
			Text = text;
		}

		public Transform Text { get; private set; }
	}
}
