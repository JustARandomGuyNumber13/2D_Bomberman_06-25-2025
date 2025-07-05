using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimatorHandler : MonoBehaviour
{
    [SerializeField] Animator anim;
    Rigidbody2D rb;
    private void Awake()
    {
        TryGetComponent<Rigidbody2D>(out rb);
    }
    public void Anim_Die()
    {
        anim.SetTrigger(die);
    }
    void OnMove(InputValue value)
    {
        var dir = value.Get<Vector2>();
        anim.SetFloat(xDir, dir.x);
        anim.SetFloat(yDir, dir.y);
    }

    int xDir = Animator.StringToHash("xDir");
    int yDir = Animator.StringToHash("yDir");
    int die = Animator.StringToHash("Die");
}
