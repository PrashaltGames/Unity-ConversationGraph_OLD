using Cysharp.Threading.Tasks;
using MagicTween;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Packages.com.prashalt.unity.conversationgraph.Animation
{
	public class ObjectShakeAnimation : ObjectAnimation
	{
		public float stlength;
		public float duration;

		public override ConversationAnimation SetAnimation(TextMeshProUGUI textMeshPro)
		{
			var list = new ConversationAnimation();
			list.Add(textMeshPro.transform.ShakeEulerAnglesZ(stlength, duration).SetAutoKill(false));
			return list;
		}
	}
}
