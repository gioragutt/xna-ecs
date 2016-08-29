namespace XnaCommonLib.ECS.Components
{
    public class InputData : DirectionalInput
    {
        public InputData(DirectionalInput other)
        {
            Horizontal = other.Horizontal;
            Vertical = other.Vertical;
        }

        public InputData()
        {
            Horizontal = 0;
            Vertical = 0;
        }

        public override void Update(long delta) {  }
    }
}
