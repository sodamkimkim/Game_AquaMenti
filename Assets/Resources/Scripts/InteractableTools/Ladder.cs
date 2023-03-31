using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour, IInteractableTool
{
    public Transform GetLadderTransform() { return transform; }
    public Vector3 GetLadderPos() { return transform.position; }
    public string GetName()
    {
        return this.gameObject.name;
    }
    /// <summary>
    /// Ladder의 position을 바꿔주는 메서드
    /// </summary>
    /// <param name="_newPos"></param>
    public void SetLadderPos(Vector3 _newPos)
    {
        GetLadderTransform().position = _newPos;
    }
 /// <summary>
 /// Ladder의 x position 값만 변경해 주는 메서드
 /// </summary>
 /// <param name="_x"></param>
    public void SetLadderXPos(float _x)
    {
        Vector3 nowPos = GetLadderPos();
        Vector3 newPos = nowPos;
        newPos.x = _x;
    }
    /// <summary>
    /// Ladder의 z position 값만 변경해 주는 메서드
    /// </summary>
    /// <param name="_x"></param>
    public void SetLadderZPos(float _z)
    {
        Vector3 nowPos = GetLadderPos();
        Vector3 newPos = nowPos;
        newPos.z = _z;
    }
} // end of class
