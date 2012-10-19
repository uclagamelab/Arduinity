/*
*   This controls the onscreen blinking light for 
*   the Fade example in Arduinity.
*
*/

#pragma strict

var arduinityToWatch : Arduinity;
var pinToWatch : int;
var pointLight : Light;
var offMaterial : Material;
var onMaterial : Material;

private var maxLightVal : float = 255.0f;

function Update () {
    var color : Color;
    var pinVal : float = arduinityToWatch.digitalPins[pinToWatch];
    if ( pinVal == 0) 
    {
        color = renderer.materials[1].GetColor("_TintColor");
        color.a = 0.0f;
        renderer.materials[1].SetColor("_TintColor", color);
        pointLight.intensity = 0.0f;
    }
    else
    {
        color = renderer.materials[1].GetColor("_TintColor");

        color.a = Mathf.Lerp(0.0f,1.0f,(pinVal)/(maxLightVal));
        renderer.materials[1].SetColor("_TintColor", color);
        Debug.Log("set to " + color.a);
        pointLight.intensity = Mathf.Lerp(0.0f,1.0f,arduinityToWatch.digitalPins[pinToWatch]/maxLightVal);
    }
}