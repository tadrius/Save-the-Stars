using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{

    public const string Collectible = "Collectible";
    public const string Finish = "Finish";
    public const string Friendly = "Friendly";

    private void OnCollisionEnter(Collision other) {
        switch (other.gameObject.tag) {
            case Collectible:
                Debug.Log("Collided with a Collectible.");
                break;
            case Finish:
                Debug.Log("Collided with a Finish.");
                break;
            case Friendly:
                Debug.Log("Collided with a Friendly.");
                break;
            default:
                Debug.Log("Collided.");
                break;
        }
    }
}
