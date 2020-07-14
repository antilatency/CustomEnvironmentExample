﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Antilatency.Alt.Tracking;

namespace Antilatency.Integration {
    /// <summary>
    /// The Basic Alt Environment implementation uses AltSystem to get the environment that is marked as default.
    /// </summary>
    public class AltEnvironment : AltEnvironmentComponent {
        private IEnvironment _environment = null;

        /// <returns>An IEnvironment object which was created from AltSystem's environment that is marked as default.</returns>
        public override IEnvironment GetEnvironment() {
            //If the environment has been already created, just return it.
            if (_environment != null) {
                return _environment;
            }

            //Get local storage to read environment code from.
            using (var localStorage = StorageClient.GetLocalStorage()) {
                if (localStorage == null) {
                    return null;
                }

                //Get environment code.
                var environmentCode = localStorage.read("environment", "default");
                if (string.IsNullOrEmpty(environmentCode)) {
                    Debug.LogError("Failed to get environment code");
                    return null;
                }

                //Load tracking library to create environment from the code.
                using (var trackingLibrary = Library.load()) {
                    if (trackingLibrary == null) {
                        Debug.LogError("Failed to load tracking library");
                        return null;
                    }

                    //Create environment from the code.
                    _environment = trackingLibrary.createEnvironment(environmentCode);
                    if (_environment == null) {
                        Debug.LogError("Failed to create environment");
                        return null;
                    }

                    return _environment;
                }
            }
        }

        private void OnDestroy() {
            if (_environment != null) {
                _environment.Dispose();
                _environment = null;
            }
        }
    }
}