# CuttingEdge-VR-TransitionTool
A Unity VR Toolkit developed at CMU ETC Project Cutting Edge. This toolkit help designers to make cinematic cuts and transitions in VR. 

## Features:
Camera Blending
Camera Recalibration
Attention Detection - TODO
Interaction Trigger: Gaze, Grab
Scene Management:  Load/Reload, Prepare Loading
Sound Management: 
Keep audio playing through scene transitions
Interactive sound queue (to be renamed)

## Script Structure:
Sequence Controller: one GameFlow Controller for each example sequence
CECameraBrain: On main rendering camera (MindPalace Cam).
CECameraSlave: Just a helper class, on the parent object of the blending camera
CECameraSlaveCam: On the blending cameras
CEMindPalaceController: On the parent object of CE_MindPalace (prefab)
CESceneHelper: On a designated game manager assembly game object
CESync: On the grounding game object to be synced
CEUtilities: On a designated game manager assembly game object (todo)
CECollisionHelper: tobe deleted
CESoundManager: FadeIn and FadeOut sound transition


## Progress Snapshots
![] (swipe.gif)
