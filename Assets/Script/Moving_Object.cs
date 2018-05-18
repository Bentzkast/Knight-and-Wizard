using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Moving_Object : MonoBehaviour {

    public float move_time = .1f;
	public LayerMask blockingLayer;

	private BoxCollider2D _boxCollider;
	private Rigidbody2D _rigidbody2D;
	private float _inverseMoveTime;
    protected bool isMoving = false;

    protected virtual void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _inverseMoveTime = 1f / move_time;
    }

    protected IEnumerator SmoothMovement(Vector3 target_pos)
    {

        float sqr_remaining_distance = (transform.position - target_pos).sqrMagnitude;
        while(sqr_remaining_distance > float.Epsilon){
            Vector3 new_position = Vector3.MoveTowards(transform.position, target_pos, _inverseMoveTime * Time.deltaTime);
            _rigidbody2D.MovePosition(new_position);
            sqr_remaining_distance = (transform.position - target_pos).sqrMagnitude;

            yield return null;
        }
		isMoving = false;
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start_position = transform.position;
        Vector2 target_position = start_position + new Vector2(xDir, yDir);
        _boxCollider.enabled = false;
        hit = Physics2D.Linecast(start_position, target_position, blockingLayer);
        _boxCollider.enabled = true;

        if(hit.transform == null)
        {
            StartCoroutine(SmoothMovement(target_position));
            return true;
        }
        return false;
    }

    protected virtual void AttemptMove<T>(int xDir, int yDir) where T : Component
    {
        RaycastHit2D hit2D;
        bool can_move = Move(xDir, yDir, out hit2D);
        if (hit2D.transform == null) return;
        T hit_component = hit2D.transform.GetComponent<T>();
		if (!can_move && hit_component != null){
			OnCantMove<T>(hit_component);
		}     
    }
    protected abstract void OnCantMove<T>(T component) where T : Component;
}
