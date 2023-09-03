using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ConversationSettings
{
    [SerializeField] public bool isNeedClick;
    [SerializeField] public bool shouldTextAnimation;
    [SerializeField] public int time;
}
