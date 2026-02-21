using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneManager))]
public class SceneController : MonoBehaviour
{
	[SerializeField]
	private InputActionReference _togglePlanesAction;
	
	private ARPlaneManager _planeManager;
	private bool _isVisible = true;
	private int _numPlanesAddedOccurred = 0;
	
    // Start is called before the first frame update
    void Start()
    {
        
		_planeManager = GetComponent<ARPlaneManager>();
		
		if (_planeManager is null)
		{
		//baszhatod
		}
		
		_togglePlanesAction.action.performed += OnTogglePlanesAction;
		_planeManager.planesChanged += OnPlanesChanged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	private void OnTogglePlanesAction(InputAction.CallbackContext obj)
	{
		_isVisible = !_isVisible;
		float fillAlpha = _isVisible ? 0.3f : 0f;
		float lineAlpha = _isVisible ? 1.0f : 0f;
		
		foreach (var plane in _planeManager.trackables)
		{
			SetPlaneAlpha(plane, fillAlpha, lineAlpha);
		}
	}
	
	private void SetPlaneAlpha(ARPlane plane, float fillAlpha, float lineAlpha)
	{
		var meshRenderer = plane.GetComponentInChildren<MeshRenderer>();
		var lineRenderer = plane.GetComponentInChildren<LineRenderer>();
		
		if (meshRenderer != null)
		{
			Color color = meshRenderer.material.color;
			color.a = fillAlpha;
			meshRenderer.material.color = color;
		}
		
		if (lineRenderer != null)
		{
			//kezdo es befejezo szin
			Color startColor = lineRenderer.startColor;
			Color endColor = lineRenderer.endColor;
			
			//alfa
			startColor.a = lineAlpha;
			endColor.a = lineAlpha;
			
			//uj szin a frissitett alfara
			lineRenderer.startColor = startColor;
			lineRenderer.endColor = endColor;
		}
	}
	
	private void OnPlanesChanged(ARPlanesChangedEventArgs args)
	{
		if(args.added.Count > 0)
		{
			_numPlanesAddedOccurred++;
			
			foreach (var plane in _planeManager.trackables)
			{
				PrintPlaneLabel(plane);
			}
			
		}
	}
	
	private void PrintPlaneLabel(ARPlane plane)
	{
		string label = plane.classification.ToString();
		string log = $"Plane ID: {plane.trackableId}, Label: {label}";
		//Debug.Log(log);
	}
	
	void OnDestroy()
	{
	 _togglePlanesAction.action.performed -= OnTogglePlanesAction;
	 _planeManager.planesChanged -= OnPlanesChanged;
	}
}
