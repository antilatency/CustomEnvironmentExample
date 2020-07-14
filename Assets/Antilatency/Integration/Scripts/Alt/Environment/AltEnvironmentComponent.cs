using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Antilatency.Integration {
    /// <summary>
    /// Alt environment base abstract class, represents the Antilatency tracking environment (IR markers pattern).
    /// </summary>
    public abstract class AltEnvironmentComponent : MonoBehaviour {

        /// <returns>Native IEnvironment object.</returns>
        public abstract Antilatency.Alt.Tracking.IEnvironment GetEnvironment();
    }
}
