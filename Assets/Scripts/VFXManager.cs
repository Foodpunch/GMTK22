using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager instance;

    public GameObject[] VFXObjects;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    public void Poof(Vector3 pos)
    {   //disgusting
        Destroy(Instantiate(VFXObjects[0], pos, Quaternion.identity),2f);
    }
    public void Boom(Vector3 pos)
    {
        Quaternion test = Quaternion.Euler(0, 0, Random.Range(0, 360));
        Destroy(Instantiate(VFXObjects[2], pos,test),2f);
    }
    public void Spark(Vector3 pos, Vector3 normal)
    {
        GameObject spark = Instantiate(VFXObjects[1], pos, Quaternion.identity);
        spark.transform.right = normal;
        Destroy(spark, 2f);
    }
    public void BloodSplat(Vector3 dir)
    {
        GameObject splat = Instantiate(VFXObjects[3], transform.position, Quaternion.identity);
        splat.transform.right = dir;
        Destroy(splat, 2f);
    }
    public void BloodSpray(Vector3 pos)
    {
        Instantiate(VFXObjects[4], transform.position, Quaternion.identity);
    }
  
}
