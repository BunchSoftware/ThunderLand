using System;

[Serializable]
public class Vector3Package
{
    public float x { get; }
    public float y { get; }
    public float z { get; }

    public Vector3Package(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}
