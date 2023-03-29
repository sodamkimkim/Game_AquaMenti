using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// PlayerMovement 인터페이스
/// </summary>
public interface IPlayerMovement
{
    /// <summary>
    /// walk vs run이냐에 따라 _direction에 대한 moveSpeed달라짐
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_direction"></param>
    void Walk(Vector3 _direction); // 걷기
    void Run(); // 뛰기
    void StandUp(); // 서기
    void SitDown(); // 앉기
    void KneelDown(); // 엎드리기
    void Jump();
    void SetAnimation();
}
