# Treap Visualizador

## Integrantes
- Alejandro Oré
- José Tipula
- Valeria Valdez

## Código fuente:

Esta herramienta de visualización del funcionamiento de un Treap se creó mediante el motor gráfico/de videojuegos Godot, y el visualizador fue creado con ayuda de este y programado en C#. En esta carpeta se puede encontrar los archivos .cs del código base, el más importante de estos siendo 'Treap.cs', puesto a que contiene el código fuente del Treap y sus funciones en el visualizador.

## Instrucciones de uso:

Para ejecutar el visualizar debe dirigirse a la carpeta 'Output', donde encontrará el ejecutable 'Treap.exe'. Si se encuentra en Windows ejecútelo con doble click.

Para usar el visualizador escriba un número en el text input, y elija el botón que represente la opción que desee realizar.

Si por algún motivo el Treap crece tan grande que se sale de la cámara, la misma se puede mover utilizando las flechitas del teclado, y se puede hacer zoom in y zoom out utilizando las teclas '+' y '-' respectivamente.

Si por algún motivo imprevisto el Treap toma una forma extraña entonces el visualizador ha fallado en mover los nodos a sus lugares correctos. Ello se puede arreglar utilizando la opción extra que es presionar la tecla 'esc', la cual reorganizará los nodos por si están mal colocados (esto es puramente visual, no hay ninguna rotación siendo aplicada).