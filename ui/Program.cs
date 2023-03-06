using dl;

var controller = new Controller();

int option;

do
{
    Console.WriteLine("Escoga una opcion:");
    Console.WriteLine("1- Agregar elemento en arbol AVL");
    Console.WriteLine("2- Borrar elemento en arbol AVL");
    Console.WriteLine("3- Agregar elemento en arbol B");
    Console.WriteLine("4- Borrar elemento en arbol B");
    Console.WriteLine("5- Agregar elemento en arbol B+");
    Console.WriteLine("6- Borrar elemento en arbol B+");
    Console.WriteLine("7- Agregar elemento en arbol Rojo-Negro");
    Console.WriteLine("8- Eliminar elemento en arbol Rojo-Negro");
    Console.WriteLine("9- Salir");
    option = Convert.ToInt32(Console.ReadLine());

    switch (option)
    {
        case 1:
            controller.AddInAvl(ReadNumber());
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Dibujando el arbol");
            Console.WriteLine("\n\n\n");
            Console.WriteLine(controller.DrawAvl());
            Console.WriteLine("\n\n\n");
            break;

        case 2:
            controller.DeleteInAvl(ReadNumber());
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Dibujando el arbol");
            Console.WriteLine("\n\n\n");
            Console.WriteLine(controller.DrawAvl());
            Console.WriteLine("\n\n\n");
            break;

        case 3:
            controller.AddInBTree(ReadNumber());
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Mostrando el arbol");
            Console.WriteLine(controller.ShowBTree());
            break;

        case 4:
            controller.DeleteInBTree(ReadNumber());
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Mostrando el arbol");
            Console.WriteLine(controller.ShowBTree());
            break;

        case 5:
            // Code B+ tree
            break;

        case 6:
            // Code B+ tree
            break;

        case 7:
            int x;

            Console.WriteLine("Ingrese el nodo para agregar");
            controller.AddInRedBlack(ReadNumber());
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Dibujando el arbol");
            Console.WriteLine("\n\n\n");
            Console.WriteLine(controller.DrawRedBlack());
            Console.WriteLine("\n\n\n");
            break;

        case 8:
            Console.WriteLine("Ingrese el nodo para eliminar");
            controller.DeleteInRedBlack(ReadNumber());
            Console.WriteLine("Dibujando el arbol");
            Console.WriteLine("\n\n\n");
            Console.WriteLine(controller.DrawRedBlack());
            Console.WriteLine("\n\n\n");
            break;

        default:
            Console.WriteLine("Opcion no valida");
            break;
    }
} while (option != 9);

static int ReadNumber()
{
    Console.WriteLine("Digite el numero");
    var number = Convert.ToInt32(Console.ReadLine());
    return number;
}