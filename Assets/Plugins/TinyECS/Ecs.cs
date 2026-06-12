using System.Collections.Generic;

namespace TinyECS
{
    public class IEntity
    {
        private List<IComponent> comps = new List<IComponent>();

        public void AddComp(IComponent comp)
        {
            comps.Add(comp);
        }

        public void RemoveComp(IComponent comp)
        {
            comps.Remove(comp);
        }

        public bool HasComp<T>() where T : IComponent
        {
            foreach (IComponent comp in comps)
            {
                if (comp.GetType() == typeof(T))
                {
                    return true;
                }
            }
            return false;
        }

        public T GetComp<T>() where T : IComponent
        {
            foreach (IComponent comp in comps)
            {
                if (comp.GetType() == typeof(T)) { return (T)comp; }
            }
            return null;
        }

        public List<T> GetComps<T>() where T : IComponent
        {
            List<T> comps = new List<T>();
            foreach (IComponent comp in comps)
            {
                if (comp.GetType() == typeof(T)) { comps.Add((T)comp); }
            }
            return comps;
        }
    }

    public class IComponent { }

    public class ISystem
    {
        public virtual void Update(float deltaTime) { }
        public virtual void OnAddToEngine() { }
        public virtual void OnRemoveFromEngine() { }
    }

    public class Engine
    {
        public List<IEntity> entities = new List<IEntity>();
        public List<ISystem> systems = new List<ISystem>();
        public IEntity sharedConfig = new IEntity();

        public void Update(float deltaTime)
        {
            systems.ForEach((ISystem sys) =>
            {
                sys.Update(deltaTime);
            });
        }

        public ISystem GetSystem<T>() where T : ISystem
        {
            return Util.Find(systems, (ISystem s) => s.GetType() == typeof(T));
        }

        public List<IEntity> FindEntities<T>() where T : IComponent
        {
            List<IEntity> entites = new List<IEntity>();
            foreach (IEntity entity in entities)
            {
                if (entity.HasComp<T>()) { entities.Add(entity); }
            }
            return entities;
        }

        public void AddSystem(ISystem system)
        {
            systems.Add(system);
            system.OnAddToEngine();
        }
        public void RemoveSystem(ISystem system)
        {
            system.OnRemoveFromEngine();
            systems.Remove(system);
        }
        public void AddEntity(IEntity e) { entities.Add(e); }
        public void RemoveEntity(IEntity e) { entities.Remove(e); }
    }

    class Util
    {
        public delegate bool FindHandler<T>(T t) where T : class;
        public static T Find<T>(List<T> list, FindHandler<T> handler) where T : class
        {
            foreach (T t in list)
            {
                if (handler(t)) return t;
            }
            return null;
        }
    }
}