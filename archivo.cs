using System;
using System.IO;
using UnityEngine;

public class archivo : MonoBehaviour
{
    // Estructura del archivo JSON
    public class Info
    {
        public int ancho;
        public int alto;
        public int[] colores;
    }

    // Use this for initialization
    void Start()
    {
        // Leer archivo JSON
        string json = "";
        StreamReader reader = new StreamReader("C:/Users/Oscar/Desktop/clase/pika.json");
        json = reader.ReadToEnd();
        reader.Close();
        Info info = JsonUtility.FromJson<Info>(json);

        // Llenar archivo de ceros
        byte[] bytes = new byte[122 + (4 * info.ancho * info.alto)];
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = 0;
        }

        // Encabezado BMP
        // Identificador del tipo de archivo
        bytes[0] = (byte)'B';
        bytes[1] = (byte)'M';

        // Byte en donde empiezan los pixeles
        bytes[10] = 122;

        // Bytes en el archivo
        byte[] tam = BitConverter.GetBytes(bytes.Length);
        if (BitConverter.IsLittleEndian)
        {
            bytes[2] = tam[0];
            bytes[3] = tam[1];
            bytes[4] = tam[2];
            bytes[5] = tam[3];
        }
        else
        {
            bytes[5] = tam[0];
            bytes[4] = tam[1];
            bytes[3] = tam[2];
            bytes[2] = tam[3];
        }

        // Lo que sea, para identificar la aplicacion
        bytes[6] = (byte)'1';
        bytes[7] = (byte)'3';
        bytes[8] = (byte)'3';
        bytes[9] = (byte)'7';

        // Encabezado DIB
        // Bytes del encabezado DIB
        bytes[14] = 108;

        // Ancho de la imagen en pixeles
        bytes[18] = (byte)info.ancho;

        // Alto de la imagen en pixeles
        bytes[22] = (byte)info.alto;

        // Numero de planos de color
        bytes[26] = 1;

        // Bits por pixel
        bytes[28] = 32;

        // Numero de canales de color
        bytes[30] = 3;

        // Bytes de la informacion de color
        bytes[34] = 32;

        // Pixeles por metro horizontales (resolucion)
        byte[] res = BitConverter.GetBytes(2835);
        if (BitConverter.IsLittleEndian)
        {
            bytes[38] = res[0];
            bytes[39] = res[1];
            bytes[40] = res[2];
            bytes[41] = res[3];
        }
        else
        {
            bytes[41] = res[0];
            bytes[40] = res[1];
            bytes[39] = res[2];
            bytes[38] = res[3];
        }

        // Pixeles por metro verticales (resolucion)
        if (BitConverter.IsLittleEndian)
        {
            bytes[42] = res[0];
            bytes[43] = res[1];
            bytes[44] = res[2];
            bytes[45] = res[3];
        }
        else
        {
            bytes[45] = res[0];
            bytes[44] = res[1];
            bytes[43] = res[2];
            bytes[42] = res[3];
        }

        // Mascara del rojo
        bytes[54] = 0;
        bytes[55] = 0;
        bytes[56] = 255;
        bytes[57] = 0;

        // Mascara del verde
        bytes[58] = 0;
        bytes[59] = 255;
        bytes[60] = 0;
        bytes[61] = 0;

        // Mascara del azul
        bytes[62] = 255;
        bytes[63] = 0;
        bytes[64] = 0;
        bytes[65] = 0;

        // Mascara del alpha
        bytes[66] = 0;
        bytes[67] = 0;
        bytes[68] = 0;
        bytes[69] = 255;

        // Espacio de colores
        bytes[73] = (byte)'W';
        bytes[72] = (byte)'i';
        bytes[71] = (byte)'n';
        bytes[70] = (byte)' ';

        // Datos de color
        for (int i = 0; i < info.ancho * info.alto; i++)
        {
            int y = i / info.ancho;
            int x = i % info.ancho;

            int j = x + (info.ancho - y - 1) * info.ancho;

            bytes[(j * 4) + 122] = (byte)info.colores[i * 3 + 2];
            bytes[(j * 4) + 122 + 1] = (byte)info.colores[i * 3 + 1];
            bytes[(j * 4) + 122 + 2] = (byte)info.colores[i * 3];
            bytes[(j * 4) + 122 + 3] = 255;
        }

        // Escribir el archivo
        File.WriteAllBytes("C:/Users/Oscar/Desktop/clase/prueba.bmp", bytes);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
