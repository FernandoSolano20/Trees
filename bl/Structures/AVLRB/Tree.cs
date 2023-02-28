using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bl.Structures.AVLRB
{
    public class Tree

    {
        private RedBlackNode raiz;//Raiz del arbol

        public Tree() //Constructor
        {

            raiz = null;// iniciando

        }
        public RedBlackNode devolverRaiz()
        {
            return raiz;
        }
        public void fijarRaiz(RedBlackNode Raiz)
        {
            this.raiz = Raiz;
        }
        //Funciones de get y set
        public Boolean vacio()
        {
            return devolverRaiz() == null;
        }
        public void Insertar(int llave)
        {
            RedBlackNode nodoI = new RedBlackNode(llave);//El nodo que se va insertar
            int id = nodoI.devolverLlave();
            if (devolverRaiz() == null)//Si el arbol esta vacio
            {
                fijarRaiz(nodoI);
                devolverRaiz().fijarPadre(null);
                devolverRaiz().fijarColor("negro");//Raiz siempre negra
            }
            else// si ya tienen parametros
            {
                RedBlackNode cursor = devolverRaiz();
                RedBlackNode padre = cursor;
                while (true)
                {
                    if (id < cursor.devolverLlave())//Decidir si ir a izquierda o derecha
                    {
                        padre = cursor;
                        cursor = cursor.devolverHijoI();
                        if (cursor == null)//Al llegar al final se inserta en ese lugar
                        {
                            padre.fijarHijoI(nodoI);
                            nodoI.fijarPadre(padre);
                            break;// se rompe 
                        }
                    }
                    else if (id > cursor.devolverLlave())
                    {
                        padre = cursor;
                        cursor = cursor.devolverHijoD();
                        if (cursor == null)
                        {
                            padre.fijarHijoD(nodoI);
                            nodoI.fijarPadre(padre);
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("\t\nValor ya ingresado en el arbol");
                        Console.ReadKey();

                    }
                }
                arreglarInsercion(nodoI); //funcion que ayudara a mantener los lineamientos de arboles rojo negro
            }
        }


        //Proporciona el arreglo de los nodos si estos rompieron una de las 4 reglas
        public void arreglarInsercion(RedBlackNode z)
        {
            while (z.devolverPadre().devolverColor().Equals("Rojo"))// z sera el parametro de el nodo que se ingreso
            {
                if (z.devolverPadre() == z.devolverPadre().devolverPadre().devolverHijoI())//aqui se comprueba si es el nodo que va para la izquierda
                {
                    // z = arreglarIzqAbuelo(z);
                }
                else// y aqui se comprueba hacia la derecha
                {
                    //z =  arreglarDerAbuelo(z);
                }
                if (z.devolverPadre() == null)
                {
                    break;
                }
            }
            devolverRaiz().fijarColor("negro");// Por si se hace cambios en la raiz
        }
        public void RotarDer(RedBlackNode x) //Son las mismas que se utilizan en los ABB
        {
            RedBlackNode nuevoNodo = x.devolverHijoI();
           // x.devolverHijoD() = nuevoNodo.devolverHijoI();
           // nuevoNodo.devolverHijoI() = x;
           // nuevoNodo.fijarColor("Rojo") = x.fijarColor("Rojo");

        }
        public void RotarIzq(RedBlackNode x)//Son las mismas que se utilizan en los ABB
        {

        }
        //Invocada por los arreglos
        public RedBlackNode ArreglarIzqAbuelo(RedBlackNode z)
        {
            Boolean tioRojo = false;//Primer tipo de desajuste
            RedBlackNode y = z.devolverPadre().devolverPadre().devolverHijoD();
            if (y != null)
            {
                if (y.devolverColor().Equals("rojo"))
                { tioRojo = true; }// se verifica si entra el primer tipo de desajuste
            }
            if (tioRojo)
            {
                TioRojo(z, y);// ayudara con el primer tipo de desajuste en el arbol
                z = z.devolverPadre().devolverPadre();// Se ajusta al abuelo para ver si puede subir al arbol
            }
            else
            {
                if (z == z.devolverPadre().devolverPadre())// verificamos el segundo tipo de desajuste
                {
                    z = RotarIzqPadre(z); // se utiliza la funcion para rotar el padre
                }
                rotarDerAbuelo(z,y); // rotamos al abuelo
            }
            return z;
        }
        public RedBlackNode ArreglarDerAbuelo(RedBlackNode z)
        {
            Boolean tioRojo = false;//Primer tipo de desajuste
            RedBlackNode y = z.devolverPadre().devolverPadre().devolverHijoD();
            if (y != null)
            {
                if (y.devolverColor().Equals("rojo"))
                { tioRojo = true; }// se verifica si entra el primer tipo de desajuste
            }
            if (tioRojo)
            {
                TioRojo(z, y);// ayudara con el primer tipo de desajuste en el arbol
                z = z.devolverPadre().devolverPadre();// Se ajusta al abuelo para ver si puede subir al arbol
            }
            else
            {
                if (z == z.devolverPadre().devolverPadre())// verificamos el segundo tipo de desajuste
                {
                    z = RotarDerPadre(z); // se utiliza la funcion para rotar el padre
                }
                rotarIzqAbuelo(z,y); // rotamos al abuelo
            }
            return z;
        }
        public void TioRojo(RedBlackNode z, RedBlackNode y)
        {
            z.devolverPadre().fijarColor("negro");
            y.fijarColor("negro");
            z.devolverPadre().devolverPadre().fijarColor("rojo");//Esta funcion ayuda cambiar el primer tipo de desajuste en cambiar el tio y el padre a negro
        }
        public RedBlackNode RotarIzqPadre(RedBlackNode z)
        {
            z = z.devolverPadre();
            RotarIzq(z);//Esta rotacion  simple 
            return z;//Sirve para rotar el padre como en una rotacion doble izquierda
        }
        public RedBlackNode RotarDerPadre(RedBlackNode z)
        {
            z = z.devolverPadre();
            RotarDer(z);//Esta rotacion  simple 
            return z;//Sirve para rotar el padre como en una rotacion doble derecha
        }

        public void rotarDerAbuelo(RedBlackNode z, RedBlackNode y)
        {
            z.devolverPadre().fijarColor("negro");// fijar como negro el padre
            if (z.devolverPadre().devolverPadre() != null)
            {
                z.devolverPadre().devolverPadre().fijarColor("rojo");//Y el rojo el abeulo

            }
            RotarDer(z.devolverPadre().devolverPadre());//Segundo paso en el tipo de desajuste 2
        }
        public void rotarIzqAbuelo(RedBlackNode z, RedBlackNode y)
        {
            z.devolverPadre().fijarColor("negro");// fijar como negro el padre
            if (z.devolverPadre().devolverPadre() != null)
            {
                z.devolverPadre().devolverPadre().fijarColor("rojo");//Y el rojo el abeulo

            }
            RotarIzq(z.devolverPadre().devolverPadre());//Segundo paso en el tipo de desajuste 2
        }
        public void Recorrido(RedBlackNode reco)
        {
            if (reco != null)
            {
                Recorrido(reco.devolverHijoI());
                reco.mostrar(reco);
                Recorrido(reco.devolverHijoD());
            }//Funcion de recorrido en orden
        }
        public void Eliminar(RedBlackNode eliminar)
        { // eliminar es la misma que se utiliza en un ABB normal solo que se agrega los arreglos de insercion para balancear

            if (eliminar.devolverHijoD() == null && eliminar.devolverHijoI() == null)
            {
                if (eliminar.devolverPadre().devolverHijoI() == null)
                {
                    eliminar.devolverPadre().fijarHijoI(null);
                }
                else
                {
                    eliminar.devolverPadre().fijarHijoD(null);
                }//No se necesita balancear el caso hoja ya que no afecta como el de un AVL
            }
           else if (eliminar.devolverHijoD()!=null && eliminar.devolverHijoI()==null)
            {
                RedBlackNode hijoAct = eliminar.devolverHijoD();
                eliminar.devolverPadre().fijarHijoI(hijoAct);
                hijoAct.fijarPadre(eliminar.devolverPadre());
                eliminar.fijarHijoD(null);
                eliminar.fijarHijoI(null);
                arreglarInsercion(hijoAct);// SI ES UN PADRE DE UNA HOJA
            }
            else if (eliminar.devolverHijoI()!=null&&eliminar.devolverHijoD==null)
            {
                RedBlackNode hijoAct = eliminar.devolverHijoI();
                eliminar.devolverPadre().fijarHijoD(hijoAct);
                hijoAct.fijarPadre(eliminar.devolverPadre());
                eliminar.fijarHijoD(null);
                eliminar.fijarHijoI(null);
                arreglarInsercion(hijoAct);
            }
            else if (eliminar.devolverHijoI()!=null && eliminar.devolverHijoD()!=null)
            {
                RedBlackNode hijoAct = eliminar.devolverHijoD();
                RedBlackNode aux = hijoAct.devolverHijoI();
                while(aux!=null)
                {
                    hijoAct = aux;
                    aux = aux.devolverHijoI();
                }
                if (hijoAct!=null)
                {
                    eliminar.fijarLlave(hijoAct.devolverLlave());
                    Eliminar(hijoAct);
                }
            }
        }
        public RedBlackNode buscar(int llave)
        {
            RedBlackNode current = raiz;

            while (current != null && current.devolverLlave() != llave)
            {
                if (llave < current.devolverLlave())
                {
                    current = current.devolverHijoI();
                }
                else
                {
                    current = current.devolverHijoD();
                }
            }

            return current;
        }



    }
}