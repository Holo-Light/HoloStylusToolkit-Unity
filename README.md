<img src="./Images/Logo_small.png" width="40%"/>

# Index
1. [What is HoloStylusToolkit-Unity](#whatis)
1. [Features](#feats)
1. [Required Software](#softrequirements)
1. [Holo-Stylus Driver SDK](#driversdk)
1. [Getting Started](#getstarted)
1. [Basic Examples](#basicexamples)
1. [Input Examples](#inputexamples)
1. [Drawing Examples](#drawingexamples)

# <a name="whatis"></a> What is HoloStylusToolkit-Unity

The Holo-Stylus Toolkit is a collection of scripts and components intended to accelerate development of applications targeting the new 3D input device [Holo-Stylus](https://www.holo-stylus.com/) for head-mounted units. Holo-Stylus works as interaction and creation tool in Mixed Reality with yet unmatched precision and was developed by [HoloLight](https://www.holo-light.com/). Currently Holo-Stylus Toolkit supports the creation of Mixed Reality applications for Microsoft HoloLens with [Unity 3D](https://unity3d.com/de).

# <a name="feats"></a> Features

The project contains a toolkit which helps developers to create Mixed Reality Apps with ease and loads of useful and cool examples. There is also an emulator included to use the mouse as input device for testing your applications within the Unity Inspector in order to reduce development overhead.

# <a name="softrequirements"></a> Required Software

- [Windows 10 FCU](https://www.microsoft.com/software-download/windows10)
- [Unity 3D](https://unity3d.com/get-unity/download/archive)
- [Visual Studio 2017](http://dev.windows.com/downloads)

# <a name="driversdk"></a> Holo-Stylus Driver SDK

Holo-Stylus Driver SDK is a Universal Windows Platform Library for establishing a bluetooth connection between Microsoft HoloLens running your app and the Headmounted Unit of Holo-Stylus to acquire pose data and device events of Holo-Stylus. Holo-Stylus Driver SDK is included as .NET DLL within Holo-Stylus Toolkit in the Plugins folder of the Unity project.

# <a name="getstarted"></a> Getting started with Holo-Stylus Toolkit

To take advantage of Holo-Stylus’ various capabilities, have a look at our [guides](./Docs/GetStarted/getstarted.md) that help you getting started with your first Mixed Reality Apps for Holo-Stylus in Unity 3D.

# <a name="basicexamples"></a> Examples - Stylus Basics <img src="./Images/Icon-App_Holo-Stylus_white.png" width="5%" />

Intro | Scene Collection | Calibration | Connection |
------------ | ------------- | ------------- | ------------- |
<img src="./Images/scenes/IntroScene_sm.png"/> | <img src="./Images/scenes/Main_Menu_sm.png"/> | <img src="./Images/scenes/Rough_Calibration1_sm.png"/> | <img src="./Images/scenes/Holo-Stylus_sm.png"/> |
Contains an animated stylus logo | Provides an overview of all sample scenes | The Stylus can be calibrated here | Learn how to connect the Stylus by bluetooth |

# <a name="inputexamples"></a> Examples - Stylus Interaction <img src="./Images/Icon-App_Holo-Stylus_white.png" width="5%" />

Focussing | Grabbing | Painting | Measuring |
------------ | ------------- | ------------- | ------------- |
<img src="./Images/scenes/FocusTests_sm.png"/> | <img src="./Images/scenes/Grab_And_Physics_sm.png"/> | <img src="./Images/scenes/Paint_sm.png"/> | <img src="./Images/scenes/Measuring2_sm.png"/> |
Samples for focussing objects in a 3D scene | Grab holograms with the Stylus | Colorize a dinosaur with Stylus | Learn how to use the Stylus for measurement tasks |

# <a name="drawingexamples"></a> Examples - Drawing <img src="./Images/Icon-App_Holo-Stylus_white.png" width="5%" />

Simple Points | Lines | Triangles | Writing | 
------------ | ------------- | ------------- | ------------- |
<img src="./Images/scenes/Create_Points_sm.png"/> | <img src="./Images/scenes/Draw_Lines_sm.png"/> | <img src="./Images/scenes/Create_Triangles_sm.png"/> | <img src="./Images/scenes/Write_Something_sm.png"/> |
Draw simple points in 3D space | Draw some lines | Learn how to draw multiple triangles | Use your Stylus for 3D Writing in space |