using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// PlayerMovement ¿Œ≈Õ∆‰¿ÃΩ∫
/// </summary>
public interface IPlayerMovement
{

    void Walk(Vector3 _direction); // ∞»±‚
    void Run(); // ∂Ÿ±‚
    void StandUp(); // º≠±‚
    void SitDown(); // æ…±‚
    void KneelDown(); // æ˛µÂ∏Æ±‚
    void Jump();
    void SetAnimation();
}
