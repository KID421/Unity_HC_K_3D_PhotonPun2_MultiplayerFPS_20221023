using UnityEngine;

namespace KID
{
    /// <summary>
    /// ¤l¼u
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        private string nameTarget = "¤jºµ";

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
