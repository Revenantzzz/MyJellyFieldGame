using System;
using JetBrains.Annotations;
using UnityEngine;

namespace JellyField
{
    public class GridSystem<T>
    {
        const float CELLSIZE = 1f;
        int width;
        int height;
        Vector3 origin;
        CoordinateConverter converter;
        T[,] gridArray;

        public GridSystem (int x, int y, Vector3 origin, CoordinateConverter converter, bool debug)
        {
            this.width = x;
            this. height = y;
            this. origin = origin;
            this.converter = converter?? new VerticalConverter();
        
            gridArray = new T[width, height];
            if(debug)
            {
                DrawDebugLines();
            }
        }

        public static GridSystem<T> VerticalGrid (int x, int y, Vector3 origin, bool debug)
        => new GridSystem<T>(x,y, origin, new VerticalConverter(), debug);

        public bool IsValidPos(int x, int y) => x>=0 && y>=0 && x<width && y<height;
        public void SetValue(int x, int y, T value)
        {
            if(IsValidPos(x,y))
                gridArray[x,y] = value;
        }
        public void SetValue(Vector3 worldPos, T value)
        {
            var gridPos = GetGridPosition(worldPos);
            SetValue(gridPos.x, gridPos.y, value);
        }

        //Get Value of a cell
        public T GetValue(int x, int y) => IsValidPos(x,y)? gridArray[x,y] : default(T);
        public T GetValue(Vector3 worldPos)
        {
            var gridPos = GetGridPosition(worldPos);
            return GetValue(gridPos.x, gridPos.y);
        }

        //Get Position methods
        public Vector3 GetWorldPosition(int x, int y) => converter.GridToWorld(x, y, origin);
        public Vector3 GetWorldCenterPosition(int x, int y) => converter.GridCenterToWorld(x, y, origin);
        public Vector2Int GetGridPosition(Vector3 worldPos) => converter.WorldToGrid(worldPos, origin);
        
        //Draw debug lines
        void DrawDebugLines() {
            const float duration = 100f;
            var parent = new GameObject("Debugging");

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, duration);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, duration);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, duration);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, duration);
        }
        public class VerticalConverter : CoordinateConverter
        {
            public override Vector3 GridCenterToWorld(int x, int y, Vector3 origin)
            {
                return new Vector3(x * CELLSIZE + CELLSIZE/2, y * CELLSIZE + CELLSIZE/2, 0) + origin;
            }
            public override Vector3 GridToWorld(int x, int y, Vector3 origin)
            {
                return new Vector3(x, y, 0) * CELLSIZE + origin;
            }

            public override Vector2Int WorldToGrid(Vector3 worldPos, Vector3 origin)
            {
                var posX = Mathf.FloorToInt((worldPos.x - origin.x)/CELLSIZE);
                var posY = Mathf.FloorToInt((worldPos.y - origin.y)/CELLSIZE);

                return new Vector2Int(posX, posY);
            }
        }
        public abstract class CoordinateConverter
        {
            public abstract Vector3 GridToWorld(int x, int y, Vector3 origin);
            public abstract Vector3 GridCenterToWorld(int x, int y, Vector3 origin);
            public abstract Vector2Int WorldToGrid(Vector3 worldPos, Vector3 origin);
        }
    }
}
