using UnityEngine;

namespace KID
{
    /// <summary>
    /// �l�u
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        private string nameTarget = "�j��";

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.name.Contains(nameTarget))
            {
                collision.gameObject.GetComponent<PlayerControl>().Damage();
            }

            Destroy(gameObject);
        }
    }
}
