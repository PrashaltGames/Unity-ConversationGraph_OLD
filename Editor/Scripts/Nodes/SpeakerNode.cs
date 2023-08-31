using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using System;

namespace Prashalt.Unity.ConvasationGraph.Nodes
{
    [Serializable]
    public class SpeakerNode : TextNode
    {
        [SerializeField] private string speakerName;
        [NonSerialized] private TextField _speakerNameText;
        public SpeakerNode() : base()
        {
            title = "Speaker";

            _speakerNameText = new TextField();
            extensionContainer.Add(_speakerNameText);
            RefreshExpandedState();
        }

        public override void Initialize(string guid, Rect rect, string json)
        {
            base.Initialize(guid, rect, json);
            var jsonObj = JsonUtility.FromJson<SpeakerNode>(json);
            _textField.SetValueWithoutNotify(jsonObj?.text);
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

