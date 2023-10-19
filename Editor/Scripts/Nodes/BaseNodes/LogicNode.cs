using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LogicNode : MasterNode
{
	[SerializeField] public List<string> inputNodeGuids = new();
	[SerializeField] public List<string> outputNodeGuids = new();
}
