using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetroTrain : MonoBehaviour
{
    public float trainSpeed = 300f;
    public float waitTime = 1f;
    public bool isWait = false;

    public MetroLine CurrentMetroLine { get => _currentMetroLine; set => _currentMetroLine = value; }

    public CityNode CurrentCityNode { get => _currentCityNode; set => _currentCityNode = value; }


    private CityNode _currentCityNode;
    private MetroLine _currentMetroLine;
    private List<CityNode> _cityNodes;
    private int _currentNodeIndex = 0;
    private bool _isForward = true;

    private void Start()
    {
        InitializeRoute();
    }

    private void InitializeRoute()
    {
        if(CurrentMetroLine != null)
        {
            _cityNodes = CurrentMetroLine.CityNodes;
            _currentNodeIndex = 0;
            _isForward = true;
            if(_cityNodes.Count > 0)
            {
                CurrentCityNode = _cityNodes[_currentNodeIndex];
            }
        }
    }

    private void Update()
    {
        if(CurrentMetroLine != null && CurrentCityNode != null && !isWait)
        {
            MoveTowardsNextNode();
        }
    }

    private void MoveTowardsNextNode()
    {
        CityNode targetNode = _cityNodes[_currentNodeIndex];
        Transform targetTransform = targetNode.transform;

        float step = trainSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetTransform.position,
            step
        );

        if(Vector3.Distance(transform.position, targetTransform.position) < 0.1f)
        {
            StartCoroutine(WaitAtNode());
        }
    }

    private IEnumerator WaitAtNode()
    {
        isWait = true;
        yield return new WaitForSeconds(waitTime);
        UpdateCurrentNode();
        isWait = false;
    }

    private void UpdateCurrentNode()
    {
        if(_isForward)
        {
            _currentNodeIndex++;
            if(_currentNodeIndex >= _cityNodes.Count)
            {
                _currentNodeIndex = _cityNodes.Count - 2;
                _isForward = false;
            }
        }
        else
        {
            _currentNodeIndex--;
            if(_currentNodeIndex < 0)
            {
                _currentNodeIndex = 1;
                _isForward = true;
            }
        }
        CurrentCityNode = _cityNodes[_currentNodeIndex];
    }
}