using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Prashalt.Unity.ConversationGraph
{
    [CreateAssetMenu(menuName = "ConversationGraph/ConversationGraph")]
    public class ConversationGraphAsset : ScriptableObject
    {
        [SerializeField] private List<NodeData> _nodes;
        [SerializeField] private List<EdgeData> _edges;
        [SerializeField] public ConversationSettings settings;
        public IReadOnlyList<NodeData> Nodes
        {
            get { return _nodes; }
        }
        public IReadOnlyList<EdgeData> Edges
        {
            get { return _edges; }
        }

        public NodeData StartNode
        {
            get
            {
                if(Nodes == null || Nodes.Count <= 0)
                {
                    return null;
                }
			    return Nodes.FirstOrDefault(x => x.typeName == "Prashalt.Unity.ConversationGraph.Nodes.StartNode");
            }
        }

        public void SaveNode(NodeData nodeData)
        {
            var index = _nodes.FindIndex(x => x.guid == nodeData.guid);

            if (index == -1)
            {
                _nodes.Add(nodeData);
            }
            else
            {
                _nodes[index] = nodeData;
            }
        }
        public void RemoveNode(NodeData nodeData)
        {
            var index = _nodes.FindIndex(x => x.guid == nodeData.guid);

            if(index == -1) 
            {
                Debug.LogWarning("‘¶Ý‚µ‚Ü‚¹‚ñB");
            }
            else
            {
                _nodes.RemoveAt(index);
            }
        }
        public void SaveEdge(EdgeData edgeData)
        {
            var index = _edges.FindIndex(x => x.guid == edgeData.guid);

            if (index == -1)
            {
                _edges.Add(edgeData);
            }
            else
            {
                _edges[index] = edgeData;
            }
        }
        public void RemoveEdge(EdgeData edgeData)
        {
            var index = _edges.FindIndex(x => x.guid == edgeData.guid);

            if (index == -1)
            {
                Debug.LogWarning("‘¶Ý‚µ‚Ü‚¹‚ñB");
                return;
            }
            else
            {
                _edges.RemoveAt(index);
            }
        }
        public void ClearNodes()
        {
            _nodes.Clear();
        }
        public void ClearEdges()
        {
            _edges.Clear();
        }
        public NodeData FindNode(string nodeGuid)
        {
			var node = Nodes.First(x => x.guid == nodeGuid.Split(":")[1]);
            return node;
		}

        public List<NodeData> GetNextNode(NodeData nodeData)
        {
            var edges = Edges.Where(x => x.baseNodeGuid.Split(":")[0] == nodeData.guid);
            List<NodeData> result = new();
            foreach(var edge in edges)
            {
                var nextNode = Nodes.First(x => x.guid == edge.targetNodeGuid.Split(":")[0]);
                result.Add(nextNode);
            }

            return result;
        }
	}
}
