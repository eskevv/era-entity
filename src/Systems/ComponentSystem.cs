namespace OrionLibrary;

public class ComponentSystem {

   public HashSet<Entity> Entities { get; set; }
   public bool WasUpdated { get; set; }

   protected EntityScene Coordinator => Orion.Scene;

   public ComponentSystem()
   {
      Entities = new HashSet<Entity>();
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
