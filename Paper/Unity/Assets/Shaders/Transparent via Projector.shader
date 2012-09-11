Shader "Transparent via Projector" {
	
SubShader
{
	Tags
		{"Queue" = "Overlay"}
	
	// The alpha is supplied by the projector.
	Blend OneMinusDstAlpha DstAlpha
	
	Color (.6,0,.7)
	Pass {}
}


}