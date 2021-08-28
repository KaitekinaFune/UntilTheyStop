using System;

public class HealthChangeArgs : EventArgs
{
    public float Max { get; }
    public float Current { get; }

    public HealthChangeArgs(float max, float current)
    {
        Max = max;
        Current = current;
    }
}