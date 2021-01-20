// Copyright 2020, ALT LLC. All Rights Reserved.
// This file is part of Antilatency SDK.
// It is subject to the license terms in the LICENSE file found in the top-level directory
// of this distribution and at http://www.antilatency.com/eula
// You may not use this file except in compliance with the License.
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using UnityEditor;
[CustomPropertyDrawer(typeof(Antilatency.Alt.Tracking.MarkerIndex))]
class AntilatencyAltTrackingMarkerIndexPropertyDrawer : AntilatencyInterfaceContractEnumPropertyDrawer<Antilatency.Alt.Tracking.MarkerIndex>{}
[CustomPropertyDrawer(typeof(Antilatency.Alt.Tracking.Stage))]
class AntilatencyAltTrackingStagePropertyDrawer : AntilatencyInterfaceContractEnumPropertyDrawer<Antilatency.Alt.Tracking.Stage>{}
[CustomPropertyDrawer(typeof(Antilatency.DeviceNetwork.UsbVendorId))]
class AntilatencyDeviceNetworkUsbVendorIdPropertyDrawer : AntilatencyInterfaceContractEnumPropertyDrawer<Antilatency.DeviceNetwork.UsbVendorId>{}
[CustomPropertyDrawer(typeof(Antilatency.DeviceNetwork.NodeHandle))]
class AntilatencyDeviceNetworkNodeHandlePropertyDrawer : AntilatencyInterfaceContractEnumPropertyDrawer<Antilatency.DeviceNetwork.NodeHandle>{}
[CustomPropertyDrawer(typeof(Antilatency.DeviceNetwork.NodeStatus))]
class AntilatencyDeviceNetworkNodeStatusPropertyDrawer : AntilatencyInterfaceContractEnumPropertyDrawer<Antilatency.DeviceNetwork.NodeStatus>{}
[CustomPropertyDrawer(typeof(Antilatency.DeviceNetwork.LogLevel))]
class AntilatencyDeviceNetworkLogLevelPropertyDrawer : AntilatencyInterfaceContractEnumPropertyDrawer<Antilatency.DeviceNetwork.LogLevel>{}
