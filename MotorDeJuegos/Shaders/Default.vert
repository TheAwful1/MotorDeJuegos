#version 330 core
layout (location = 0) in vec3 aPosition;
void main()
{
	gl_Position = vec4(aPosition, 1.0);//Este codigo indica las coordenadas del triangulo
}