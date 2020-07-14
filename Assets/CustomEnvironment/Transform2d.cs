using UnityEngine;

public class Transform2d {
	Vector2 _translate = Vector2.zero;
    float _s = 0.0f, _c = 1.0f;

	public Transform2d() {
    }

    public Transform2d(float c, float s, float tx, float ty) {
        _translate = new Vector2(tx, ty);
        _s = s;
        _c = c;
    }

	public Transform2d(Vector2 translate, float angle, float scale) {
		_translate = translate;
        _s = scale * Mathf.Sin(angle);
        _c = scale * Mathf.Cos(angle);
    }

    public Vector2 Translate {
        get { return _translate; }
        set { _translate = value; }
    }

    public float Angle {
        get {
            return Mathf.Atan2(_s, _c);
        }

        set {
            var scale = Scale;
            _s = scale*Mathf.Sin(value);
            _c = scale*Mathf.Cos(value);
        }
    }

    public float Scale {
        get { return Mathf.Sqrt(_s*_s + _c*_c); }
        set { float k = value / Scale; _s *= k; _c *= k; }
    }

	public Vector2 apply(Vector2 inp) {
        return new Vector2(_c*inp.x - _s*inp.y, _s*inp.x + _c*inp.y) + _translate;
    }
}
