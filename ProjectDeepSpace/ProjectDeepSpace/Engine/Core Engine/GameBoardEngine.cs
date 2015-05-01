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
    public class GameBoardEngine
    {
        #region Variables, Getters and Setters

        private GlobalVariables globalVariables;
        private ContentManager content;
        private Game game;

        private Effect effect;
        private Texture2D hex;

        private MyOwnVertexFormat[] vertexArray;
        private int[] indexArray;

        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;

        private Vector2 oldMouseLocation;
        private Vector3 oldCameraTarget;

        #endregion Variables, Getters and Setters

        public GameBoardEngine(Game game, ContentManager content, ref GlobalVariables globalVariables)
        {
            this.globalVariables = globalVariables;
            this.content = content;
            this.game = game;

            globalVariables.TargetHex = new Vector2(0, 0);

            indexBuffer = new IndexBuffer(game.GraphicsDevice, typeof(int), (18 * globalVariables.X * globalVariables.Y), BufferUsage.WriteOnly);
            vertexBuffer = new VertexBuffer(game.GraphicsDevice, MyOwnVertexFormat.VertexDeclaration, (7 * globalVariables.X * globalVariables.Y), BufferUsage.WriteOnly);
        }

        public void LoadContent()
        {
            effect = content.Load<Effect>("EffectsAndFonts/MyEffects");
            hex = content.Load<Texture2D>("GameBoard/Hex");
        }

        public void Update(GameTime gameTime)
        {
            UpdateBuffers();

            if (!oldCameraTarget.Equals(globalVariables.CameraTarget) || !oldMouseLocation.Equals(globalVariables.MouseLocation))
            {
                UpdateOldTargetHex();
                UpdateTargetHex();
            }

            oldCameraTarget = globalVariables.CameraTarget;
            oldMouseLocation = globalVariables.MouseLocation;
        }

        public void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            RasterizerState stat = new RasterizerState();
            stat.CullMode = CullMode.None;
            game.GraphicsDevice.RasterizerState = stat;

            effect.CurrentTechnique = effect.Techniques["Simplest"];
            effect.Parameters["xViewProjection"].SetValue(globalVariables.ViewMatrix * globalVariables.ProjectionMatrix);
            effect.Parameters["xTexture"].SetValue(hex);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                game.GraphicsDevice.Indices = indexBuffer;
                game.GraphicsDevice.SetVertexBuffer(vertexBuffer);
                game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, (7 * globalVariables.X * globalVariables.Y), 0, (6 * globalVariables.X * globalVariables.Y));
            }
        }
        
        private void UpdateTargetHex()
        {
            Ray cursorRay = CalculateCursorRay(globalVariables.ProjectionMatrix, globalVariables.ViewMatrix);
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
                                y = (i / 7) % globalVariables.Y;
                                x = ((i / 7) - y) / globalVariables.Y;
                            }

                            globalVariables.TargetHex = new Vector2(x, y);
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
                                y = (i / 7) % globalVariables.Y;
                                x = ((i / 7) - y) / globalVariables.Y;
                            }

                            globalVariables.TargetHex = new Vector2(x, y);
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
                                y = (i / 7) % globalVariables.Y;
                                x = ((i / 7) - y) / globalVariables.Y;
                            }

                            globalVariables.TargetHex = new Vector2(x, y);
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
                                y = (i / 7) % globalVariables.Y;
                                x = ((i / 7) - y) / globalVariables.Y;
                            }

                            globalVariables.TargetHex = new Vector2(x, y);
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
                                y = (i / 7) % globalVariables.Y;
                                x = ((i / 7) - y) / globalVariables.Y;
                            }

                            globalVariables.TargetHex = new Vector2(x, y);
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
                                y = (i / 7) % globalVariables.Y;
                                x = ((i / 7) - y) / globalVariables.Y;
                            }

                            globalVariables.TargetHex = new Vector2(x, y);
                        }
                    }
                #endregion
            }
        } //DONE

        private void UpdateOldTargetHex()
        {
            globalVariables.OldTargetHex = globalVariables.TargetHex;
        } //DONE

        private void UpdateBuffers()
        {
            vertexArray = GetVertexArray(globalVariables.ActiveHex, globalVariables.TargetHex, globalVariables.MovePathInRange, globalVariables.MovePathOutOfRange, globalVariables.AttackTargets);
            indexArray = GetIndexArray();
            indexBuffer.SetData(indexArray);
            vertexBuffer.SetData(vertexArray);
        } //DONE

        private int[] GetIndexArray()
        {
            int[] tempIndexArray = new int[18 * globalVariables.X * globalVariables.Y];

            int vCounter = 0;
            int iCounter = 0;

            for (int x = 0; x < globalVariables.X; x++)
            {
                for (int y = 0; y < globalVariables.Y; y++)
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

        private MyOwnVertexFormat[] GetVertexArray(Vector2? active, Vector2 target, List<Vector2> moveInRange, List<Vector2> moveOutOfRange, List<Vector2> attackTargets)
        {
            MyOwnVertexFormat[] tempVertexArray = new MyOwnVertexFormat[7 * globalVariables.X * globalVariables.Y];

            int vCounter = 0;
            int iCounter = 0;

            float xTextureOffset;
            float yTextureOffset = 0f;

            for (int x = 0; x < globalVariables.X; x++)
            {
                for (int y = 0; y < globalVariables.Y; y++)
                {
                    if (active == new Vector2(x, y))
                    {
                        xTextureOffset = 0.2f;
                    }
                    else if (moveInRange != null && moveInRange.Contains(new Vector2(x, y)))
                    {
                        xTextureOffset = 0.4f;
                    }
                    else if (moveOutOfRange != null && moveOutOfRange.Contains(new Vector2(x, y)))
                    {
                        xTextureOffset = 0.6f;
                    }
                    else if (attackTargets != null && attackTargets.Contains(new Vector2(x, y)))
                    {
                        xTextureOffset = 0.8f;
                    }
                    else
                    {
                        xTextureOffset = 0f;
                    }

                    //Top Left
                    tempVertexArray[vCounter + 0] = new MyOwnVertexFormat((Vector3)globalVariables.GetHex(new Vector2(x, y)).TopLeft, new Vector2((xTextureOffset + 0.05f), (yTextureOffset + 0f)));
                    //Top Right
                    tempVertexArray[vCounter + 1] = new MyOwnVertexFormat((Vector3)globalVariables.GetHex(new Vector2(x, y)).TopRight, new Vector2((xTextureOffset + 0.15f), (yTextureOffset + 0f)));
                    //Middle Left
                    tempVertexArray[vCounter + 2] = new MyOwnVertexFormat((Vector3)globalVariables.GetHex(new Vector2(x, y)).MiddleLeft, new Vector2((xTextureOffset + 0.0005f), (yTextureOffset + 0.25f)));
                    //Middle Right
                    tempVertexArray[vCounter + 3] = new MyOwnVertexFormat((Vector3)globalVariables.GetHex(new Vector2(x, y)).MiddleRight, new Vector2((xTextureOffset + 0.1995f), (yTextureOffset + 0.25f)));
                    //Bottom Left
                    tempVertexArray[vCounter + 4] = new MyOwnVertexFormat((Vector3)globalVariables.GetHex(new Vector2(x, y)).BottomLeft, new Vector2((xTextureOffset + 0.05f), (yTextureOffset + 0.5f)));
                    //Bottom Right
                    tempVertexArray[vCounter + 5] = new MyOwnVertexFormat((Vector3)globalVariables.GetHex(new Vector2(x, y)).BottomRight, new Vector2((xTextureOffset + 0.15f), (yTextureOffset + 0.5f)));
                    //Centre
                    tempVertexArray[vCounter + 6] = new MyOwnVertexFormat((Vector3)globalVariables.GetHex(new Vector2(x, y)).Centre, new Vector2((xTextureOffset + 0.1f), (yTextureOffset + 0.25f)));

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

        private Ray CalculateCursorRay(Matrix projectionMatrix, Matrix viewMatrix)
        {
            // create 2 positions in screenspace using the cursor position. 0 is as
            // close as possible to the camera, 1 is as far away as possible.
            Vector3 nearSource = new Vector3(globalVariables.MouseLocation, 0f);
            Vector3 farSource = new Vector3(globalVariables.MouseLocation, 1f);

            // use Viewport.Unproject to tell what those two screen space positions
            // would be in world space. we'll need the projection matrix and view
            // matrix, which we have saved as member variables. We also need a world
            // matrix, which can just be identity.
            Vector3 nearPoint = game.GraphicsDevice.Viewport.Unproject(nearSource,
                projectionMatrix, viewMatrix, Matrix.Identity);

            Vector3 farPoint = game.GraphicsDevice.Viewport.Unproject(farSource,
                projectionMatrix, viewMatrix, Matrix.Identity);

            // find the direction vector that goes from the nearPoint to the farPoint
            // and normalize it....
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            // and then create a new ray using nearPoint as the source.
            return new Ray(nearPoint, direction);
        } //DONE
    }
}


