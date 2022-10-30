using System.Text.RegularExpressions;
using  BankConsole;
using System.Globalization;

if(args.Length == 0)    
    EmailService.SendMail();
else    
    ShowMenu();

void ShowMenu()
{
    Console.Clear();
    Console.WriteLine("Seleccione una opción: ");
    Console.WriteLine("1 - Crear un usuario nuevo: ");
    Console.WriteLine("2 - Eliminar un un usuario existente: ");
    Console.WriteLine("3 - Salir");

    int option = 0;

    do 
    {
        string input = Console.ReadLine();

        if(!int.TryParse(input, out option))
            Console.WriteLine("Debes ingresar un número (1,2 o 3)");
        else if (option<1 || option>3)
            Console.WriteLine("Debes ingresar un número válido (1,2 o 3)");

    }while(option<1 || option>3);

    switch(option)
    {
        case 1:
            createUser();
            break;
        case 2:
            DeleteUser();
            break;
        case 3:
            Environment.Exit(0);
            break;
    }

}

void createUser()
{
    Console.Clear();
    Console.WriteLine("Ingrese los datos del usuario: ");
    Console.Write("ID: ");
    int ID = IDValido();
    
   
    
   while(Storage.UserExists(ID) == 1){
        Console.WriteLine("*El ID del usuario ya existe, ingrese uno nuevo*");
        Console.Write("Ingresa el ID del usuario a añadir: ");
        ID = IDValido();
      
    }


    Console.Write("Nombre: ");
    string name = Console.ReadLine();

    Console.Write("Email: ");
    string email = Console.ReadLine();

    while(!IsValidEmail(email)){
        Console.WriteLine("*Ingrese un correo valido*");
        Console.Write("Ingrese el email nuevamente: ");
        email = Console.ReadLine();
    }

    Console.Write("Saldo: ");
    decimal balance = BalanceValido();

    Console.Write("Escribe 'c' si el usuario es Cliente, 'e' si es Empleado: ");
    char userType = char.Parse(Console.ReadLine());

    while(userType!= 'c' && userType!='e'){
        Console.WriteLine("*Ingrese un rol válido*");
        Console.Write("Ingrese el rol nuevamente: ");
        userType = char.Parse(Console.ReadLine());
    }

    User newUser;

    if(userType.Equals('c')){
        Console.Write("Régime Fiscal: ");
        char TaxRegime = char.Parse(Console.ReadLine());

        newUser = new Client(ID, name, email, balance, TaxRegime);
    }else{
        Console.Write("Departamento: ");
        string department = Console.ReadLine();

        newUser = new Employee(ID, name, email, balance, department);
    }

    Storage.AddUser(newUser);

    Console.WriteLine("Usuario creado exitosamente.");
    Thread.Sleep(2000);
    ShowMenu();
}

void DeleteUser(){
    Console.Clear();

    Console.Write("Ingresa el ID del usuario a eliminar: ");
    int ID = int.Parse(Console.ReadLine());

    while(Storage.UserExists(ID) == 0){
        Console.WriteLine("*El ID del usuario no existe, ingrese uno correcto*");
        Console.Write("Ingresa el ID del usuario a eliminar: ");
        ID = int.Parse(Console.ReadLine());
        Console.Write(Storage.UserExists(ID) + "" + ID);
        
    }
    Console.Write(Storage.UserExists(ID) + "" + ID);

    string result = Storage.DeleteUser(ID);

    if(result.Equals("Success")){
        Console.Write("Usuario eliminado.");
        Thread.Sleep(2000);
        ShowMenu();
    }
   
}

 int IDValido(){
            bool numValido = false;
            int numero = 0;

             while(!numValido){
                
                   Console.Write("");
                   if(int.TryParse(Console.ReadLine(), out numero) && numero>0)
                        numValido = true;
                    else
                        Console.WriteLine("*El valor ingresado es inválido. Vuelva a ingresarlo*");
            }

            return numero;
}

decimal BalanceValido(){
            bool numValido = false;
            decimal numero = 0;

             while(!numValido){
                
                   Console.Write("");
                   if(decimal.TryParse(Console.ReadLine(), out numero) && numero>0)
                        numValido = true;
                    else
                        Console.WriteLine("*El valor ingresado es inválido. Vuelva a ingresarlo*");
            }

            return numero;
}

bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }