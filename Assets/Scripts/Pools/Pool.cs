using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Pools
{
    public class Pool<T> : IEnumerable where T : IResettable
    {
        private readonly List<T> Members = new List<T>();
        private readonly HashSet<T> Unavailable = new HashSet<T>();
        private readonly IFactory<T> Factory;

        public Pool(IFactory<T> factory, int poolSize = 5)
        {
            Factory = factory;

            for (var i = 0; i < poolSize; i++) 
                Create();
        }

        public T Allocate()
        {
            foreach (var t in Members.Where(t => !Unavailable.Contains(t)))
            {
                Unavailable.Add(t);
                return t;
            }

            var newMember = Create();
            Unavailable.Add(newMember);
            return newMember;
        }

        public void Release(T member)
        {
            member.Reset();
            Unavailable.Remove(member);
        }

        private T Create()
        {
            var member = Factory.Create();
            member.Reset();
            Members.Add(member);
            return member;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Members.GetEnumerator();
        }

        public void ReturnAll()
        {
            foreach (var t in Members.Where(t => Unavailable.Contains(t)))
            {
                Unavailable.Remove(t);
                t.Reset();
            }
        }
    }
}