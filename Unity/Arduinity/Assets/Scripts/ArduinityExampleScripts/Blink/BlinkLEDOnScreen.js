/*
*   This controls the onscreen blinking light for 
*   the Blink example in Arduinity.
*
*/


#pragma strict

var arduinityToWatch : Arduinity;
var pinToWatch : int;
var pointLight : Light;
var offMaterial : Material;
var onMaterial : Material;

function Update () {
    if (arduinityToWatch.digitalPins[pinToWatch] == 0) 
    {
        renderer.sharedMaterial = offMaterial;
        pointLight.intensity = 0;
    }
    else
    {
        renderer.sharedMaterial = onMaterial;
        pointLight.intensity = 1;
    }
}