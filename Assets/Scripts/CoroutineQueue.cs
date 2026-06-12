using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineQueue : MonoBehaviour
{
    private Queue<IEnumerator> queue = new Queue<IEnumerator>();
    private bool isRunning = false;
    public static CoroutineQueue inst;
    private void Start()
    {
        inst = this;
    }

    // 外部调用：添加任务
    public void Enqueue(IEnumerator coroutine)
    {
        queue.Enqueue(coroutine);

        if (!isRunning)
        {
            StartCoroutine(Run());
        }
    }

    private IEnumerator Run()
    {
        isRunning = true;

        while (queue.Count > 0)
        {
            yield return StartCoroutine(queue.Dequeue());
        }

        isRunning = false;
    }
}