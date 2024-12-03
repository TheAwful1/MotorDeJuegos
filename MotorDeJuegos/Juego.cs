using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotorDeJuegos._3Dobj;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MotorDeJuegos
{
    public class Juego : GameWindow
    {
        float[] vertices = //Un vertice es el punto que conecta la figura geometrica
        {
            0f, 0.5f, 0f,  //Vertice de arriba
            -0.5f, -0.5f, 0f, //Vertice de la izquierda 
            0.5f,-0.5f, 0f  //Vertice de la derecha
        
        };
        //El Rendering Pipeline es la secuencia de pasos que toma OpenGL cuando renderiza un objeto
        //Una operacion de renderizado es 
        // Render Pipeline vars (variables de )
        int vao; 
        int shaderProgram;
        int vbo;
        


        //CONSTANTES
        int Anchura, Altura;

        public Juego(int anchura, int altura): base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            //Centrar la ventana en el monitor
           

           this.Anchura = anchura;
            this.Altura = altura;

            CenterWindow(new Vector2i(anchura,altura));
        }
        //Esta funcion se llama cada vez que se le cambia el tamaño a la pantalla
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            this.Anchura = e.Width;
            this.Altura = e.Height;
        }
        //Se llama una vez que inicia esta clase
        protected override void OnLoad()
        {
            base.OnLoad();

            //Genera el VBO
            vao = GL.GenVertexArray(); 


            //Crea un buffer
            vbo = GL.GenBuffer();
            //Une el buffer como un buffer array
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo); 
            //Almacena los datos en el vbo
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);


            //Une el VAO
            GL.BindVertexArray(vao);
            //Apunta el espacio (0) de el VAO al VBO actual unido
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //Habilita el espacio 
            GL.EnableVertexArrayAttrib(vao, 0);

            //Separa el vbo y el vao respectivamente
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);


            //Creando el programa de shaders
            shaderProgram = GL.CreateProgram();

            //Creando el shader de vertices
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);

            // add the source code from "Default.vert" in the Shaders file
            GL.ShaderSource(vertexShader, LoadShaderSource("Default.vert"));
            // Compile the Shader
            GL.CompileShader(vertexShader);

            // Same as vertex shader
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, LoadShaderSource("Default.frag"));
            GL.CompileShader(fragmentShader);

            // Attach the shaders to the shader program
            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);

            // Link the program to OpenGL
            GL.LinkProgram(shaderProgram);

            // delete the shaders
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);




        }

        protected override void OnUnload()
        {
            base.OnUnload();

            // Delete, VAO, VBO, Shader Program
            GL.DeleteVertexArray(vao);
            GL.DeleteBuffer(vbo);
            GL.DeleteProgram(shaderProgram);
        }

        //llamado en cada frame, toda la renderizacion pasa aqui
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            //pone el color con el que se llenara la pantalla
            GL.ClearColor(0.6f,0.3f,1.0f,1.0f);

            //LLena la pantalla con ese color
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //Esto dibuja el triangulo
            GL.UseProgram(shaderProgram); // une el vao
            GL.BindVertexArray(vao); // usa el programa de shader
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);//Dibuja el triangulo  | args = Primitive type , first vertex, last vertex. Recibe los parametros: Tipo primitivo (clases que se usan para construir figuras en OpenGL), primer vertice y ultimo vertice
            //
            Context.SwapBuffers();
            base.OnRenderFrame(args);

           
        }

        // se llama esta funcion en cada frame. Todas las actualizaciones (Updates) pasan aqui
        protected override void OnUpdateFrame(FrameEventArgs args) //Nota que esto es un Override una sobreescritura de una funcion de la clase GameWindow
        {
            base.OnUpdateFrame(args);
        }

        // Function to load a text file and return its contents as a string
        public static string LoadShaderSource(string filePath)
        {
            string shaderSource = "";

            try
            {
                using (StreamReader reader = new StreamReader("../../../Shaders/" + filePath))
                {
                    shaderSource = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to load shader source file: " + e.Message);
            }

            return shaderSource;
        }




    }
}
