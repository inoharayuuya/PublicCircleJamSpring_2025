using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 5f; // �J�����̒Ǐ]���x

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 targetPosition = new Vector3(player.position.x, player.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }

}
