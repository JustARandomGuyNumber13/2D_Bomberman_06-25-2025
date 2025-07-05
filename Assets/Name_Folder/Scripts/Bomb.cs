using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public void SpawnBomb(float explosionRange, float explosionTime)
    { 
    
        StartCoroutine(ExplosionCoroutine(explosionRange, explosionTime));
    }
    IEnumerator ExplosionCoroutine(float explosionRange, float explosionTime)
    {
        yield return new WaitForSeconds(explosionTime);

        Vector2 pos = transform.position;

        Vector2 left = new Vector2(pos.x - explosionRange, pos.y);
        Vector2 right = new Vector2(pos.x + explosionRange, pos.y);
        Debug.DrawLine(left, right, Color.blue, 3f);

        Vector2 up = new Vector2(pos.x, pos.y + explosionRange);
        Vector2 down = new Vector2(pos.x, pos.y - explosionRange);
        Debug.DrawLine(up, down, Color.blue, 3f);

    }
}
