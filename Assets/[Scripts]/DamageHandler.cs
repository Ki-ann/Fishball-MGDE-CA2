using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DamageHandler : MonoBehaviour
{

    public GameManager gameManager;
    public Image currentHpImage;
    public string opposingTag;
    public string opposingTagBody;
    Quaternion originalRotation;

    public float impactDamage;

    void Start()
    {
        originalRotation = currentHpImage.transform.rotation;
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == opposingTag || other.tag == opposingTagBody)
        {
            Debug.Log(other.tag);
            if (transform.GetComponent<NavMeshAgent>())
            {
                transform.GetComponent<NavMeshAgent>().ResetPath();
                transform.GetComponent<NavMeshAgent>().enabled = false;
                gameManager.enemyAIController.enabled = false;
            }
            gameManager.playerController.enabled = false;

            StartCoroutine(Reactivate());


            if (currentHpImage.fillAmount == 0)
            {
                StartCoroutine(SlowMo());
            }
            CheckDamage(other);
        }
    }
    void CheckDamage(Collider other)
    {

        if (other.transform.tag == opposingTag)
        {
            currentHpImage.fillAmount -= impactDamage / 2;

            other.transform.GetComponent<Rigidbody>().AddForce(this.transform.forward * 50f);
            transform.GetComponent<Rigidbody>().AddForce(-transform.forward * 50f);
        }
        else if (other.transform.tag == opposingTagBody)
        {
            other.GetComponentInParent<DamageHandler>().currentHpImage.fillAmount -= impactDamage;
            RaycastHit hit;
            Vector3 dir = transform.forward;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                dir = hit.point - transform.position;
            }
            // Calculate Angle Between the collision point and the player
            // We then get the opposite (-Vector3) and normalize it
            dir = dir.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the player
            other.transform.parent.GetComponent<Rigidbody>().AddForce(dir * 100f);
            transform.GetComponent<Rigidbody>().AddForce(-dir * 50f);
            //collision.transform.parent.GetComponent<Rigidbody>().AddForce(transform.forward * 40f);

            //transform.GetComponent<Rigidbody>().velocity = -transform.forward * 10f;
        }
    }

    IEnumerator SlowMo()
    {

        Time.timeScale = 0.125f;
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 1;


    }
    IEnumerator Reactivate()
    {
        var rb1 = gameManager.playerController.GetComponent<Rigidbody>();
        var rb2 = gameManager.enemyAIController.GetComponent<Rigidbody>();
        var co = StartCoroutine(Turn(rb1, rb2));
        yield return new WaitForSeconds(0.5f);

        StopCoroutine(co);
        gameManager.playerController.enabled = true;
        if (transform.GetComponent<NavMeshAgent>())
        {
            yield return new WaitForSeconds(0.5f);
            transform.GetComponent<NavMeshAgent>().enabled = true;
            gameManager.enemyAIController.enabled = true;
        }
    }
    IEnumerator Turn(Rigidbody RB1, Rigidbody RB2)
    {
        while (true)
        {

            gameManager.playerController.transform.rotation = Quaternion.RotateTowards(gameManager.playerController.transform.rotation, Quaternion.LookRotation(RB1.velocity), 200 * Time.deltaTime);

            gameManager.enemyAIController.transform.rotation = Quaternion.RotateTowards(gameManager.enemyAIController.transform.rotation, Quaternion.LookRotation(RB2.velocity), 200 * Time.deltaTime);
            yield return null;
        }
    }


    float shakeRange = 100;
    float shakeTime = 0.3f;

    private IEnumerator UIShake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeTime)
        {

            elapsed += Time.deltaTime;
            float z = Random.value * shakeRange - (shakeRange / 2);
            currentHpImage.transform.eulerAngles = new Vector3(currentHpImage.transform.rotation.x, currentHpImage.transform.rotation.y, currentHpImage.transform.rotation.z + z);
            yield return null;
        }

        currentHpImage.transform.rotation = originalRotation;
    }
}
