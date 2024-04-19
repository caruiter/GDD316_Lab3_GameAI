using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundControl : MonoBehaviour
{
    GameObject[] theEnemies;
    [SerializeField] double soundVolume;
    [SerializeField] double hearingRange;
    [SerializeField] double sightRange;

    // Start is called before the first frame update
    void Start()
    {
        theEnemies = GameObject.FindGameObjectsWithTag("Enemy");   
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            foreach(GameObject enemy in theEnemies)
            {
                double distance = Mathf.Sqrt(Mathf.Pow(enemy.transform.position.x - transform.position.x, 2) + Mathf.Pow(enemy.transform.position.z - transform.position.z, 2));
                double volume = soundVolume / distance;

                if(volume >= 1) //check if enemy hears sound
                {
                    if (distance <= sightRange) //if sound occurs within sight range
                    {
                        enemy.GetComponent<AgentController_FSM>().CaughtSight();
                        Debug.Log("in sight range");
                        Debug.Log(volume);
                    }
                    else if (distance <= hearingRange) // if sound occurs within hearing range
                    {
                        enemy.GetComponent<AgentController_FSM>().HeardNoise(transform.position);
                        Debug.Log("in hearing range");
                        Debug.Log(volume);
                        
                    }
                }
            }
        }
    }

    public double GetSightRange()
    {
        return sightRange;
    }
}
