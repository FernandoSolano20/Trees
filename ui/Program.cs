using dl;

var controller = new Controller();
int option;

do
{
    Console.WriteLine("Escoga una opcion:");
    Console.WriteLine("1- Agregar elemento en arbol AVL");
    Console.WriteLine("2- Agregar elemento en arbol B");
    Console.WriteLine("3- Agregar elemento en arbol B+");
    Console.WriteLine("4- Agregar elemento en arbol Rojo-Negro");
    Console.WriteLine("5- Salir");
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
            // Code B tree
            break;

        case 3:
            // Code B+ tree
            break;

        case 4:
            // Code Red-Black tree
            break;

        case 5:
            Console.WriteLine("Adios");
            break;

        default:
            Console.WriteLine("Opcion no valida");
            break;
    }
} while (option != 5);

static int ReadNumber()
{
    Console.WriteLine("Digite el numero");
    var number = Convert.ToInt32(Console.ReadLine());
    return number;
}