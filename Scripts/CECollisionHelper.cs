using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CECollisionHelper : MonoBehaviour
{
    [System.Serializable]
    public class CollisionEvent {
        public string TargetName;
        public UnityEvent EnterEvent, ExitEvent;
    }
    [SerializeField]
    List<CollisionEvent> events;
    private void OnCollisionEnter(Collision other) {
        foreach (var e in events) {
            if (other.gameObject.name == e.TargetName) {
                e.EnterEvent.Invoke();
            }
        }
    }
    private void OnCollisionExit(Collision other) {
        foreach (var e in events) {
            if (other.gameObject.name == e.TargetName) {
                e.ExitEvent.Invoke();
            }
        }
    }
}
