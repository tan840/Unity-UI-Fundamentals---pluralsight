using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class HeroController : MonoBehaviour
{
    public float enemyDetectRange = 2;
    public GameObject hitFX;
    public Transform hitFXPoint;

    Animator anim;
    NavMeshAgent navMeshAgent;
    GameObject target;
    bool go = true;

    void Awake()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        anim.SetBool("Attack", false);
        anim.SetFloat("Speed", navMeshAgent.velocity.magnitude);

        // click to move
        if (go)
        {
            Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Input.GetMouseButtonDown(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject()) // if not a UI element
                {
                    navMeshAgent.stoppingDistance = 0;
                    if (Physics.Raycast(interactionRay, out hit))
                    {
                        if (hit.collider.gameObject.tag == "Enemy")
                        {
                            navMeshAgent.destination = hit.collider.transform.position;
                            target = hit.collider.gameObject;
                            navMeshAgent.stoppingDistance = 1.5f;
                        }
                        else
                        {
                            navMeshAgent.destination = hit.point;
                        }
                    }
                }
            }

            // target check
            if (target != null)
            {
                if (Vector3.Distance(transform.position, target.transform.position) <= enemyDetectRange)
                {
                    Vector3 tPos = target.transform.position;
                    tPos.y = transform.position.y;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((tPos - transform.position).normalized), Time.deltaTime * 10);
                    anim.SetBool("Attack", true);
                }
            }
        }
    }

    public void Hit()
    {
        if (target != null)
        {
            target.GetComponent<Enemy>().TakeDamage(1);
            Instantiate(hitFX, hitFXPoint.position, hitFXPoint.rotation);
        }
    }

    public void DisableMovement()
    {
        go = false;
    }
}
