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
namespace Antilatency.DeviceNetwork {

[System.Serializable]
[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
public partial struct UsbVendorId {
	public static readonly UsbVendorId Antilatency = new UsbVendorId(){ value = 0x3237 };
	public static readonly UsbVendorId AntilatencyLegacy = new UsbVendorId(){ value = 0x301 };
	[System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
	public ushort value;
	public override string ToString() {
		switch (value) {
			case 0x3237: return "Antilatency";
			case 0x301: return "AntilatencyLegacy";
		}
		return value.ToString();
	}
	public static implicit operator ushort(UsbVendorId value) { return value.value;}
	public static explicit operator UsbVendorId(ushort value) { return new UsbVendorId() { value = value }; }
}

/// <summary>USB device identifier.</summary>
[System.Serializable]
[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
public partial struct UsbDeviceType {
	/// <summary>USB device vendor ID. Default value for Antilatency devices is 0x3237</summary>
	public Antilatency.DeviceNetwork.UsbVendorId vid;
	/// <summary>USB device product ID. Default value for Antilatency Sockets is 0x0000</summary>
	public ushort pid;
}

/// <summary>The handle to the Antilatency Device Network device. Every time a device is connected, a unique handle will be applied to it. Therefore, when the device turns off, the NodeStatus for its node becomes Invalid. After reconnection, the devices receives a new NodeHandle.</summary>
[System.Serializable]
[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
public partial struct NodeHandle {
	/// <summary>Any socket node connected directly by USB to PC, smartphone or HMD, will have Null as its parent.</summary>
	public static readonly NodeHandle Null = new NodeHandle(){ value = 0x0 };
	[System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
	public uint value;
	public override string ToString() {
		switch (value) {
			case 0x0: return "Null";
		}
		return value.ToString();
	}
	public static implicit operator uint(NodeHandle value) { return value.value;}
	public static explicit operator NodeHandle(uint value) { return new NodeHandle() { value = value }; }
}

/// <summary>Defines different node conditions.</summary>
[System.Serializable]
[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
public partial struct NodeStatus {
	/// <summary>The node in connected and no task is currently running on it. Any supported task can be started on it.</summary>
	public static readonly NodeStatus Idle = new NodeStatus(){ value = 0x0 };
	/// <summary>The node in connected and a task is currently running on it. Before running any task on such node, you need to stop the current task first.</summary>
	public static readonly NodeStatus TaskRunning = new NodeStatus(){ value = 0x1 };
	/// <summary>The node in not valid, no tasks can be executed on it.</summary>
	public static readonly NodeStatus Invalid = new NodeStatus(){ value = 0x2 };
	[System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
	public int value;
	public override string ToString() {
		switch (value) {
			case 0x0: return "Idle";
			case 0x1: return "TaskRunning";
			case 0x2: return "Invalid";
		}
		return value.ToString();
	}
	public static implicit operator int(NodeStatus value) { return value.value;}
	public static explicit operator NodeStatus(int value) { return new NodeStatus() { value = value }; }
}

/// <summary>Synchronous task read/write stream.</summary>
[Guid("af7402e8-2a23-442b-8230-d41f55ef5dc0")]
public interface ISynchronousConnection : Antilatency.InterfaceContract.IInterface {
	/// <summary>Get received packets. Blocks until any packets received or task finished.</summary>
	/// <returns>Received packets array. Zero packets count if task is finished.</returns>
	Antilatency.DeviceNetwork.Interop.Packet[] getPackets();
	/// <summary>Get received packets.</summary>
	/// <param name = "taskFinished">
	/// Is task finished.
	/// </param>
	/// <returns>Received packets array. Zero packets count if no packets received.</returns>
	Antilatency.DeviceNetwork.Interop.Packet[] getAvailablePackets(out Antilatency.InterfaceContract.Bool taskFinished);
	Antilatency.InterfaceContract.Bool writePacket(Antilatency.DeviceNetwork.Interop.Packet packet);
}
namespace Details {
	public class ISynchronousConnectionWrapper : Antilatency.InterfaceContract.Details.IInterfaceWrapper, ISynchronousConnection {
		private ISynchronousConnectionRemap.VMT _VMT = new ISynchronousConnectionRemap.VMT();
		protected new int GetTotalNativeMethodsCount() {
		    return base.GetTotalNativeMethodsCount() + typeof(ISynchronousConnectionRemap.VMT).GetFields().Length;
		}
		public ISynchronousConnectionWrapper(System.IntPtr obj) : base(obj) {
		    _VMT = LoadVMT<ISynchronousConnectionRemap.VMT>(base.GetTotalNativeMethodsCount());
		}
		public Antilatency.DeviceNetwork.Interop.Packet[] getPackets() {
			Antilatency.DeviceNetwork.Interop.Packet[] result;
			var resultMarshaler = Antilatency.InterfaceContract.Details.ArrayOutMarshaler.create<Antilatency.DeviceNetwork.Interop.Packet>();
			HandleExceptionCode(_VMT.getPackets(_object, resultMarshaler));
			result = resultMarshaler.value;
			resultMarshaler.Dispose();
			return result;
		}
		public Antilatency.DeviceNetwork.Interop.Packet[] getAvailablePackets(out Antilatency.InterfaceContract.Bool taskFinished) {
			Antilatency.DeviceNetwork.Interop.Packet[] result;
			var resultMarshaler = Antilatency.InterfaceContract.Details.ArrayOutMarshaler.create<Antilatency.DeviceNetwork.Interop.Packet>();
			Antilatency.InterfaceContract.Bool taskFinishedMarshaler;
			HandleExceptionCode(_VMT.getAvailablePackets(_object, out taskFinishedMarshaler, resultMarshaler));
			taskFinished = taskFinishedMarshaler;
			result = resultMarshaler.value;
			resultMarshaler.Dispose();
			return result;
		}
		public Antilatency.InterfaceContract.Bool writePacket(Antilatency.DeviceNetwork.Interop.Packet packet) {
			Antilatency.InterfaceContract.Bool result;
			Antilatency.InterfaceContract.Bool resultMarshaler;
			HandleExceptionCode(_VMT.writePacket(_object, packet, out resultMarshaler));
			result = resultMarshaler;
			return result;
		}
	}
	public class ISynchronousConnectionRemap : Antilatency.InterfaceContract.Details.IInterfaceRemap {
		public new struct VMT {
			public delegate Antilatency.InterfaceContract.ExceptionCode getPacketsDelegate(System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result);
			public delegate Antilatency.InterfaceContract.ExceptionCode getAvailablePacketsDelegate(System.IntPtr _this, out Antilatency.InterfaceContract.Bool taskFinished, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result);
			public delegate Antilatency.InterfaceContract.ExceptionCode writePacketDelegate(System.IntPtr _this, Antilatency.DeviceNetwork.Interop.Packet packet, out Antilatency.InterfaceContract.Bool result);
			#pragma warning disable 0649
			public getPacketsDelegate getPackets;
			public getAvailablePacketsDelegate getAvailablePackets;
			public writePacketDelegate writePacket;
			#pragma warning restore 0649
		}
		public new static readonly NativeInterfaceVmt NativeVmt;
		static ISynchronousConnectionRemap() {
			var vmtBlocks = new System.Collections.Generic.List<object>();
			AppendVmt(vmtBlocks);
			NativeVmt = new NativeInterfaceVmt(vmtBlocks);
		}
		protected static new void AppendVmt(System.Collections.Generic.List<object> buffer) {
			Antilatency.InterfaceContract.Details.IInterfaceRemap.AppendVmt(buffer);
			var vmt = new VMT();
			vmt.getPackets = (System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result) => {
				try {
					var obj = GetContext(_this) as ISynchronousConnection;
					var resultMarshaler = obj.getPackets();
					result.assign(resultMarshaler);
				}
				catch (System.Exception ex) {
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.getAvailablePackets = (System.IntPtr _this, out Antilatency.InterfaceContract.Bool taskFinished, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result) => {
				try {
					var obj = GetContext(_this) as ISynchronousConnection;
					Antilatency.InterfaceContract.Bool taskFinishedMarshaler;
					taskFinished = default(Antilatency.InterfaceContract.Bool);
					var resultMarshaler = obj.getAvailablePackets(out taskFinishedMarshaler);
					taskFinished = taskFinishedMarshaler;
					result.assign(resultMarshaler);
				}
				catch (System.Exception ex) {
					taskFinished = default(Antilatency.InterfaceContract.Bool);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.writePacket = (System.IntPtr _this, Antilatency.DeviceNetwork.Interop.Packet packet, out Antilatency.InterfaceContract.Bool result) => {
				try {
					var obj = GetContext(_this) as ISynchronousConnection;
					var resultMarshaler = obj.writePacket(packet);
					result = resultMarshaler;
				}
				catch (System.Exception ex) {
					result = default(Antilatency.InterfaceContract.Bool);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			buffer.Add(vmt);
		}
		public ISynchronousConnectionRemap() { }
		public ISynchronousConnectionRemap(System.IntPtr context, ushort lifetimeId) {
			AllocateNativeInterface(NativeVmt.Handle, context, lifetimeId);
		}
	}
}

[Guid("fd95f649-562a-4819-a816-26b76cd9d8d6")]
public interface ICotask : Antilatency.InterfaceContract.IInterface {
	Antilatency.InterfaceContract.Bool isTaskFinished();
}
namespace Details {
	public class ICotaskWrapper : Antilatency.InterfaceContract.Details.IInterfaceWrapper, ICotask {
		private ICotaskRemap.VMT _VMT = new ICotaskRemap.VMT();
		protected new int GetTotalNativeMethodsCount() {
		    return base.GetTotalNativeMethodsCount() + typeof(ICotaskRemap.VMT).GetFields().Length;
		}
		public ICotaskWrapper(System.IntPtr obj) : base(obj) {
		    _VMT = LoadVMT<ICotaskRemap.VMT>(base.GetTotalNativeMethodsCount());
		}
		public Antilatency.InterfaceContract.Bool isTaskFinished() {
			Antilatency.InterfaceContract.Bool result;
			Antilatency.InterfaceContract.Bool resultMarshaler;
			HandleExceptionCode(_VMT.isTaskFinished(_object, out resultMarshaler));
			result = resultMarshaler;
			return result;
		}
	}
	public class ICotaskRemap : Antilatency.InterfaceContract.Details.IInterfaceRemap {
		public new struct VMT {
			public delegate Antilatency.InterfaceContract.ExceptionCode isTaskFinishedDelegate(System.IntPtr _this, out Antilatency.InterfaceContract.Bool result);
			#pragma warning disable 0649
			public isTaskFinishedDelegate isTaskFinished;
			#pragma warning restore 0649
		}
		public new static readonly NativeInterfaceVmt NativeVmt;
		static ICotaskRemap() {
			var vmtBlocks = new System.Collections.Generic.List<object>();
			AppendVmt(vmtBlocks);
			NativeVmt = new NativeInterfaceVmt(vmtBlocks);
		}
		protected static new void AppendVmt(System.Collections.Generic.List<object> buffer) {
			Antilatency.InterfaceContract.Details.IInterfaceRemap.AppendVmt(buffer);
			var vmt = new VMT();
			vmt.isTaskFinished = (System.IntPtr _this, out Antilatency.InterfaceContract.Bool result) => {
				try {
					var obj = GetContext(_this) as ICotask;
					var resultMarshaler = obj.isTaskFinished();
					result = resultMarshaler;
				}
				catch (System.Exception ex) {
					result = default(Antilatency.InterfaceContract.Bool);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			buffer.Add(vmt);
		}
		public ICotaskRemap() { }
		public ICotaskRemap(System.IntPtr context, ushort lifetimeId) {
			AllocateNativeInterface(NativeVmt.Handle, context, lifetimeId);
		}
	}
}

[Guid("81ea9312-f66e-4708-acd1-d40a3e6ef9aa")]
public interface IPropertyCotask : Antilatency.DeviceNetwork.ICotask {
	string getPropertyKey(uint index);
	string getStringProperty(string key);
	void setStringProperty(string key, string value);
	byte[] getBinaryProperty(string key);
	void setBinaryProperty(string key, byte[] value);
	void deleteProperty(string key);
}
namespace Details {
	public class IPropertyCotaskWrapper : Antilatency.DeviceNetwork.Details.ICotaskWrapper, IPropertyCotask {
		private IPropertyCotaskRemap.VMT _VMT = new IPropertyCotaskRemap.VMT();
		protected new int GetTotalNativeMethodsCount() {
		    return base.GetTotalNativeMethodsCount() + typeof(IPropertyCotaskRemap.VMT).GetFields().Length;
		}
		public IPropertyCotaskWrapper(System.IntPtr obj) : base(obj) {
		    _VMT = LoadVMT<IPropertyCotaskRemap.VMT>(base.GetTotalNativeMethodsCount());
		}
		public string getPropertyKey(uint index) {
			string result;
			var resultMarshaler = Antilatency.InterfaceContract.Details.ArrayOutMarshaler.create();
			HandleExceptionCode(_VMT.getPropertyKey(_object, index, resultMarshaler));
			result = resultMarshaler.value;
			resultMarshaler.Dispose();
			return result;
		}
		public string getStringProperty(string key) {
			string result;
			var resultMarshaler = Antilatency.InterfaceContract.Details.ArrayOutMarshaler.create();
			var keyMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(key);
			HandleExceptionCode(_VMT.getStringProperty(_object, keyMarshaler, resultMarshaler));
			keyMarshaler.Dispose();
			result = resultMarshaler.value;
			resultMarshaler.Dispose();
			return result;
		}
		public void setStringProperty(string key, string value) {
			var keyMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(key);
			var valueMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(value);
			HandleExceptionCode(_VMT.setStringProperty(_object, keyMarshaler, valueMarshaler));
			keyMarshaler.Dispose();
			valueMarshaler.Dispose();
		}
		public byte[] getBinaryProperty(string key) {
			byte[] result;
			var resultMarshaler = Antilatency.InterfaceContract.Details.ArrayOutMarshaler.create<byte>();
			var keyMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(key);
			HandleExceptionCode(_VMT.getBinaryProperty(_object, keyMarshaler, resultMarshaler));
			keyMarshaler.Dispose();
			result = resultMarshaler.value;
			resultMarshaler.Dispose();
			return result;
		}
		public void setBinaryProperty(string key, byte[] value) {
			var keyMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(key);
			var valueMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(value);
			HandleExceptionCode(_VMT.setBinaryProperty(_object, keyMarshaler, valueMarshaler));
			keyMarshaler.Dispose();
			valueMarshaler.Dispose();
		}
		public void deleteProperty(string key) {
			var keyMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(key);
			HandleExceptionCode(_VMT.deleteProperty(_object, keyMarshaler));
			keyMarshaler.Dispose();
		}
	}
	public class IPropertyCotaskRemap : Antilatency.DeviceNetwork.Details.ICotaskRemap {
		public new struct VMT {
			public delegate Antilatency.InterfaceContract.ExceptionCode getPropertyKeyDelegate(System.IntPtr _this, uint index, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result);
			public delegate Antilatency.InterfaceContract.ExceptionCode getStringPropertyDelegate(System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate key, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result);
			public delegate Antilatency.InterfaceContract.ExceptionCode setStringPropertyDelegate(System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate key, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate value);
			public delegate Antilatency.InterfaceContract.ExceptionCode getBinaryPropertyDelegate(System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate key, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result);
			public delegate Antilatency.InterfaceContract.ExceptionCode setBinaryPropertyDelegate(System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate key, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate value);
			public delegate Antilatency.InterfaceContract.ExceptionCode deletePropertyDelegate(System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate key);
			#pragma warning disable 0649
			public getPropertyKeyDelegate getPropertyKey;
			public getStringPropertyDelegate getStringProperty;
			public setStringPropertyDelegate setStringProperty;
			public getBinaryPropertyDelegate getBinaryProperty;
			public setBinaryPropertyDelegate setBinaryProperty;
			public deletePropertyDelegate deleteProperty;
			#pragma warning restore 0649
		}
		public new static readonly NativeInterfaceVmt NativeVmt;
		static IPropertyCotaskRemap() {
			var vmtBlocks = new System.Collections.Generic.List<object>();
			AppendVmt(vmtBlocks);
			NativeVmt = new NativeInterfaceVmt(vmtBlocks);
		}
		protected static new void AppendVmt(System.Collections.Generic.List<object> buffer) {
			Antilatency.DeviceNetwork.Details.ICotaskRemap.AppendVmt(buffer);
			var vmt = new VMT();
			vmt.getPropertyKey = (System.IntPtr _this, uint index, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result) => {
				try {
					var obj = GetContext(_this) as IPropertyCotask;
					var resultMarshaler = obj.getPropertyKey(index);
					result.assign(resultMarshaler);
				}
				catch (System.Exception ex) {
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.getStringProperty = (System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate key, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result) => {
				try {
					var obj = GetContext(_this) as IPropertyCotask;
					var resultMarshaler = obj.getStringProperty(key);
					result.assign(resultMarshaler);
				}
				catch (System.Exception ex) {
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.setStringProperty = (System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate key, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate value) => {
				try {
					var obj = GetContext(_this) as IPropertyCotask;
					obj.setStringProperty(key, value);
				}
				catch (System.Exception ex) {
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.getBinaryProperty = (System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate key, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result) => {
				try {
					var obj = GetContext(_this) as IPropertyCotask;
					var resultMarshaler = obj.getBinaryProperty(key);
					result.assign(resultMarshaler);
				}
				catch (System.Exception ex) {
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.setBinaryProperty = (System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate key, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate value) => {
				try {
					var obj = GetContext(_this) as IPropertyCotask;
					obj.setBinaryProperty(key, value.toArray<byte>());
				}
				catch (System.Exception ex) {
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.deleteProperty = (System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate key) => {
				try {
					var obj = GetContext(_this) as IPropertyCotask;
					obj.deleteProperty(key);
				}
				catch (System.Exception ex) {
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			buffer.Add(vmt);
		}
		public IPropertyCotaskRemap() { }
		public IPropertyCotaskRemap(System.IntPtr context, ushort lifetimeId) {
			AllocateNativeInterface(NativeVmt.Handle, context, lifetimeId);
		}
	}
}

/// <summary>Network of Nodes</summary>
[Guid("4cb2369c-7a66-4e85-9a0c-dbc89fc1c75e")]
public interface INetwork : Antilatency.InterfaceContract.IInterface {
	/// <summary>Every time any supported device is connected or disconnected, the update ID will be incremented. You can use this method to check if there are any changes in the ADN.</summary>
	/// <returns>Current factory update ID.</returns>
	uint getUpdateId();
	/// <summary>Get the USB device types selected to work with this factory instance.</summary>
	/// <returns>The array of the USB device identifiers which this factory instance is working with.</returns>
	Antilatency.DeviceNetwork.UsbDeviceType[] getDeviceTypes();
	/// <summary>Get all currently connected nodes.</summary>
	Antilatency.DeviceNetwork.NodeHandle[] getNodes();
	/// <summary>Get the current NodeStatus for the node.</summary>
	/// <param name = "node">
	/// Node handle to get status of.
	/// </param>
	Antilatency.DeviceNetwork.NodeStatus nodeGetStatus(Antilatency.DeviceNetwork.NodeHandle node);
	/// <summary>Checks if the task is supported by the node.</summary>
	/// <param name = "node">
	/// Node handle.
	/// </param>
	/// <param name = "taskId">
	/// Task ID.
	/// </param>
	/// <returns>True if node supports such task, otherwise false.</returns>
	Antilatency.InterfaceContract.Bool nodeIsTaskSupported(Antilatency.DeviceNetwork.NodeHandle node, System.Guid taskId);
	/// <summary>Get parent for the node, in case of USB node this method will return Antilatency.DeviceNetwork.NodeHandle.Null</summary>
	/// <param name = "node">
	/// Node handle.
	/// </param>
	Antilatency.DeviceNetwork.NodeHandle nodeGetParent(Antilatency.DeviceNetwork.NodeHandle node);
	/// <summary>Physical address path to the first level device that contains this node in the network tree.</summary>
	/// <param name = "node">
	/// Node handle.
	/// </param>
	/// <returns>String represents physical path to first level device (connected via USB).</returns>
	string nodeGetPhysicalPath(Antilatency.DeviceNetwork.NodeHandle node);
	/// <summary>Run the task on the node with the asynchronous packet receive API.</summary>
	/// <param name = "node">
	/// Node handle to start task on.
	/// </param>
	/// <param name = "taskId">
	/// Task ID.
	/// </param>
	/// <param name = "taskDataReceiver">
	/// Task data receiver.
	/// </param>
	Antilatency.DeviceNetwork.Interop.IDataReceiver nodeStartTask(Antilatency.DeviceNetwork.NodeHandle node, System.Guid taskId, Antilatency.DeviceNetwork.Interop.IDataReceiver taskDataReceiver);
	/// <summary>Run the task on the node with the synchronous packet receive API.</summary>
	/// <param name = "node">
	/// Node handle to start task on.
	/// </param>
	/// <param name = "taskId">
	/// Task ID.
	/// </param>
	Antilatency.DeviceNetwork.ISynchronousConnection nodeStartTask2(Antilatency.DeviceNetwork.NodeHandle node, System.Guid taskId);
	/// <summary>Get the node's string property value.</summary>
	/// <param name = "node">
	/// Node handle to get string property from.
	/// </param>
	/// <param name = "key">
	/// Property key. List of predefined properties you can find in the documentation, also there are some properties defined in the Antilatency.DeviceNetwork.Constants that is valid for every Antilatency device.
	/// </param>
	/// <returns>The node's string property value.</returns>
	string nodeGetStringProperty(Antilatency.DeviceNetwork.NodeHandle node, string key);
	/// <summary>Get the node's binary property value.</summary>
	/// <param name = "node">
	/// Node handle to get binary property from.
	/// </param>
	/// <param name = "key">
	/// Property key.
	/// </param>
	/// <returns>The node's binary property value.</returns>
	byte[] nodeGetBinaryProperty(Antilatency.DeviceNetwork.NodeHandle node, string key);
	/// <summary>Start the property task on the node.</summary>
	/// <param name = "node">
	/// Node handle to start property task on.
	/// </param>
	Antilatency.DeviceNetwork.IPropertyCotask nodeStartPropertyTask(Antilatency.DeviceNetwork.NodeHandle node);
}
namespace Details {
	public class INetworkWrapper : Antilatency.InterfaceContract.Details.IInterfaceWrapper, INetwork {
		private INetworkRemap.VMT _VMT = new INetworkRemap.VMT();
		protected new int GetTotalNativeMethodsCount() {
		    return base.GetTotalNativeMethodsCount() + typeof(INetworkRemap.VMT).GetFields().Length;
		}
		public INetworkWrapper(System.IntPtr obj) : base(obj) {
		    _VMT = LoadVMT<INetworkRemap.VMT>(base.GetTotalNativeMethodsCount());
		}
		public uint getUpdateId() {
			uint result;
			uint resultMarshaler;
			HandleExceptionCode(_VMT.getUpdateId(_object, out resultMarshaler));
			result = resultMarshaler;
			return result;
		}
		public Antilatency.DeviceNetwork.UsbDeviceType[] getDeviceTypes() {
			Antilatency.DeviceNetwork.UsbDeviceType[] result;
			var resultMarshaler = Antilatency.InterfaceContract.Details.ArrayOutMarshaler.create<Antilatency.DeviceNetwork.UsbDeviceType>();
			HandleExceptionCode(_VMT.getDeviceTypes(_object, resultMarshaler));
			result = resultMarshaler.value;
			resultMarshaler.Dispose();
			return result;
		}
		public Antilatency.DeviceNetwork.NodeHandle[] getNodes() {
			Antilatency.DeviceNetwork.NodeHandle[] result;
			var resultMarshaler = Antilatency.InterfaceContract.Details.ArrayOutMarshaler.create<Antilatency.DeviceNetwork.NodeHandle>();
			HandleExceptionCode(_VMT.getNodes(_object, resultMarshaler));
			result = resultMarshaler.value;
			resultMarshaler.Dispose();
			return result;
		}
		public Antilatency.DeviceNetwork.NodeStatus nodeGetStatus(Antilatency.DeviceNetwork.NodeHandle node) {
			Antilatency.DeviceNetwork.NodeStatus result;
			Antilatency.DeviceNetwork.NodeStatus resultMarshaler;
			HandleExceptionCode(_VMT.nodeGetStatus(_object, node, out resultMarshaler));
			result = resultMarshaler;
			return result;
		}
		public Antilatency.InterfaceContract.Bool nodeIsTaskSupported(Antilatency.DeviceNetwork.NodeHandle node, System.Guid taskId) {
			Antilatency.InterfaceContract.Bool result;
			Antilatency.InterfaceContract.Bool resultMarshaler;
			HandleExceptionCode(_VMT.nodeIsTaskSupported(_object, node, taskId, out resultMarshaler));
			result = resultMarshaler;
			return result;
		}
		public Antilatency.DeviceNetwork.NodeHandle nodeGetParent(Antilatency.DeviceNetwork.NodeHandle node) {
			Antilatency.DeviceNetwork.NodeHandle result;
			Antilatency.DeviceNetwork.NodeHandle resultMarshaler;
			HandleExceptionCode(_VMT.nodeGetParent(_object, node, out resultMarshaler));
			result = resultMarshaler;
			return result;
		}
		public string nodeGetPhysicalPath(Antilatency.DeviceNetwork.NodeHandle node) {
			string result;
			var resultMarshaler = Antilatency.InterfaceContract.Details.ArrayOutMarshaler.create();
			HandleExceptionCode(_VMT.nodeGetPhysicalPath(_object, node, resultMarshaler));
			result = resultMarshaler.value;
			resultMarshaler.Dispose();
			return result;
		}
		public Antilatency.DeviceNetwork.Interop.IDataReceiver nodeStartTask(Antilatency.DeviceNetwork.NodeHandle node, System.Guid taskId, Antilatency.DeviceNetwork.Interop.IDataReceiver taskDataReceiver) {
			Antilatency.DeviceNetwork.Interop.IDataReceiver result;
			System.IntPtr resultMarshaler;
			var taskDataReceiverMarshaler = Antilatency.InterfaceContract.Details.InterfaceMarshaler.ManagedToNative<Antilatency.DeviceNetwork.Interop.IDataReceiver>(taskDataReceiver);
			HandleExceptionCode(_VMT.nodeStartTask(_object, node, taskId, taskDataReceiverMarshaler, out resultMarshaler));
			result = (resultMarshaler==System.IntPtr.Zero) ? null : new Antilatency.DeviceNetwork.Interop.Details.IDataReceiverWrapper(resultMarshaler);
			return result;
		}
		public Antilatency.DeviceNetwork.ISynchronousConnection nodeStartTask2(Antilatency.DeviceNetwork.NodeHandle node, System.Guid taskId) {
			Antilatency.DeviceNetwork.ISynchronousConnection result;
			System.IntPtr resultMarshaler;
			HandleExceptionCode(_VMT.nodeStartTask2(_object, node, taskId, out resultMarshaler));
			result = (resultMarshaler==System.IntPtr.Zero) ? null : new Antilatency.DeviceNetwork.Details.ISynchronousConnectionWrapper(resultMarshaler);
			return result;
		}
		public string nodeGetStringProperty(Antilatency.DeviceNetwork.NodeHandle node, string key) {
			string result;
			var resultMarshaler = Antilatency.InterfaceContract.Details.ArrayOutMarshaler.create();
			var keyMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(key);
			HandleExceptionCode(_VMT.nodeGetStringProperty(_object, node, keyMarshaler, resultMarshaler));
			keyMarshaler.Dispose();
			result = resultMarshaler.value;
			resultMarshaler.Dispose();
			return result;
		}
		public byte[] nodeGetBinaryProperty(Antilatency.DeviceNetwork.NodeHandle node, string key) {
			byte[] result;
			var resultMarshaler = Antilatency.InterfaceContract.Details.ArrayOutMarshaler.create<byte>();
			var keyMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(key);
			HandleExceptionCode(_VMT.nodeGetBinaryProperty(_object, node, keyMarshaler, resultMarshaler));
			keyMarshaler.Dispose();
			result = resultMarshaler.value;
			resultMarshaler.Dispose();
			return result;
		}
		public Antilatency.DeviceNetwork.IPropertyCotask nodeStartPropertyTask(Antilatency.DeviceNetwork.NodeHandle node) {
			Antilatency.DeviceNetwork.IPropertyCotask result;
			System.IntPtr resultMarshaler;
			HandleExceptionCode(_VMT.nodeStartPropertyTask(_object, node, out resultMarshaler));
			result = (resultMarshaler==System.IntPtr.Zero) ? null : new Antilatency.DeviceNetwork.Details.IPropertyCotaskWrapper(resultMarshaler);
			return result;
		}
	}
	public class INetworkRemap : Antilatency.InterfaceContract.Details.IInterfaceRemap {
		public new struct VMT {
			public delegate Antilatency.InterfaceContract.ExceptionCode getUpdateIdDelegate(System.IntPtr _this, out uint result);
			public delegate Antilatency.InterfaceContract.ExceptionCode getDeviceTypesDelegate(System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result);
			public delegate Antilatency.InterfaceContract.ExceptionCode getNodesDelegate(System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result);
			public delegate Antilatency.InterfaceContract.ExceptionCode nodeGetStatusDelegate(System.IntPtr _this, Antilatency.DeviceNetwork.NodeHandle node, out Antilatency.DeviceNetwork.NodeStatus result);
			public delegate Antilatency.InterfaceContract.ExceptionCode nodeIsTaskSupportedDelegate(System.IntPtr _this, Antilatency.DeviceNetwork.NodeHandle node, System.Guid taskId, out Antilatency.InterfaceContract.Bool result);
			public delegate Antilatency.InterfaceContract.ExceptionCode nodeGetParentDelegate(System.IntPtr _this, Antilatency.DeviceNetwork.NodeHandle node, out Antilatency.DeviceNetwork.NodeHandle result);
			public delegate Antilatency.InterfaceContract.ExceptionCode nodeGetPhysicalPathDelegate(System.IntPtr _this, Antilatency.DeviceNetwork.NodeHandle node, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result);
			public delegate Antilatency.InterfaceContract.ExceptionCode nodeStartTaskDelegate(System.IntPtr _this, Antilatency.DeviceNetwork.NodeHandle node, System.Guid taskId, System.IntPtr taskDataReceiver, out System.IntPtr result);
			public delegate Antilatency.InterfaceContract.ExceptionCode nodeStartTask2Delegate(System.IntPtr _this, Antilatency.DeviceNetwork.NodeHandle node, System.Guid taskId, out System.IntPtr result);
			public delegate Antilatency.InterfaceContract.ExceptionCode nodeGetStringPropertyDelegate(System.IntPtr _this, Antilatency.DeviceNetwork.NodeHandle node, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate key, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result);
			public delegate Antilatency.InterfaceContract.ExceptionCode nodeGetBinaryPropertyDelegate(System.IntPtr _this, Antilatency.DeviceNetwork.NodeHandle node, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate key, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result);
			public delegate Antilatency.InterfaceContract.ExceptionCode nodeStartPropertyTaskDelegate(System.IntPtr _this, Antilatency.DeviceNetwork.NodeHandle node, out System.IntPtr result);
			#pragma warning disable 0649
			public getUpdateIdDelegate getUpdateId;
			public getDeviceTypesDelegate getDeviceTypes;
			public getNodesDelegate getNodes;
			public nodeGetStatusDelegate nodeGetStatus;
			public nodeIsTaskSupportedDelegate nodeIsTaskSupported;
			public nodeGetParentDelegate nodeGetParent;
			public nodeGetPhysicalPathDelegate nodeGetPhysicalPath;
			public nodeStartTaskDelegate nodeStartTask;
			public nodeStartTask2Delegate nodeStartTask2;
			public nodeGetStringPropertyDelegate nodeGetStringProperty;
			public nodeGetBinaryPropertyDelegate nodeGetBinaryProperty;
			public nodeStartPropertyTaskDelegate nodeStartPropertyTask;
			#pragma warning restore 0649
		}
		public new static readonly NativeInterfaceVmt NativeVmt;
		static INetworkRemap() {
			var vmtBlocks = new System.Collections.Generic.List<object>();
			AppendVmt(vmtBlocks);
			NativeVmt = new NativeInterfaceVmt(vmtBlocks);
		}
		protected static new void AppendVmt(System.Collections.Generic.List<object> buffer) {
			Antilatency.InterfaceContract.Details.IInterfaceRemap.AppendVmt(buffer);
			var vmt = new VMT();
			vmt.getUpdateId = (System.IntPtr _this, out uint result) => {
				try {
					var obj = GetContext(_this) as INetwork;
					var resultMarshaler = obj.getUpdateId();
					result = resultMarshaler;
				}
				catch (System.Exception ex) {
					result = default(uint);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.getDeviceTypes = (System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result) => {
				try {
					var obj = GetContext(_this) as INetwork;
					var resultMarshaler = obj.getDeviceTypes();
					result.assign(resultMarshaler);
				}
				catch (System.Exception ex) {
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.getNodes = (System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result) => {
				try {
					var obj = GetContext(_this) as INetwork;
					var resultMarshaler = obj.getNodes();
					result.assign(resultMarshaler);
				}
				catch (System.Exception ex) {
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.nodeGetStatus = (System.IntPtr _this, Antilatency.DeviceNetwork.NodeHandle node, out Antilatency.DeviceNetwork.NodeStatus result) => {
				try {
					var obj = GetContext(_this) as INetwork;
					var resultMarshaler = obj.nodeGetStatus(node);
					result = resultMarshaler;
				}
				catch (System.Exception ex) {
					result = default(Antilatency.DeviceNetwork.NodeStatus);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.nodeIsTaskSupported = (System.IntPtr _this, Antilatency.DeviceNetwork.NodeHandle node, System.Guid taskId, out Antilatency.InterfaceContract.Bool result) => {
				try {
					var obj = GetContext(_this) as INetwork;
					var resultMarshaler = obj.nodeIsTaskSupported(node, taskId);
					result = resultMarshaler;
				}
				catch (System.Exception ex) {
					result = default(Antilatency.InterfaceContract.Bool);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.nodeGetParent = (System.IntPtr _this, Antilatency.DeviceNetwork.NodeHandle node, out Antilatency.DeviceNetwork.NodeHandle result) => {
				try {
					var obj = GetContext(_this) as INetwork;
					var resultMarshaler = obj.nodeGetParent(node);
					result = resultMarshaler;
				}
				catch (System.Exception ex) {
					result = default(Antilatency.DeviceNetwork.NodeHandle);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.nodeGetPhysicalPath = (System.IntPtr _this, Antilatency.DeviceNetwork.NodeHandle node, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result) => {
				try {
					var obj = GetContext(_this) as INetwork;
					var resultMarshaler = obj.nodeGetPhysicalPath(node);
					result.assign(resultMarshaler);
				}
				catch (System.Exception ex) {
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.nodeStartTask = (System.IntPtr _this, Antilatency.DeviceNetwork.NodeHandle node, System.Guid taskId, System.IntPtr taskDataReceiver, out System.IntPtr result) => {
				try {
					var obj = GetContext(_this) as INetwork;
					var taskDataReceiverMarshaler = taskDataReceiver == System.IntPtr.Zero ? null : new Antilatency.DeviceNetwork.Interop.Details.IDataReceiverWrapper(taskDataReceiver);
					var resultMarshaler = obj.nodeStartTask(node, taskId, taskDataReceiverMarshaler);
					result = Antilatency.InterfaceContract.Details.InterfaceMarshaler.ManagedToNative<Antilatency.DeviceNetwork.Interop.IDataReceiver>(resultMarshaler);
				}
				catch (System.Exception ex) {
					result = default(System.IntPtr);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.nodeStartTask2 = (System.IntPtr _this, Antilatency.DeviceNetwork.NodeHandle node, System.Guid taskId, out System.IntPtr result) => {
				try {
					var obj = GetContext(_this) as INetwork;
					var resultMarshaler = obj.nodeStartTask2(node, taskId);
					result = Antilatency.InterfaceContract.Details.InterfaceMarshaler.ManagedToNative<Antilatency.DeviceNetwork.ISynchronousConnection>(resultMarshaler);
				}
				catch (System.Exception ex) {
					result = default(System.IntPtr);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.nodeGetStringProperty = (System.IntPtr _this, Antilatency.DeviceNetwork.NodeHandle node, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate key, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result) => {
				try {
					var obj = GetContext(_this) as INetwork;
					var resultMarshaler = obj.nodeGetStringProperty(node, key);
					result.assign(resultMarshaler);
				}
				catch (System.Exception ex) {
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.nodeGetBinaryProperty = (System.IntPtr _this, Antilatency.DeviceNetwork.NodeHandle node, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate key, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result) => {
				try {
					var obj = GetContext(_this) as INetwork;
					var resultMarshaler = obj.nodeGetBinaryProperty(node, key);
					result.assign(resultMarshaler);
				}
				catch (System.Exception ex) {
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.nodeStartPropertyTask = (System.IntPtr _this, Antilatency.DeviceNetwork.NodeHandle node, out System.IntPtr result) => {
				try {
					var obj = GetContext(_this) as INetwork;
					var resultMarshaler = obj.nodeStartPropertyTask(node);
					result = Antilatency.InterfaceContract.Details.InterfaceMarshaler.ManagedToNative<Antilatency.DeviceNetwork.IPropertyCotask>(resultMarshaler);
				}
				catch (System.Exception ex) {
					result = default(System.IntPtr);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			buffer.Add(vmt);
		}
		public INetworkRemap() { }
		public INetworkRemap(System.IntPtr context, ushort lifetimeId) {
			AllocateNativeInterface(NativeVmt.Handle, context, lifetimeId);
		}
	}
}

/// <summary>Antilatency Device Network log verbosity level.</summary>
[System.Serializable]
[System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
public partial struct LogLevel {
	public static readonly LogLevel Trace = new LogLevel(){ value = 0x0 };
	public static readonly LogLevel Debug = new LogLevel(){ value = 0x1 };
	public static readonly LogLevel Info = new LogLevel(){ value = 0x2 };
	public static readonly LogLevel Warning = new LogLevel(){ value = 0x3 };
	public static readonly LogLevel Error = new LogLevel(){ value = 0x4 };
	public static readonly LogLevel Critical = new LogLevel(){ value = 0x5 };
	public static readonly LogLevel Off = new LogLevel(){ value = 0x6 };
	[System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
	public int value;
	public override string ToString() {
		switch (value) {
			case 0x0: return "Trace";
			case 0x1: return "Debug";
			case 0x2: return "Info";
			case 0x3: return "Warning";
			case 0x4: return "Error";
			case 0x5: return "Critical";
			case 0x6: return "Off";
		}
		return value.ToString();
	}
	public static implicit operator int(LogLevel value) { return value.value;}
	public static explicit operator LogLevel(int value) { return new LogLevel() { value = value }; }
}

/// <summary>Antilatency Device Network library entry point.</summary>
[Guid("36219fe8-3ad9-4b70-8c47-a032fe0b5c10")]
public interface ILibrary : Antilatency.InterfaceContract.IInterface {
	/// <summary>Create Antilatency Device Network object</summary>
	/// <param name = "deviceTypes">
	/// The array of USB device identifiers which will be used by network.
	/// </param>
	Antilatency.DeviceNetwork.INetwork createNetwork(Antilatency.DeviceNetwork.UsbDeviceType[] deviceTypes);
	/// <summary>Get the Antilatency Device Network library version.</summary>
	string getVersion();
	/// <summary>Set the Antilatency Device Network log verbosity level.</summary>
	void setLogLevel(Antilatency.DeviceNetwork.LogLevel level);
}
public static class Library{
    [DllImport("AntilatencyDeviceNetwork")]
    private static extern Antilatency.InterfaceContract.ExceptionCode getLibraryInterface(System.IntPtr unloader, out System.IntPtr result);
    public static ILibrary load(){
        System.IntPtr libraryAsIInterfaceIntermediate;
        getLibraryInterface(System.IntPtr.Zero, out libraryAsIInterfaceIntermediate);
        Antilatency.InterfaceContract.IInterface libraryAsIInterface = new Antilatency.InterfaceContract.Details.IInterfaceWrapper(libraryAsIInterfaceIntermediate);
        var library = libraryAsIInterface.QueryInterface<ILibrary>();
        libraryAsIInterface.Dispose();
        return library;
    }
}
namespace Details {
	public class ILibraryWrapper : Antilatency.InterfaceContract.Details.IInterfaceWrapper, ILibrary {
		private ILibraryRemap.VMT _VMT = new ILibraryRemap.VMT();
		protected new int GetTotalNativeMethodsCount() {
		    return base.GetTotalNativeMethodsCount() + typeof(ILibraryRemap.VMT).GetFields().Length;
		}
		public ILibraryWrapper(System.IntPtr obj) : base(obj) {
		    _VMT = LoadVMT<ILibraryRemap.VMT>(base.GetTotalNativeMethodsCount());
		}
		public Antilatency.DeviceNetwork.INetwork createNetwork(Antilatency.DeviceNetwork.UsbDeviceType[] deviceTypes) {
			Antilatency.DeviceNetwork.INetwork result;
			System.IntPtr resultMarshaler;
			var deviceTypesMarshaler = Antilatency.InterfaceContract.Details.ArrayInMarshaler.create(deviceTypes);
			HandleExceptionCode(_VMT.createNetwork(_object, deviceTypesMarshaler, out resultMarshaler));
			deviceTypesMarshaler.Dispose();
			result = (resultMarshaler==System.IntPtr.Zero) ? null : new Antilatency.DeviceNetwork.Details.INetworkWrapper(resultMarshaler);
			return result;
		}
		public string getVersion() {
			string result;
			var resultMarshaler = Antilatency.InterfaceContract.Details.ArrayOutMarshaler.create();
			HandleExceptionCode(_VMT.getVersion(_object, resultMarshaler));
			result = resultMarshaler.value;
			resultMarshaler.Dispose();
			return result;
		}
		public void setLogLevel(Antilatency.DeviceNetwork.LogLevel level) {
			HandleExceptionCode(_VMT.setLogLevel(_object, level));
		}
	}
	public class ILibraryRemap : Antilatency.InterfaceContract.Details.IInterfaceRemap {
		public new struct VMT {
			public delegate Antilatency.InterfaceContract.ExceptionCode createNetworkDelegate(System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate deviceTypes, out System.IntPtr result);
			public delegate Antilatency.InterfaceContract.ExceptionCode getVersionDelegate(System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result);
			public delegate Antilatency.InterfaceContract.ExceptionCode setLogLevelDelegate(System.IntPtr _this, Antilatency.DeviceNetwork.LogLevel level);
			#pragma warning disable 0649
			public createNetworkDelegate createNetwork;
			public getVersionDelegate getVersion;
			public setLogLevelDelegate setLogLevel;
			#pragma warning restore 0649
		}
		public new static readonly NativeInterfaceVmt NativeVmt;
		static ILibraryRemap() {
			var vmtBlocks = new System.Collections.Generic.List<object>();
			AppendVmt(vmtBlocks);
			NativeVmt = new NativeInterfaceVmt(vmtBlocks);
		}
		protected static new void AppendVmt(System.Collections.Generic.List<object> buffer) {
			Antilatency.InterfaceContract.Details.IInterfaceRemap.AppendVmt(buffer);
			var vmt = new VMT();
			vmt.createNetwork = (System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayInMarshaler.Intermediate deviceTypes, out System.IntPtr result) => {
				try {
					var obj = GetContext(_this) as ILibrary;
					var resultMarshaler = obj.createNetwork(deviceTypes.toArray<Antilatency.DeviceNetwork.UsbDeviceType>());
					result = Antilatency.InterfaceContract.Details.InterfaceMarshaler.ManagedToNative<Antilatency.DeviceNetwork.INetwork>(resultMarshaler);
				}
				catch (System.Exception ex) {
					result = default(System.IntPtr);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.getVersion = (System.IntPtr _this, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result) => {
				try {
					var obj = GetContext(_this) as ILibrary;
					var resultMarshaler = obj.getVersion();
					result.assign(resultMarshaler);
				}
				catch (System.Exception ex) {
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.setLogLevel = (System.IntPtr _this, Antilatency.DeviceNetwork.LogLevel level) => {
				try {
					var obj = GetContext(_this) as ILibrary;
					obj.setLogLevel(level);
				}
				catch (System.Exception ex) {
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			buffer.Add(vmt);
		}
		public ILibraryRemap() { }
		public ILibraryRemap(System.IntPtr context, ushort lifetimeId) {
			AllocateNativeInterface(NativeVmt.Handle, context, lifetimeId);
		}
	}
}

[Guid("9ecfa909-d13c-4c29-a87e-8925b7846638")]
public interface ICotaskConstructor : Antilatency.InterfaceContract.IInterface {
	Antilatency.InterfaceContract.Bool isSupported(Antilatency.DeviceNetwork.INetwork network, Antilatency.DeviceNetwork.NodeHandle node);
	Antilatency.DeviceNetwork.NodeHandle[] findSupportedNodes(Antilatency.DeviceNetwork.INetwork network);
}
namespace Details {
	public class ICotaskConstructorWrapper : Antilatency.InterfaceContract.Details.IInterfaceWrapper, ICotaskConstructor {
		private ICotaskConstructorRemap.VMT _VMT = new ICotaskConstructorRemap.VMT();
		protected new int GetTotalNativeMethodsCount() {
		    return base.GetTotalNativeMethodsCount() + typeof(ICotaskConstructorRemap.VMT).GetFields().Length;
		}
		public ICotaskConstructorWrapper(System.IntPtr obj) : base(obj) {
		    _VMT = LoadVMT<ICotaskConstructorRemap.VMT>(base.GetTotalNativeMethodsCount());
		}
		public Antilatency.InterfaceContract.Bool isSupported(Antilatency.DeviceNetwork.INetwork network, Antilatency.DeviceNetwork.NodeHandle node) {
			Antilatency.InterfaceContract.Bool result;
			Antilatency.InterfaceContract.Bool resultMarshaler;
			var networkMarshaler = Antilatency.InterfaceContract.Details.InterfaceMarshaler.ManagedToNative<Antilatency.DeviceNetwork.INetwork>(network);
			HandleExceptionCode(_VMT.isSupported(_object, networkMarshaler, node, out resultMarshaler));
			result = resultMarshaler;
			return result;
		}
		public Antilatency.DeviceNetwork.NodeHandle[] findSupportedNodes(Antilatency.DeviceNetwork.INetwork network) {
			Antilatency.DeviceNetwork.NodeHandle[] result;
			var resultMarshaler = Antilatency.InterfaceContract.Details.ArrayOutMarshaler.create<Antilatency.DeviceNetwork.NodeHandle>();
			var networkMarshaler = Antilatency.InterfaceContract.Details.InterfaceMarshaler.ManagedToNative<Antilatency.DeviceNetwork.INetwork>(network);
			HandleExceptionCode(_VMT.findSupportedNodes(_object, networkMarshaler, resultMarshaler));
			result = resultMarshaler.value;
			resultMarshaler.Dispose();
			return result;
		}
	}
	public class ICotaskConstructorRemap : Antilatency.InterfaceContract.Details.IInterfaceRemap {
		public new struct VMT {
			public delegate Antilatency.InterfaceContract.ExceptionCode isSupportedDelegate(System.IntPtr _this, System.IntPtr network, Antilatency.DeviceNetwork.NodeHandle node, out Antilatency.InterfaceContract.Bool result);
			public delegate Antilatency.InterfaceContract.ExceptionCode findSupportedNodesDelegate(System.IntPtr _this, System.IntPtr network, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result);
			#pragma warning disable 0649
			public isSupportedDelegate isSupported;
			public findSupportedNodesDelegate findSupportedNodes;
			#pragma warning restore 0649
		}
		public new static readonly NativeInterfaceVmt NativeVmt;
		static ICotaskConstructorRemap() {
			var vmtBlocks = new System.Collections.Generic.List<object>();
			AppendVmt(vmtBlocks);
			NativeVmt = new NativeInterfaceVmt(vmtBlocks);
		}
		protected static new void AppendVmt(System.Collections.Generic.List<object> buffer) {
			Antilatency.InterfaceContract.Details.IInterfaceRemap.AppendVmt(buffer);
			var vmt = new VMT();
			vmt.isSupported = (System.IntPtr _this, System.IntPtr network, Antilatency.DeviceNetwork.NodeHandle node, out Antilatency.InterfaceContract.Bool result) => {
				try {
					var obj = GetContext(_this) as ICotaskConstructor;
					var networkMarshaler = network == System.IntPtr.Zero ? null : new Antilatency.DeviceNetwork.Details.INetworkWrapper(network);
					var resultMarshaler = obj.isSupported(networkMarshaler, node);
					result = resultMarshaler;
				}
				catch (System.Exception ex) {
					result = default(Antilatency.InterfaceContract.Bool);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			vmt.findSupportedNodes = (System.IntPtr _this, System.IntPtr network, Antilatency.InterfaceContract.Details.ArrayOutMarshaler.Intermediate result) => {
				try {
					var obj = GetContext(_this) as ICotaskConstructor;
					var networkMarshaler = network == System.IntPtr.Zero ? null : new Antilatency.DeviceNetwork.Details.INetworkWrapper(network);
					var resultMarshaler = obj.findSupportedNodes(networkMarshaler);
					result.assign(resultMarshaler);
				}
				catch (System.Exception ex) {
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			buffer.Add(vmt);
		}
		public ICotaskConstructorRemap() { }
		public ICotaskConstructorRemap(System.IntPtr context, ushort lifetimeId) {
			AllocateNativeInterface(NativeVmt.Handle, context, lifetimeId);
		}
	}
}

[Guid("1f3f7579-813e-4528-82f9-5a5fc35a9295")]
public interface ICotaskBatteryPowered : Antilatency.DeviceNetwork.ICotask {
	/// <summary>Get actual battery level.</summary>
	/// <returns>Battery level in range 0 .. 1. Value 0 - empty battery, value 1 - full battery.</returns>
	float getBatteryLevel();
}
namespace Details {
	public class ICotaskBatteryPoweredWrapper : Antilatency.DeviceNetwork.Details.ICotaskWrapper, ICotaskBatteryPowered {
		private ICotaskBatteryPoweredRemap.VMT _VMT = new ICotaskBatteryPoweredRemap.VMT();
		protected new int GetTotalNativeMethodsCount() {
		    return base.GetTotalNativeMethodsCount() + typeof(ICotaskBatteryPoweredRemap.VMT).GetFields().Length;
		}
		public ICotaskBatteryPoweredWrapper(System.IntPtr obj) : base(obj) {
		    _VMT = LoadVMT<ICotaskBatteryPoweredRemap.VMT>(base.GetTotalNativeMethodsCount());
		}
		public float getBatteryLevel() {
			float result;
			float resultMarshaler;
			HandleExceptionCode(_VMT.getBatteryLevel(_object, out resultMarshaler));
			result = resultMarshaler;
			return result;
		}
	}
	public class ICotaskBatteryPoweredRemap : Antilatency.DeviceNetwork.Details.ICotaskRemap {
		public new struct VMT {
			public delegate Antilatency.InterfaceContract.ExceptionCode getBatteryLevelDelegate(System.IntPtr _this, out float result);
			#pragma warning disable 0649
			public getBatteryLevelDelegate getBatteryLevel;
			#pragma warning restore 0649
		}
		public new static readonly NativeInterfaceVmt NativeVmt;
		static ICotaskBatteryPoweredRemap() {
			var vmtBlocks = new System.Collections.Generic.List<object>();
			AppendVmt(vmtBlocks);
			NativeVmt = new NativeInterfaceVmt(vmtBlocks);
		}
		protected static new void AppendVmt(System.Collections.Generic.List<object> buffer) {
			Antilatency.DeviceNetwork.Details.ICotaskRemap.AppendVmt(buffer);
			var vmt = new VMT();
			vmt.getBatteryLevel = (System.IntPtr _this, out float result) => {
				try {
					var obj = GetContext(_this) as ICotaskBatteryPowered;
					var resultMarshaler = obj.getBatteryLevel();
					result = resultMarshaler;
				}
				catch (System.Exception ex) {
					result = default(float);
					return handleRemapException(ex, _this);
				}
				return Antilatency.InterfaceContract.ExceptionCode.Ok;
			};
			buffer.Add(vmt);
		}
		public ICotaskBatteryPoweredRemap() { }
		public ICotaskBatteryPoweredRemap(System.IntPtr context, ushort lifetimeId) {
			AllocateNativeInterface(NativeVmt.Handle, context, lifetimeId);
		}
	}
}


}
