using UnityEngine;
using System.Collections;
using System;

public abstract class Lerp<T>
{
    public T StartValue { get; set; }
    public T EndValue { get; set; }
    public Boolean Loop { get; set; }
    public Boolean Reverse { get; set; }
    public float Duration { get; set; }
    public float Step { get; set; }
    public T Value { get; set; }

    protected float startTime;

    protected bool stopped;
    protected T valueStopped;
    protected T temp;

    public Lerp(T startValue, T endValue, Boolean loop, Boolean reverse, float duration)
    {
        this.StartValue = startValue;
        this.EndValue = endValue;
        this.Loop = loop;
        this.Reverse = reverse;
        this.Duration = duration;

        this.startTime = float.MinValue;
    }

    public float Percent
    {
        get
        {
            if (startTime == float.MinValue)
                startTime = Time.time;

            return (Time.time - startTime) / Duration;
        }
    }

    public T Eval(Boolean stop)
    {
        valueStopped = Eval();
        stopped = stop;
        return valueStopped;
    }

    public T Eval()
    {
        if (stopped)
            return valueStopped;

        float time = Time.time;

        if (startTime == float.MinValue)
            startTime = time;

        if (Percent < 1f || Reverse || Loop)
        {
            ComputeEval(time);

            if (Percent > 1f && Reverse)
            {
                temp = StartValue;
                StartValue = EndValue;
                EndValue = temp;

                startTime = time;
            }
            else if (Percent > 1f && Loop)
            {
                startTime = time;
            }
        }

        return Value;
    }

    protected abstract void ComputeEval(float time);

    public bool IsFinished()
    {
        return Percent >= 1f;
    }
}

public class LerpFloat : Lerp<float>
{
    public LerpFloat(float startValue, float endValue, Boolean loop, Boolean reverse, float duration)
        : base(startValue, endValue, loop, reverse, duration)
    {
    }

    protected override void ComputeEval(float time)
    {
        Value = Mathf.Lerp(StartValue, EndValue, Percent);
    }
}

public class LerpVector3 : Lerp<Vector3>
{
    public LerpVector3(Vector3 startValue, Vector3 endValue, Boolean loop, Boolean reverse, float duration)
        : base(startValue, endValue, loop, reverse, duration)
    {
    }

    protected override void ComputeEval(float time)
    {
        Value = Vector3.Lerp(StartValue, EndValue, Percent);
    }
}
