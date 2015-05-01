using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ProjectDeepSpace
{
    public class HexManager : Microsoft.Xna.Framework.DrawableGameComponent, IHexManagerService
    {
        #region Variables, Getters and Setters

        private ICursorManagerService cursorManagerService;
        private ICameraManagerService cameraManagerService;

        private ContentManager content;
        private GameBoard myGameBoard;

        private Effect effect;
        private Texture2D hex;

        private MyOwnVertexFormat[] vertexArray;
        private int[] indexArray;
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;

        private Vector2 targetHex;
        private Vector2 oldTargetHex;
        private Vector2? pickedHex;

        public Vector2 TargetHex
        {
            get { return targetHex; }
        }

        public Vector2 OldTargetHex
        {
            get { return oldTargetHex; }
        }

        public Vector2? PickedHex
        {
            get { return pickedHex; }
            set { pickedHex = value; }
        }

        #endregion Variables, Getters and Setters

        public HexManager(Game game, ContentManager content, ref GameBoard myGameBoard)
            : base(game)
        {
            this.myGameBoard = myGameBoard;
            this.content = content;
        }

        public override void Initialize()
        {
            LoadServices();

            targetHex = new Vector2(0, 0);

            indexBuffer = new IndexBuffer(Game.GraphicsDevice, typeof(int), (18 * myGameBoard.X * myGameBoard.Y), BufferUsage.WriteOnly);
            vertexBuffer = new VertexBuffer(Game.GraphicsDevice, MyOwnVertexFormat.VertexDeclaration, (7 * myGameBoard.X * myGameBoard.Y), BufferUsage.WriteOnly);

            UpdateBuffers();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            effect = content.Load<Effect>("EffectsAndFonts/MyEffects");
            hex = content.Load<Texture2D>("GameBoard/Hex");
            
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            UpdateOldTargetHex();
            UpdateTargetHex();
            UpdatePickedHex();
            UpdateBuffers();
  
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            RasterizerState stat = new RasterizerState();
            stat.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = stat;
            
            DrawBoard();

            base.Draw(gameTime);
        }

        private void DrawBoard()
        {            
            effect.CurrentTechnique = effect.Techniques["Simplest"];
            effect.Parameters["xViewProjection"].SetValue(cameraManagerService.ViewMatrix * cameraManagerService.ProjectionMatrix);
            effect.Parameters["xTexture"].SetValue(hex);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                Game.GraphicsDevice.Indices = indexBuffer;
                Game.GraphicsDevice.SetVertexBuffer(vertexBuffer);
                Game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, (7 * myGameBoard.X * myGameBoard.Y), 0, (6 * myGameBoard.X * myGameBoard.Y));
            }
        } //DONE

        private void UpdatePickedHex()
        {
            if (cursorManagerService.IsLMBClicked() == true)
            {
                pickedHex = targetHex;
            }
        } //DONE

        private void UpdateTargetHex()
        {
            Ray cursorRay = cursorManagerService.CalculateCursorRay(cameraManagerService.ProjectionMatrix, cameraManagerService.ViewMatrix);
            float? closestIntersection = null;

            for (int i = 0; i < vertexArray.Length; i += 7)
            {
                #region triangle 1

                    // Perform a ray to triangle intersection test.
                    float? intersection;

                    RayIntersectsTriangle(ref cursorRay,
                                          ref vertexArray[i + 0].position,
                                          ref vertexArray[i + 1].position,
                                          ref vertexArray[i + 6].position,
                                          out intersection);

                    // Does the ray intersect this triangle?
                    if (intersection != null)
                    {
                        // If so, is it closer than any other previous triangle?
                        if ((closestIntersection == null) ||
                            (intersection < closestIntersection))
                        {
                            // Store the distance to this triangle.
                            closestIntersection = intersection;
                            // Store Hex position.
                            int x;
                            int y;

                            if (i == 0)
                            {
                                x = 0;
                                y = 0;
                            }
                            else
                            {
                                y = (i / 7) % myGameBoard.Y;
                                x = ((i / 7) - y) / myGameBoard.Y;
                            }

                            targetHex = new Vector2(x, y);
                        }
                    }
                    #endregion
                #region triangle 2

                    // Perform a ray to triangle intersection test.
                    intersection = null;

                    RayIntersectsTriangle(ref cursorRay,
                                          ref vertexArray[i + 6].position,
                                          ref vertexArray[i + 1].position,
                                          ref vertexArray[i + 3].position,
                                          out intersection);

                    // Does the ray intersect this triangle?
                    if (intersection != null)
                    {
                        // If so, is it closer than any other previous triangle?
                        if ((closestIntersection == null) ||
                            (intersection < closestIntersection))
                        {
                            // Store the distance to this triangle.
                            closestIntersection = intersection;
                            // Store Hex position.
                            int x;
                            int y;

                            if (i == 0)
                            {
                                x = 0;
                                y = 0;
                            }
                            else
                            {
                                y = (i / 7) % myGameBoard.Y;
                                x = ((i / 7) - y) / myGameBoard.Y;
                            }

                            targetHex = new Vector2(x, y);
                        }
                    }
                    #endregion
                #region triangle 3

                    // Perform a ray to triangle intersection test.
                    intersection = null;

                    RayIntersectsTriangle(ref cursorRay,
                                          ref vertexArray[i + 6].position,
                                          ref vertexArray[i + 3].position,
                                          ref vertexArray[i + 5].position,
                                          out intersection);

                    // Does the ray intersect this triangle?
                    if (intersection != null)
                    {
                        // If so, is it closer than any other previous triangle?
                        if ((closestIntersection == null) ||
                            (intersection < closestIntersection))
                        {
                            // Store the distance to this triangle.
                            closestIntersection = intersection;
                            // Store Hex position.
                            int x;
                            int y;

                            if (i == 0)
                            {
                                x = 0;
                                y = 0;
                            }
                            else
                            {
                                y = (i / 7) % myGameBoard.Y;
                                x = ((i / 7) - y) / myGameBoard.Y;
                            }

                            targetHex = new Vector2(x, y);
                        }
                    }
                    #endregion
                #region triangle 4

                    // Perform a ray to triangle intersection test.
                    intersection = null;

                    RayIntersectsTriangle(ref cursorRay,
                                          ref vertexArray[i + 4].position,
                                          ref vertexArray[i + 6].position,
                                          ref vertexArray[i + 5].position,
                                          out intersection);

                    // Does the ray intersect this triangle?
                    if (intersection != null)
                    {
                        // If so, is it closer than any other previous triangle?
                        if ((closestIntersection == null) ||
                            (intersection < closestIntersection))
                        {
                            // Store the distance to this triangle.
                            closestIntersection = intersection;
                            // Store Hex position.
                            int x;
                            int y;

                            if (i == 0)
                            {
                                x = 0;
                                y = 0;
                            }
                            else
                            {
                                y = (i / 7) % myGameBoard.Y;
                                x = ((i / 7) - y) / myGameBoard.Y;
                            }

                            targetHex = new Vector2(x, y);
                        }
                    }
                    #endregion
                #region triangle 5

                    // Perform a ray to triangle intersection test.
                    intersection = null;

                    RayIntersectsTriangle(ref cursorRay,
                                          ref vertexArray[i + 2].position,
                                          ref vertexArray[i + 6].position,
                                          ref vertexArray[i + 4].position,
                                          out intersection);

                    // Does the ray intersect this triangle?
                    if (intersection != null)
                    {
                        // If so, is it closer than any other previous triangle?
                        if ((closestIntersection == null) ||
                            (intersection < closestIntersection))
                        {
                            // Store the distance to this triangle.
                            closestIntersection = intersection;
                            // Store Hex position.
                            int x;
                            int y;

                            if (i == 0)
                            {
                                x = 0;
                                y = 0;
                            }
                            else
                            {
                                y = (i / 7) % myGameBoard.Y;
                                x = ((i / 7) - y) / myGameBoard.Y;
                            }

                            targetHex = new Vector2(x, y);
                        }
                    }
                    #endregion
                #region triangle 6

                    // Perform a ray to triangle intersection test.
                    intersection = null;

                    RayIntersectsTriangle(ref cursorRay,
                                          ref vertexArray[i + 2].position,
                                          ref vertexArray[i + 0].position,
                                          ref vertexArray[i + 6].position,
                                          out intersection);

                    // Does the ray intersect this triangle?
                    if (intersection != null)
                    {
                        // If so, is it closer than any other previous triangle?
                        if ((closestIntersection == null) ||
                            (intersection < closestIntersection))
                        {
                            // Store the distance to this triangle.
                            closestIntersection = intersection;
                            // Store Hex position.
                            int x;
                            int y;

                            if (i == 0)
                            {
                                x = 0;
                                y = 0;
                            }
                            else
                            {
                                y = (i / 7) % myGameBoard.Y;
                                x = ((i / 7) - y) / myGameBoard.Y;
                            }

                            targetHex = new Vector2(x, y);
                        }
                    }
                    #endregion
            }
        } //DONE

        private void UpdateOldTargetHex()
        {
            oldTargetHex = targetHex;
        }

        private void UpdateBuffers()
        {
            vertexArray = GetVertexArray(pickedHex, targetHex, myGameBoard.MovePathInRange, myGameBoard.MovePathOutOfRange, myGameBoard.AttackTargets);
            indexArray = GetIndexArray();
            indexBuffer.SetData(indexArray);
            vertexBuffer.SetData(vertexArray);
        } //DONE

        private int[] GetIndexArray()
        {
            int[] tempIndexArray = new int[18 * myGameBoard.X * myGameBoard.Y];

            int vCounter = 0;
            int iCounter = 0;

            for (int x = 0; x < myGameBoard.X; x++)
            {
                for (int y = 0; y < myGameBoard.Y; y++)
                {
                    tempIndexArray[iCounter] = vCounter + 0;
                    tempIndexArray[iCounter + 1] = vCounter + 1;
                    tempIndexArray[iCounter + 2] = vCounter + 6;

                    tempIndexArray[iCounter + 3] = vCounter + 6;
                    tempIndexArray[iCounter + 4] = vCounter + 1;
                    tempIndexArray[iCounter + 5] = vCounter + 3;

                    tempIndexArray[iCounter + 6] = vCounter + 6;
                    tempIndexArray[iCounter + 7] = vCounter + 3;
                    tempIndexArray[iCounter + 8] = vCounter + 5;

                    tempIndexArray[iCounter + 9] = vCounter + 4;
                    tempIndexArray[iCounter + 10] = vCounter + 6;
                    tempIndexArray[iCounter + 11] = vCounter + 5;

                    tempIndexArray[iCounter + 12] = vCounter + 2;
                    tempIndexArray[iCounter + 13] = vCounter + 6;
                    tempIndexArray[iCounter + 14] = vCounter + 4;

                    tempIndexArray[iCounter + 15] = vCounter + 2;
                    tempIndexArray[iCounter + 16] = vCounter + 0;
                    tempIndexArray[iCounter + 17] = vCounter + 6;

                    vCounter = vCounter + 7;
                    iCounter = iCounter + 18;
                }
            }
            return tempIndexArray;
        } //DONE

        private MyOwnVertexFormat[] GetVertexArray(Vector2? picked, Vector2 target, List<Vector2> moveInRange, List<Vector2> moveOutOfRange, List<Vector2> attackTargets)
        {
            MyOwnVertexFormat[] tempVertexArray = new MyOwnVertexFormat[7 * myGameBoard.X * myGameBoard.Y];

            int vCounter = 0;
            int iCounter = 0;

            float textureOffest = 0f;

            for (int x = 0; x < myGameBoard.X; x++)
            {
                for (int y = 0; y < myGameBoard.Y; y++)
                {
                    if (picked == new Vector2(x, y))
                    {
                        textureOffest = 0.4f;
                    }
                    else if (moveInRange != null && moveInRange.Contains(new Vector2(x, y)))
                    {
                        textureOffest = 0.6f;
                    }
                    else if (moveOutOfRange != null && moveOutOfRange.Contains(new Vector2(x, y)))
                    {
                        textureOffest = 0.8f;
                    }
                    else if (attackTargets != null && attackTargets.Contains(new Vector2(x, y)))
                    {
                        textureOffest = 0.8f;
                    }
                    else if (target == new Vector2(x, y))
                    {
                        textureOffest = 0.2f;
                    }
                    else
                    {
                        textureOffest = 0f;
                    }

                    //Top Left
                    tempVertexArray[vCounter + 0] = new MyOwnVertexFormat((Vector3)myGameBoard.GetHex(x, y).TopLeft, new Vector2((textureOffest + 0.05f), 0.0005f));
                    //Top Right
                    tempVertexArray[vCounter + 1] = new MyOwnVertexFormat((Vector3)myGameBoard.GetHex(x, y).TopRight, new Vector2((textureOffest + 0.15f), 0.0005f));
                    //Middle Left
                    tempVertexArray[vCounter + 2] = new MyOwnVertexFormat((Vector3)myGameBoard.GetHex(x, y).MiddleLeft, new Vector2((textureOffest + 0.0005f), 0.5f));
                    //Middle Right
                    tempVertexArray[vCounter + 3] = new MyOwnVertexFormat((Vector3)myGameBoard.GetHex(x, y).MiddleRight, new Vector2((textureOffest + 0.1995f), 0.5f));
                    //Bottom Left
                    tempVertexArray[vCounter + 4] = new MyOwnVertexFormat((Vector3)myGameBoard.GetHex(x, y).BottomLeft, new Vector2((textureOffest + 0.05f), 0.9995f));
                    //Bottom Right
                    tempVertexArray[vCounter + 5] = new MyOwnVertexFormat((Vector3)myGameBoard.GetHex(x, y).BottomRight, new Vector2((textureOffest + 0.15f), 0.9995f));
                    //Centre
                    tempVertexArray[vCounter + 6] = new MyOwnVertexFormat((Vector3)myGameBoard.GetHex(x, y).Centre, new Vector2((textureOffest + 0.1f), 0.5f));

                    vCounter = vCounter + 7;
                    iCounter = iCounter + 18;
                }
            }

            return tempVertexArray;
        } //DONE

        private void RayIntersectsTriangle(ref Ray ray, ref Vector3 vertex1, ref Vector3 vertex2, ref Vector3 vertex3, out float? result)
        {
            // Compute vectors along two edges of the triangle.
            Vector3 edge1, edge2;

            Vector3.Subtract(ref vertex2, ref vertex1, out edge1);
            Vector3.Subtract(ref vertex3, ref vertex1, out edge2);

            // Compute the determinant.
            Vector3 directionCrossEdge2;
            Vector3.Cross(ref ray.Direction, ref edge2, out directionCrossEdge2);

            float determinant;
            Vector3.Dot(ref edge1, ref directionCrossEdge2, out determinant);

            // If the ray is parallel to the triangle plane, there is no collision.
            if (determinant > -float.Epsilon && determinant < float.Epsilon)
            {
                result = null;
                return;
            }

            float inverseDeterminant = 1.0f / determinant;

            // Calculate the U parameter of the intersection point.
            Vector3 distanceVector;
            Vector3.Subtract(ref ray.Position, ref vertex1, out distanceVector);

            float triangleU;
            Vector3.Dot(ref distanceVector, ref directionCrossEdge2, out triangleU);
            triangleU *= inverseDeterminant;

            // Make sure it is inside the triangle.
            if (triangleU < 0 || triangleU > 1)
            {
                result = null;
                return;
            }

            // Calculate the V parameter of the intersection point.
            Vector3 distanceCrossEdge1;
            Vector3.Cross(ref distanceVector, ref edge1, out distanceCrossEdge1);

            float triangleV;
            Vector3.Dot(ref ray.Direction, ref distanceCrossEdge1, out triangleV);
            triangleV *= inverseDeterminant;

            // Make sure it is inside the triangle.
            if (triangleV < 0 || triangleU + triangleV > 1)
            {
                result = null;
                return;
            }

            // Compute the distance along the ray to the triangle.
            float rayDistance;
            Vector3.Dot(ref edge2, ref distanceCrossEdge1, out rayDistance);
            rayDistance *= inverseDeterminant;

            // Is the triangle behind the ray origin?
            if (rayDistance < 0)
            {
                result = null;
                return;
            }

            result = rayDistance;
        } //DONE

        private void LoadServices()
        {
            cameraManagerService = (ICameraManagerService)Game.Services.GetService(typeof(ICameraManagerService));
            cursorManagerService = (ICursorManagerService)Game.Services.GetService(typeof(ICursorManagerService));
        } //DONE
    }
}


