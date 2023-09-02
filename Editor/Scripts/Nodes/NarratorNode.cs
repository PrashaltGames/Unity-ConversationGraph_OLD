using Prashalt.Unity.ConversationGraph.Components;
using Prashalt.Unity.ConversationGraph.Editor;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Prashalt.Unity.ConversationGraph.Nodes
{
    [Serializable]
    public class NarratorNode : MasterNode
    {
        [NonSerialized] protected List<TextField> textFieldList;
        [NonSerialized] protected Button addTextFieldButton;
        [NonSerialized] protected TemplateContainer defaultContainer;

        private const string elementPath = ConversationGraphEditorUtility.packageFilePath + "Editor/UXML/NarratorNode.uxml";
        

        public NarratorNode()
        {
            textFieldList = new();
            title = "Narrator";

            // 入力用のポートを作成
            var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float)); // 第三引数をPort.Capacity.Multipleにすると複数のポートへの接続が可能になる
            inputPort.portName = "Input";
            inputContainer.Add(inputPort); // 入力用ポートはinputContainerに追加する

            //出力ポート
            var outputPort = Port.Create<Edge>(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(float));
            outputPort.portName = "Output";
            outputContainer.Add(outputPort);

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(elementPath);
            defaultContainer = visualTree.Instantiate();
            mainContainer.Add(defaultContainer);

            addTextFieldButton = mainContainer.Q<Button>("addButton");
            addTextFieldButton.clicked += OnAddTextButton;

            textFieldList.Add(mainContainer.Q<TextField>("mainTextField"));
        }
        public void OnAddTextButton()
        {
            var newTextField = new PrashaltTextFiled();

            newTextField.Q<Label>().text = $"Main Text {textFieldList.Count + 1}";
            textFieldList.Add(newTextField.Q<TextField>());

            defaultContainer.Add(newTextField);
            ConversationGraphEditorUtility.MoveDown(defaultContainer, addTextFieldButton);
        }

        public override void Initialize(string guid, Rect rect, string json)
        {
            base.Initialize(guid, rect, json);
            var jsonObj = JsonUtility.FromJson<NarratorNode>(json);
            if(jsonObj?.textList.Count != 0)
            {
                textFieldList[0].SetValueWithoutNotify(jsonObj?.textList[0]);
            }
        }
        public override string ToJson()
        {
            textList = new();
            foreach ( var item in textFieldList )
            {
                textList.Add(item.text);
            }
            return JsonUtility.ToJson(this);
        }
    }
}