using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _player;

    private void LateUpdate()
    {
        if (_player != null)
        {
            Vector3 vector3 = transform.position;

            vector3.x = _player.position.x;
            vector3.y = _player.position.y;

            transform.position = vector3;
        }
    }
}