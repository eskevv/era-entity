namespace EraEntity.Systems;

public class ComponentSystem {

   public HashSet<int> Entities { get; set; }
   public EntityScene Scene { get; private set;  }
   public bool WasUpdated { get; set; }

   public ComponentSystem(EntityScene scene)
   {
      Entities = new HashSet<int>();
      Scene = scene;
   }

   protected void RecentUpdates()
   {
      if (WasUpdated)
      {
         PerformSetUpdate();
         WasUpdated = false;
      }
   }

   protected virtual void PerformSetUpdate() { }
}
