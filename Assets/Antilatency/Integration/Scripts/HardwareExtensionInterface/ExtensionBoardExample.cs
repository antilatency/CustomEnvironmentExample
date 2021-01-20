// Copyright (c) 2020 ALT LLC
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of source code located below and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//  
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//  
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

using Antilatency.DeviceNetwork;
using Antilatency.Integration;

// Usage sample of Antilatency Hardware Extension Interface library.
public class ExtensionBoardExample : MonoBehaviour {
	[Serializable]
	public class PressEvent : UnityEvent { }

	bool _playing = false;

	public PressEvent OnButtonPressed = new PressEvent();
	public PressEvent OnButtonReleased = new PressEvent();

    public DeviceNetwork Network;

    public string Tag;

    public bool AheiTaskState {
        get {
            return _aheiCotask != null;
        }
    }

    protected Antilatency.HardwareExtensionInterface.ILibrary _aheiLibrary;

    protected NodeHandle _aheiNode;

    private Antilatency.HardwareExtensionInterface.ICotask _aheiCotask;

    protected virtual void Awake() {
        Init();
    }

    private void Init() {
        if (Network == null) {
            Debug.LogError("Network is null");
            return;
        }

        _aheiLibrary = Antilatency.HardwareExtensionInterface.Library.load();

        if (_aheiLibrary == null) {
            Debug.LogError("Failed to create hardware extension interface library");
            return;
        }
    }

    protected virtual void OnEnable() {
        if (Network == null) {
            return;
        }

        Network.DeviceNetworkChanged.AddListener(OnDeviceNetworkChanged);

        OnDeviceNetworkChanged();
    }

    protected virtual void OnDisable() {
        if (Network == null) {
            return;
        }

        Network.DeviceNetworkChanged.RemoveListener(OnDeviceNetworkChanged);

        StopAhei();
    }

    private void OnDeviceNetworkChanged() {
        if (_aheiCotask != null) {
            if (_aheiCotask.isTaskFinished()) {
                StopAhei();
            } else {
                return;
            }
        }

        if (_aheiCotask == null) {
            var node = GetAvailableAheiNode();
            if (node != NodeHandle.Null) {
                StartAhei(node);
            }
        }
    }

    protected virtual void Update() {
        if (_aheiCotask != null && _aheiCotask.isTaskFinished()) {
            StopAhei();
            return;
        }
    }

    // Running ahei examples
    private void RunAhei() {
        TestButton();
        //TestBoard();
        //InputTest();
        //OutputTest();
        //PulseCounterTest();
        //AnalogTest();
    }

    // An example of using Input pin
    private void TestButton()
    {
        var inputPin = _aheiCotask.createInputPin(Antilatency.HardwareExtensionInterface.Interop.Pins.IO5);

        _aheiCotask.run();

        StartCoroutine(TestButton(inputPin));
    }

    // An example of using Input, Output and Analog pins
    private void TestBoard()
    {
        var inputPin = _aheiCotask.createInputPin(Antilatency.HardwareExtensionInterface.Interop.Pins.IO1);

        var outputPin = _aheiCotask.createOutputPin(Antilatency.HardwareExtensionInterface.Interop.Pins.IO2, Antilatency.HardwareExtensionInterface.Interop.PinState.Low);

        var analogPin = _aheiCotask.createAnalogPin(Antilatency.HardwareExtensionInterface.Interop.Pins.IOA3, 10);

        _aheiCotask.run();

        StartCoroutine(TestBoardCoroutine(inputPin, outputPin, analogPin));
    }

    // An example of using all IOs as Input pins
    private void InputTest()
    {
        var inputPins = new List<Antilatency.HardwareExtensionInterface.IInputPin> {
            _aheiCotask.createInputPin(Antilatency.HardwareExtensionInterface.Interop.Pins.IO1),
            _aheiCotask.createInputPin(Antilatency.HardwareExtensionInterface.Interop.Pins.IO2),
            _aheiCotask.createInputPin(Antilatency.HardwareExtensionInterface.Interop.Pins.IOA3),
            _aheiCotask.createInputPin(Antilatency.HardwareExtensionInterface.Interop.Pins.IOA3),
            _aheiCotask.createInputPin(Antilatency.HardwareExtensionInterface.Interop.Pins.IO5),
            _aheiCotask.createInputPin(Antilatency.HardwareExtensionInterface.Interop.Pins.IO6),
            _aheiCotask.createInputPin(Antilatency.HardwareExtensionInterface.Interop.Pins.IO7),
            _aheiCotask.createInputPin(Antilatency.HardwareExtensionInterface.Interop.Pins.IO8)
        };

        _aheiCotask.run();

        StartCoroutine(InputTest(inputPins));
    }

    // An example of using all IOs as Output pins
    private void OutputTest()
    {
        var outputPins = new List<Antilatency.HardwareExtensionInterface.IOutputPin> {
            _aheiCotask.createOutputPin(Antilatency.HardwareExtensionInterface.Interop.Pins.IO1, Antilatency.HardwareExtensionInterface.Interop.PinState.High),
            _aheiCotask.createOutputPin(Antilatency.HardwareExtensionInterface.Interop.Pins.IO2, Antilatency.HardwareExtensionInterface.Interop.PinState.High),
            _aheiCotask.createOutputPin(Antilatency.HardwareExtensionInterface.Interop.Pins.IOA3, Antilatency.HardwareExtensionInterface.Interop.PinState.High),
            _aheiCotask.createOutputPin(Antilatency.HardwareExtensionInterface.Interop.Pins.IOA4, Antilatency.HardwareExtensionInterface.Interop.PinState.High),
            _aheiCotask.createOutputPin(Antilatency.HardwareExtensionInterface.Interop.Pins.IO5, Antilatency.HardwareExtensionInterface.Interop.PinState.High),
            _aheiCotask.createOutputPin(Antilatency.HardwareExtensionInterface.Interop.Pins.IO6, Antilatency.HardwareExtensionInterface.Interop.PinState.High),
            _aheiCotask.createOutputPin(Antilatency.HardwareExtensionInterface.Interop.Pins.IO7, Antilatency.HardwareExtensionInterface.Interop.PinState.High),
            _aheiCotask.createOutputPin(Antilatency.HardwareExtensionInterface.Interop.Pins.IO8, Antilatency.HardwareExtensionInterface.Interop.PinState.High)
        };

        _aheiCotask.run();

        StartCoroutine(OutputTest(outputPins));
    }

    // An example of using PulseCounters
    private void PulseCounterTest()
    {
        var output = _aheiCotask.createOutputPin(Antilatency.HardwareExtensionInterface.Interop.Pins.IO1, Antilatency.HardwareExtensionInterface.Interop.PinState.High);

        var pulseCounter1 = _aheiCotask.createPulseCounterPin(Antilatency.HardwareExtensionInterface.Interop.Pins.IO2, 100);
        var pulseCounter2 = _aheiCotask.createPulseCounterPin(Antilatency.HardwareExtensionInterface.Interop.Pins.IO8, 100);

        _aheiCotask.run();

        StartCoroutine(PulseCounterTest(output, pulseCounter1, pulseCounter2));
    }

    // An example of using Analog pins
    private void AnalogTest()
    {
        var a1 = _aheiCotask.createAnalogPin(Antilatency.HardwareExtensionInterface.Interop.Pins.IOA3, 10);
        var a2 = _aheiCotask.createAnalogPin(Antilatency.HardwareExtensionInterface.Interop.Pins.IOA4, 10);

        _aheiCotask.run();

        StartCoroutine(AnalogTest(a1, a2));
    }

    protected virtual void OnDestroy() {
        StopAllCoroutines();

        if (_aheiCotask != null) {
            _aheiCotask.Dispose();
            _aheiCotask = null;
        }

        if (_aheiLibrary != null) {
            _aheiLibrary.Dispose();
            _aheiLibrary = null;
        }
    }

    protected INetwork GetNativeNetwork() {
        if (Network == null) {
            Debug.LogError("Network is null");
            return null;
        }

        if (Network.NativeNetwork == null) {
            Debug.LogError("Native network is null");
            return null;
        }

        return Network.NativeNetwork;
    }

    protected void StartAhei(NodeHandle node) {
        var network = GetNativeNetwork();
        if (network == null) {
            return;
        }

        if (network.nodeGetStatus(node) != NodeStatus.Idle) {
            Debug.LogError("Wrong node status");
            return;
        }

        using (var cotaskConstructor = _aheiLibrary.getCotaskConstructor()) {
            _aheiCotask = cotaskConstructor.startTask(network, node);

            if (_aheiCotask == null) {
                StopAhei();
                Debug.LogWarning("Failed to start hardware extension interface task on node " + node.value);
                return;
            }

            _aheiNode = node;

            RunAhei();
        }
    }

    private IEnumerator TestButton(Antilatency.HardwareExtensionInterface.IInputPin inputPin) {
        while (_aheiCotask != null) {
            yield return new WaitForSeconds(0.001f);

            var inputValue = inputPin.getState();

			if (inputValue == Antilatency.HardwareExtensionInterface.Interop.PinState.Low){
				OnButtonReleased.Invoke();
				if(_playing == true){
				    OnButtonReleased.Invoke();
					_playing = false;
				}
			} else {
				if(_playing == false){
				    OnButtonPressed.Invoke();
					_playing = true;
				}
			}
        }
    }

    private IEnumerator TestBoardCoroutine(Antilatency.HardwareExtensionInterface.IInputPin inputPin, Antilatency.HardwareExtensionInterface.IOutputPin outputPin, Antilatency.HardwareExtensionInterface.IAnalogPin analogPin) {
        while (_aheiCotask != null) {
            yield return new WaitForSeconds(0.001f);

            var inputValue = inputPin.getState();

            if (inputValue == Antilatency.HardwareExtensionInterface.Interop.PinState.Low) {
                outputPin.setState(Antilatency.HardwareExtensionInterface.Interop.PinState.High);
            } else {
                outputPin.setState(Antilatency.HardwareExtensionInterface.Interop.PinState.Low);
            }

            var analogValue = analogPin.getValue();
        }
    }
    private IEnumerator InputTest(List<Antilatency.HardwareExtensionInterface.IInputPin> inputPins)
    {
        while (_aheiCotask != null)
        {
            yield return new WaitForSeconds(0.001f);

            Debug.Log("Input test");
            foreach (var input in inputPins)
            {
                Debug.Log("    " + input.getState().ToString());
            }
            Debug.Log("-----");
        }
    }

    private IEnumerator OutputTest(List<Antilatency.HardwareExtensionInterface.IOutputPin> outputPins) {
        while (_aheiCotask != null)
        {
            yield return new WaitForSeconds(0.001f);

            Debug.Log("Output test");

            foreach (var item in outputPins)
            {
                item.setState(Antilatency.HardwareExtensionInterface.Interop.PinState.Low);
            }

            yield return new WaitForSeconds(0.1f);

            foreach (var item in outputPins)
            {
                item.setState(Antilatency.HardwareExtensionInterface.Interop.PinState.High);
            }

            yield return new WaitForSeconds(0.1f);

            Debug.Log("-----");
        }
    }

    private IEnumerator PulseCounterTest(Antilatency.HardwareExtensionInterface.IOutputPin output,
        Antilatency.HardwareExtensionInterface.IPulseCounterPin pulseCounter1,
        Antilatency.HardwareExtensionInterface.IPulseCounterPin pulseCounter2
        )
    {
        while (_aheiCotask != null)
        {
            yield return new WaitForSeconds(0.001f);

            Debug.Log("Pulse counter test test");
            output.setState(Antilatency.HardwareExtensionInterface.Interop.PinState.Low);

            yield return new WaitForSeconds(0.1f);

            output.setState(Antilatency.HardwareExtensionInterface.Interop.PinState.High);

            yield return new WaitForSeconds(0.1f);

            Debug.Log($"    Pulse counter IO2 test:{pulseCounter1.getValue()}\n    Pulse counter IO8 test:{pulseCounter2.getValue()}");

            yield return null;
            Debug.Log("-----");
        }
    }

    private IEnumerator AnalogTest(Antilatency.HardwareExtensionInterface.IAnalogPin a1, Antilatency.HardwareExtensionInterface.IAnalogPin a2) {

        while (_aheiCotask != null)
        {
            yield return new WaitForSeconds(0.001f);
            Debug.Log("Analog test");

            Debug.Log($"    Input 3 analog value: {a1.getValue()}");
            Debug.Log($"    Input 4 analog value: {a2.getValue()}");

            yield return new WaitForSeconds(0.01f);
        }
    }

    protected void StopAhei() {
        StopAllCoroutines();

        if (_aheiCotask == null) {
            return;
        }

        _aheiCotask.Dispose();
        _aheiCotask = null;
        _aheiNode = new NodeHandle();
    }

    protected NodeHandle GetAvailableAheiNode() {
        return GetFirstIdleTagAheiNode();
    }

    protected NodeHandle[] GetIdleAheiNodes() {
        var nativeNetwork = GetNativeNetwork();

        if (nativeNetwork == null) {
            return new NodeHandle[0];
        }

        using (var cotaskConstructor = _aheiLibrary.getCotaskConstructor()) {
            var nodes = cotaskConstructor.findSupportedNodes(nativeNetwork).Where(v =>
                    nativeNetwork.nodeGetStatus(v) == NodeStatus.Idle
                ).ToArray();

            return nodes;
        }
    }

    protected NodeHandle GetFirstIdleAheiNode() {
        var nodes = GetIdleAheiNodes();
        if (nodes.Length == 0) {
            return new NodeHandle();
        }
        return nodes[0];
    }

    protected NodeHandle GetFirstIdleTagAheiNode() {
        var nativeNetwork = GetNativeNetwork();

        if (nativeNetwork == null) {
            return new NodeHandle();
        }

        var nodes = GetIdleAheiNodes();
        if (nodes.Length == 0) {
            return new NodeHandle();
        }

        var node = nodes.Where(v => nativeNetwork.nodeGetStringProperty(v, "Tag") == Tag).FirstOrDefault();
        return node;
    }

}