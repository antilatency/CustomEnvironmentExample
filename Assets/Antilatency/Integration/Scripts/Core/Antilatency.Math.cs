#pragma warning disable IDE1006 // Do not warn about naming style violations
#pragma warning disable IDE0017 // Do not suggest to simplify object initialization
using System.Runtime.InteropServices; //GuidAttribute
namespace Antilatency.Math {

[System.Serializable]
[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
public partial struct float2x3 {
	public UnityEngine.Vector3 x;
	public UnityEngine.Vector3 y;
}


}
