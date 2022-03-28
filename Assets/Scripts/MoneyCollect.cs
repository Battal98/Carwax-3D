using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyCollect : MonoBehaviour
{
    public float Speed;
    public float failSpeed;
    public float alphaSpeed;

    public Transform target;
    public Transform failTarget;
    public GameObject MoneyPrefab;
    public Camera cam;
    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartMoneyMove (Vector3 _intial)
    {
        Vector3 targetPos = new Vector3(target.position.x, target.position.y, 0);
        GameObject _Money = Instantiate(MoneyPrefab,transform);
        StartCoroutine(MoveMoney(_Money.transform,_intial,targetPos));
        Destroy(_Money,3f);
    }

    public void FailMoneyMove (Vector3 _intial)
    {
        Vector3 targetPos = new Vector3(failTarget.position.x, failTarget.position.y, 0);
        GameObject _Money = Instantiate(MoneyPrefab, transform);
        alphaSpeed = 0.5f;
        StartCoroutine(FailMoveMoney(_Money.transform, _intial, targetPos));
        Destroy(_Money, 3f);
    }
    IEnumerator MoveMoney(Transform obj, Vector3 startPos , Vector3 endPos)
    {
        float time = 0;

        while(time < 1)
        {
            time += Time.deltaTime * Speed;
            obj.position = Vector3.Lerp(startPos,endPos,time);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    IEnumerator FailMoveMoney(Transform obj, Vector3 startPos, Vector3 endPos)
    {
        float time = 0;

        while (time < 1)
        {
            time += Time.deltaTime * failSpeed;
            alphaSpeed -= Time.deltaTime;
            obj.position = Vector3.Lerp(startPos, endPos, time);
            obj.GetComponent<Image>().color = new Color(1, 1, 1, alphaSpeed);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
