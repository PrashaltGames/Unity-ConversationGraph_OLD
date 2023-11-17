using MagicTween;
using TMPro;
using UnityEngine;

namespace Prashalt.Unity.ConversationGraph.Animation.Letter
{
    public class LetterSizeAnimation : LetterAnimation
    {
        public float animationSpeed = 0.2f;
        public float delay = 0.2f;
        
        protected override ConversationAnimation GenerateAnimation(int letterIndex, TextMeshProUGUI textMeshPro)
        {
            var tween = textMeshPro.TweenCharScale(letterIndex, Vector3.zero, animationSpeed).SetInvert().SetDelay(letterIndex * delay).SetAutoKill(false);
            var animation = new ConversationAnimation();
            animation.Add(tween);
            
            return animation;
        }
    }
}

