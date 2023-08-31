using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using System;
using UnityEditor;
using Cysharp.Threading.Tasks;

namespace Prashalt.Unity.ConvasationGraph.Nodes
{
    [Serializable]
    public class SpeakerNode : NarratorNode
    {
        [SerializeField] private string speakerName;
        [NonSerialized] private TextField _speakerNameText;

        private const string packageFilePath = "../Packages/com.prashalt.unity.convasationgraph/PATH/TO/ASSET/";
        private const string elementPath = packageFilePath + "Editor/UXML/SpeakerNode.uxml";
        public SpeakerNode() : base()
        {
            title = "Speaker";

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(elementPath);
            var template = visualTree.Instantiate();
            mainContainer.Add(template);

            _speakerNameText = mainContainer.Q<TextField>("speakerNameField");
            var index = mainContainer.IndexOf(template);
            var next = mainContainer.ElementAt(index - 1);
            template.PlaceBehind(next);

            RefreshExpandedState();
        }

        public override void Initialize(string guid, Rect rect, string json)
        {
            rect.width = 200;
            base.Initialize(guid, rect, json);
            var jsonObj = JsonUtility.FromJson<SpeakerNode>(json);
            _speakerNameText.SetValueWithoutNotify(jsonObj?.speakerName);
        }
        public override string ToJson()
        {
            text = _textField.text;
            speakerName = _speakerNameText.text;
            return JsonUtility.ToJson(this);
        }
    }
}

