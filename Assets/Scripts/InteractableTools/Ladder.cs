using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour, IInteractableTool
{
    public bool isMoveable_ { get; set; }

    public Transform GetTransform() { return transform; }
    public Vector3 GetPos() { return transform.position; }
    public string GetName()
    {
        return this.gameObject.name;
    }

    /// <summary>
    /// Ladder�� position�� �ٲ��ִ� �޼���
    /// </summary>
    /// <param name="_newPos"></param>
    public void SetLadderPos(Vector3 _newPos)
    {
        GetTransform().position = _newPos;
    }
    public void RotateLadderLeft()
    {
        Vector3 euler = new Vector3(0f, -45f, 0f);
        GetTransform().Rotate(euler);
    }
    public void RotateLadderRight()
    {
        Vector3 euler = new Vector3(0f, 45f, 0f);
        GetTransform().Rotate(euler);
    }
    

} // end of class