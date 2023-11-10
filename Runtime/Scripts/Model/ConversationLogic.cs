public static class ConversationLogic
{
	public static NodeProcess NodeProcess { get; private set; }
	public static ConversationAssetData ConversationData { get; private set; }
	public static ConversationInput ConversationInput { get; private set; }
	static ConversationLogic()
	{
		NodeProcess = new();
		ConversationData = new();
		ConversationInput = new();
	}
}
