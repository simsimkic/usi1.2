using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Observer;
using ZdravoCorp.Serializer;
using ZdravoCorp.Storage;

namespace ZdravoCorp.Model.DAO
{
    public class DAO<T> : ISubject where T : ISerializable, new()
    { 

        private List<IObserver> _observers;

        private Storage<T> _storage;
        private Dictionary<int,T> _objects;
        public DAO(string filePath)
        {
            _storage = new Storage<T>(filePath);
            _observers = new List<IObserver>();
            _objects = _storage.Load() ?? new Dictionary<int, T>();

        }
        public int NextId()
        {
            return (_objects?.Any() ?? false) && (_objects?.Keys.Max() + 1 ?? 1) > 0 ? _objects.Keys.Max() + 1 : 1;
        }

        public void SaveAll()
        {
            _storage.Save(_objects);
        }
        public void Add(T obj)
        {
            obj.SetId(NextId());
            _objects.Add(obj.GetId(), obj);
            _storage.Save(_objects);
            NotifyObservers();
        }

        public T GetById(int id)
        {
            if (_objects.TryGetValue(id, out T obj))
            {
                return obj;
            }
            throw new ArgumentException("Object with the given ID not found.");
        }
        public void Remove(int id)
        {
            _objects.Remove(id);
            _storage.Save(_objects);
            NotifyObservers();
        }
        public Dictionary<int, T> GetAll()
        {
            return _objects;
        }
        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }

        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }
    }
}
