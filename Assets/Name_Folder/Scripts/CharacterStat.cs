using System.Collections;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    public float MoveSpeed = 3;
    public int MaxBombCount = 1;
    [HideInInspector] public int BombCount;
    public int BombExplosionRange = 3;

    #region INT MODIFIER 
    public enum IntModifier
    { 
        None,
        MaxBombCount,
        BombExplosionRange
    }
    public void ApplyIntModifier(IntModifier modifier, int value, float duration)
    {
        StartCoroutine(ApplyIntModifierCoroutine(modifier, value, duration));
    }
    IEnumerator ApplyIntModifierCoroutine(IntModifier modifier, int value, float duration)
    {
        switch (modifier)
        {
            case IntModifier.MaxBombCount:
                MaxBombCount += value;
                BombCount += value;
                break;
            case IntModifier.BombExplosionRange:
                BombExplosionRange += value; 
                break;
        }

        if (duration <= 0) yield break;

        yield return new WaitForSeconds(duration);

        switch (modifier)
        {
            case IntModifier.MaxBombCount:
                MaxBombCount -= value;
                BombCount -= value;
                break;
            case IntModifier.BombExplosionRange:
                BombExplosionRange -= value;
                break;
        }
    }
    #endregion

    #region FLOAT MODIFIER 
    public enum FloatModifier
    {
        None,
        MoveSpeed
    }
    public void ApplyFloatModifier(FloatModifier modifier, float value, float duration)
    {
        StartCoroutine(ApplyFloatModifierCoroutine(modifier, value, duration));
    }
    IEnumerator ApplyFloatModifierCoroutine(FloatModifier modifier, float value, float duration)
    {
        switch (modifier)
        {
            case FloatModifier.MoveSpeed:
                MoveSpeed += value;
                break;
        }

        if(duration  <= 0) yield break;

        yield return new WaitForSeconds(duration);

        switch (modifier)
        {
            case FloatModifier.MoveSpeed:
                MoveSpeed -= value;
                break;
        }
    }
    #endregion
}
