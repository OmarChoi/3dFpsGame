using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float        _lifeTime        = 0.5f;
    [SerializeField] private int          _totalSegments   = 5;
    [SerializeField] private int          _visibleSegments = 3;
    private bool[] _activeSegments;

    private void Awake()
    {
        _activeSegments = new bool[_totalSegments];
    }

    public void Init(Vector3 startPosition, Vector3 endPosition)
    {
        GetRandomVisibleSegments();
        _lineRenderer.positionCount = _visibleSegments * 2;
        CalculateSegmentPosition(startPosition, endPosition);
        StartCoroutine(ReleaseCoroutine());
    }
    
    private void CalculateSegmentPosition(Vector3 startPosition, Vector3 endPosition)
    {
        int currentIndex = 0;
        for (int i = 0; i < _totalSegments; i++)
        {
            if (!_activeSegments[i]) continue;
        
            float segmentStart = i / (float)_totalSegments;
            float segmentEnd = (i + 1) / (float)_totalSegments;
        
            Vector3 start = Vector3.Lerp(startPosition, endPosition, segmentStart);
            Vector3 end = Vector3.Lerp(startPosition, endPosition, segmentEnd);
        
            _lineRenderer.SetPosition(currentIndex++, start);
            _lineRenderer.SetPosition(currentIndex++, end);
        }
    }
    
    private void GetRandomVisibleSegments()
    {
        System.Array.Clear(_activeSegments, 0, _activeSegments.Length);

        int selected = 0;
        while (selected < _visibleSegments)
        {
            int randomIndex = Random.Range(0, _totalSegments);
            if (_activeSegments[randomIndex]) continue;

            _activeSegments[randomIndex] = true;
            selected++;
        }
    }

    private IEnumerator ReleaseCoroutine()
    {
        yield return new WaitForSeconds(_lifeTime);
        BulletFactory.Instance.Release(this);
    }
}