using UnityEngine;

public enum VectorAnimType { Position, Rotation, Scale }

public class VectorAnim : Anim
{
    public VectorAnimType type;

    private Vector3 sourceVector;
    private Vector3 targetVector;

    public void Set(float duration, Vector3 targetVector)
    {
        Vector3 v = Vector3.zero;
        if (type == VectorAnimType.Position)
            v = transform.position;
        else if (type == VectorAnimType.Rotation)
            v = transform.eulerAngles;
        else if (type == VectorAnimType.Scale)
            v = transform.localScale;

        time = 0.0f;
        sourceVector = v;
        this.duration = duration;
        this.targetVector = targetVector;

        enabled = true;
    }

    private void Update()
    {
        time += Time.deltaTime / duration;

        Vector3 v = Vector3.Lerp(sourceVector, targetVector, time);
        if (type == VectorAnimType.Position)
            transform.position = v;
        else if (type == VectorAnimType.Rotation)
            transform.eulerAngles = v;
        else if (type == VectorAnimType.Scale)
            transform.localScale = v;

        if (time >= 1)
            enabled = false;
    }
}
