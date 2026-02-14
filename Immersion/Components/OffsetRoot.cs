using UnityEngine;

namespace Immersion.Components;

public class OffsetRoot : MonoBehaviour
{
    private Vector3 nextLocalPosition;

    private Quaternion nextLocalRotation;

    private GameObject pivotObject;

    public static OffsetRoot NewOffsetRoot(string name, GameObject objectToOffset)
    {
        var offsetRoot = new GameObject(name).AddComponent<OffsetRoot>();
        offsetRoot.transform.parent = objectToOffset.transform.parent;
        offsetRoot.transform.localPosition = Vector3.zero;
        offsetRoot.transform.localEulerAngles = Vector3.zero;
        offsetRoot.pivotObject = objectToOffset;
        objectToOffset.transform.parent = offsetRoot.transform;
        return offsetRoot;
    }

    /// <summary>
    /// Adds a translational offset to be applied on the next LateUpdate
    /// </summary>
    /// <param name="position">The translational component of the offset</param>
    public void AddOffset(Vector3 position)
    {
        nextLocalPosition += position;
    }

    /// <summary>
    /// Adds a rotational offset to be applied on the next LateUpdate
    /// </summary>
    /// <param name="rotation">The rotational component of the offset</param>
    public void AddOffset(Quaternion rotation)
    {
        nextLocalRotation *= rotation;
    }

    /// <summary>
    /// Adds an offset to be applied on the next LateUpdate
    /// </summary>
    /// <param name="position">The translational component of the offset</param>
    /// <param name="rotation">The rotational component of the offset</param>
    public void AddOffset(Vector3 position, Quaternion rotation)
    {
        AddOffset(position);
        AddOffset(rotation);
    }

    private void ApplyOffset()
    {
        ResetOffset();

        transform.localPosition = nextLocalPosition;
        nextLocalRotation.ToAngleAxis(out float angle, out Vector3 axis);
        transform.RotateAround(pivotObject.transform.position, pivotObject.transform.TransformDirection(axis), angle);

        (nextLocalPosition, nextLocalRotation) = (Vector3.zero, Quaternion.identity);
    }

    private void ResetOffset()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    private void LateUpdate()
    {
        ApplyOffset();
    }
}