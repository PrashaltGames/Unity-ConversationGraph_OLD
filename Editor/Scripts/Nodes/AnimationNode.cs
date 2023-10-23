using Prashalt.Unity.ConversationGraph.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.InputField;

namespace Prashalt.Unity.ConversationGraph.Nodes
{
	[Serializable]
	public class AnimationNode<T> : MasterNode where T : ConversationAnimation
	{
		[SerializeField] public List<int> intProperties = new();
		[SerializeField] public List<float> floatProperties = new();
		public string AnimationName { get; private set; }
		public AnimationNode()
		{
			title = $"{typeof(T).Name} (Animation)";

			//出力ポート
			var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(ConversationAnimation));
			outputPort.portName = "Output";
			outputPort.portColor = Color.red;
			outputContainer.Add(outputPort);

			AnimationName = nameof(T);		
		}
		public override void Initialize(string guid, Rect rect, string json)
		{
			base.Initialize(guid, rect, json);
			var obj = JsonUtility.FromJson<AnimationNode<T>>(json);

			intProperties = obj.intProperties;
			floatProperties = obj.floatProperties;

			//プロパティを指定できるように
			var fields = typeof(T).GetFields();
			AddPropertyTextField<int>(fields);
			AddPropertyTextField<float>(fields);
		}
		public void AddPropertyTextField<T1>(FieldInfo[] infoArray)
		{
			var fields = infoArray.Where(x => x.FieldType == typeof(T1));

			var intIndex = 0;
			var floatIndex = 0;
			foreach(var info in fields)
			{
				if (typeof(T1) == typeof(int))
				{
					var integerField = new IntegerField
					{
						label = info.Name
					};
					if (intProperties.Count - 1 < intIndex)
					{
						intProperties.Add(0);
					}
					else
					{
						integerField.SetValueWithoutNotify(intProperties[intIndex]);
					}

					//イベントを登録
					int i = intIndex;
					integerField.RegisterValueChangedCallback(e => {
						intProperties[i] = e.newValue;
					});
					mainContainer.Add(integerField);

					intIndex++;
				}
				else if (typeof(T1) == typeof(float))
				{
					var floatField = new FloatField
					{
						label = info.Name
					};
					if(floatProperties.Count - 1 < floatIndex)
					{
						floatProperties.Add(0);
					}
					else
					{
						floatField.SetValueWithoutNotify(floatProperties[floatIndex]);
					}

					//イベントを登録
					int i = floatIndex;
					floatField.RegisterValueChangedCallback(e => {
						floatProperties[i] = e.newValue;
					});
					mainContainer.Add(floatField);

					floatIndex++;
				}
				else
				{
					Debug.LogError("パッケージ開発者さんミスってますよ");
				}
			}
		}
	}
}
