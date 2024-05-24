using UnityEngine;

[RequireComponent(typeof(Camera))]
public sealed class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Player _target;

    [SerializeField] private float _depth;
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _minZoom;
    [SerializeField] private float _maxZoom;

    private void Start() => _camera = GetComponent<Camera>();

    private void Update()
    {
        MoveTo(_target.transform);
        ChangeZoom(Input.GetAxis("MouseScrollWheel"));
    }

    private void MoveTo(Transform target)
    {
        float x = target.position.x;
        float y = target.position.y;
        var position = new Vector3(x, y, _depth);

        transform.position = position;
    }

    private void ChangeZoom(float change)
    {
        float resultChange = -change * _zoomSpeed * _camera.orthographicSize;
        resultChange += _camera.orthographicSize;
        resultChange = Mathf.Clamp(resultChange, _minZoom, _maxZoom);
        _camera.orthographicSize = resultChange;
    }
}