using System;
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

    // �ⲿ���ã���������
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

    /// <summary>
    /// 下一帧执行 action。用法：CoroutineQueue.inst.NextFrame(() => { ... });
    /// </summary>
    public void NextFrame(Action action)
    {
        Enqueue(NextFrameRoutine(action));
    }

    private IEnumerator NextFrameRoutine(Action action)
    {
        yield return null;
        action?.Invoke();
    }
}