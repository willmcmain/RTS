using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour, IOrder {
	public float speed = 3f;

	private IEnumerator moveRoutine;
	private Vector2 lastMove = Vector2.zero;

	private IEnumerator MoveRoutine(Vector2 dest) {
		while (Vector2.Distance (transform.position, dest) > 0.05f) {
			Vector2 path = (dest - (Vector2)transform.position);
			Vector2 movement = path.normalized * speed * Time.deltaTime;
			transform.Translate(movement);
			lastMove = movement;
			yield return new WaitForFixedUpdate();
		}
	}

	public void Order(Vector2 dest) {
		if (moveRoutine != null) {
			StopCoroutine (moveRoutine);
		}
		moveRoutine = MoveRoutine(dest);
		StartCoroutine(moveRoutine);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.transform.tag != "Unit") {
			if (moveRoutine != null) {
				StopCoroutine(moveRoutine);
			}
			Vector2 dir = lastMove.normalized;
			transform.Translate(-dir * 0.05f);
		}
	}
}
