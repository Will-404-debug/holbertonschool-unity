# Unity-Shadergraph

This project focuses on learning and applying **Unity’s ShaderGraph** to create custom shaders and visual effects in Unity. You will explore rendering concepts and build visually distinctive shaders like glowing effects, holograms, animated water, dissolves, and more. These shaders are designed to be **visually identifiable by non-technical users**, while showcasing deep understanding of Unity’s rendering pipeline.

---

## ✨ Objectives

By the end of this project, you will be able to:

- Explain the concepts of **rendering**, **render pipelines**, and **shaders**.
- Describe how Unity’s **Scriptable Render Pipeline (SRP)** enables ShaderGraph.
- Implement both **fragment and vertex** stage logic in shaders.
- Create visual effects using:
  - **Glow/Emission**
  - **Pulsing animation**
  - **Disintegration/Dissolve**
  - **Holographic flicker**
  - **Water with sine wave motion**
- Utilize **UVs**, **world position**, **time**, **noise**, and **math nodes** creatively.
- Animate **mesh geometry** using **vertex displacement**.

---

## 🧱 Project Structure

- All shaders use Unity’s **ShaderGraph** in the **Universal Render Pipeline (URP)**.
- Shaders are assigned to **GameObjects** for demonstration and review.
- Each shader is **visually distinct and labeled**.
- All shaders are exported as a **Unity Asset package**.
- No final game build is required.

---

## ✅ Key Shader Implementations

| Shader Name     | Description                                               |
|------------------|-----------------------------------------------------------|
| `Glow Up`        | Emits light with HDR base color + pulsing logic           |
| `Pulse`          | Color intensifies with sine over time                     |
| `Mr. Stark`      | Dissolve effect using noise, edge highlight, and alpha    |
| `Iceman`         | Transparent ice effect with distortion and color tinting  |
| `Cortana`        | Flickering hologram using UV + sine + texture modulation  |
| `Sine of the Sea`| Water shader with:                                        |
|                  | • Color depth blend (shallow/deep)                        |
|                  | • Foam crest detection using Simple Noise                 |
|                  | • **Fresnel edge glow**                                   |
|                  | • **Vertex displacement using sine**                      |
|                  | • Animated surface that moves with time                   |

## Key Commands
To activate the animation for `Mr. Stark` press `spacebar` and to reset press `r`.
---

## 🧰 Requirements

- Unity version **2022 LTS or later**
- Use **URP** and **ShaderGraph** (no HLSL hand-coding)
- All shaders must produce a **visibly recognizable result**
- Shaders should be customizable via **Inspector properties**
- Include this `README.md` in your repo
- Use Unity’s default `.gitignore`
- Push the entire project folder to your GitHub repository

---

## 📚 References

### Watch:
- [Shaders 101](https://www.youtube.com/watch?v=T-HXmQAMhG0)
- [Intro to ShaderGraph](https://www.youtube.com/playlist?list=PLX2vGYjWbI0RyhAsNJg4sLLKgCZsRSim2)
- [Shaders Case Studies](https://www.youtube.com/playlist?list=PLJ4rOFLQFH4C0zPBu-fgFKMvrHadY6dhm)

### Read:
- [Unity Learn - Shading](https://learn.unity.com/tutorial/rendering-and-shading)
- [ShaderGraph Documentation](https://docs.unity3d.com/Packages/com.unity.shadergraph@latest/)
- [URP Manual](https://docs.unity3d.com/Manual/universalrp-overview.html)
- [Asset Packages](https://docs.unity3d.com/Manual/AssetPackages.html)

---

## 📦 Deliverables

- ✅ Fully functional shaders built with ShaderGraph
- ✅ Exported `.unitypackage` with all shader assets
- ✅ Unity project in version-controlled repository
- ✅ This updated `README.md`

---

## 👤 Author

**William Guilon Dronnier**

---

Happy shading! 🎨✨  
Let your pixels move and glow!
