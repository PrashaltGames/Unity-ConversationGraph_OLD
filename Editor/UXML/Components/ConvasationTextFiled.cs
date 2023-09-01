using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Prashalt.Unity.ConvasationGraph.Components
{
    public class ConvasationTextFiled : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<ConvasationTextFiled> { }

        public ConvasationTextFiled()
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
