using Antilatency.SDK;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ThreeMarkersEnvironmentComponent : AltEnvironmentComponent {
    public Transform MarkerA, MarkerB, MarkerC;
    private Antilatency.Alt.Environment.IEnvironment _environment = null;
    private ThreeMarkersEnvironment _customEnvironment = null;

    public void Start() {
        var markers = new List<Transform> { MarkerA, MarkerB, MarkerC }
                .Select(t => new Vector2(t.position.x, t.position.z))
                .ToList();

        _customEnvironment = new ThreeMarkersEnvironment(markers);
    }

    public override Antilatency.Alt.Environment.IEnvironment GetEnvironment() {
        if(_environment == null && _customEnvironment != null) {
            _environment = _customEnvironment.QueryInterface<Antilatency.Alt.Environment.IEnvironment>();
        }
        return _environment;
    }

    public void OnDestroy() {
        Antilatency.Utils.SafeDispose(ref _environment);
    }

public void OnDrawGizmos() {
        if (_customEnvironment == null)
            return;

        _customEnvironment.getMatchVisualization().Draw(_environment.getMarkers());
        _customEnvironment.getMatchByPositionVisualization().Draw(_environment.getMarkers());
    }
}
