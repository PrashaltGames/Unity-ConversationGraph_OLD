using Cysharp.Threading.Tasks;
using MagicTween;
using System.Collections.Generic;
using UnityEngine;

namespace Packages.com.prashalt.unity.conversationgraph.Animation
{
	public class ObjectShakeAnimation : ObjectAnimation
	{
		public float stlength;
		public float duration;
		public ObjectShakeAnimation(Transform text) : base(text)
		{
		}

		public override ConversationAnimation SetAnimation()
		{
			var list = new ConversationAnimation();
			list.Add(Text.ShakeEulerAnglesZ(stlength, duration).SetAutoKill(false));
			return list;
		}
	}
}
