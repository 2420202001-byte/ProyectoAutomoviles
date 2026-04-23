using System;
using System.Collections.Generic;

namespace AutoMovilAppCliente
{
    // Interface Observer
    public interface IObserver
    {
        void Actualizar();
    }

    // Interface Observable
    public interface IObservable
    {
        void AgregarObserver(IObserver observer);
        void EliminarObserver(IObserver observer);
        void NotificarObservers();
    }

    // Clase Observable para manejar los observers
    public class AutoObservable : IObservable
    {
        private static AutoObservable instancia;
        private List<IObserver> observers = new List<IObserver>();

        private AutoObservable() { }

        public static AutoObservable GetInstancia()
        {
            if (instancia == null)
                instancia = new AutoObservable();
            return instancia;
        }

        public void AgregarObserver(IObserver observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
        }

        public void EliminarObserver(IObserver observer)
        {
            observers.Remove(observer);
        }

        public void NotificarObservers()
        {
            foreach (var observer in observers)
                observer.Actualizar();
        }
    }
}