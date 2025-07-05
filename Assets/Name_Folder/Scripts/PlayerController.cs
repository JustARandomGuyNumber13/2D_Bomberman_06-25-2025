using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    CharacterStat cStat;
    Rigidbody2D rb;

    Vector2 inputValue;

    private void Awake()
    {
        TryGetComponent<CharacterStat>(out cStat);
        TryGetComponent<Rigidbody2D>(out rb);
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = inputValue * cStat.MoveSpeed;
    }

    void PlantBomb()
    {
        Vector2 plantPos = transform.position;
        plantPos.x = plantPos.x < 0 ? Mathf.Floor(plantPos.x) : Mathf.Ceil(plantPos.x);
        plantPos.y = plantPos.y < 0 ? Mathf.Floor(plantPos.y) : Mathf.Ceil(plantPos.y);

        Debug.Log("Plant bomb at: " + plantPos);
    }

    #region Input Handler 
    private void OnMove(InputValue value)
    { 
        var dir = value.Get<Vector2>();

        // Prevent from moving diagnally => Prioritize y-axis
        if (dir.x != 0 && dir.y != 0)
            inputValue = new Vector2(0, dir.y);
        else
            inputValue = dir;
    }
    private void OnPlantBomb(InputValue value)
    {
        if (value.Get<float>() != 0)
            PlantBomb();
    }
    #endregion
}
