namespace RiftGL.Objects
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using JSIL;

    using RiftGL.View;

    public class HeightMap
    {
        public float[] Values { get; set; }
    }

    public class TerrainData
    {
        public float[] Vertices;

        public float[] TextureCoords;
        
        public float[] Normals;

        public ushort[] Indices;
    }

    public class Terrain : GlObject
    {
        public BufferCollection Buffers = new BufferCollection();

        public TerrainData TerrainData = new TerrainData();

        public HeightMap HeightMap { get; set; }

        public float[] fogColor;

        public float GetWidth() { return Width; }

        public float GetMul() { return terrainMul; }

        public float GetScanDepth() { return scanDepth; }

     	public float GetHeight(double x, double z)
	    {
     	    if (x < Position.X) x = Position.X;
            if (z < Position.Z) z = Position.Z;

		    // divide by the grid-spacing if it is not 1
		    float projCameraX = (float)x - this.Position.X;
		    float projCameraZ = (float)z - this.Position.Z;

		    // compute the height field coordinates (Col0, Row0)
		    // and (Col1, Row1) that identify the height field cell 
		    // directly below the camera.
		    int col0 = (int)(projCameraX);
		    int row0 = (int)(projCameraZ);
		    int col1 = col0 + 1;
		    int row1 = row0 + 1;
		
		    // make sure that the cell coordinates don't fall
		    // outside the height field.
		    if (col1 > Width)
			    col1 = 0;
		    if (row1 > Width)
			    row1 = 0;

		    // get the four corner heights of the cell from the height field
		    float h00 = heightMul * HeightMap.Values[col0 + row0*Width];
		    float h01 = heightMul * HeightMap.Values[col1 + row0*Width];
		    float h11 = heightMul * HeightMap.Values[col1 + row1*Width];
		    float h10 = heightMul * HeightMap.Values[col0 + row1*Width];

		    // calculate the position of the camera relative to the cell.
		    // note, that 0 <= tx, ty <= 1.
		    float tx = projCameraX - col0;
		    float ty = projCameraZ - row0;

		    // the next step is to perform a bilinear interpolation
		    // to compute the height of the terrain directly below
		    // the object.
		    float txty = tx * ty;

		    float final_height = h00 * (1.0f - ty - tx + txty)
						    + h01 * (tx - txty)
						    + h11 * txty
						    + h10 * (ty - txty);

		    return final_height;
	    }


        public void BuildTerrain(int seed, int w, float rFactor, Camera camera)
        {
            Random = new Random(seed);

            Width = w;

            scanDepth = 80.0f;
            terrainMul = 1.0f;
            textureMul = 0.25f;
            heightMul = 10.0f;

            fogColor = new float[4];
            fogColor[0] = 0.75f;
            fogColor[1] = 0.9f;
            fogColor[2] = 1.0f;
            fogColor[3] = 1.0f;

            HeightMap = null;

            Size = (float)Math.Sqrt(Width * terrainMul * Width * terrainMul + Width * terrainMul * Width * terrainMul);

            HeightMap = new HeightMap
                            {
                                Values = new float[Width * Width]
                            };

            MakeTerrainPlasma(HeightMap, Width, rFactor);
            if (camera != null)
            {
                this.InitShaders(camera);
                this.InitTexture(camera);
                this.CreateVertesArray();
                this.CreateIndices();

                this.InitBuffers(camera);
            }
        }

        #region Implementation

        private Random Random { get; set; }

        public int Width { get; set; }

        private float terrainMul { get; set; }

        private float heightMul { get; set; }

        private float scanDepth { get; set; }

        private float textureMul { get; set; }

        private object TerrainTexture;

        private void InitTexture(Camera camera)
        {
            TerrainTexture = camera.GL.createTexture();

            var imageElement = ViewPort.Document.createElement("img");
            imageElement.onload = (Action)(
                () => camera.UploadTexture(TerrainTexture, imageElement)
            );

            try
            {
                var imageBytes = File.ReadAllBytes("ground.png");
                var objectUrl = Builtins.Global["JSIL"].GetObjectURLForBytes(imageBytes, "image/png");
                imageElement.src = objectUrl;
            }
            catch
            {
                // Object URLs probably aren't supported. Load the image a second time. ;/
                Console.WriteLine("Falling back to a second HTTP request for crate.png because Object URLs are not available");
                imageElement.src = "Files/ground.png";
            }
        }

        public void InitBuffers(Camera camera)
        {
            var gl = camera.GL;

            if (this.Buffers.VertexPositions != null)
            {
                gl.deleteBuffer(this.Buffers.VertexPositions);
                gl.deleteBuffer(this.Buffers.VertexNormals);
                gl.deleteBuffer(this.Buffers.TextureCoords);
            }

            //if (this.Buffers.VertexPositions == null)
            {
                this.Buffers.VertexPositions = gl.createBuffer();
                this.Buffers.VertexNormals = camera.GL.createBuffer();
                this.Buffers.TextureCoords = camera.GL.createBuffer();
            }

            gl.bindBuffer(gl.ARRAY_BUFFER, this.Buffers.VertexPositions);
            gl.bufferData(gl.ARRAY_BUFFER, this.TerrainData.Vertices, gl.STATIC_DRAW);

            gl.bindBuffer(camera.GL.ARRAY_BUFFER, this.Buffers.VertexNormals);
            gl.bufferData(camera.GL.ARRAY_BUFFER, TerrainData.Normals, camera.GL.STATIC_DRAW);

            gl.bindBuffer(camera.GL.ARRAY_BUFFER, this.Buffers.TextureCoords);
            gl.bufferData(camera.GL.ARRAY_BUFFER, TerrainData.TextureCoords, camera.GL.STATIC_DRAW);

            this.Buffers.Indices = gl.createBuffer();
            gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, this.Buffers.Indices);
            gl.bufferData(gl.ELEMENT_ARRAY_BUFFER, this.TerrainData.Indices, gl.STATIC_DRAW);
        }

        public void CreateIndices()
        {
            var indices = new List<ushort>();

            var lineTriangles = new List<ushort>();
            //{ 4, 0, 5,  0, 5, 1,  5, 1, 6, 1, 6, 2, 2, 6, 3, 3, 7, 6 };

            for (int i = 0; i < Width - 1; i++)
            {
                lineTriangles.AddRange(new[] { (ushort)(Width + i), (ushort)i, (ushort)(Width + i + 1) });
                lineTriangles.AddRange(new[] { (ushort)i, (ushort)(Width + i + 1), (ushort)(i + 1) });
            }

            for (int i = 0; i < this.Width - 1; i++)
            {
                foreach (var index in lineTriangles)
                {
                    indices.Add((ushort)(index + i * this.Width));
                }
            }

            this.TerrainData.Indices = indices.ToArray();
        }

        public void CreateVertesArray()
        {
            var vertexBuffer = new List<Vector>();
            var textureBuffer = new List<Vector>();
            float textureU = this.Width *0.1f;
            float textureV = this.Width *0.1f;

            for (int i = 0; i < this.Width; i++)
            {
                for (int j = 0; j < this.Width; j++)
                {
                    float scaleR = ((j + Position.X) % this.Width / ((float)this.Width - 1));
                    float scaleC = ((i + Position.Z) % this.Width / ((float)this.Width - 1));

                    float x = -this.Width / 2.0f + scaleR * this.Width;
                    float y = this.HeightMap.Values[i * this.Width + j] * heightMul;
                    float z = -this.Width / 2.0f + scaleC * this.Width;

                    vertexBuffer.Add(new Vector(x, y, z));

                    float u = (textureU * scaleR );
                    float v = (textureV * scaleC );
                    textureBuffer.Add(new Vector(u, v, 0));
                }
            }

            var normals = new List<Vector>();

            for (int i = 0; i < this.Width - 1; i++)
            {
                for (int j = 0; j < this.Width - 1; j++)
                {
                    Vector t00 = vertexBuffer[i * this.Width + j];
                    Vector t01 = vertexBuffer[(i + 1) * this.Width + j];
                    Vector t02 = vertexBuffer[(i + 1) * this.Width + (j + 1)];

                    Vector t10 = vertexBuffer[(i + 1) * this.Width + (j + 1)];
                    Vector t11 = vertexBuffer[i * this.Width + (j + 1)];
                    Vector t12 = vertexBuffer[i * this.Width + j];

                    Vector norm0 = (t00 - t01) ^ (t01 - t02);
                    Vector norm1 = (t10 - t11) ^ (t11 - t12);
                    //norm1.Y = -1;
                    normals.AddRange(new[] { norm0, norm1 });
                }
            }

            var finalNormals = new List<Vector>();

            for (int i = 0; i < this.Width; i++)
            {
                for (int j = 0; j < this.Width; j++)
                {
                    Vector finalNormal = new Vector();

                    if (j != 0 && i != 0)
                    {
                        var vector1 = normals[((i - 1)) + ((j - 1) * this.Width)];
                        var vector2 = normals[((i - 1)) + ((j - 1) * this.Width) + 1];

                        finalNormal += vector1 + vector2;
                    }

                    if (i != 0 && j != this.Width - 1)
                    {
                        var vector = normals[((i - 1) * 3) + ((j) * this.Width)];
                        finalNormal += vector;
                    }

                    if (i != this.Width - 1 && j != this.Width - 1)
                    {
                        var vector1 = normals[((i) * 3) + ((j) * this.Width)];
                        var vector2 = normals[((i) * 3) + ((j) * this.Width) + 1];
                        finalNormal += vector1 + vector2;
                    }

                    if (i != this.Width - 1 && j != 0)
                    {
                        var vector = normals[((i) * 3) + ((j - 1) * this.Width)];
                        finalNormal += vector;
                    }

                    finalNormal.Normalize();
                    finalNormals.Add(finalNormal);
                }
            }

            var vertexDataBuffer = new List<float>();
            var normalsDataBuffer = new List<float>();
            var textureDataBuffer = new List<float>();

            for (int i = 0; i < this.Width; i++)
            {
                for (int j = 0; j < this.Width; j++)
                {
                    var vertex = vertexBuffer[j + i * this.Width];
                    var texture = textureBuffer[j + i * this.Width];
                    var normal = finalNormals[j + i * this.Width];
                    textureDataBuffer.AddRange(new[] { texture.X, texture.Y });
                    normalsDataBuffer.AddRange(new[] { normal.X, normal.Y, normal.Z });
                    vertexDataBuffer.AddRange(new[] { vertex.X, vertex.Y, vertex.Z });
                        // , normal.X, normal.Y, normal.Z, texture.X, texture.Y
                }
            }

            this.TerrainData.Vertices = vertexDataBuffer.ToArray();
            this.TerrainData.Normals = normalsDataBuffer.ToArray();
            this.TerrainData.TextureCoords = textureDataBuffer.ToArray();
        }

        public static dynamic TerrainShaderProgram;

        public void InitShaders(Camera viewPort)
        {
            var fragmentShader = Crate.CompileShader(viewPort, "crate.fs");
            var vertexShader = Crate.CompileShader(viewPort, "crate.vs");

            TerrainShaderProgram = viewPort.GL.createProgram();
            viewPort.GL.attachShader(TerrainShaderProgram, vertexShader);
            viewPort.GL.attachShader(TerrainShaderProgram, fragmentShader);
            viewPort.GL.linkProgram(TerrainShaderProgram);

            bool linkStatus = viewPort.GL.getProgramParameter(TerrainShaderProgram, viewPort.GL.LINK_STATUS);
            if (!linkStatus)
            {
                Builtins.Global["alert"]("Could not link shader");
                return;
            }

            viewPort.GL.useProgram(TerrainShaderProgram);

            TerrainShaderProgram.VertexPosition = viewPort.GL.getAttribLocation(TerrainShaderProgram, "aVertexPosition");
            viewPort.GL.enableVertexAttribArray(TerrainShaderProgram.VertexPosition);

            TerrainShaderProgram.VertexNormal = viewPort.GL.getAttribLocation(TerrainShaderProgram, "aVertexNormal");
            viewPort.GL.enableVertexAttribArray(TerrainShaderProgram.VertexNormal);

            TerrainShaderProgram.TextureCoord = viewPort.GL.getAttribLocation(TerrainShaderProgram, "aTextureCoord");
            viewPort.GL.enableVertexAttribArray(TerrainShaderProgram.TextureCoord);

            ViewPort.Uniforms.ProjectionMatrix = viewPort.GL.getUniformLocation(TerrainShaderProgram, "uPMatrix");
            ViewPort.Uniforms.ModelViewMatrix = viewPort.GL.getUniformLocation(TerrainShaderProgram, "uMVMatrix");
            ViewPort.Uniforms.NormalMatrix = viewPort.GL.getUniformLocation(TerrainShaderProgram, "uNMatrix");

            ViewPort.Uniforms.Sampler = viewPort.GL.getUniformLocation(TerrainShaderProgram, "uSampler");
            ViewPort.Uniforms.UseLighting = viewPort.GL.getUniformLocation(TerrainShaderProgram, "uUseLighting");
            ViewPort.Uniforms.AmbientColor = viewPort.GL.getUniformLocation(TerrainShaderProgram, "uAmbientColor");
            ViewPort.Uniforms.LightingDirection = viewPort.GL.getUniformLocation(TerrainShaderProgram, "uLightingDirection");
            ViewPort.Uniforms.DirectionalColor = viewPort.GL.getUniformLocation(TerrainShaderProgram, "uDirectionalColor");
        }

        public static float DegreesToRadians(float degrees)
        {
            return (float)(degrees * Math.PI / 180);
        }

        protected override void OnDraw(Camera camera)
        {
            var offsetX = Position.X;
            var offsetZ = Position.Z;
            var matrix = ViewPort.GLMatrix4.create();
            ViewPort.GLMatrix4.identity(matrix);
            matrix = ViewPort.GLMatrix4.translate(matrix, Position.ToFloatVector());
            ViewPort.Matrices.ModelView = ViewPort.GLMatrix4.multiply(
                ViewPort.Matrices.ModelView, matrix, ViewPort.Matrices.ModelView);

            camera.GL.bindBuffer(camera.GL.ARRAY_BUFFER, this.Buffers.VertexPositions);
            camera.GL.vertexAttribPointer(TerrainShaderProgram.VertexPosition, 3, camera.GL.FLOAT, false, 0, 0);

            camera.GL.bindBuffer(camera.GL.ARRAY_BUFFER, this.Buffers.VertexNormals);
            camera.GL.vertexAttribPointer(TerrainShaderProgram.VertexNormal, 3, camera.GL.FLOAT, false, 0, 0);

            camera.GL.bindBuffer(camera.GL.ARRAY_BUFFER, this.Buffers.TextureCoords);
            camera.GL.vertexAttribPointer(TerrainShaderProgram.TextureCoord, 2, camera.GL.FLOAT, false, 0, 0);

            camera.GL.activeTexture(camera.GL.TEXTURE0);
            camera.GL.bindTexture(camera.GL.TEXTURE_2D, TerrainTexture);
            camera.GL.uniform1i(ViewPort.Uniforms.Sampler, 0);

            camera.GL.uniformMatrix4fv(ViewPort.Uniforms.ProjectionMatrix, false, ViewPort.Matrices.Projection);
            camera.GL.uniformMatrix4fv(ViewPort.Uniforms.ModelViewMatrix, false, ViewPort.Matrices.ModelView);

            ViewPort.GLMatrix4.toInverseMat3(ViewPort.Matrices.ModelView, ViewPort.Matrices.Normal);
            ViewPort.GLMatrix3.transpose(ViewPort.Matrices.Normal);
            camera.GL.uniformMatrix3fv(ViewPort.Uniforms.NormalMatrix, false, ViewPort.Matrices.Normal);

            camera.GL.bindBuffer(camera.GL.ELEMENT_ARRAY_BUFFER, this.Buffers.Indices);
            camera.GL.drawElements(camera.GL.TRIANGLES, this.TerrainData.Indices.Length, camera.GL.UNSIGNED_SHORT, 0);
            //camera.GL.drawArrays(camera.GL.TRIANGLE_STRIP, 0, this.TerrainData.Indices.Length);
        }

        private float RangedRandom(float v1, float v2)
        {
            return v1 + (v2 - v1) * (float)Random.NextDouble();
        }

        private void NormalizeTerrain(HeightMap field,int size)
        {
            int i;

            /*
            Find the maximum and minimum values in the height field
            */
            float maxVal = field.Values[0];
            float minVal = field.Values[0];

            for (i = 1; i < size * size; i++)
            {
                if (field.Values[i] > maxVal)
                {
                    maxVal = field.Values[i];
                }
                else if (field.Values[i] < minVal)
                {
                    minVal = field.Values[i];
                }
            }

            /*
            Find the altitude range (dh)
            */
            if (maxVal <= minVal) return;
            float dh = maxVal - minVal;

            /*
            Scale all the values so they are in the range 0-1
            */
            for (i = 0; i < size * size; i++)
            {
                field.Values[i] = (field.Values[i] - minVal) / dh;
            }
        }

        //private void FilterHeightBand(float[] band, int start, int stride, int count, float filter)
        //{
        //    int j = stride;
        //    float v = band[start];
        //    for (int i = 0; i < count - 1; i++)
        //    {
        //        band[start + j] = filter * v + (1 - filter) * band[start + j];
        //        v = band[start + j];
        //        j += stride;
        //    }
        //}

        //private void FilterHeightField(float[] field, int size, float filter)
        //{
        //    int i;

        //    // Erode rows left to right
        //    for (i = 0; i < size; i++)
        //    {
        //        FilterHeightBand(field, size * i, 1, size, filter);
        //    }

        //    // Erode rows right to left
        //    for (i = 0; i < size; i++)
        //    {
        //        FilterHeightBand(field, size * i + size - 1, -1, size, filter);
        //    }

        //    // Erode columns top to bottom
        //    for (i = 0; i < size; i++)
        //    {
        //        FilterHeightBand(field, i, size, size, filter);
        //    }

        //    // Erode columns bottom to top
        //    for (i = 0; i < size; i++)
        //    {
        //        FilterHeightBand(field, size * (size - 1) + i, -size, size, filter);
        //    }
        //}

        private void MakeTerrainPlasma(HeightMap field, int size, float rough)
        {
            int i, j, ni, nj, mi, mj, pmi, pmj, rectSize = size;
            float dh = (float)rectSize / 2, r = (float)Math.Pow(2, -1 * rough);

            //	Since the terrain wraps, all 4 "corners" are represented by the value at 0,0,
            //		so seeding the heightfield is very straightforward
            //	Note that it doesn't matter what we use for a seed value, since we're going to
            //		renormalize the terrain after we're done
            field.Values[0] = 0;


            while (rectSize > 0)
            {

                /*
                Diamond step -

                Find the values at the center of the retangles by averaging the values at 
                the corners and adding a random offset:


                a.....b
                .     .  
                .  e  .
                .     .
                c.....d   

                e  = (a+b+c+d)/4 + random

                In the code below:
                a = (i,j)
                b = (ni,j)
                c = (i,nj)
                d = (ni,nj)
                e = (mi,mj)

                */

                for (i = 0; i < size; i += rectSize)
                    for (j = 0; j < size; j += rectSize)
                    {

                        ni = (i + rectSize) % size;
                        nj = (j + rectSize) % size;

                        mi = (i + rectSize / 2);
                        mj = (j + rectSize / 2);

                        field.Values[mi + mj * size] = (field.Values[i + j * size] + field.Values[ni + j * size] + field.Values[i + nj * size]
                                                 + field.Values[ni + nj * size]) / 4 + RangedRandom(-dh / 2, dh / 2);
                    }

                /*
                Square step -

                Find the values on the left and top sides of each rectangle
                The right and bottom sides are the left and top sides of the neighboring rectangles,
                  so we don't need to calculate them

                The height field wraps, so we're never left hanging.  The right side of the last
                    rectangle in a row is the left side of the first rectangle in the row.  The bottom
                    side of the last rectangle in a column is the top side of the first rectangle in
                    the column

                      .......
                      .     .
                      .     .
                      .  d  .
                      .     .
                      .     .
                ......a..g..b
                .     .     .
                .     .     .
                .  e  h  f  .
                .     .     .
                .     .     .
                ......c......

                g = (d+f+a+b)/4 + random
                h = (a+c+e+f)/4 + random
		
                In the code below:
                    a = (i,j) 
                    b = (ni,j) 
                    c = (i,nj) 
                    d = (mi,pmj) 
                    e = (pmi,mj) 
                    f = (mi,mj) 
                    g = (mi,j)
                    h = (i,mj)

                */
                for (i = 0; i < size; i += rectSize)
                    for (j = 0; j < size; j += rectSize)
                    {

                        ni = (i + rectSize) % size;
                        nj = (j + rectSize) % size;

                        mi = (i + rectSize / 2);
                        mj = (j + rectSize / 2);

                        pmi = (i - rectSize / 2 + size) % size;
                        pmj = (j - rectSize / 2 + size) % size;

                        // Calculate the square value for the top side of the rectangle
                        field.Values[mi + j * size] = (field.Values[i + j * size] + field.Values[ni + j * size] + field.Values[mi + pmj * size]
                                                + field.Values[mi + mj * size]) / 4 + RangedRandom(-dh / 2, dh / 2);

                        // Calculate the square value for the left side of the rectangle
                        field.Values[i + mj * size] = (field.Values[i + j * size] + field.Values[i + nj * size] + field.Values[pmi + mj * size]
                                                + field.Values[mi + mj * size]) / 4 + RangedRandom(-dh / 2, dh / 2);

                    }

                // Setup values for next iteration
                // At this point, the height field has valid values at each of the coordinates that fall on a rectSize/2 boundary

                rectSize /= 2;
                dh *= r;
            }

            // Normalize terrain so minimum value is 0 and maximum value is 1
            NormalizeTerrain(field, size);
        }

        #endregion
    }
}
