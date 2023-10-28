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

		public override List<Tween> SetAnimation()
		{
			Debug.Log(stlength);
			var list = new List<Tween> { Text.ShakeEulerAnglesZ(stlength, duration).SetAutoKill(false)};
			return list;
		}
	}
}
