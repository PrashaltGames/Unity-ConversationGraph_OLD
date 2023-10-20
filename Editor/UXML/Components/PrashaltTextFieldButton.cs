using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
            button.Add(textField);
        }
	}
}
