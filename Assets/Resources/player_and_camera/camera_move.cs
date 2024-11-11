using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // 玩家对象的Transform
    public Vector3 offset; // 摄像机与玩家之间的偏移

    void Start() {
        offset=transform.position-player.position;
    }
    void Update() {
        transform.position=player.position+offset;
    }

}