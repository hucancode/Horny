using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActivatedEvent : GameEvent
{
	public string skillName;
	// Add event parameters here
}

public class SkillCanceledEvent : GameEvent
{
	public string skillName;
	// Add event parameters here
}

public class OnPlayerCrash : GameEvent
{
	public GameObject enemy;
	// Add event parameters here
}

public class RacerSkillBase : MonoBehaviour
{
	public enum SkillType
	{
		PASSIVE,
		ACTIVE
		//CHANNEL,
		//TOGGLE,
	}

	public enum CoolDownType
	{
		FIXED,
		CHARGED
	}

	public enum SkillState
	{
		READY,
		CASTING,
		COOLING_DOWN,
		DISABLED
	}

	public float coolDownTime;
	protected float coolDownTimer;
	public int chargeCount;
	public int chargeMax;
	public float castTime;
	protected float castTimer;

	public SkillType skillType;
	public CoolDownType coolDownType;
	public SkillState skillState;
}
