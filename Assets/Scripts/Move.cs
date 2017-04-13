using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
	public float speed = 0.1f;

	private IEnumerator moveRoutine;

	private IEnumerator MoveRoutine(Vector2 dest) {
		while (Vector2.Distance (transform.position, dest) > float.Epsilon) {
			Vector2 path = (dest - (Vector2)transform.position);
			Vector2 movement = path.normalized * speed;
			if (movement.magnitude > path.magnitude) {
				transform.position = dest;
			} else {
				transform.position += (Vector3)(path.normalized * speed);
			}
			yield return null;
		}
	}

	public void MoveTo(Vector2 dest) {
		if (moveRoutine != null) {
			StopCoroutine (moveRoutine);
		}
		moveRoutine = MoveRoutine (dest);
		StartCoroutine(moveRoutine);
	}
}
