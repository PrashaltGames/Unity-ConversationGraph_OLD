using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Prashalt.Unity.ConversationGraph.Components
{
    public class PrashaltTextFiled : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<PrashaltTextFiled> { }

        public PrashaltTextFiled()
        {

            var label = new Label
            {
                name = "label",
                text = "Label",
            };
            label.style.marginLeft = 4;
            Add(label);

            var textField = new TextField();
            textField.multiline = true;
            textField.style.maxWidth = 200;
            textField.style.whiteSpace = WhiteSpace.Normal;
            Add(textField);
        }
    }
}
