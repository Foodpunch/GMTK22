using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager instance;

    public GameObject[] VFXObjects;
    public GameObject[] BloodSplats;

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
    public void BloodSplat(Vector3 dir,Vector3 pos)
    {
        GameObject splat = Instantiate(BloodSplats[Random.Range(0,BloodSplats.Length)], pos, Quaternion.identity);
        Color newColor = Color.HSVToRGB(0, 0, (Random.Range(0.4f, 1f)));
        splat.GetComponent<SpriteRenderer>().color = newColor;
        splat.transform.right = dir;
        splat.transform.rotation *= Quaternion.Euler(0, 0, Random.Range(-5f, 5f));
        splat.transform.SetParent(transform);
    }
    public void BloodSpray(Vector3 pos)
    {
        Instantiate(VFXObjects[3], pos, Quaternion.identity);
    }
  
}
