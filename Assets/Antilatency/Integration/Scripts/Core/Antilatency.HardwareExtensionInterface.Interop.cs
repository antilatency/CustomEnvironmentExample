//Copyright 2020, ALT LLC. All Rights Reserved.
//This file is part of Antilatency SDK.
//It is subject to the license terms in the LICENSE file found in the top-level directory
//of this distribution and at http://www.antilatency.com/eula
//You may not use this file except in compliance with the License.
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
#pragma warning disable IDE1006 // Do not warn about naming style violations
#pragma warning disable IDE0017 // Do not suggest to simplify object initialization
using System.Runtime.InteropServices; //GuidAttribute
namespace Antilatency.HardwareExtensionInterface.Interop {

/// <summary>Logical level on pin.</summary>
[System.Serializable]
[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
public partial struct PinState {
	public static readonly PinState Low = new PinState(){ value = 0x0 };
	public static readonly PinState High = new PinState(){ value = 0x1 };
	[System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
	public int value;
	public override string ToString() {
		switch (value) {
			case 0x0: return "Low";
			case 0x1: return "High";
		}
		return value.ToString();
	}
	public static implicit operator int(PinState value) { return value.value;}
	public static explicit operator PinState(int value) { return new PinState() { value = value }; }
}

/// <summary>Available pins.</summary>
[System.Serializable]
[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
public partial struct Pins {
	public static readonly Pins IO1 = new Pins(){ value = 0x0 };
	public static readonly Pins IO2 = new Pins(){ value = 0x1 };
	public static readonly Pins IOA3 = new Pins(){ value = 0x2 };
	public static readonly Pins IOA4 = new Pins(){ value = 0x3 };
	public static readonly Pins IO5 = new Pins(){ value = 0x4 };
	public static readonly Pins IO6 = new Pins(){ value = 0x5 };
	public static readonly Pins IO7 = new Pins(){ value = 0x6 };
	public static readonly Pins IO8 = new Pins(){ value = 0x7 };
	[System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
	public byte value;
	public override string ToString() {
		switch (value) {
			case 0x0: return "IO1";
			case 0x1: return "IO2";
			case 0x2: return "IOA3";
			case 0x3: return "IOA4";
			case 0x4: return "IO5";
			case 0x5: return "IO6";
			case 0x6: return "IO7";
			case 0x7: return "IO8";
		}
		return value.ToString();
	}
	public static implicit operator byte(Pins value) { return value.value;}
	public static explicit operator Pins(byte value) { return new Pins() { value = value }; }
}

public static partial class Constants {
	public const uint MaxInputPinsCount = 8;
	public const uint MaxOutputPinsCount = 8;
	public const uint MaxAnalogPinsCount = 2;
	public const uint MaxPulseCounterPinsCount = 2;
	public const uint MaxPwmPinsCount = 4;
}


}
