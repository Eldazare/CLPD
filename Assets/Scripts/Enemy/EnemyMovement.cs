using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyMovement {

	public static SuperMove GetPattern(int index){
		switch (index) {
		case 0:
			return new MovePattern0 ();
		case 1:
			return new MovePattern1 ();
		case 2:
			return new MovePattern2 ();
		case 3:
			return new MovePattern3 ();
		default:
			Debug.LogError ("False index in enemy move patterns");
			return null;
		}
	}
}




// helper class structure, Movement() is called in Enemy FixedUpdate and MovStart in Start.
public abstract class SuperMove{

	public abstract void MovStart (Rigidbody2D rig);
	public abstract void Movement (Rigidbody2D rig, float speed);

	protected GameObject ChooseNearestPlayer(Vector3 selfPos){
		float distance = 1000;
		GameObject newTarget = null;
		GameObject[] playerList = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject g in playerList) {
			if((g.transform.position-selfPos).magnitude < distance){
				newTarget = g;
			}
		}
		return newTarget;
	}

	protected float AngleTowards (Vector2 target, Rigidbody2D rig){
		float AngleRad = Mathf.Atan2(target.y - rig.transform.position.y, target.x - rig.transform.position.x);
		float AngleDeg = (180 / Mathf.PI) * AngleRad - 90;
		return AngleDeg;
	}

	protected float DistanceToTarget (Vector3 target, Rigidbody2D rig){
		Vector2 vec = (Vector2)target - (Vector2)rig.position;
		float mag = vec.magnitude;
		return mag;
	}

	protected float TurnTowards (GameObject target, Rigidbody2D rig, float turnSpeed){
		Vector2 targetLookPoint = target.transform.position - rig.transform.position;
		targetLookPoint.Normalize ();
		targetLookPoint = Vector3.RotateTowards (rig.transform.up, targetLookPoint, turnSpeed * Time.fixedDeltaTime, 0.0f);
		//targetLookPoint = Vector3.Slerp (rig.transform.up, targetLookPoint, Time.fixedDeltaTime * turnSpeed);
		targetLookPoint += (Vector2)rig.transform.position;
		return AngleTowards (targetLookPoint, rig);
	}
}

public class MovePattern0 : SuperMove{
	//Pattern 0: Choose random direction in arenas direction
	override
	public void MovStart (Rigidbody2D rig){
		Vector2 origo = new Vector2 (0, 0);
		float angleDeg = AngleTowards (origo, rig);
		float random = Random.Range (-35, 35);
		rig.transform.rotation = Quaternion.Euler(0, 0, angleDeg+random);
	}

	override
	public void Movement(Rigidbody2D rig, float speed){
		rig.velocity = rig.transform.up * speed;
	}
}

public class MovePattern1 : SuperMove{
	//Pattern 1: Choose nearest player and periodically choose new nearest player and try to reach him/her
	private float chooseCounter;
	private float chooseCounterMax;
	private GameObject target;

	override
	public void MovStart (Rigidbody2D rig){
		chooseCounterMax = 3.0f; //TODO: read data from file?
		chooseCounter = chooseCounterMax;
		target = ChooseNearestPlayer(rig.transform.position);
	}

	override
	public void Movement(Rigidbody2D rig, float speed){
		if (chooseCounter < 0) {
			target = ChooseNearestPlayer (rig.transform.position);
			chooseCounter = chooseCounterMax;
		} else {
			chooseCounter -= Time.fixedDeltaTime;
		}
		float angleDeg = AngleTowards (target.transform.position, rig);
		rig.transform.rotation = Quaternion.Euler(0, 0, angleDeg);
		rig.velocity = rig.transform.up * speed;
	}
}

public class MovePattern2 : SuperMove{
	private float minDistance;
	private float lungeCD;
	private float lungeCDMax;
	private float lungeTimer;
	private float lungeTimerMax;
	private GameObject target;
	private float turnSpeed;

	override
	public void MovStart(Rigidbody2D rig){
		minDistance = 2.4f;
		lungeTimerMax = 1f;
		turnSpeed = 2.5f;
		lungeCDMax = 2.0f;
		lungeCD = lungeCDMax;
		lungeTimer = 0f;
		target = ChooseNearestPlayer (rig.transform.position);
	}

	override
	public void Movement(Rigidbody2D rig, float speed){
		if (lungeTimer > 0) {
			rig.velocity = rig.transform.up * speed * 3;
			lungeTimer -= Time.fixedDeltaTime;
		} else {
			if (DistanceToTarget (target.transform.position, rig) < minDistance && lungeCD < 0) {
				lungeTimer = lungeTimerMax;
				lungeCD = lungeCDMax;
			} else {
				float angle = TurnTowards (target, rig, turnSpeed);
				rig.transform.rotation = Quaternion.Euler (0, 0, angle);
				lungeCD -= Time.fixedDeltaTime;
			}
			rig.velocity = rig.transform.up * speed;
		}
	}
}


public class MovePattern3 : SuperMove {
	private GameObject target;
	private Vector3 latestPoint;
	private int turnRightMod;
	float minDistance = 1.8f;
	float turnSpeed = 3.0f;

	float kequlTimerMax = 2.0f;
	float kequlTimerCur;

	override
	public void MovStart (Rigidbody2D rig){
		target = ChooseNearestPlayer (rig.transform.position);
	}


	override
	public void Movement (Rigidbody2D rig, float speed){
		if (kequlTimerCur > 0) {
			kequlTimerCur -= Time.deltaTime;
			RotateAroundOneFrame (latestPoint, rig, speed*2.0f);
			if (kequlTimerCur < 0) {
				target = ChooseNearestPlayer (rig.transform.position);
			}
			
		} else {
			if (DistanceToTarget (target.transform.position, rig) < minDistance) {
				kequlTimerCur = kequlTimerMax;
				latestPoint = target.transform.position;
				int rand = Random.Range (0, 2);
				if (rand == 0) {
					turnRightMod = -1;
				} else {
					turnRightMod = 1;
				}
				Debug.Log ("turnRight "+turnRightMod);
			} else {
				float angle = TurnTowards (target, rig, turnSpeed);
				rig.transform.rotation = Quaternion.Euler (0, 0, angle);
			}
			rig.velocity = rig.transform.up * speed;
		}
	}

	private void RotateAroundOneFrame(Vector3 point, Rigidbody2D rig, float speed){
		float angle = speed * turnRightMod * Time.deltaTime;
		Vector2 relativePos = rig.transform.position - point;
		float x = relativePos.x * Mathf.Cos (angle) - relativePos.y * Mathf.Sin (angle) + point.x;
		float y = relativePos.x * Mathf.Sin (angle) + relativePos.y * Mathf.Cos (angle) + point.y;
		Vector2 newPos = new Vector2(x,y);
		rig.MovePosition (newPos);
		float angleTowards = AngleTowards (point, rig);
		rig.transform.rotation = Quaternion.Euler(0, 0, angleTowards);
	}
}