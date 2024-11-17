namespace phy.Entities
{
    public class Force
    {
        public float Value { get; set; }
        public PhysicsObject.Direction Direction { get; set; }
        public float TimeRemained { get; set; }
        public string ForceName { get; set; }

        public Force(float value, PhysicsObject.Direction direction, string forceName="RANDOM FORCE", float timeRemained = 1.0f)
        {
            Value = value;
            Direction = direction;
            TimeRemained = timeRemained;
            ForceName = forceName;
        }


    }
}