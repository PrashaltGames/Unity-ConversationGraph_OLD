using Prashalt.Unity.ConversationGraph.SourceGenerator.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;

public static class ConversationGraphUtility
{
	public static readonly Dictionary<string, MemberInfo> ConversationProperties;
	static ConversationGraphUtility()
	{
		ConversationProperties = GetAllConversationProperties();
	}
	public static Dictionary<string, MemberInfo> GetAllConversationProperties()
	{
		var dic = new Dictionary<string, MemberInfo>();
		//TODO: ƒAƒZƒ“ƒuƒŠ‚ÌŽí—Þ‚É‘Î‰ž
		var classList = GetHasConversationPropertyClasses(Assembly.Load("Assembly-CSharp"));
		foreach (var typeInfo in classList)
		{
			var properties = GetConversationProperties(typeInfo);

			foreach (var info in properties)
			{
				dic.Add(info.Name, info);
			}
		}

		return dic;
	}
	private static IEnumerable<System.Reflection.TypeInfo> GetHasConversationPropertyClasses(Assembly assembly)
	{
		foreach (Type type in assembly.GetTypes())
		{
			if (type.GetCustomAttributes(typeof(HasConversationPropertyAttribute), true).Length > 0)
			{
				yield return type.GetTypeInfo();
			}
		}
	}
	private static IEnumerable<MemberInfo> GetConversationProperties(System.Reflection.TypeInfo type)
	{
		foreach (MemberInfo info in type.GetMembers())
		{
			if (info.IsDefined(typeof(ConversationPropertyAttribute), true))
			{
				yield return info;
			}
		}
	}
}
