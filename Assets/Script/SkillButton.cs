using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : MonoBehaviour {

	public string skillName;
	
	void Start ()
	{
		
	}
	
	void Update ()
	{
		
	}
	
	public void Cast()
	{
		SkillActivatedEvent e = new SkillActivatedEvent();
		e.skillName = skillName;
		Events.instance.Raise(e);
	}
}
