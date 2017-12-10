

namespace SFS_BattleTank.Effects.ParticleSys
{
    public class FadeHelper
    {
        public FadeHelper() { }

        public int UpdateFade(int fade, float lifeTime, float totalElapsedTime)
        {
            int result = 255;
            float degreeOfCompletion = totalElapsedTime / lifeTime;
            result -= (int)(result * degreeOfCompletion);
            return result;
        }
    }
}
