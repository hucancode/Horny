using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RacerPicker : MonoBehaviour, IEndDragHandler {

	static public int currentIndex;
	public ScrollRect scrollRect;
	private GameObject content;
	private float[] markers;

	void Start ()
	{
		content = scrollRect.content.gameObject;
		float view_w = scrollRect.gameObject.GetComponent<RectTransform>().rect.width;
		float content_w = scrollRect.content.sizeDelta.x;
		float w = content_w - view_w;
		markers = new float[content.transform.childCount];
		for(int i = 0;i< content.transform.childCount;i++)
		{
			RectTransform child = content.transform.GetChild(i).GetComponent<RectTransform>();
			markers[i] = (child.localPosition.x - view_w/2)/w;
		}
		Array.Sort(markers);
		currentIndex = 0;
	}
	
	public void Next()
	{
		currentIndex++;
		currentIndex = Mathf.Clamp(currentIndex, 0, markers.Length - 1);
		scrollRect.horizontalNormalizedPosition = markers[currentIndex];
	}

	public void Prev()
	{
		currentIndex--;
		currentIndex = Mathf.Clamp(currentIndex, 0, markers.Length - 1);
		scrollRect.horizontalNormalizedPosition = markers[currentIndex];
	}

	public void OnEndDrag(PointerEventData data)
    {
		float key = scrollRect.horizontalNormalizedPosition;
		if(key <= markers[0])
		{
			scrollRect.horizontalNormalizedPosition = markers[0];
			currentIndex = 0;
		}
		else if(key >= markers[markers.Length - 1])
		{
			scrollRect.horizontalNormalizedPosition = markers[markers.Length - 1];
			currentIndex = markers.Length - 1;
		}
		else
		{
			for(int i = 1;i< markers.Length;i++)
			{
				if(key >= markers[i-1] && key <= markers[i])
				{
					float d = (key - markers[i-1])/(markers[i] - markers[i-1]);
					if(d > 0.5f)
					{
						scrollRect.horizontalNormalizedPosition = markers[i];
						currentIndex = i;
					}
					else
					{
						scrollRect.horizontalNormalizedPosition = markers[i-1];
						currentIndex = i-1;
					}
				}
			}
		}
		scrollRect.StopMovement();
	}

    public void setPickedCharacter()
    {
        GameManager.instance.characterToSpawn = currentIndex;
    }
}
