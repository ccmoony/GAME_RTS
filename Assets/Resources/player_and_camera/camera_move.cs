using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // 玩家对象的Transform
    public Vector3 offset; // 摄像机与玩家之间的偏移
    public float stayDuration = 2f; // 停留时间
    public float moveDuration = 1f; // 移动时间
    private bool canUpdate = false;
    void Start() {
        offset=transform.position-player.position;
        transform.position=new Vector3(0,81.8f,0.6f);
        transform.eulerAngles=new Vector3(90,0,0);
        
        StartCoroutine(MoveCamera());
        
    }
    private IEnumerator MoveCamera()
    {
        // 停留在当前位置
        yield return new WaitForSeconds(stayDuration);

        // 获取起始位置
        Vector3 startPosition = transform.position;
        
        Vector3 endPosition = player.position+offset;
        Vector3 startRotation = transform.eulerAngles;
        Vector3 endRotation = new Vector3(60,0,0);
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            // 计算插值
            endPosition = player.position+offset;
            float t = elapsedTime / moveDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            transform.eulerAngles = Vector3.Lerp(startRotation, endRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null; // 等待下一帧
        }
        canUpdate = true;
        // 确保摄像机最终到达目标位置
        transform.position = endPosition;
    }
    void Update() {
        if (canUpdate)
        {
        transform.position=player.position+offset;
        }
    }

}