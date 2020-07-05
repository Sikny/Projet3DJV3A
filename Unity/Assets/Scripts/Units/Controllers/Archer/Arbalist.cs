namespace Units.Controllers.Archer {
    public class Arbalist : Archer
    {
        public Arbalist(AbstractUnit body) : base(body)
        {
            MultiplierScore = 2f;
            tickAttack *= 2;
        }
    
    }
}
