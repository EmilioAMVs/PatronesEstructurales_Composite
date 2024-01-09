using System;
using System.Collections.Generic;

namespace PatronesDeDiseño.Composite.Conceptual
{
    // La clase base Componente declara operaciones comunes para objetos simples y complejos de una composición.
    abstract class Componente
    {
        public Componente() { }

        // El Componente base puede implementar algún comportamiento predeterminado o dejarlo a las clases concretas (declarando el método que contiene el comportamiento como "abstracto").
        public abstract string Operacion();

        // En algunos casos, sería beneficioso definir las operaciones de gestión de hijos directamente en la clase base Componente. De esta manera, no tendrá que exponer ninguna clase de componente concreta al código del cliente, ni siquiera durante el ensamblaje del árbol de objetos. La desventaja es que estos métodos estarán vacíos para los componentes de nivel hoja.
        public virtual void Agregar(Componente componente)
        {
            throw new NotImplementedException();
        }

        public virtual void Eliminar(Componente componente)
        {
            throw new NotImplementedException();
        }

        // Puede proporcionar un método que permita al código del cliente determinar si un componente puede tener hijos.
        public virtual bool EsCompuesto()
        {
            return true;
        }
    }

    // La clase Hoja representa los objetos finales de una composición. Una hoja no puede tener hijos.
    //
    // Por lo general, son los objetos Hoja los que realizan el trabajo real, mientras que los objetos Compuesto solo delegan a sus subcomponentes.
    class Hoja : Componente
    {
        public override string Operacion()
        {
            return "Hoja";
        }

        public override bool EsCompuesto()
        {
            return false;
        }
    }

    // La clase Compuesto representa los componentes complejos que pueden tener hijos. Por lo general, los objetos Compuesto delegan el trabajo real a sus hijos y luego "suman" el resultado.
    class Compuesto : Componente
    {
        protected List<Componente> _hijos = new List<Componente>();

        public override void Agregar(Componente componente)
        {
            this._hijos.Add(componente);
        }

        public override void Eliminar(Componente componente)
        {
            this._hijos.Remove(componente);
        }

        // El Compuesto ejecuta su lógica principal de una manera particular. Recorre recursivamente todos sus hijos, recopilando y sumando sus resultados. Dado que los hijos del compuesto pasan estas llamadas a sus hijos y así sucesivamente, todo el árbol de objetos se recorre como resultado.
        public override string Operacion()
        {
            int i = 0;
            string resultado = "Rama(";

            foreach (Componente componente in this._hijos)
            {
                resultado += componente.Operacion();
                if (i != this._hijos.Count - 1)
                {
                    resultado += "+";
                }
                i++;
            }

            return resultado + ")";
        }
    }

    class Cliente
    {
        // El código del cliente funciona con todos los componentes a través de la interfaz base.
        public void CodigoCliente(Componente hoja)
        {
            Console.WriteLine($"Resultado: {hoja.Operacion()}\n");
        }
        // Gracias al hecho de que las operaciones de gestión de hijos se declaran en la clase base Componente, el código del cliente puede trabajar con cualquier componente, simple o complejo, sin depender de sus clases concretas\.
        public void CodigoCliente2(Componente componente1, Componente componente2)
        {
            if (componente1.EsCompuesto())
            {
                componente1.Agregar(componente2);
            }
            Console.WriteLine($"Resultado: {componente1.Operacion()}");
        }
    }

    class Programa
    {
        static void Main(string[] args)
        {
            Cliente cliente = new Cliente();

            // De esta manera, el código del cliente puede admitir los componentes simples de la hoja...
            Hoja hoja = new Hoja();
            Console.WriteLine("Cliente: Obtengo un componente simple:");
            cliente.CodigoCliente(hoja);

            // ...así como los compuestos complejos.
            Compuesto arbol = new Compuesto();
            Compuesto rama1 = new Compuesto();
            rama1.Agregar(new Hoja());
            rama1.Agregar(new Hoja());
            Compuesto rama2 = new Compuesto();
            rama2.Agregar(new Hoja());
            arbol.Agregar(rama1);
            arbol.Agregar(rama2);
            Console.WriteLine("Cliente: Ahora tengo un arbol compuesto:");
            cliente.CodigoCliente(arbol);

            Console.Write("Cliente: No necesito verificar las clases de componentes incluso cuando administro el árbol:\n");
            cliente.CodigoCliente2(arbol, hoja);

        }
    }
}
