//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UIElements;

//public class PropertyDragAndDropManipulator : PointerManipulator
//{
//	public PropertyDragAndDropManipulator()
//	{
	
//	}

//	protected override void RegisterCallbacksOnTarget()
//	{
//		target.RegisterCallback<PointerDownEvent>(PointerDownHandler);
//		target.RegisterCallback<PointerMoveEvent>(PointerMoveHandler);
//		target.RegisterCallback<PointerUpEvent>(PointerUpHandler);
//		target.RegisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
//	}

//	protected override void UnregisterCallbacksFromTarget()
//	{
//		target.UnregisterCallback<PointerDownEvent>(PointerDownHandler);
//		target.UnregisterCallback<PointerMoveEvent>(PointerMoveHandler);
//		target.UnregisterCallback<PointerUpEvent>(PointerUpHandler);
//		target.UnregisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
//	}
//	private Vector2 targetStartPosition { get; set; }

//	private Vector3 pointerStartPosition { get; set; }

//	private bool enabled { get; set; }

//	private VisualElement root { get; }

//	// This method stores the starting position of target and the pointer,
//	// makes target capture the pointer, and denotes that a drag is now in progress.
//	private void PointerDownHandler(PointerDownEvent evt)
//	{
//		Debug.Log(target.Q<Label>("Property").text);
//		if (target.Q<Label>("") == null) return;
//		targetStartPosition = target.transform.position;
//		pointerStartPosition = evt.position;
//		target.CapturePointer(evt.pointerId);
//		enabled = true;
//	}

//	// This method checks whether a drag is in progress and whether target has captured the pointer.
//	// If both are true, calculates a new position for target within the bounds of the window.
//	private void PointerMoveHandler(PointerMoveEvent evt)
//	{
//		Debug.Log("Move");
//		if (enabled && target.HasPointerCapture(evt.pointerId))
//		{
//			Debug.Log("Move(in)");
//			Vector3 pointerDelta = evt.position - pointerStartPosition;

//			target.transform.position = new Vector2(
//				Mathf.Clamp(targetStartPosition.x + pointerDelta.x, 0, target.panel.visualTree.worldBound.width),
//				Mathf.Clamp(targetStartPosition.y + pointerDelta.y, 0, target.panel.visualTree.worldBound.height));
//		}
//	}

//	// This method checks whether a drag is in progress and whether target has captured the pointer.
//	// If both are true, makes target release the pointer.
//	private void PointerUpHandler(PointerUpEvent evt)
//	{
//		if (enabled && target.HasPointerCapture(evt.pointerId))
//		{
//			target.ReleasePointer(evt.pointerId);
//		}
//	}

//	// This method checks whether a drag is in progress. If true, queries the root
//	// of the visual tree to find all slots, decides which slot is the closest one
//	// that overlaps target, and sets the position of target so that it rests on top
//	// of that slot. Sets the position of target back to its original position
//	// if there is no overlapping slot.
//	private void PointerCaptureOutHandler(PointerCaptureOutEvent evt)
//	{
//		if (enabled)
//		{
//			Debug.Log("Capture Out");
//		}
//	}
//}
