using MagicTween;
using System.Collections.Generic;

public class ConversationAnimation
{
	private List<Tween> animations = new();
	public bool IsPlaying
	{
		get
		{
			return animations.Exists(x => x.IsPlaying());
		}
	}

	public void Add(Tween tween)
	{
		animations.Add(tween);
	}
	public void Add(ConversationAnimation animation)
	{
		animations.AddRange(animation.animations);
	}
	public void Play()
	{
		foreach(var animation in animations)
		{
			animation.Play();
		}
	}
	public void Puase()
	{
		foreach(var animation in animations)
		{
			animation.Pause();
		}
	}
	public void Clear()
	{
		animations.Clear();
	}
}
