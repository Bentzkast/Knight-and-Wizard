using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Moving_Object : MonoBehaviour {

    public float move_time = .1f;
    public LayerMask blocking_layer;

    private BoxCollider2D box_collider;
    private Rigidbody2D rigidbody_2d;
    private float inverse_move_time;
    protected bool moving = false;

    protected virtual void Start()
    {
        box_collider = GetComponent<BoxCollider2D>();
        rigidbody_2d = GetComponent<Rigidbody2D>();
        inverse_move_time = 1f / move_time;
    }

    protected IEnumerator SmoothMovement(Vector3 target_pos)
    {
        moving = true;
        float sqr_remaining_distance = (transform.position - target_pos).sqrMagnitude;
        while(sqr_remaining_distance > float.Epsilon){
            Vector3 new_position = Vector3.MoveTowards(transform.position, target_pos, inverse_move_time * Time.deltaTime);
            rigidbody_2d.MovePosition(new_position);
            sqr_remaining_distance = (transform.position - target_pos).sqrMagnitude;

            yield return null;
        }
        moving = false;
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start_position = transform.position;
        Vector2 target_position = start_position + new Vector2(xDir, yDir);
        box_collider.enabled = false;
        hit = Physics2D.Linecast(start_position, target_position, blocking_layer);
        box_collider.enabled = true;

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
        if (!can_move && hit_component != null)
            OnCantMove<T>(hit_component);
            
    }
    protected abstract void OnCantMove<T>(T component) where T : Component;
}
