using System.Collections;
using UnityEngine;

namespace Toolbox
{
    public class Health2D : Health
    {
        public SpriteRenderer model;
        public Color hurtTint = Color.red;
        public float hurtFlashDuration = 0.1f;

        Color originalTint;

        public override void Start()
        {
            base.Start();
            originalTint = model.color;
        }

        public override bool ApplyDamage(float damage)
        {
            bool result = base.ApplyDamage(damage);

            if (result)
            {
                model.color = hurtTint;
                StartCoroutine(TurnOffTint());
            }

            return result;
        }

        IEnumerator TurnOffTint()
        {
            yield return new WaitForSeconds(hurtFlashDuration);

            model.color = originalTint;
        }
    }
}