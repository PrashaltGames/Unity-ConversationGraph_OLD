using NUnit.Framework;
using Prashalt.Unity.ConversationGraph.Animation;
using Prashalt.Unity.ConversationGraph.Nodes;
using Prashalt.Unity.ConversationGraph.Nodes.Conversation;
using System.Collections;
using System.Security.Cryptography;
using UnityEditor.Experimental.GraphView;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.TestTools;

public class ConversationGraphNodeTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void AnimationNode_LetterInFadeAnimation_Title()
    {
        var node = new AnimationNode<LetterFadeInAnimation>();

        Assert.That(node.title.Contains(typeof(LetterFadeInAnimation).Name), Is.EqualTo(true));
    }
    [Test]
    public void AnimationNode_LetterInFadeAnimation_OutputPortCount()
    {
        var node = new AnimationNode<LetterFadeInAnimation>();

        Assert.That(node.outputContainer.childCount, Is.EqualTo(1));
    }
    [Test]
    public void AnimationNode_LetterInFadeAnimation_InputPortCount()
    {
        var node = new AnimationNode<LetterFadeInAnimation>();

        Assert.That(node.inputContainer.childCount, Is.EqualTo(0));
    }
    [Test]
    public void AnimationNode_LetterInFadeAnimation_Initialize()
    {
        var node = new AnimationNode<LetterFadeInAnimation>();

        var json = "{\"intProperties\":[],\"floatProperties\":[1.0700000524520875,1.590000033378601],\"animationName\":\"LetterFadeInAnimation\"}";

		node.Initialize("", new UnityEngine.Rect(), json);

        Assert.That(node.mainContainer.childCount, Is.EqualTo(4));
    }
    [Test]
    public void AnimationNode_LetterInFadeOffsetYAnimation_Initialize()
    {
		var node = new AnimationNode<LetterFadeInOffsetYAnimation>();

		var json = "{\"intProperties\":[29],\"floatProperties\":[0.699999988079071,1.1200000047683716],\"animationName\":\"LetterFadeInOffsetYAnimation\"}";

		node.Initialize("", new Rect(), json);

		Assert.That(node.mainContainer.childCount, Is.EqualTo(5));
	}
    [Test]
    public void NarratorNode_Initialize()
    {
        var node = new NarratorNode();

        var json = "{\"textList\":[\"AAAAAAAAAA\"],\"animationGuid\":\"f08f4bace7a24154bdb0f00fd8222756\"}";

        node.Initialize("", new Rect(), json);

        Assert.That(node.mainContainer.childCount, Is.EqualTo(3));
	}

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator ConversationTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
