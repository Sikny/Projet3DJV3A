namespace Units.Controllers.Mage {
    public class Bard : Wizard
    {
    
    
        public Bard(AbstractUnit body) : base(body)
        {
            MultiplierScore = 4f;
            basisDefense *= 4;
        }
    
    }
}
