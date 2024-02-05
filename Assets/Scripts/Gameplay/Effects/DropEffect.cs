using UnityEngine;
using UnityEngine.UI;

namespace Minesweeper.Gameplay
{
    public class DropEffect : MonoBehaviour
    {
        private float verticalForce = 30f;
        private float horizontalForce = 30f;
        private float horizontalTorque = 15f;
        private float timeToDestroy = 3f;

        public void Init(Transform dropHolder, bool goToRight)
        {
            Rigidbody2D rig = gameObject.AddComponent<Rigidbody2D>();
            int rng = goToRight ? 1 : -1;
            float randomHorizontalForce = horizontalForce * rng;
            rig.AddForce(new Vector2(randomHorizontalForce, verticalForce), ForceMode2D.Impulse);
            rig.AddTorque(horizontalTorque * rng);
            rig.gravityScale = 9f;

            Image img = gameObject.GetComponent<Image>();
            img.CrossFadeAlpha(0, timeToDestroy, true);

            transform.SetParent(dropHolder);

            Destroy(gameObject, timeToDestroy);
        }
    }
}
