using UnityEngine;

public class EnemiesScript : MonoBehaviour
{
    public GameObject targetObj;
    public int velocidadEnemigos = 5;

    private void Start()
    {
        targetObj = GetComponent<GameObject>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(this.transform.position, targetObj.transform.position, velocidadEnemigos * Time.deltaTime);
    }
}
