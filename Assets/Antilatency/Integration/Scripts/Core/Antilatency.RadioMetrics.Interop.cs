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
namespace Antilatency.RadioMetrics.Interop {

/// <summary>Extended metrics.</summary>
[System.Serializable]
[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
public partial struct ExtendedMetrics {
	/// <summary>Count of bytes, that was sent to targetNode.</summary>
	public uint txBytes;
	/// <summary>Count of packet, that was sent to targetNode.</summary>
	public uint txPacketsCount;
	/// <summary>Count of bytes, that was received from targetNode.</summary>
	public uint rxBytes;
	/// <summary>Count of packets, that was received from targetNode.</summary>
	public uint rxPacketsCount;
	/// <summary>Count of special empty packets. Should be 0. Indicator of problem with usb.</summary>
	public uint flowCount;
	/// <summary>Average rssi value in dBm.</summary>
	public sbyte averageRssi;
	/// <summary>Min rssi value in dBm.(worse)</summary>
	public sbyte minRssi;
	/// <summary>Max rssi value in dBm.(best)</summary>
	public sbyte maxRssi;
	/// <summary>Count of packets that wasn't receive.</summary>
	public uint missedPacketsCount;
	/// <summary>Count of packets with error.</summary>
	public uint failedPacketsCount;
}


}
