using System.Text.RegularExpressions;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Prashalt.Unity.ConversationGraph.Editor
{
    public static class ConversationGraphEditorUtility
    {
        public const string packageFilePath = "Packages/com.prashalt.unity.conversationgraph/";
        public static NodeData NodeToData(MasterNode node)
        {
            var guid = node.guid;
            var rect = node.GetPosition();
            var json = node.ToJson();
            var type = node.GetType().FullName;

            return new NodeData { guid = guid, rect = rect, json = json, typeName = type };
        }
        public static EdgeData EdgeToData(Edge edge)
        {
            var targetNode = edge.input.node as MasterNode;
            var baseNode = edge.output.node as MasterNode;

            if (baseNode is null || targetNode is null) return null;

            var edgeData = new EdgeData("", baseNode.guid, targetNode.guid);

            return edgeData;
        }
        public static void MoveUp(VisualElement parent, VisualElement target)
        {
            var index = parent.IndexOf(target);
            var next = parent.ElementAt(index - 1);
            target.PlaceBehind(next);
        }
        public static void MoveDown(VisualElement parent, VisualElement target)
        {
            var index = parent.IndexOf(target);
            var next = parent.ElementAt(index + 1);
            target.PlaceInFront(next);
        }
        public static string OnChangeTextField(string text)
		{
			if (text is null || text == "") return "";
			var Matches = new Regex(@"\{(.+?)\}").Matches(text);

			foreach (Match propertyNameMatch in Matches)
			{
				//ê≥ãKï\åªï™Ç©ÇÁÇ»Ç¢ÇÃÇ≈ÅAÉSÉäâüÇ∑
				var propertyName = propertyNameMatch.Value.Replace("{", "");
				propertyName = propertyName.Replace("}", "");

				var hasProperty = ConversationGraphUtility.ConversationProperties.TryGetValue(propertyName, out _);
				
                if(hasProperty)
                {
					var coloredText = propertyNameMatch.Value.Replace("{", "<color=#4169e1>{");
					coloredText = coloredText.Replace("}", "}</color>");

                    text = text.Replace(propertyName, coloredText);
				}

			}
            return text;
		}
	}
}
