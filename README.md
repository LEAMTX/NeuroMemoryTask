# NeuroMemoryTask

Unity prototype for a spatial memory task.

The player sees several cubes. For each trial, one cube is chosen as the target. After a short delay, only this target cube disappears. The player clicks where they remember the target was. The project calculates the distance between the clicked position and the real target position.

## Goal

This project shows a simple experimental loop in Unity:

1. Show objects in a 3D scene.
2. Choose one target object.
3. Hide only the target object.
4. Read the player answer.
5. Calculate the memory error.
6. Save the result in a CSV file.

## Features

1. One target cube disappears per trial.
2. The target can be chosen randomly.
3. The player can answer with mouse or touch input.
4. The result contains the clicked position, the real position, the error distance, and a score.
5. The trial can be restarted with the `R` key.
6. Results can be saved in CSV format.

## Technical Stack

1. Unity `6000.4.11f1`.
2. C#.
3. Universal Render Pipeline.
4. Unity Input System package installed.
5. Unity Test Framework package installed.

## Main Files

1. `Assets/Scripts/MemoryObject.cs`, controls one memory object.
2. `Assets/Scripts/MemoryManager.cs`, chooses the target, reads the answer, calculates the result, and saves the data.
3. `Assets/Scenes/SampleScene.unity`, contains the current scene.
4. `Packages/manifest.json`, lists the Unity packages.
5. `ProjectSettings/ProjectVersion.txt`, gives the Unity version.

## How To Test

1. Open the project in Unity Hub.
2. Use Unity `6000.4.11f1`, or a compatible Unity 6 version.
3. Open `Assets/Scenes/SampleScene.unity`.
4. Press Play.
5. Wait until one cube disappears.
6. Click where the cube was.
7. Read the result in the Unity Console.
8. Press `R` to start a new trial.

## Data Output

When CSV saving is enabled, the result is saved here:

```text
Application.persistentDataPath/memory_trial_results.csv
```

CSV columns:

```text
timestamp,clicked_x,clicked_y,clicked_z,target_x,target_y,target_z,error_distance,score
```

## Why It Is Useful

This project is a small proof of concept for a memory experiment. It connects a visual stimulus, a player response, an objective error measure, and a saved result. It can be extended later with repeated trials, participant identifiers, haptic feedback, XR input, or result analysis.

## Sources

1. Git remote verified locally, `git@github.com:LEAMTX/NeuroMemoryTask.git`.
2. Unity version verified in `ProjectSettings/ProjectVersion.txt`.
3. Unity packages verified in `Packages/manifest.json`.
4. Gameplay logic verified in `Assets/Scripts/MemoryObject.cs` and `Assets/Scripts/MemoryManager.cs`.
5. Application context based on the InterDigital job text provided in the conversation.
