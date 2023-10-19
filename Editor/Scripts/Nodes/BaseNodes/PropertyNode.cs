using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public class PropertyNode : MasterNode
{
	[SerializeField] public string memberName;
	public void SetTitle(string name)
	{
		title = $"{name} {title}";
		memberName = name;
	}
}
//TODO: MemberInfoをISerializationCallbackReceiverでシリアライズできるようにして実装し直す
[Serializable]
public class MemberInfoWrapper
{
	[SerializeField] public MemberInfo info;
}
