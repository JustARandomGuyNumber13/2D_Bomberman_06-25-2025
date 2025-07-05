using System;
using UnityEngine;

public class StatModifier : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] float value;

    [SerializeField] CharacterStat.FloatModifier floatModifier = CharacterStat.FloatModifier.None;
    [SerializeField] CharacterStat.IntModifier intModifier = CharacterStat.IntModifier.None;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterStat pStat;
        collision.TryGetComponent<CharacterStat>(out pStat);

        if (pStat != null)
        {
            if (floatModifier != CharacterStat.FloatModifier.None)
            {
                pStat.ApplyFloatModifier(floatModifier, value, duration);
                Destroy(gameObject);
            }
            else if (intModifier != CharacterStat.IntModifier.None)
            {
                pStat.ApplyIntModifier(intModifier, (int)Math.Round(value, MidpointRounding.AwayFromZero), duration);
                Destroy(gameObject);
            }
        }
    }
}
