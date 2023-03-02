using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bl.Structures.AVLRB
{
    public class RedBlackNode
    {
        private int llave;
        private RedBlackNode hijoIzq;
        private RedBlackNode hijoDer;
        private String color;
        private RedBlackNode padre;

        //Constructor
        public RedBlackNode (int id)
        {
            fijarLlave(id);
            fijarHijoD(null);
            fijarHijoI(null);
            fijarPadre(null);
            fijarColor("Rojo");
        }
        public RedBlackNode( )
        {
        
        }
        public int devolverLlave()
        {
            return llave;
        }
        public void fijarLlave(int llave)
        {
            this.llave = llave;
        }
        public RedBlackNode devolverHijoD()
        {
            return hijoDer;
        }
        public void fijarHijoD(RedBlackNode hijo)
        
        {
            hijoDer = hijo;
        }
        public RedBlackNode devolverHijoI()
        {
            return hijoIzq;
        }
        public void fijarHijoI(RedBlackNode hijo)

        {
            hijoIzq = hijo;
        }
        public RedBlackNode devolverPadre()
        {
            return padre;
        }
        public void fijarPadre(RedBlackNode padre)
        {
            this.padre = padre;
        }
        public String devolverColor()
        {
            return color;
        }
        public void fijarColor(string color)
        {
           this.color=color;
        }
        //Funciones basicas
        public void mostrar(RedBlackNode y)
        {
            int hi, hd;
            if (y.devolverHijoI() == null)
            {
                hi = 0;
            }
            else
            {
                hi = y.devolverHijoI().devolverLlave();
            }
            if (y.devolverHijoD() == null)
            {
                hd = 0;
            }
            else
            {
                hd=y.devolverHijoD().devolverLlave();   
            }
            Console.WriteLine("({0},hijoI{1},hijoD{2},color{3})", devolverLlave().ToString(),hi.ToString(),hd.ToString(),devolverColor());
        }
    }
    
}
