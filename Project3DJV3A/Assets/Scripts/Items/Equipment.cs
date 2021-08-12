using Utility;

namespace Items {
    public class Equipment : Item
    {
        public int stat;
        //public String description = ""; 
        //public int statDif

        public override void Use()
        {
            base.Use();
            GameSingleton.Instance.soundManager.Play("ItemUse");

            //do stuff
        }
    }
}