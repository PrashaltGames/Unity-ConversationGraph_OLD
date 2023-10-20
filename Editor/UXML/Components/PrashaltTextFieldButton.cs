using Prashalt.Unity.ConversationGraph.Editor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

namespace Prashalt.Unity.ConversationGraph.Components
{
    public class PrashaltTextFieldButton : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<PrashaltTextFieldButton> { }

        public PrashaltTextFieldButton()
        {
            var button = new Button
            {
                style =
                {
                    backgroundColor = Color.gray,
                    justifyContent = Justify.FlexStart,
                    unityTextAlign = TextAnchor.MiddleLeft
                }
            };
            Add(button);

            var label = new Label
            {
                name = "label",
                text = "MainText",
            };
            label.style.marginLeft = 4;
            button.Add(label);

            var textField = new TextField
            {
                name = "mainTextField"
            };
            textField.multiline = true;
            textField.style.maxWidth = 200;
            textField.style.whiteSpace = WhiteSpace.Normal;

            //TextFieldにはまだRichTextが適応されていなかった。
            //フォーラムを見た感じ次期に対応しそうなので、応急処置はせずに保留。

            //textField.RegisterValueChangedCallback(evt =>
            //{
            //    var coloredText = ConversationGraphEditorUtility.OnChangeTextField(evt.newValue);
            //    textField.SetValueWithoutNotify(coloredText);
            //});
            button.Add(textField);
        }
	}
}
