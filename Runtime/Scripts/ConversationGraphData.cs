using System;
using UnityEngine;

namespace Prashalt.Unity.ConversationGraph
{
    [Serializable]
    public class NodeData
    {
        public string guid;
        public string typeName;
        public Rect rect;
        public string json;
    }
    [Serializable]
    public class EdgeData
    {
        public string guid;
        public string baseNodeGuid;
        public string targetNodeGuid;

        public EdgeData(string guid, string baseNodeGuid, string targetNodeGuid)
        {
            if(guid == null || guid == "")
            {
                this.guid = Guid.NewGuid().ToString("N");
            }
            else
            {
                this.guid = guid;
            }

            this.baseNodeGuid = baseNodeGuid;
            this.targetNodeGuid = targetNodeGuid;
        }
    }
}
