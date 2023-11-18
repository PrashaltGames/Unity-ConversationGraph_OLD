using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Prashalt.Unity.ConversationGraph.Nodes
{
    [Serializable]
    public class EndNode : MasterNode
    {
        public EndNode()
        {
            title = "End";

            // 入力用のポートを作成
            AddInputPort(typeof(float), "End");
        }

        public override void Initialize(string guid, Rect rect, string json)
        {
            base.Initialize(guid, rect, json);
        }
    }
}