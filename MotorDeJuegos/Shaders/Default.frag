#version 330 core

out vec4 FragColor; //Desde aqui se mandan los datos a la gpu creo que en este caso es un post proceso
void main(){
	FragColor = vec4(1.0,0.0,0.0,1.0); //Este codigo indica de que color es es triangulo
}
