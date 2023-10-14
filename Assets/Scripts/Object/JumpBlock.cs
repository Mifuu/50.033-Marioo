using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JumpBlock : MonoBehaviour
{
    public Collider2D enabledCollider;
    public Collider2D disabledCollider;
    new public Collider2D collider;

    public Animator questionBlockAnimator;

    public int defaultActivatationCount = 1;
    private int remainingActivation = 1;

    public UnityEvent triggerEvent;

    void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            if (collision.collider.bounds.max.y < collider.bounds.min.y && player.playerMovement.rb.velocity.y >= 0)
                Trigger();
        }
    }

    void Trigger()
    {
        if (remainingActivation < 1) return;

        SFXManager.TryPlaySFX("mario_coin", gameObject);

        triggerEvent.Invoke();
        GameManager.instance.AddScore();

        remainingActivation--;
        if (remainingActivation < 1)
        {
            SetDisable();
        }
    }

    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        remainingActivation = defaultActivatationCount;
        if (remainingActivation > 0)
            SetEnable();
    }

    void SetEnable()
    {
        if (enabledCollider != null) enabledCollider.enabled = true;
        if (disabledCollider != null) disabledCollider.enabled = false;

        questionBlockAnimator.SetBool("isEnabled", true);
    }

    void SetDisable()
    {
        if (enabledCollider != null) enabledCollider.enabled = false;
        if (disabledCollider != null) disabledCollider.enabled = true;

        questionBlockAnimator.SetBool("isEnabled", false);
    }
}
