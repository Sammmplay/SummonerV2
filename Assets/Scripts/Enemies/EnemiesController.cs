using UnityEngine;

public class EnemiesScript : MonoBehaviour
{
    public Transform targetObj;
    public int velocidadEnemigos = 5;

    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(this.transform.position, targetObj.position, velocidadEnemigos * Time.deltaTime);
    }
}
