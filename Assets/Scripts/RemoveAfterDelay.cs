using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAfterDelay : MonoBehaviour
{
    // Задержка в секундах перед удалением.
    public float delay = 1f;
    void Start()
    {
        // Запустить сопрограмму 'Remove'.
        StartCoroutine("Remove");
    }

    IEnumerator Remove()
    {
        // gameObject, присоединенный к объекту this.
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
