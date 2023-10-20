using System;

namespace Prashalt.Unity.ConversationGraph.Attributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class ConversationPropertyAttribute : Attribute
	{

	}
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = true)]
	public class HasConversationPropertyAttribute : Attribute
	{

	}
}