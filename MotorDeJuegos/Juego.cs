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
        float[] vertices = //OpenGL utiliza vertices en formato array para la renderizacion posterior
        {
            0f, 0.5f, 0f,  //Vertice de arriba
            -0.5f, -0.5f, 0f, //Vertice de la izquierda 
            0.5f,-0.5f, 0f  //Vertice de la derecha
        
        };
        //Este paso se llama especificacion de vertices el proceso de ¨setupear¨ los objetos necesarios para su renderizacion posterior
        //Para enviar los datos de vertices a renderizar es necesario crear una secuencia de vertices, para posteriormente indicarle a opengl como interpretar  la secuencia
        // 
        int vao; 
        int shaderProgram;
        int vbo;
        //CONSTANTES
        int Anchura, Altura;


        //Cada una de estos metodos son propios de la clase GameWindow, se llaman event handlers
        public Juego(int anchura, int altura): base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            //Centrar la ventana en el monitor         

           this.Anchura = anchura;
            this.Altura = altura;

            CenterWindow(new Vector2i(anchura,altura));
        }
        //Esta funcion se llama cada vez que se le cambia el tamaño a la pantalla
        protected override void OnResize(ResizeEventArgs e)//Esta clase ResizeEventArgs tiene tres propiedades: Width, Height, Size
        {
           
            base.OnResize(e); //Recibe el evento de Resize
            GL.Viewport(0, 0, e.Width, e.Height); //El viewport es la parte de la pantalla en la que se muestran los graficos
            this.Anchura = e.Width; //Las propiedades anchura y altura se les cambia el valor a el valor de las propiedades de "e"
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
            //Almacena los datos en el vbo               //Creo que esta parte del codigo transforma la longitud de los vertices que esta en datos flotantes e enteros
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

            //Aqui se utiliza la funcion que sirve para cargar los shaders
            // add the source code from "Default.vert" in the Shaders file
            GL.ShaderSource(vertexShader, LoadShaderSource("Default.vert"));
            // Compile the Shader
            GL.CompileShader(vertexShader);
            //Aqui se utiliza la funcion que sirve para cargar los shaders
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
            
            
            
          //   pone el color con el que se llenara la pantalla
            GL.ClearColor(0.6f,0.3f,1.0f,1.0f);
          //      //LLena la pantalla con ese color
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
        //Esta es la funcion que se utiliza para llamar a los archivos que estan en /Shaders
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
