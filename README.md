Custom environment demonstration
================================

[![Video Antilatency Custom Environment](https://img.youtube.com/vi/XAbDLjyo0A4/0.jpg)](https://www.youtube.com/watch?v=XAbDLjyo0A4)

[Video AntilatencyCustomEnvironmentDemo](https://www.youtube.com/watch?v=XAbDLjyo0A4 "https://www.youtube.com/watch?v=XAbDLjyo0A4")

Prerequisites:
  - 3 IR LED markers to construct the environment. Note that the markers should not form a regular shape, such as an equilateral triangle..
  - 1 Alt tracker.

How to run:
1. Open the project in Unity.
2. Open SampleScene.
3. Place objects marker0, marker1 and marker2 according to positions of your real markers. 
4. Connect Alt tracker.
5. Run the sample in the editor.

Visualization:
Markers displayed as encircled white dots.
The last successful IEnvironment.match() call provides:
  - lines (painted blue) from the calculated position to the projection on the floor
  - projections, transformed to match the markers (blue crosses on the floor plane)
  - differences between transformed projections and corresponding markers (painted red) 
Last IEnvironment.matchByPosition() provides:
  - matched rays painted green
  - unmatched rays painted magenta

Interesting files:
  - CustomEnvironment.cs: minimal correct IEnvironment implementation with explanations.
  - ThreeMarkersEnvironment.cs: implementation of a simple environment, consisting of 3 markers on the floor plane. 
  - ThreeMarkersEnvironmentComponent.cs: implementation of the AltEnvironmentComponent,which is required by the AltTracking script. AltEnvironmentComponent inherits MonoBehaviour and provides the IEnvironment object to AltTracking.
