using System;
using System.IO;

// Clase que representa un color
public class Color
{
    public byte r;
    public byte g;
    public byte b;
}

// Clase que se encarga de los mapas de bits
public class MapaDeBits
{
    // Propiedades
    private int ancho;
    private int alto;
    private Color[][] colores;

    // Metodo constructor para hacer un nuevo objeto de esta clase
    public MapaDeBits(int ancho, int alto)
    {
        // Guardar propiedades del objeto
        this.ancho = ancho;
        this.alto = alto;

        // Inicializar colores en negro
        colores = new Color[ancho][];
        for (int i = 0; i < ancho; i++)
        {
            colores[i] = new Color[alto];
            for (int j = 0; j < alto; j++)
            {
                colores[i][j] = new Color();
                colores[i][j].r = 0;
                colores[i][j].g = 0;
                colores[i][j].b = 0;
            }
        }
    }

    // Dibujar Linea
    public void DibujarLinea(int x0, int y0, int x1, int y1, Color c)
    {
        // Calcular diferenciales
        int dx = Math.Abs(x1 - x0);
        int dy = -Math.Abs(y1 - y0);

        // Calcular incrementos
        int sx;
        if (x0 < x1)
            sx = 1;
        else
            sx = -1;
        int sy;
        if (y0 < y1)
            sy = 1;
        else
            sy = -1;

        // Calcular error
        int err = dx + dy;
        int e2;

        while (true)
        {
            // Dibujar pixel inicial
            CambiarPixel(x0, y0, c);

            // Decidir siguiente pixel
            e2 = 2 * err;
            if (e2 >= dy)
            {
                // Terminar ciclo si ya se llego al final de la linea
                if (x0 == x1) break;

                // Aumentar
                err += dy;
                x0 += sx;
            }
            if (e2 <= dx)
            {
                // Terminar
                if (y0 == y1) break;

                // Aumentar
                err += dx;
                y0 += sy;
            }
        }
    }

    public void DibujarCirculo(int x, int y, int r, Color c)
    {
        // Numero de puntos
        int puntos = 100;

        // Diferencial del angulo
        double dt = (2 * Math.PI) / puntos;
        double t = 0;

        // punto inicial y final
        int x0;
        int y0;
        int x1;
        int y1;

        for (int i = 0; i < puntos; i++)
        {
            // Punto inicial
            x0 = x + (int)(r * Math.Sin(t));
            y0 = y + (int)(r * Math.Cos(t));

            // Punto final
            x1 = x + (int)(r * Math.Sin(t + dt));
            y1 = y + (int)(r * Math.Cos(t + dt));

            DibujarLinea(x0, y0, x1, y1, c);

            // Incrementar
            t += dt;
        }
    }

    // Cambiar elemento de imagen
    public void CambiarPixel(int x, int y, Color c)
    {
        if (x >= 0 && x <= ancho - 1 && y >= 0 && y <= alto - 1)
            colores[x][y] = c;
    }

    // Ver elemento de imagen
    public Color VerPixel(int x, int y)
    {
        return colores[x][y];
    }

    // Guardar el archivo en un BMP
    public void Guardar(string direccion)
    {
        // Un arreglo de bytes
        byte[] bytes = new byte[122 + ancho * alto * 4];

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
        bytes[6] = (byte)'C';
        bytes[7] = (byte)'U';
        bytes[8] = (byte)'A';
        bytes[9] = (byte)'U';

        // Encabezado DIB
        // Bytes del encabezado DIB
        bytes[14] = 108;

        // Ancho de la imagen en pixeles
        byte[] anch = BitConverter.GetBytes(ancho);
        if (BitConverter.IsLittleEndian)
        {
            bytes[18] = anch[0];
            bytes[19] = anch[1];
            bytes[20] = anch[2];
            bytes[21] = anch[3];
        }
        else
        {
            bytes[18] = anch[3];
            bytes[19] = anch[2];
            bytes[20] = anch[1];
            bytes[21] = anch[0];
        }

        // Alto de la imagen en pixeles
        byte[] alt = BitConverter.GetBytes(alto);
        if (BitConverter.IsLittleEndian)
        {
            bytes[22] = alt[0];
            bytes[23] = alt[1];
            bytes[24] = alt[2];
            bytes[25] = alt[3];
        }
        else
        {
            bytes[22] = alt[3];
            bytes[23] = alt[2];
            bytes[24] = alt[1];
            bytes[25] = alt[0];
        }

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

        // Mascara del alfa
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
        for (int i = 0; i < ancho * alto; i++)
        {
            int y = i / ancho;
            int x = i % ancho;

            // Los pixeles deben guardarse en reversa
            int j = x + (alto - y - 1) * ancho;

            bytes[(j * 4) + 122] = colores[x][y].b;
            bytes[(j * 4) + 123] = colores[x][y].g;
            bytes[(j * 4) + 124] = colores[x][y].r;
            bytes[(j * 4) + 125] = 255;
        }

        // Escribir el archivo
        File.WriteAllBytes(direccion, bytes);
    }
}
