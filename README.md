# Marching Cubes en la GPU · Marching Cubes on the GPU

Generación procedural de terreno 3D en Unity con el algoritmo **Marching Cubes**, calculado por completo
en la GPU con **compute shaders**.

*Procedural 3D terrain in Unity using the Marching Cubes algorithm, computed entirely on the GPU with
compute shaders.*

---

## Cómo funciona · How it works

1. **Ruido** (`NoiseCompute.compute`): un compute shader llena una rejilla de 16 × 16 × 16 puntos con
   ruido OpenSimplex2 con fractal *ridged*
   ([FastNoiseLite](https://github.com/Auburn/FastNoiseLite)). Se configuran la frecuencia, la amplitud,
   las octavas, la semilla y la altura del suelo.
2. **Marching Cubes** (`MarchingCubeShader.compute`): cada hilo de la GPU procesa un cubo de la rejilla,
   compara sus ocho esquinas con el nivel de superficie (*iso level*), busca la configuración en la
   tabla de triangulación e interpola los vértices sobre las aristas.
3. **Malla:** los triángulos se acumulan en un `AppendStructuredBuffer`. La CPU lee cuántos hay
   (`ComputeBuffer.CopyCount`) y arma la `Mesh` de Unity.

*A compute shader fills a 16³ grid with fractal noise; a second one runs Marching Cubes, one GPU thread
per cube, appending triangles to a buffer that the CPU reads back into a Unity `Mesh`.*

## Qué se puede tocar · What you can tweak

- En `NoiseGenerator`: frecuencia, amplitud, octavas, semilla y altura del suelo. Con *Auto Update*
  activado, el ruido se recalcula al cambiar un valor; si no, con el botón **Generate**.
- En `Chunk`: el nivel de superficie (*iso level*).

## Archivos · Files

| Archivo | Qué hace |
|---|---|
| `Assets/Scripts/Compute/NoiseCompute.compute` | Ruido fractal en la GPU |
| `Assets/Scripts/Compute/MarchingCubeShader.compute` | Marching Cubes, un hilo por cubo |
| `Assets/Scripts/Compute/Includes/MarchingTable.hlsl` | Tablas de aristas y triangulación |
| `Assets/Scripts/Chunk.cs` | Buffers de la GPU y construcción de la malla |
| `Assets/Scripts/NoiseGenerator.cs` | Parámetros del ruido |
| `Assets/Editor/GeneratorEditor.cs` | Regenera el terreno al cambiar el inspector |

## Stack

Unity 6 (6000.0.30f1) · C# · HLSL · Compute Shaders · Universal Render Pipeline

## Cómo abrirlo · How to open

Abrir la carpeta con Unity **6000.0.30f1** y ejecutar `Assets/Scenes/SampleScene.unity`.

## Créditos · Credits

- Ruido: [FastNoiseLite](https://github.com/Auburn/FastNoiseLite) (MIT), de Jordan Peck.
- Tablas de triangulación: las estándar del algoritmo Marching Cubes (Lorensen y Cline, 1987).
