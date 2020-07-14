using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Antilatency.Integration {
    [CustomEditor(typeof(AltEnvironmentContour))]
    public class AltContourEditor : Editor {

        private AltEnvironmentContour _altContour;
        private readonly Vector3 _snapValue = new Vector3(0.1f, 0.1f, 0.1f);

        private void OnEnable() {
            _altContour = (AltEnvironmentContour)target;
        }

        private void OnSceneGUI() {
            if (_altContour == null) {
                return;
            }

            if (_altContour.Points.Count < 2) {
                return;
            }

            Handles.color = Color.blue;
            var points = new Vector3[_altContour.Points.Count + 1];
            var gameobjectTransform = _altContour.gameObject.transform;
            for (var i = 0; i < _altContour.Points.Count;) {
                var point = new Vector3(
                    _altContour.Points[i].x + gameobjectTransform.position.x,
                    _altContour.transform.position.y,
                    _altContour.Points[i].y + gameobjectTransform.position.z
                    );

                points[i] = Handles.FreeMoveHandle(point, Quaternion.identity, .1f, _snapValue, Handles.RectangleHandleCap);

                if (Handles.Button(point, Quaternion.identity, 0.025f, 0.025f, Handles.CircleHandleCap)) {
                    _altContour.Points.RemoveAt(i);
                } else {
                    _altContour.Points[i] = new Vector2(points[i].x - gameobjectTransform.position.x, points[i].z - gameobjectTransform.position.z);
                    i++;
                }
            }

            points[points.Length - 1] = points[0];

            Handles.color = Color.red;
            Handles.DrawAAPolyLine(points);

            Handles.color = Color.green;
            for (var i = 0; i < points.Length - 1; ++i) {
                var pointA = points[i];
                var pointB = points[(i + 1) % points.Length];

                var addPointBtnPos = pointA + (pointB - pointA) / 2.0f;

                if (Handles.Button(addPointBtnPos, Quaternion.identity, 0.05f, 0.05f, Handles.RectangleHandleCap)) {
                    _altContour.Points.Insert(i + 1, new Vector2(addPointBtnPos.x, addPointBtnPos.z));
                }
            }
        }
    }
}