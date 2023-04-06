using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ladder와 같은 interactable tool이 가지는 인터페이스
/// </summary>
public interface IInteractableTool : IInteractableObject
{
    public enum EInteractableTool { Ladder, Len}
}
