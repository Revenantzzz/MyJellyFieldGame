using UnityEngine;

namespace JellyField
{
    public class JellyField : MonoBehaviour
    {
        [SerializeField] int width;
        [SerializeField] int height;
        [SerializeField] Vector3 origin;
        [SerializeField] bool debug;
        GridSystem<Jelly> grid;


        void Start()
        {
            InitializeGrid();
        }
        private void InitializeGrid()
        {
            grid = GridSystem<Jelly>.VerticalGrid(width * 2, height * 2, origin, debug);
        }
    }
}
