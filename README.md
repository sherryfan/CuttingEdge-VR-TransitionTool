# CuttingEdge-VR-TransitionTool
A Unity VR Toolkit developed at CMU ETC Project Cutting Edge. This toolkit help designers to make cinematic cuts and transitions in VR. 

## Features:
Camera Blending  
Camera Recalibration  
Attention Detection System
Sync Tool
Frame Fading
Interaction Trigger: Gaze, Grab, HeadRotation 
Scene Management:  Load/Reload, Prepare Loading  
Integrated with Wwise

## Script Structure:
Sequence Controller: one GameFlow Controller for each example sequence  
CECameraBrain: On main rendering camera (MindPalace Cam).  
CECameraSlave: Just a helper class, on the parent object of the blending camera  
CECameraSlaveCam: On the blending cameras  
CEMindPalaceController: On the parent object of CE_MindPalace (prefab)  
CESceneHelper: On a designated game manager assembly game object  
CESync: On the grounding game object to be synced  
CEUtilities: On a designated game manager assembly game object 
CECameraFOV: On main camera for attention detection system
CEMontageController: prefab/in scene controller
CEWhiteFrame: On center eye anchor



## Progress Snapshots
Spead:  
![Alt Text](https://github.com/sherryfan/CuttingEdge-VR-TransitionTool/blob/master/spread.gif)  
Wipe:  
![Alt Text](https://github.com/sherryfan/CuttingEdge-VR-TransitionTool/blob/master/wipe.gif)  
Blend:  
![Alt Text](https://github.com/sherryfan/CuttingEdge-VR-TransitionTool/blob/master/blend.gif)  


