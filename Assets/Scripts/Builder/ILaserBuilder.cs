using UnityEngine;

// The steps needed to configure a laser before it's built, and the step that builds it. A
// director drives these in order; a caller only ever needs the finished GameObject Build() hands
// back.
public interface ILaserBuilder
{
    void SetSpeed(float speed);
    void SetLifetime(float lifetime);
    void SetSize(float size);

    GameObject Build(GameObject prefab);
}
