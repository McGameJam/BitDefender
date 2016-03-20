using System;

namespace TowerDefense.Data.Models.Anim
{
    public class Animation
    {
        public string Surface;
        public int State = -1;
        public int FrameHeight;
        public int FrameWidth;
        public int MaxState;
        public int UpdateTick;
        private int LastUpdate;
        public bool Disposable = false;
        public Position Position;
        public bool Overlay = false;
        public float Rotation;

        public void Update() {
            if (LastUpdate + UpdateTick < Environment.TickCount) {

                this.State++;

                if (this.State >= MaxState) {
                    this.Disposable = true;
                }

                LastUpdate = Environment.TickCount;
            }
        }
    }
}
