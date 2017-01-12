precision highp float;
varying vec2 vUV;
void main() {
    gl_Position = gl_Vertex;
	vUV = gl_MultiTexCoords0;
}